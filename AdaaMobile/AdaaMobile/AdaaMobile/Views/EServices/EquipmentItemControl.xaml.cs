using System;
using System.Reflection;
using System.Windows.Input;
using Xamarin.Forms;

namespace AdaaMobile.Views.MasterView
{
    public partial class EquipmentItemControl : ContentView
    {
        public EquipmentItemControl()
        {
            InitializeComponent();
			if (Device.OS == TargetPlatform.iOS) {
				MainBtn.BorderRadius = 10;
			}
        }

        #region Title p


        public static readonly BindableProperty TitleProperty = BindableProperty.Create<EquipmentItemControl, object>(p => p.Title, default(object));
        
        public object Title
        {
            get { return (object)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }
        #endregion

        #region IsSelected
        public static readonly BindableProperty IsSelectedProperty = BindableProperty.Create<EquipmentItemControl, bool>(p => p.IsSelected, default(bool));

        public bool IsSelected
        {
            get { return (bool)GetValue(IsSelectedProperty); }
            set { SetValue(IsSelectedProperty, value); }
        }
        #endregion

        #region Tapped Event 
        public event EventHandler Tapped;

        protected virtual void OnTapped()
        {
            var handler = Tapped;
            if (handler != null) handler(this, EventArgs.Empty);
        }
        #endregion

        #region Command
        public static readonly BindableProperty CommandProperty = BindableProperty.Create<UserServiceItemControl, ICommand>(p => p.Command, default(ICommand));

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }
        #endregion

        #region Command Parameter
        public static readonly BindableProperty CommandParamterProperty = BindableProperty.Create<UserServiceItemControl, object>(p => p.CommandParamter, default(object));

        public object CommandParamter
        {
            get { return (object)GetValue(CommandParamterProperty); }
            set { SetValue(CommandParamterProperty, value); }
        }
        #endregion

        /// <summary>
        /// This will listen for button tapped in the underline button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_OnClicked(object sender, EventArgs e)
        {
            OnTapped();

            if (Command != null)
            {
                if (Command.CanExecute(CommandParamter))
                {
                    Command.Execute(CommandParamter);
                }
            }
        }
    }
}
