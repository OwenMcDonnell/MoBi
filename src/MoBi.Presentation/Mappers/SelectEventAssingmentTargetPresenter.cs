﻿using OSPSuite.Utility;
using MoBi.Presentation.DTO;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.Mappers
{
   public interface IReactionBuilderToDummyReactionDTOMapper
   {
      DummyReactionDTO MapFrom(IReactionBuilder reactionBuilder, IContainer container);
   }

   internal class ReactionBuilderToDummyReactionDTOMapper : ObjectBaseToObjectBaseDTOMapperBase, IReactionBuilderToDummyReactionDTOMapper
   {
      public DummyReactionDTO MapFrom(IReactionBuilder reactionBuilder, IContainer container)
      {
         var dto = Map<DummyReactionDTO>(reactionBuilder);
         dto.Id = ShortGuid.NewGuid();
         dto.ReactionBuilder = reactionBuilder;
         dto.StructureParent = container;
         return dto;
      }
   }
}