﻿using System.Collections.Generic;
using System.Data;
using OSPSuite.BDDHelper;
using FakeItEasy;
using MoBi.Core.Domain.Model;
using MoBi.Core.Domain.Model.Diagram;
using MoBi.Core.Mappers;
using MoBi.Core.Services;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;
using OSPSuite.Presentation.Diagram;

namespace MoBi.Core.Service
{
   public abstract class concern_for_ModelPartsToExcelExporterTask : ContextSpecification<ModelPartsToExcelExporterTask>
   {
      protected IReactionBuildingBlockToReactionDataTableMapper _reactionBuildingBlockToReactionDataTableMapper;
      protected IParameterListToSimulationParameterDataTableMapper _parameterListToSimulationDataTableMapper;
      protected IMoleculeStartValuesBuildingBlockToParameterDataTableMapper _moleculeStartValuesBuildingBlockToParameterDataTableMapper;
      protected override void Context()
      {
         _reactionBuildingBlockToReactionDataTableMapper = A.Fake<IReactionBuildingBlockToReactionDataTableMapper>();
         _parameterListToSimulationDataTableMapper = A.Fake<IParameterListToSimulationParameterDataTableMapper>();
         _moleculeStartValuesBuildingBlockToParameterDataTableMapper = A.Fake<IMoleculeStartValuesBuildingBlockToParameterDataTableMapper>();
         sut = new ModelPartsToExcelExporterTask(
            _reactionBuildingBlockToReactionDataTableMapper,
            _parameterListToSimulationDataTableMapper,
            _moleculeStartValuesBuildingBlockToParameterDataTableMapper
            );

         A.CallTo(() => _reactionBuildingBlockToReactionDataTableMapper.MapFrom(A<IMoBiReactionBuildingBlock>.Ignored)).Returns(new DataTable {TableName = "reactions"});
         A.CallTo(() => _parameterListToSimulationDataTableMapper.MapFrom(A<IReadOnlyList<IParameter>>.Ignored)).Returns(new DataTable { TableName = "parameters" });
         A.CallTo(() => _moleculeStartValuesBuildingBlockToParameterDataTableMapper.MapFrom(A<IEnumerable<IMoleculeStartValue>>.Ignored, A<IEnumerable<IMoleculeBuilder>>.Ignored)).Returns(new DataTable { TableName = "molecules" });
      }
   }

   public class When_mapping_model_parts : concern_for_ModelPartsToExcelExporterTask
   {
      private MoBiReactionBuildingBlock _moBiReactionBuildingBlock;
      private MoleculeStartValuesBuildingBlock _moleculeStartValuesBuildingBlock;
      private IMoBiSimulation _moBiSimulation;
      private List<IParameter> _parameterList;

      protected override void Context()
      {
         base.Context();
         _moBiReactionBuildingBlock = new MoBiReactionBuildingBlock();
         _moleculeStartValuesBuildingBlock = new MoleculeStartValuesBuildingBlock();
         _moBiSimulation = A.Fake<IMoBiSimulation>();
         _parameterList = new List<IParameter>();

         A.CallTo(() => _moBiSimulation.MoBiBuildConfiguration.MoBiReactions).Returns(_moBiReactionBuildingBlock);
         A.CallTo(() => _moBiSimulation.MoBiBuildConfiguration.MoleculeStartValues).Returns(_moleculeStartValuesBuildingBlock);
         A.CallTo(() => _moBiSimulation.Model.Root.GetAllChildren<IParameter>()).Returns(_parameterList);
      }

      protected override void Because()
      {
         sut.ExportModelPartsToExcelFile("filename.xlsx", _moBiSimulation, false);
      }

      [Observation]
      public void calls_reaction_building_block_mapper()
      {
         A.CallTo(() => _reactionBuildingBlockToReactionDataTableMapper.MapFrom(_moBiReactionBuildingBlock)).MustHaveHappened();
      }

      [Observation]
      public void calls_simulation_to_parameter_mapper()
      {
         A.CallTo(() => _parameterListToSimulationDataTableMapper.MapFrom(_parameterList)).MustHaveHappened();
      }

      [Observation]
      public void calls_molecule_building_block_mapper()
      {
         A.CallTo(() => _moleculeStartValuesBuildingBlockToParameterDataTableMapper.MapFrom(A<IEnumerable<IMoleculeStartValue>>._, A<IEnumerable<IMoleculeBuilder>>._)).MustHaveHappened();
      }
   }
}
