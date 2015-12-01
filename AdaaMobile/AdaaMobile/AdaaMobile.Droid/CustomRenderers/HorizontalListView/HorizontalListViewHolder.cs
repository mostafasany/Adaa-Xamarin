using System;
using AdaaMobile.Controls;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Xamarin.Forms.Platform.Android;

namespace AdaaMobile.Droid.CustomRenderers.HorizontalListView
{
    /// <summary>
    /// Custom view holder for views of Recylcerview
    /// </summary>
    public class HorizontalListViewHolder : RecyclerView.ViewHolder
    {
        public IVisualElementRenderer Renderer { get; set; }

        public Action<HorizontaListItemTappedEventArgs> ItemTapped { get; set; }

        public HorizontalListViewHolder(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        public HorizontalListViewHolder(View itemView) : base(itemView)
        {
        }

        public HorizontalListViewHolder(ViewGroup itemView, IVisualElementRenderer rendererBinded, Action<HorizontaListItemTappedEventArgs> itemTapped) : base(itemView)
        {
            Renderer = rendererBinded;
            ItemTapped = itemTapped;
            //wire click event, 
            EventHandler handler = null;
            handler = (sender, args) => { itemTapped(new HorizontaListItemTappedEventArgs((Xamarin.Forms.View)Renderer.Element, Renderer.Element.BindingContext)); };
            ItemView.Click -= handler; //remove handler to prevent multiple supscription, (It's a little bit defensive)
            itemView.Click += handler;
        }
    }
}