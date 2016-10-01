using System;
using Xamarin.Forms;
using AdaaMobile.ViewModels;
using AdaaMobile.Strings;
using System.Collections.Generic;
using AdaaMobile.Models.Response.ServiceDesk;
using AdaaMobile.Controls;
using System.Threading.Tasks;

namespace AdaaMobile
{
    public partial class ServiceDeskLogIncident : ContentPage
    {
        public ServiceDeskLogIncident()
        {
            InitializeComponent();
            NavigationPage.SetBackButtonTitle(this, "");

            _viewModel = Locator.Default.ServiceDeskLogIncidentViewModel;
            BindingContext = _viewModel;
            Title = AppResources.ServiceDesk_LogAnIncident;

            HandleArabicLanguageFlowDirection();
            OnBelHalfPicker.SelectedIndexChanged += OnBelHalfPicker_SelectionIndexChanged;
            ParentCategoriesPicker.SelectedIndexChanged += ParentCategoriesPicker_SelectionIndexChanged;
            ParentChildCategoriesPicker.SelectedIndexChanged += ParentChildCategoriesPicker_SelectionIndexChanged;
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            //_viewModel.PageLoadedCommand.Execute(null);
            await LoadOnBelhaf();
            await LoadParentCategories();
        }

        void HandleArabicLanguageFlowDirection()
        {
            if (Locator.Default.AppSettings.SelectedCultureName.Contains("ar"))
            {
                lblOnBelHalf.HorizontalOptions = LayoutOptions.End;
                lblOnBelHalfResult.HorizontalOptions = LayoutOptions.End;
                lblParentCategories.HorizontalOptions = LayoutOptions.End;
                lblParentCategoriesResult.HorizontalOptions = LayoutOptions.End;
                Grid.SetColumn(imageParentCategories, 0);
                imageParentCategories.RotationY = 180;
                Grid.SetColumn(imageParentChild, 0);
                imageParentChild.RotationY = 180;
            }
        }

        #region Members

        string moduleName = "Incident%20Classification";
        private readonly ServiceDeskLogIncidentViewModel _viewModel;
        List<ParentCategory> ParentCategoryList;
        List<ChildCategory> ChildCategoryList;

        #endregion

        #region OnBelHalf

        private async Task LoadOnBelhaf()
        {
            loadingControl.IsRunning = true;
            lblOnBelHalfResult.Text = AppResources.ServcieDesk_SelectName;
            var response = await Locator.Default.DataService.GetOnBelfUsers();
            loadingControl.IsRunning = false;
            if (response != null && response.ResponseStatus == AdaaMobile.DataServices.Requests.ResponseStatus.SuccessWithResult)
            {
                OnBelHalfPicker.Items.Clear();
                for (int i = 0; i < response.Result.result.Count; i++)
                {
                    OnBelHalfPicker.Items.Add(response.Result.result[i].DisplayName);
                }
            }
        }

        private void OnBelHalf_OnClicked(object sender, EventArgs e)
        {
            OnBelHalfPicker.Unfocus();
            OnBelHalfPicker.Focus();
        }

        private void OnBelHalfPicker_SelectionIndexChanged(object sender, EventArgs e)
        {
            if (sender != null && sender is Picker)
            {
                var picker = sender as Picker;
                lblOnBelHalfResult.Text = OnBelHalfPicker.Items[picker.SelectedIndex];
            }
        }

        #endregion

        #region ParentCategories

        private async Task LoadParentCategories()
        {
            loadingControl.IsRunning = true;
            lblParentCategoriesResult.Text = AppResources.ServcieDesk_SelectCategory;
            var response = await Locator.Default.DataService.GetParentCategories(moduleName);
            loadingControl.IsRunning = false;
            if (response != null && response.ResponseStatus == AdaaMobile.DataServices.Requests.ResponseStatus.SuccessWithResult)
            {
                ParentCategoryList = response.Result.result;
                ParentCategoriesPicker.Items.Clear();
                for (int i = 0; i < response.Result.result.Count; i++)
                {
                    ParentCategoriesPicker.Items.Add(response.Result.result[i].name);
                }
            }
        }

