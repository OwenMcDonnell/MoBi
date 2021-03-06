﻿using OSPSuite.DataBinding;
using OSPSuite.DataBinding.DevExpress;
using OSPSuite.UI;
using DevExpress.XtraEditors;
using MoBi.Assets;
using MoBi.Presentation.Presenter;
using MoBi.Presentation.Views;
using OSPSuite.Presentation.Extensions;
using OSPSuite.Assets;
using OSPSuite.UI.Controls;
using OSPSuite.UI.Services;

namespace MoBi.UI.Views
{
   public partial class ApplyToSelectionView : BaseUserControl, IApplyToSelectionView
   {
      protected IApplyToSelectionPresenter _presenter;
      protected readonly ScreenBinder<IApplyToSelectionPresenter> _screenBinder;

      public ApplyToSelectionView(IImageListRetriever imageListRetriever)
      {
         InitializeComponent();
         _screenBinder = new ScreenBinder<IApplyToSelectionPresenter>();
         cbSelection.Properties.SmallImages = imageListRetriever.AllImages16x16;
         cbSelection.Properties.LargeImages = imageListRetriever.AllImages32x32;
      }

      public override void InitializeResources()
      {
         base.InitializeResources();
         btnSelection.Text = AppConstants.Captions.PerformCheckSelection;
         btnSelection.Image = ApplicationIcons.OK;
         btnSelection.ImageLocation = ImageLocation.MiddleLeft;
      }

      public override string Caption
      {
         set { lblCaption.Text = value.FormatForLabel(); }
      }

      public override void InitializeBinding()
      {
         _screenBinder.Bind(x => x.CurrentSelection)
            .To(cbSelection)
            .WithImages(x => ApplicationIcons.IconIndex(x.Icon))
            .WithValues(x => _presenter.AvailableSelectOptions)
            .AndDisplays(x => x.Caption);

         btnSelection.Click += (o, e) => OnEvent(_presenter.PerformSelectionHandler);
      }

      public void AttachPresenter(IApplyToSelectionPresenter presenter)
      {
         _presenter = presenter;
      }

      public void BindToSelection()
      {
         _screenBinder.BindToSource(_presenter);
      }
   }
}