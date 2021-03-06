﻿using OSPSuite.Core.Commands.Core;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Commands
{
   public class AddInteracionContainerCommand : AddObjectBaseCommand<InteractionContainer, IMoleculeBuilder>
   {
      public AddInteracionContainerCommand(IMoleculeBuilder parent, InteractionContainer itemToAdd, IBuildingBlock buildingBlock) : base(parent, itemToAdd, buildingBlock)
      {
      }

      protected override IReversibleCommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new RemoveInteracionContainerCommand(_parent,_itemToAdd,_buildingBlock).AsInverseFor(this);
      }

      protected override void AddTo(InteractionContainer child, IMoleculeBuilder parent, IMoBiContext context)
      {
         parent.AddInteractionContainer(child);
      }
   }

   public class RemoveInteracionContainerCommand:RemoveObjectBaseCommand<InteractionContainer,IMoleculeBuilder>
   {
      public RemoveInteracionContainerCommand(IMoleculeBuilder parent, InteractionContainer itemToRemove, IBuildingBlock buildingBlock) : base(parent, itemToRemove, buildingBlock)
      {
      }

      protected override IReversibleCommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new AddInteracionContainerCommand(_parent,_itemToRemove,_buildingBlock).AsInverseFor(this);
      }

      protected override void RemoveFrom(InteractionContainer childToRemove, IMoleculeBuilder parent, IMoBiContext context)
      {
         parent.RemoveInteractionContainer(childToRemove);
      }
   }
}