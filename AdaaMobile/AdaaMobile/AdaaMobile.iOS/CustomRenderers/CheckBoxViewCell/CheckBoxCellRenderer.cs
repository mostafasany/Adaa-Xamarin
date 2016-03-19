//Source https://github.com/XLabs/Xamarin-Forms-Labs/tree/master/src/Forms

using AdaaMobile.Controls;
using AdaaMobile.iOS.CustomRenderers.CheckBoxViewCell;
using AdaaMobile.iOS.CustomRenderers.ExtendedTextCell;
using UIKit;
using Xamarin.Forms;

[assembly: ExportRenderer(typeof(CheckboxCell), typeof(CheckBoxCellRenderer))]

namespace AdaaMobile.iOS.CustomRenderers.CheckBoxViewCell
{
    /// <summary>
    /// Class CheckBoxCellRenderer.
    /// </summary>
    public class CheckBoxCellRenderer : ExtendedTextCellRenderer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CheckBoxCellRenderer"/> class.
        /// </summary>
        public CheckBoxCellRenderer ()
        {
        }

        /// <summary>
        /// Gets the cell.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="reusableCell">The reusable cell.</param>
        /// <param name="tv">The table view.</param>
        /// <returns>UITableViewCell.</returns>
        public override UITableViewCell GetCell(Cell item, UITableViewCell reusableCell, UITableView tv)
        {
            var viewCell = item as CheckboxCell;
            var nativeCell = base.GetCell(item, reusableCell, tv);
            nativeCell.SelectionStyle = UITableViewCellSelectionStyle.None;

            if (viewCell == null) return nativeCell;

            nativeCell.Accessory = viewCell.Checked ? UITableViewCellAccessory.Checkmark : UITableViewCellAccessory.None;

            viewCell.CheckedChanged += (s, e) => tv.ReloadData();

            return nativeCell;
        }

    }
}