        private void ParentCategories_OnClicked(object sender, EventArgs e)
        {
            ParentCategoriesPicker.Unfocus();
            ParentCategoriesPicker.Focus();
        }

        private void ParentCategoriesPicker_SelectionIndexChanged(object sender, EventArgs e)
        {
            if (sender != null && sender is Picker)
            {
                var picker = sender as Picker;
                var item = ParentCategoryList[picker.SelectedIndex];
                lblParentCategoriesResult.Text = item.name;
                LoadParentChildCategories(item.parent);
            }
        }

        #endregion

        #region ParentChildCategories

        private async void LoadParentChildCategories(string parentId)
        {
            loadingControl.IsRunning = true;
            lblParentChildCategoriesResult.Text = AppResources.ServcieDesk_SelectSubCategory;
            var response = await Locator.Default.DataService.GetChildCategories(moduleName, parentId);
            loadingControl.IsRunning = false;
            if (response != null && response.ResponseStatus == AdaaMobile.DataServices.Requests.ResponseStatus.SuccessWithResult)
            {
                ChildCategoryList = response.Result.result;
                ParentChildCategoriesPicker.Items.Clear();
                for (int i = 0; i < response.Result.result.Count; i++)
                {
                    ParentChildCategoriesPicker.Items.Add(response.Result.result[i].name);
                }
            }
        }

        private void ParentChildCategories_OnClicked(object sender, EventArgs e)
        {
            ParentChildCategoriesPicker.Unfocus();
            ParentChildCategoriesPicker.Focus();
        }

        private void ParentChildCategoriesPicker_SelectionIndexChanged(object sender, EventArgs e)
        {
            if (sender != null && sender is Picker)
            {
                var picker = sender as Picker;
                var item = ChildCategoryList[picker.SelectedIndex];
                lblParentChildCategoriesResult.Text = item.name;
                LoadTemplates(item.id);
            }
        }

        #endregion

        #region Template

        async void LoadTemplates(string childCategroyId)
        {
            var response = await Locator.Default.DataService.GetTemplateId(childCategroyId);
            loadingControl.IsRunning = false;
            if (response != null && response.ResponseStatus == AdaaMobile.DataServices.Requests.ResponseStatus.SuccessWithResult)
            {
                var template = response.Result.result[0];
                LoadExtensions(template.TemplateID);
            }
        }

        #endregion

        #region Extensions

