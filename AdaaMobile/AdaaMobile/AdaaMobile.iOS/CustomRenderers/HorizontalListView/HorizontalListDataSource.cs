using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AdaaMobile.Controls;
using Foundation;
using UIKit;

namespace AdaaMobile.iOS.CustomRenderers.HorizontalListView
{
    //Snippet adapted from  https://github.com/XLabs/Xamarin-Forms-Labs/blob/master/src/Forms/XLabs.Forms.iOS/Controls/GridView/GridDataSource.cs
    public class HorizontalListDataSource : UICollectionViewSource
    {
        //Delegate Definitions

        /// <summary>
        /// Delegate OnGetCell
        /// </summary>
        /// <param name="collectionView">The collection view.</param>
        /// <param name="indexPath">The index path.</param>
        /// <returns>UICollectionViewCell.</returns>
        public delegate UICollectionViewCell OnGetCellDelegate(UICollectionView collectionView, NSIndexPath indexPath);

        /// <summary>
        /// Delegate which returns items count.
        /// </summary>
        /// <returns></returns>
        public delegate nint GetItemsCountDelegate(UICollectionView collectionView, nint section);

        /// <summary>
        /// Action to be used to Raise Item Tapped.
        /// </summary>
        public delegate void ItemTappedDelgate(UICollectionView collectionView, NSIndexPath indexPath);

        //Fields
        private readonly OnGetCellDelegate _cellDelegate;
        private readonly ItemTappedDelgate _itemsTappedDelgate;
        private readonly GetItemsCountDelegate _itemsCountDelegate;

        public HorizontalListDataSource(OnGetCellDelegate cellDelegate, GetItemsCountDelegate itemsCountDelegate, ItemTappedDelgate itemsTappedDelgate)
        {
            _cellDelegate = cellDelegate;
            _itemsTappedDelgate = itemsTappedDelgate;
            _itemsCountDelegate = itemsCountDelegate;
        }

        public override UICollectionViewCell GetCell(UICollectionView collectionView, NSIndexPath indexPath)
        {

            var cell = _cellDelegate(collectionView, indexPath);

            //Wire Tap Event
            var recognizer = new UITapGestureRecognizer(v =>
            {
                _itemsTappedDelgate(collectionView, indexPath);
            });
            var foundRecognizer = cell.GestureRecognizers.FirstOrDefault((r) => r.GetType() == recognizer.GetType());
            if (foundRecognizer != null)
                cell.RemoveGestureRecognizer(foundRecognizer);
            cell.AddGestureRecognizer(recognizer);

            return cell;
        }

        public override nint GetItemsCount(UICollectionView collectionView, nint section)
        {
            return _itemsCountDelegate(collectionView, section);
        }
    }
}
