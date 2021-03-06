﻿using System.Collections.Generic;
using System.Linq;
using MoBi.Core.Domain.UnitSystem;
using OSPSuite.Core.Chart;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Utility.Extensions;
using OSPSuite.Utility.Visitor;

namespace MoBi.Core.Domain.Model
{
   public interface IMoBiProject : IProject
   {
      IDimensionFactory DimensionFactory { get; set; }
      IReadOnlyList<IMoBiSimulation> Simulations { get; }
      IReadOnlyList<IMoleculeBuildingBlock> MoleculeBlockCollection { get; }
      IReadOnlyList<IMoBiReactionBuildingBlock> ReactionBlockCollection { get; }
      IReadOnlyList<IPassiveTransportBuildingBlock> PassiveTransportCollection { get; }
      IReadOnlyList<IMoBiSpatialStructure> SpatialStructureCollection { get; }
      IReadOnlyList<IObserverBuildingBlock> ObserverBlockCollection { get; }
      IReadOnlyList<IEventGroupBuildingBlock> EventBlockCollection { get; }
      IReadOnlyList<IMoleculeStartValuesBuildingBlock> MoleculeStartValueBlockCollection { get; }
      IReadOnlyList<IParameterStartValuesBuildingBlock> ParametersStartValueBlockCollection { get; }
      IReadOnlyList<ISimulationSettings> SimulationSettingsCollection { get; }

      ReactionDimensionMode ReactionDimensionMode { get; set; }

      void AddSimulation(IMoBiSimulation newSimulation);
      void RemoveSimulation(IMoBiSimulation simualtionToRemove);
      void AddChart(CurveChart chart);
      void RemoveChart(CurveChart chartToRemove);

      //only for serialization
      IReadOnlyList<IBuildingBlock> AllBuildingBlocks();

      /// <summary>
      ///    Returns the template <see cref="BuildingBlock" /> with the given <paramref name="templateBuildingBlockId" />
      ///    or null if no template exists with the given id.
      /// </summary>
      IBuildingBlock TemplateById(string templateBuildingBlockId);

      void AddBuildingBlock(IBuildingBlock buildingBlock);
      void RemoveBuildingBlock(IBuildingBlock buildingBlockToRemove);

      /// <summary>
      ///    Gets the list of start value building blocks that refer to
      ///    <paramref name="buildingBlockToReference">buildingBlockToReference</paramref>
      ///    through the MoleculeBuildingBlockId or SpatialStructureId
      /// </summary>
      /// <param name="buildingBlockToReference">
      ///    This is the building block to check for reference. Normally a spatial structure
      ///    or molecule building block
      /// </param>
      /// <returns>The list of referring building block names</returns>
      IReadOnlyList<IBuildingBlock> ReferringStartValuesBuildingBlocks(IBuildingBlock buildingBlockToReference);

      /// <summary>
      ///    Returns the list of simulations that were created using the given <paramref name="templateBuildingBlock" />
      /// </summary>
      IReadOnlyList<IMoBiSimulation> SimulationsCreatedUsing(IBuildingBlock templateBuildingBlock);

      /// <summary>
      ///    Returns an enumeration containing all building blocks and simulation defined in the project
      /// </summary>
      /// <returns></returns>
      IEnumerable<IObjectBase> All();

      IEnumerable<CurveChart> Charts { get; }

      /// <summary>
      ///    Returns <c>true</c> if the project does not contain any building block and simulation otherwise <c>false</c>
      /// </summary>
      bool IsEmpty { get; }
   }

   public class MoBiProject : Project, IMoBiProject
   {
      private readonly List<IBuildingBlock> _buildingBlocks;
      private readonly List<CurveChart> _charts;
      private readonly List<IMoBiSimulation> _allSimulations;

      public string ChartSettings { get; set; }
      public IDimensionFactory DimensionFactory { get; set; }

      public override bool HasChanged { get; set; }

      public override IEnumerable<IUsesObservedData> AllUsersOfObservedData => AllParameterIdentifications.Cast<IUsesObservedData>().Union(Simulations);

      public ReactionDimensionMode ReactionDimensionMode { get; set; }

      public MoBiProject()
      {
         DimensionFactory = new MoBiMergedDimensionFactory();
         _charts = new List<CurveChart>();
         _buildingBlocks = new List<IBuildingBlock>();
         _allSimulations = new List<IMoBiSimulation>();
         ReactionDimensionMode = ReactionDimensionMode.AmountBased;
      }

      public override void AddParameterIdentification(ParameterIdentification parameterIdentification)
      {
         base.AddParameterIdentification(parameterIdentification);
         parameterIdentification.IsLoaded = true;
      }

