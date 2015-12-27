using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace AdaaMobile.Controls
{
    public class GestureContentView : ContentView
    {
        public GestureContentView()
        {

            var tapRecognizer = new TapGestureRecognizer();
            tapRecognizer.Tapped += TapRecognizer_Tapped;
            GestureRecognizers.Add(tapRecognizer);
        }

        private void TapRecognizer_Tapped(object sender, EventArgs e)
        {
            OnTapped(new TappedEventArgs(TappedCommandParameter));
        }

        #region Tapped Command
        public static readonly BindableProperty TappedCommandProperty = BindableProperty.Create<GestureContentView, ICommand>(p => p.TappedCommand, default(ICommand));

        public ICommand TappedCommand
        {
            get { return (ICommand)GetValue(TappedCommandProperty); }
            set { SetValue(TappedCommandProperty, value); }
        }
        #endregion

        #region Tapped Command Parameter
        public static readonly BindableProperty TappedCommandParameterProperty = BindableProperty.Create<GestureContentView, object>(p => p.TappedCommandParameter, default(object));

        public object TappedCommandParameter
        {
            get { return (object)GetValue(TappedCommandParameterProperty); }
            set { SetValue(TappedCommandParameterProperty, value); }
        }
        #endregion

        #region Tap

        public event EventHandler<TappedEventArgs> Tapped;

        #endregion

        protected virtual void OnTapped(TappedEventArgs e)
        {

            var handler = Tapped;
            if (handler != null) handler(this, e);

            if (TappedCommand != null)
            {
                TappedCommand.Execute(TappedCommandParameter);
            }
        }
    }
}
