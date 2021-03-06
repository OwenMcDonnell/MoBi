﻿using MoBi.Assets;
using OSPSuite.Presentation.MenuAndBars;
using OSPSuite.Presentation.Nodes;
using MoBi.Core.Domain.Model;
using MoBi.Presentation.UICommand;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Presentation.Presenters.ContextMenus;
using OSPSuite.Presentation.Presenters.Nodes;
using OSPSuite.Assets;

namespace MoBi.Presentation.MenusAndBars.ContextMenus
{
   public class RootContextMenuForSimulation : RootContextMenuFor<IMoBiProject, IMoBiSimulation>
   {
      public RootContextMenuForSimulation(IObjectTypeResolver objectTypeResolver, IMoBiContext context) : base(objectTypeResolver, context)
      {
      }

      public override IContextMenu InitializeWith(RootNodeType rootNodeType, IExplorerPresenter presenter)
      {
         var simulationFolderNode = presenter.NodeByType(rootNodeType);
         _allMenuItems.Add(createNewSimulationMenuBarItem());
         _allMenuItems.Add(createAddExistingSimulationMenuBarItem());
         _allMenuItems.Add(CreateReportItemForCollection());
         _allMenuItems.Add(ClassificationCommonContextMenuItems.CreateClassificationUnderMenu(simulationFolderNode, presenter).AsGroupStarter());
         _allMenuItems.Add(SimulationClassificationCommonContextMenuItems.RemoveSimulationFolderMainMenu(simulationFolderNode, presenter).AsGroupStarter());
         _allMenuItems.Add(deleteAllSimulationResults().AsGroupStarter());
         return this;
      }

      private IMenuBarItem deleteAllSimulationResults()
      {
         return CreateMenuButton.WithCaption(AppConstants.MenuNames.DeleteAllResults)
                    .WithCommand<DeleteAllResultsInAllSimulationsUICommand>();
      }

      private IMenuBarItem createAddExistingSimulationMenuBarItem()
      {
         return CreateMenuButton.WithCaption(AppConstants.MenuNames.AddExisting(ObjectTypes.Simulation))
            .WithCommand<LoadProjectUICommand>()
            .WithIcon(ApplicationIcons.SimulationLoad);
      }

      private IMenuBarItem createNewSimulationMenuBarItem()
      {
         return CreateMenuButton.WithCaption(AppConstants.MenuNames.AddNew(ObjectTypes.Simulation))
            .WithIcon(ApplicationIcons.Simulation)
            .WithCommand<NewSimulationCommand>();
      }


   }
}