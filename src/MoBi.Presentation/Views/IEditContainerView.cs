﻿using MoBi.Presentation.DTO;
using MoBi.Presentation.Presenter;
using OSPSuite.Presentation.Views;

namespace MoBi.Presentation.Views
{
   public interface IEditContainerView : IView<IEditContainerPresenter>, IActivatableView
   {
      void BindTo(ContainerDTO dto);
      void SetParameterView(IView view);
      bool ReadOnly { get; set; }
      bool ContainerPropertiesEditable { get; set; }
      void ShowParameters();
   }
}