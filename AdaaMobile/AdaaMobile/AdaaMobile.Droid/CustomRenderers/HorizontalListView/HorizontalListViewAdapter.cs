using Android.Support.V7.Widget;
using Android.Views;

namespace AdaaMobile.Droid.CustomRenderers.HorizontalListView
{
    //Snippet adapted from  https://github.com/XLabs/Xamarin-Forms-Labs/blob/master/src/Forms/XLabs.Forms.Droid/Controls/GridView/GridViewRenderer.cs

    /// <summary>
    /// Adapter for Recyclerview, It uses delegate methods to pass functionalities.
    /// </summary>
    public class HorizontalListViewAdapter : RecyclerView.Adapter
    {
        //Delegates
        public delegate void OnBindViewHolderDelegate(RecyclerView.ViewHolder holder, int position);

        public delegate RecyclerView.ViewHolder OnCreateViewHolderDelegate(ViewGroup parent, int viewType);

        public delegate int GetItemsCount();

        //Fields
        private readonly OnBindViewHolderDelegate _bindDelegate;
        private readonly OnCreateViewHolderDelegate _createDelegate;
        private readonly GetItemsCount _itemsCountDelegate;

        public HorizontalListViewAdapter(OnBindViewHolderDelegate bindDelegate, OnCreateViewHolderDelegate createDelegate, GetItemsCount itemsCountDelegate) : base()
        {
            _bindDelegate = bindDelegate;
            _createDelegate = createDelegate;
            _itemsCountDelegate = itemsCountDelegate;
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            _bindDelegate(holder, position);
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            return _createDelegate(parent, viewType);
        }


        public override int ItemCount
        {
            get { return _itemsCountDelegate(); }
        }
    }
}