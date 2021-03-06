﻿using OSPSuite.Utility;
using MoBi.Presentation.DTO;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Presentation.Mappers
{
   public interface IMoleculeBuilderToDummyMoleculeDTOMapper
   {
      DummyMoleculeDTO MapFrom(IMoleculeBuilder moleculeBuilder, IContainer container);
   }

   internal class MoleculeBuilderToDummyMoleculeDTOMapper : ObjectBaseToObjectBaseDTOMapperBase, IMoleculeBuilderToDummyMoleculeDTOMapper
   {
      public DummyMoleculeDTO MapFrom(IMoleculeBuilder moleculeBuilder, IContainer container)
      {
         var dto = Map<DummyMoleculeDTO>(moleculeBuilder);
         dto.MoleculeBuilder = moleculeBuilder;
         dto.StructureParent = container;
         dto.Id = ShortGuid.NewGuid();
         return dto;
      }
   }
}