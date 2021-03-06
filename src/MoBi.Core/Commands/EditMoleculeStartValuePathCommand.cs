using OSPSuite.Core.Commands.Core;
using MoBi.Core.Domain.Model;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Builder;

namespace MoBi.Core.Commands
{
   public class EditMoleculeStartValuePathCommand : EditStartValuePathCommand<IMoleculeStartValuesBuildingBlock, IMoleculeStartValue>
   {
      /// <summary>
      /// Changes a path for a Molecule start value
      /// </summary>
      /// <param name="buildingBlock">The building block this start value is a part of</param>
      /// <param name="startValue">The start value being modified</param>
      /// <param name="newContainerPath">The new container path for this start value</param>
      public EditMoleculeStartValuePathCommand(IMoleculeStartValuesBuildingBlock buildingBlock, IMoleculeStartValue startValue, IObjectPath newContainerPath)
         : base(buildingBlock, startValue, newContainerPath)
      {

      }

      protected override IReversibleCommand<IMoBiContext> GetInverseCommand(IMoBiContext context)
      {
         return new EditMoleculeStartValuePathCommand(_buildingBlock, _buildingBlock[_newStartValuePath], _originalContainerPath).AsInverseFor(this);
      }
   }
}