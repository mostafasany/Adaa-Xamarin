using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using CoreGraphics;
using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

namespace AdaaMobile.iOS.CustomRenderers.HorizontalListView
{
    //Source from https://github.com/XLabs/Xamarin-Forms-Labs
    public class HorizontalListCell : UICollectionViewCell
    {
        /// <summary>
        /// The key
        /// </summary>
        public const string Key = "HorizontalListCell";

        /// <summary>
        /// Used to detect if the context has changed.
        /// </summary>
        object _originalBindingContext;

        /// <summary>
        /// The _view
        /// </summary>
        private UIView _view;

        /// <summary>
        /// The _view cell
        /// </summary>
        private ViewCell _viewCell;
        /// <summary>
        /// Gets or sets the view cell.
        /// </summary>
        /// <value>The view cell.</value>
        public ViewCell ViewCell
        {
            get
            {
                return this._viewCell;
            }
            set
            {
                if (this._viewCell != value)
                {
                    _viewCell = value;
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HorizontalListCell"/> class.
        /// </summary>
        /// <param name="frame">The frame.</param>
        [Export("initWithFrame:")]
        public HorizontalListCell(CGRect frame) : base(frame)
        {
            BackgroundColor = Color.Transparent.ToUIColor();
        }

        public void RecycleCell(object data, DataTemplate dataTemplate, VisualElement parent)
        {
            if (_viewCell == null)
            {
                _viewCell = (dataTemplate.CreateContent() as ViewCell);
                _viewCell.BindingContext = data;
                _originalBindingContext = _viewCell.BindingContext;
                _viewCell.Parent = parent;
                // _viewCell.PrepareCell(new Size(Bounds.Width, Bounds.Height));

                var renderer = GetRenderer();
                _view = GetRenderer().NativeView;

                ContentView.AddSubview(_view);

                //Listen for property changed
                this._viewCell.PropertyChanged -= this.HandlePropertyChanged;
                this._viewCell.PropertyChanged += this.HandlePropertyChanged;
                return;
            }
            else if (data == _originalBindingContext)
            {
                _viewCell.BindingContext = _originalBindingContext;
            }
            else
            {
                _viewCell.BindingContext = data;
            }
        }

        /// <summary>
        /// Layouts the subviews.
        /// </summary>
        public override void LayoutSubviews()
        {
            base.LayoutSubviews();
            var frame = this.ContentView.Frame;
            frame.X = (this.Bounds.Width - frame.Width) / 2;
            frame.Y = (this.Bounds.Height - frame.Height) / 2;
            this.ViewCell.View.Layout(frame.ToRectangle());
            this._view.Frame = frame;
        }


        /// <summary>
        /// Handles the property changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="PropertyChangedEventArgs"/> instance containing the event data.</param>
        private void HandlePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            UpdateView();
        }

        /// <summary>
        /// Updates the view.
        /// </summary>
        private void UpdateView()
        {
            //TODO:Investigate if it can be update without removing
            if (this._view != null)
            {
                this._view.RemoveFromSuperview();
            }

            _view = GetRenderer().NativeView;

            AddSubview(this._view);
        }

        private IVisualElementRenderer GetRenderer()
        {
            var renderer = Platform.CreateRenderer(this._viewCell.View);
            this._view = renderer.NativeView;
            this._view.AutoresizingMask = UIViewAutoresizing.All;
            this._view.ContentMode = UIViewContentMode.ScaleToFill;
            return renderer;
        }
    }
}