        async void LoadExtensions(string templateId)
        {
            customControls.Children.Clear();
            var response = await Locator.Default.DataService.GetTemplateExtension(templateId);
            loadingControl.IsRunning = false;
            if (response != null && response.ResponseStatus == AdaaMobile.DataServices.Requests.ResponseStatus.SuccessWithResult)
            {
                var extensions = response.Result.Table1;
                foreach (var extension in extensions)
                {
                    await Locator.Default.DialogManager.DisplayAlert(AppResources.ApplicationName, extension.Type, AppResources.Ok);
                    switch (extension.Type)
                    {
                        case "datetime":
                            RenderDateTime(extension);
                            break;
                        case "string":
                            RenderString(extension);
                            break;
                        case "bool":
                            RenderBool(extension);
                            break;
                        case "enum":
                            RenderEnum(extension);
                            break;
                        case "int":
                            RenderInt(extension);
                            break;
                        case "double":
                            RenderDouble(extension);
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        void RenderDateTime(TemplateExtension extension)
        {
            Label dateLabel = new Label
            {
                Text = extension.DisplayName,
                HorizontalOptions = LayoutOptions.StartAndExpand,
                TextColor = Xamarin.Forms.Color.Gray,
                BackgroundColor = Xamarin.Forms.Color.Transparent,
                HorizontalTextAlignment = TextAlignment.Start,
                FontSize = 20,
            };
            customControls.Children.Add(dateLabel);
            DatePicker datePicker = new DatePicker
            {
                Format = "d MMM yyyy",
                HorizontalOptions = LayoutOptions.Fill,
                MinimumDate = DateTime.Now,
                BackgroundColor = Xamarin.Forms.Color.Transparent,
                VerticalOptions = LayoutOptions.FillAndExpand,
            };
            customControls.Children.Add(datePicker);
        }

        void RenderString(TemplateExtension extension)
        {
            Label stringLabel = new Label
            {
                Text = extension.DisplayName,
                HorizontalOptions = LayoutOptions.StartAndExpand,
                TextColor = Xamarin.Forms.Color.Gray,
                BackgroundColor = Xamarin.Forms.Color.Transparent,
                HorizontalTextAlignment = TextAlignment.Start,
                FontSize = 20,
            };
            customControls.Children.Add(stringLabel);
            ExtendedEditor stringEditor = new ExtendedEditor
            {
                HorizontalOptions = LayoutOptions.Fill,
                TextColor = Xamarin.Forms.Color.Black,
                BackgroundColor = Xamarin.Forms.Color.Transparent,
                VerticalOptions = LayoutOptions.FillAndExpand,
                MaxLength = 60,
            };
            customControls.Children.Add(stringEditor);
        }

        void RenderDouble(TemplateExtension extension)
        {
            Label doubleLabel = new Label
            {
                Text = extension.DisplayName,
                HorizontalOptions = LayoutOptions.StartAndExpand,
                TextColor = Xamarin.Forms.Color.Gray,
                BackgroundColor = Xamarin.Forms.Color.Transparent,
                HorizontalTextAlignment = TextAlignment.Start,
                FontSize = 20,
            };
            customControls.Children.Add(doubleLabel);
            ExtendedEditor doubleEditor = new ExtendedEditor
            {
                HorizontalOptions = LayoutOptions.Fill,
                TextColor = Xamarin.Forms.Color.Black,
                BackgroundColor = Xamarin.Forms.Color.Transparent,
                VerticalOptions = LayoutOptions.FillAndExpand,
                MaxLength = 60,
            };
            customControls.Children.Add(doubleEditor);
        }

        void RenderInt(TemplateExtension extension)
        {
            Label intLabel = new Label
            {
                Text = extension.DisplayName,
                HorizontalOptions = LayoutOptions.StartAndExpand,
                TextColor = Xamarin.Forms.Color.Gray,
                BackgroundColor = Xamarin.Forms.Color.Transparent,
                HorizontalTextAlignment = TextAlignment.Start,
                FontSize = 20,
            };
            customControls.Children.Add(intLabel);
            ExtendedEditor intEditor = new ExtendedEditor
            {
                HorizontalOptions = LayoutOptions.Fill,
                TextColor = Xamarin.Forms.Color.Black,
                BackgroundColor = Xamarin.Forms.Color.Transparent,
                VerticalOptions = LayoutOptions.FillAndExpand,
                MaxLength = 60,
            };
            customControls.Children.Add(intEditor);
        }

        void RenderBool(TemplateExtension extension)
        {
            Label boolLabel = new Label
            {
                Text = extension.DisplayName,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Start,
                TextColor = Xamarin.Forms.Color.Gray,
                BackgroundColor = Xamarin.Forms.Color.Transparent,
                HorizontalTextAlignment = TextAlignment.Start,
                FontSize = 20,
            };
            customControls.Children.Add(boolLabel);
            Switch switchToggle = new Switch
            {
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.End,
                IsToggled = false,
            };
            customControls.Children.Add(switchToggle);
        }

        void RenderEnum(TemplateExtension extension)
        {

        }

        #endregion

        private void ShowPassword_Toggled(object sender, ToggledEventArgs e)
        {
        }

        private void LogIncident()
        {
        }
    }
}