      public override IReadOnlyCollection<T> All<T>()
      {
         return get<T>();
      }

      public IEnumerable<CurveChart> Charts => _charts;

      public bool IsEmpty => !_buildingBlocks.Any() && !_allSimulations.Any();

      public IReadOnlyList<IMoBiSimulation> Simulations => _allSimulations;

      private IReadOnlyList<T> get<T>()
      {
         return _buildingBlocks.OfType<T>().ToList();
      }

      public IReadOnlyList<IMoleculeBuildingBlock> MoleculeBlockCollection => get<IMoleculeBuildingBlock>();

      public IReadOnlyList<IMoBiReactionBuildingBlock> ReactionBlockCollection => get<IMoBiReactionBuildingBlock>();

      public IReadOnlyList<ISimulationSettings> SimulationSettingsCollection => get<ISimulationSettings>();

      public IReadOnlyList<IPassiveTransportBuildingBlock> PassiveTransportCollection => get<IPassiveTransportBuildingBlock>();

      public IReadOnlyList<IMoBiSpatialStructure> SpatialStructureCollection => get<IMoBiSpatialStructure>();

      public IReadOnlyList<IObserverBuildingBlock> ObserverBlockCollection => get<IObserverBuildingBlock>();

      public IReadOnlyList<IEventGroupBuildingBlock> EventBlockCollection => get<IEventGroupBuildingBlock>();

      public IReadOnlyList<IMoleculeStartValuesBuildingBlock> MoleculeStartValueBlockCollection => get<IMoleculeStartValuesBuildingBlock>();

      public IReadOnlyList<IParameterStartValuesBuildingBlock> ParametersStartValueBlockCollection => get<IParameterStartValuesBuildingBlock>();

      public void AddSimulation(IMoBiSimulation newSimulation)
      {
         _allSimulations.Add(newSimulation);
      }

      public void RemoveSimulation(IMoBiSimulation simualtionToRemove)
      {
         _allSimulations.Remove(simualtionToRemove);
         RemoveClassifiableForWrappedObject(simualtionToRemove);
      }

      public void AddChart(CurveChart chart)
      {
         _charts.Add(chart);
      }

      public void RemoveChart(CurveChart chartToRemove)
      {
         _charts.Remove(chartToRemove);
      }

      public IReadOnlyList<IBuildingBlock> AllBuildingBlocks()
      {
         return _buildingBlocks;
      }

      public IBuildingBlock TemplateById(string templateBuildingBlockId)
      {
         return AllBuildingBlocks().FindById(templateBuildingBlockId);
      }

      public void AddBuildingBlock(IBuildingBlock buildingBlock)
      {
         _buildingBlocks.Add(buildingBlock);
      }

      public void RemoveBuildingBlock(IBuildingBlock buildingBlockToRemove)
      {
         _buildingBlocks.Remove(buildingBlockToRemove);
         RemoveClassifiableForWrappedObject(buildingBlockToRemove);
      }

      public override void AcceptVisitor(IVisitor visitor)
      {
         base.AcceptVisitor(visitor);
         _buildingBlocks.Each(x => x.AcceptVisitor(visitor));
         _allSimulations.Each(x => x.AcceptVisitor(visitor));
         _charts.Each(x => x.AcceptVisitor(visitor));
      }

      public IReadOnlyList<IBuildingBlock> ReferringStartValuesBuildingBlocks(IBuildingBlock buildingBlockToRemove)
      {
         return referringStartValuesBuildingBlocks(buildingBlockToRemove, ParametersStartValueBlockCollection)
            .Concat(referringStartValuesBuildingBlocks(buildingBlockToRemove, MoleculeStartValueBlockCollection))
            .ToList();
      }

      public IReadOnlyList<IMoBiSimulation> SimulationsCreatedUsing(IBuildingBlock templateBuildingBlock)
      {
         return Simulations.Where(simulation => simulation.IsCreatedBy(templateBuildingBlock)).ToList();
      }

      public IEnumerable<IObjectBase> All()
      {
         return All<IObjectBase>().Union(Simulations);
      }

      private IEnumerable<IBuildingBlock> referringStartValuesBuildingBlocks<T>(IBuildingBlock buildingBlockToRemove, IEnumerable<IStartValuesBuildingBlock<T>> buildingBlockCollection) where T : class, IStartValue
      {
         return buildingBlockCollection
            .Where(msvBB => msvBB.Uses(buildingBlockToRemove))
            .OfType<IBuildingBlock>()
            .ToList();
      }
   }
}