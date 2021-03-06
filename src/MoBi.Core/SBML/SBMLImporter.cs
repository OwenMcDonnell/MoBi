﻿using System.Linq;
using OSPSuite.Core.Commands.Core;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

using Model = libsbmlcs.Model;

namespace MoBi.Core.SBML
{
   public abstract class SBMLImporter
   {
      protected ASTHandler _astHandler;
      protected IMoBiProject _sbmlProject;
      protected SBMLInformation _sbmlInformation;
      protected ICommandCollector _command;
      protected IMoBiContext _context;

      protected SBMLImporter(IObjectPathFactory objectPathFactory, IObjectBaseFactory objectBaseFactory, ASTHandler astHandler, IMoBiContext context)
      {
         ObjectPathFactory = objectPathFactory;
         ObjectBaseFactory = objectBaseFactory;
         _astHandler = astHandler;
         _context = context;
      }

      /// <summary>
      ///     Gets the MoBi Spatial Structure generated for the SBML Import.
      /// </summary>
      public IMoBiSpatialStructure GetMainSpatialStructure(Model model)
      {
         return _sbmlProject.SpatialStructureCollection.FindByName(SBMLConstants.SBML_MODEL + model.getName());
      }

      /// <summary>
      ///     Gets the MoBi Reaction Building Block generated for the SBML Import.
      /// </summary>
      public IMoBiReactionBuildingBlock GetMainReactionBuildingBlock()
      {
         return _sbmlProject.ReactionBlockCollection.FindByName(SBMLConstants.SBML_REACTION_BB);
      }

      /// <summary>
      ///     Gets the MoBi Top Container generated for the SBML Import.
      /// </summary>
      public IContainer GetMainTopContainer()
      {
         return
            _sbmlProject.SpatialStructureCollection.Select(ss => ss.TopContainers.FindById(SBMLConstants.SBML_TOP_CONTAINER))
               .FirstOrDefault();
      }

      /// <summary>
      ///     Gets the MoBi Eventes Top Container generated for the SBML Import.
      /// </summary>
      public IContainer GetEventsTopContainer()
      {
         if (_sbmlProject == null) return null;
         if (_sbmlProject.SpatialStructureCollection == null) return null;
         return
            _sbmlProject.SpatialStructureCollection.Select(
               ss => ss.TopContainers.FindByName(SBMLConstants.SBML_EVENTS_TOP_CONTAINER)).FirstOrDefault();
      }

      /// <summary>
      ///     Gets the MoBi Molecule Building Block generated for the SBML Import.
      /// </summary>
      public IMoleculeBuildingBlock GetMainMoleculeBuildingBlock()
      {
         return _sbmlProject.MoleculeBlockCollection.FirstOrDefault(mb => mb.Name == SBMLConstants.SBML_SPECIES_BB);
      }

      /// <summary>
      ///     Gets the MoBi Parameter Start Values Building Block generated for the SBML Import.
      /// </summary>
      public IParameterStartValuesBuildingBlock GetMainParameterStartValuesBuildingBlock()
      {
         return
            _sbmlProject.ParametersStartValueBlockCollection.FirstOrDefault(
               mb => mb.Id == SBMLConstants.SBML_PARAMETERSTARTVALUES_BB);
      }

      /// <summary>
      ///     Gets the MoBi Molecule Start Values Building Block generated for the SBML Import.
      /// </summary>
      public IMoleculeStartValuesBuildingBlock GetMainMSVBuildingBlock()
      {
         return
            _sbmlProject.MoleculeStartValueBlockCollection.FirstOrDefault(
               mb => mb.Id == SBMLConstants.SBML_MOLECULESTARTVALUES_BB);
      }

      /// <summary>
      ///     Gets the Mobi Container by a given SBML Compartment.
      /// </summary>
      public IEntity GetContainerFromCompartment(string compartment)
      {
         return GetMainTopContainer().GetSingleChildByName(compartment);
      }

      public IContainer GetContainerFromCompartment_(string compartment)
      {
         if (GetMainTopContainer() == null) return null;
         var children = GetMainTopContainer().GetAllChildren<IContainer>();
         return children.FirstOrDefault(child => child.Name == compartment);
      }

      /// <summary>
      ///     Gets the MoBi Molecule by it's name.
      /// </summary>
      protected IMoleculeBuilder GetMoleculeByName(string moleculeName)
      {
         var mbEnumerator = _sbmlProject.MoleculeBlockCollection.GetEnumerator();
         while (mbEnumerator.MoveNext())
         {
            if (mbEnumerator.Current.ExistsByName(moleculeName))
               return mbEnumerator.Current.FindByName(moleculeName);
         }
         return null;
      }

      public void DoImport(Model sbmlModel, IMoBiProject moBiProject, SBMLInformation sbmlInformation, ICommandCollector command)
      {
         _sbmlProject = moBiProject;
         _sbmlInformation = sbmlInformation;
         _command = command;
         try
         {
            Import(sbmlModel);
         }
         finally
         {
            _sbmlProject = null;
            _sbmlInformation = null;
            _command = null;
         }
      }


      protected abstract void Import(Model model);

      public abstract void AddToProject();
      public static IWithIdRepository IdRepository { get; private set; }
      public IObjectPathFactory ObjectPathFactory { get; set; }
      public IObjectBaseFactory ObjectBaseFactory { get; set; }
   }
}