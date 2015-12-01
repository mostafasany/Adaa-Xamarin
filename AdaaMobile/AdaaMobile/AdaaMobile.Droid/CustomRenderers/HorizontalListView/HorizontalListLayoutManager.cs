using Android.Content;
using Android.Support.V7.Widget;
using Android.Views;

namespace AdaaMobile.Droid.CustomRenderers.HorizontalListView
{
    //Ported from this Android Answer http://stackoverflow.com/a/28303955/2086772
    //It solves Wrap_Content issue for Recycler Viewer for vertical and horiztonal orientations.
    public class HorizontalListLayoutManager : LinearLayoutManager
    {

        public HorizontalListLayoutManager(Context context, int orientation, bool reverseLayout) :
            base(context, orientation, reverseLayout)
        {

        }

        private int[] _mMeasuredDimension = new int[2];

        public int GetMeasuredWidth()
        {
            return _mMeasuredDimension[0];
        }

        public int GetMeasuredHeight()
        {
            return _mMeasuredDimension[1];
        }

        public override void OnMeasure(RecyclerView.Recycler recycler, RecyclerView.State state,
                              int widthSpec, int heightSpec)
        {
            MeasureSpecMode widthMode = View.MeasureSpec.GetMode(widthSpec);
            MeasureSpecMode heightMode = View.MeasureSpec.GetMode(heightSpec);
            int widthSize = View.MeasureSpec.GetSize(widthSpec);
            int heightSize = View.MeasureSpec.GetSize(heightSpec);
            int width = 0;
            int height = 0;
            for (int i = 0; i < ItemCount; i++)
            {
                if (Orientation == Horizontal)
                {
                    MakeMeasureSpec(recycler, i,
                            View.MeasureSpec.MakeMeasureSpec(i, MeasureSpecMode.Unspecified),
                            heightSpec,
                            _mMeasuredDimension);

                    width = width + _mMeasuredDimension[0];
                    if (i == 0)
                    {
                        height = _mMeasuredDimension[1];
                    }
                }
                else
                {
                    MakeMeasureSpec(recycler, i,
                            widthSpec,
                            View.MeasureSpec.MakeMeasureSpec(i, MeasureSpecMode.Unspecified),
                            _mMeasuredDimension);
                    height = height + _mMeasuredDimension[1];
                    if (i == 0)
                    {
                        width = _mMeasuredDimension[0];
                    }
                }
            }
            switch (widthMode)
            {
                case MeasureSpecMode.Exactly:
                    width = widthSize;
                    break;
                case MeasureSpecMode.AtMost:
                case MeasureSpecMode.Unspecified:
                    break;
            }

            switch (heightMode)
            {
                case MeasureSpecMode.Exactly:
                    height = heightSize;
                    break;
                case MeasureSpecMode.AtMost:
                case MeasureSpecMode.Unspecified:
                    break;
            }

            SetMeasuredDimension(width, height);

        }

        private void MakeMeasureSpec(RecyclerView.Recycler recycler, int position, int widthSpec,
                                   int heightSpec, int[] measuredDimension)
        {
            View view = recycler.GetViewForPosition(position);
            recycler.BindViewToPosition(view, position);
            if (view != null)
            {
                RecyclerView.LayoutParams p = (RecyclerView.LayoutParams)view.LayoutParameters;
                int childWidthSpec = ViewGroup.GetChildMeasureSpec(widthSpec,
                        PaddingLeft + PaddingRight, p.Width);
                int childHeightSpec = ViewGroup.GetChildMeasureSpec(heightSpec,
                        PaddingTop + PaddingBottom, p.Height);
                view.Measure(childWidthSpec, childHeightSpec);
                measuredDimension[0] = view.MeasuredWidth + p.LeftMargin + p.RightMargin;
                measuredDimension[1] = view.MeasuredHeight + p.BottomMargin + p.TopMargin;
                recycler.RecycleView(view);
            }
        }
    }
}