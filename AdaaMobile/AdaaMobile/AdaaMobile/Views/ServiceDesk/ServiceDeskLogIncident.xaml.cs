﻿using System;
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

        private async void LoadParentCategories()
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

        private async void ParentCategoriesPicker_SelectionIndexChanged(object sender, EventArgs e)
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

        private async void ParentChildCategoriesPicker_SelectionIndexChanged(object sender, EventArgs e)
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
            // await Locator.Default.DialogManager.DisplayAlert(AppResources.ApplicationName, childCategroyId, AppResources.Ok);
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
            var response = await Locator.Default.DataService.GetTemplateExtension(templateId);
            loadingControl.IsRunning = false;
            if (response != null && response.ResponseStatus == AdaaMobile.DataServices.Requests.ResponseStatus.SuccessWithResult)
            {
                var extensions = response.Result.Table1;
                foreach (var extension in extensions)
                {
                    switch (extension.Type)
                    {
                        case "datetime":

                        case "string":

                        case "bool":

                        case "enum":

                        case "int":

                        case "double":

                        default:
                            break;
                    }
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