using System;
using System.Collections.Generic;
using System.Text;
using CoreGraphics;
using Foundation;
using UIKit;

namespace AdaaMobile.iOS.CustomRenderers.HorizontalListView
{
    //Source from https://github.com/XLabs/Xamarin-Forms-Labs

    public class HorizontalCollectionView : UICollectionView
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="GridCollectionView"/> class.
        /// </summary>
        public HorizontalCollectionView() : this(default(CGRect), new UICollectionViewFlowLayout())
        {
        }


        public HorizontalCollectionView(CGRect frame, UICollectionViewLayout layout) : base(frame, layout)
        {

            AutoresizingMask = UIViewAutoresizing.All;
            ContentMode = UIViewContentMode.ScaleToFill;
            RegisterClassForCell(typeof(HorizontalListCell), new NSString(HorizontalListCell.Key));
        }

        /// <summary>
        /// Gets or sets a value indicating whether [selection enable].
        /// </summary>
        /// <value><c>true</c> if [selection enable]; otherwise, <c>false</c>.</value>
        public bool SelectionEnable
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the row spacing.
        /// </summary>
        /// <value>The row spacing.</value>
        public double RowSpacing
        {
            get
            {
                return ((UICollectionViewFlowLayout)this.CollectionViewLayout).MinimumLineSpacing;
            }
            set
            {
                ((UICollectionViewFlowLayout)this.CollectionViewLayout).MinimumLineSpacing = (nfloat)value;
            }
        }

        /// <summary>
        /// Gets or sets the column spacing.
        /// </summary>
        /// <value>The column spacing.</value>
        public double ColumnSpacing
        {
            get
            {
                return ((UICollectionViewFlowLayout)this.CollectionViewLayout).MinimumInteritemSpacing;
            }
            set
            {
                ((UICollectionViewFlowLayout)this.CollectionViewLayout).MinimumInteritemSpacing = (nfloat)value;
            }
        }

        /// <summary>
        /// Gets or sets the size of the item.
        /// </summary>
        /// <value>The size of the item.</value>
        public CGSize ItemSize
        {
            get
            {
                return ((UICollectionViewFlowLayout)this.CollectionViewLayout).ItemSize;
            }
            set
            {
                ((UICollectionViewFlowLayout)this.CollectionViewLayout).ItemSize = value;
            }
        }

        /// <summary>
        /// Cells for item.
        /// </summary>
        /// <param name="indexPath">The index path.</param>
        /// <returns>UICollectionViewCell.</returns>
        public override UICollectionViewCell CellForItem(NSIndexPath indexPath)
        {
            if (indexPath == null)
            {
                //calling base.CellForItem(indexPath) when indexPath is null causes an exception.
                //indexPath could be null in the following scenario:
                // - GridView is configured to show 2 cells per row and there are 3 items in ItemsSource collection
                // - you're trying to drag 4th cell (empty) like you're trying to scroll
                return null;
            }
            return base.CellForItem(indexPath);
        }

        /// <summary>
        /// Draws the specified rect.
        /// </summary>
        /// <param name="rect">The rect.</param>
        public override void Draw(CGRect rect)
        {
            this.CollectionViewLayout.InvalidateLayout();

            base.Draw(rect);
        }
    }
}
