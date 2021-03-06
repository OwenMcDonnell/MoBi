﻿using MoBi.Assets;
using OSPSuite.Core.Commands.Core;
using MoBi.Core.Domain.Model;
using MoBi.Core.Events;
using MoBi.Core.Helper;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Commands
{
   public class AddBuildingBlockCommand<T> : MoBiReversibleCommand, ISilentCommand where T : class, IBuildingBlock
   {
      protected T _buildingBlock;
      public string BuildingBlockId { get; private set; }
      public bool Silent { get; set; }

      public AddBuildingBlockCommand(T buildingBlock)
      {
         ObjectType = new ObjectTypeResolver().TypeFor<T>();
         CommandType = AppConstants.Commands.AddCommand;
         _buildingBlock = buildingBlock;
         BuildingBlockId = buildingBlock.Id;
         Description = AppConstants.Commands.AddToProjectDescription(ObjectType, buildingBlock.Name);
         Silent = false;
      }

      protected override void ExecuteWith(IMoBiContext context)
      {
         var project = context.CurrentProject;
         context.Register(_buildingBlock);
         addToProject(project);

         if (!Silent)
            context.PublishEvent(new AddedEvent<T>(_buildingBlock, project));
      }

      public override void RestoreExecutionData(IMoBiContext context)
      {
         _buildingBlock = context.Get<T>(BuildingBlockId);
      }

      protected override IReversibleCommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new RemoveBuildingBlockCommand<T>(_buildingBlock).AsInverseFor(this);
      }

      protected override void ClearReferences()
      {
         _buildingBlock = null;
      }

      private void addToProject(IMoBiProject project)
      {
         project.AddBuildingBlock(_buildingBlock);
      }

   }
}