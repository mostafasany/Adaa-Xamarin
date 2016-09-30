using System;
using Xamarin.Forms;
using AdaaMobile.ViewModels;
using AdaaMobile.Strings;
using System.Collections.Generic;
using AdaaMobile.Models.Response.ServiceDesk;

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

            LoadOnBelhaf();
            LoadParentCategories();
            HandleArabicLanguageFlowDirection();
            OnBelHalfPicker.SelectedIndexChanged += OnBelHalfPicker_SelectionIndexChanged;
            ParentCategoriesPicker.SelectedIndexChanged += ParentCategoriesPicker_SelectionIndexChanged;
            ParentChildCategoriesPicker.SelectedIndexChanged += ParentChildCategoriesPicker_SelectionIndexChanged;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.PageLoadedCommand.Execute(null);
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

        private async void LoadOnBelhaf()
        {
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

        private async void LoadParentCategories()
        {
            lblParentCategoriesResult.Text = AppResources.ServcieDesk_SelectCategory;
            var response = await Locator.Default.DataService.GetParentCategories(moduleName);
            loadingControl.IsRunning = false;
            if (response != null && response.ResponseStatus == AdaaMobile.DataServices.Requests.ResponseStatus.SuccessWithResult)
            {
                await Locator.Default.DialogManager.DisplayAlert(AppResources.ApplicationName, response.Result.result.Count.ToString(), AppResources.Ok);
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
                LoadParentChildCategories(item.id);
            }
        }

        #endregion

        #region ParentChildCategories

        private async void LoadParentChildCategories(string parentId)
        {
            lblParentChildCategoriesResult.Text = AppResources.ServcieDesk_SelectSubCategory;
            var response = await Locator.Default.DataService.GetChildCategories(moduleName, parentId);
            if (response != null && response.ResponseStatus == AdaaMobile.DataServices.Requests.ResponseStatus.SuccessWithResult)
            {
                await Locator.Default.DialogManager.DisplayAlert(AppResources.ApplicationName, response.Result.result.Count.ToString(), AppResources.Ok);
                ChildCategoryList = response.Result.result;
                ParentChildCategoriesPicker.Items.Clear();
                for (int i = 0; i < response.Result.result.Count; i++)
                {
                    ParentChildCategoriesPicker.Items.Add(response.Result.result[i].name);
                }
            }
            loadingControl.IsRunning = false;
        }

        private void ParentChildCategories_OnClicked(object sender, EventArgs e)
        {
            ParentChildCategoriesPicker.Unfocus();
            ParentChildCategoriesPicker.Focus();
        }

        private async void ParentChildCategoriesPicker_SelectionIndexChanged(object sender, EventArgs e)
        {
            if (sender != null && sender is Picker)
            {
                var picker = sender as Picker;
                var item = ChildCategoryList[picker.SelectedIndex];
                lblParentChildCategoriesResult.Text = item.name;
                var response = await Locator.Default.DataService.GetChildCategories(moduleName, item.id);
                if (response != null && response.ResponseStatus == AdaaMobile.DataServices.Requests.ResponseStatus.SuccessWithResult)
                {
                    ChildCategoryList = response.Result.result;
                }
            }
        }

        #endregion

        private void ShowPassword_Toggled(object sender, ToggledEventArgs e)
        {
        }

        private async void LogIncident()
        {
        }
    }
}