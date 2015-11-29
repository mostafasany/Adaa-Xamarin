using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AdaaMobile.Controls
{
    public class ListViewWrapper : ListView
    {
        private Size _firstViewSize;
        private RelativeLayout _parent;
        public ListViewWrapper()
        {
            //var relativeLayout = new RelativeLayout();
            Rotation = 270;
        }

        protected override void SetupContent(Cell content, int index)
        {
            var viewCell = (ViewCell)content;
            viewCell.View.Rotation = 90;
            var sizeRequest = viewCell.View.GetSizeRequest(double.PositiveInfinity, double.PositiveInfinity);
            if (_firstViewSize.IsZero)
            {
                _firstViewSize = sizeRequest.Request;
                UpdateConstraints();
            }
            base.SetupContent(content, index);
        }

        private void UpdateConstraints()
        {
            TranslationX = Width / 2 - _firstViewSize.Width;
            //TranslationY = -1 * Width  + _firstViewSize.Width / 2;
            //if (_parent != null)
            //{
            //    HeightRequest = _firstViewSize.Width +20;
            //}
        }

        protected override void OnParentSet()
        {
            base.OnParentSet();
            _parent = (RelativeLayout)Parent;
        }

        public override SizeRequest GetSizeRequest(double widthConstraint, double heightConstraint)
        {
            return base.GetSizeRequest(widthConstraint, heightConstraint);
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);
        }
    }
}
