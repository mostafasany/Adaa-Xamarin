using System;
using Xamarin.Forms;
using AdaaMobile.ViewModels;
using AdaaMobile.Strings;
using System.Collections.Generic;
using AdaaMobile.Models.Response.ServiceDesk;
using AdaaMobile.Controls;
using System.Threading.Tasks;
using System.Linq;
using Xamarin.Forms.Labs.Services.Media;
using AdaaMobile.Models.Request;
using AdaaMobile.DataServices.Requests;

namespace AdaaMobile
{
    public class RenderdControl
    {
        public TemplateExtension TemplateExtension { get; set; }
        public string Value { get; set; }

    }
    public partial class ServiceDeskLogIncident : ContentPage
    {
        public ServiceDeskLogIncident()
        {
            InitializeComponent();
            NavigationPage.SetBackButtonTitle(this, "");

            _viewModel = Locator.Default.ServiceDeskLogIncidentViewModel;
            BindingContext = _viewModel;
            Title = AppResources.ServiceDesk_RequestITService;

            //HandleArabicLanguageFlowDirection();
            OnBelHalfPicker.SelectedIndexChanged += OnBelHalfPicker_SelectionIndexChanged;
            ParentCategoriesPicker.SelectedIndexChanged += ParentCategoriesPicker_SelectionIndexChanged;
            ParentChildCategoriesPicker.SelectedIndexChanged += ParentChildCategoriesPicker_SelectionIndexChanged;

            Action action = () =>
            {
                LogIncident();
            };
            ToolbarItems.Add(
                new ToolbarItem("", "right.png", action, ToolbarItemOrder.Primary));

        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            //_viewModel.PageLoadedCommand.Execute(null);
            await LoadOnBelhaf();
            await LoadParentCategories();
            if (!string.IsNullOrEmpty(Locator.Default.ServiceDeskHomeViewModel.Module))
            {
                moduleName = Locator.Default.ServiceDeskHomeViewModel.Module;
            }

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
        // string moduleName = "Service%20request%20area";
        private readonly ServiceDeskLogIncidentViewModel _viewModel;
        string SelectedOnBehalf;
        List<ParentCategory> ParentCategoryList;
        ParentCategory SelectedParentCategory;
        List<ChildCategory> ChildCategoryList;
        ChildCategory SelectedChildCategory;
        CategoryTemplate SelectedTemplate;
        List<RenderdControl> RenderdControlList;

        #endregion

        #region OnBelHalf

        private async Task LoadOnBelhaf()
        {
            try
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
            catch (Exception ex)
            {
            }
        }

        private void OnBelHalf_OnClicked(object sender, EventArgs e)
        {
            OnBelHalfPicker.Unfocus();
            OnBelHalfPicker.Focus();
        }

        private void OnBelHalfPicker_SelectionIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (sender != null && sender is Picker)
                {
                    var picker = sender as Picker;
                    SelectedOnBehalf = OnBelHalfPicker.Items[picker.SelectedIndex];
                    lblOnBelHalfResult.Text = SelectedOnBehalf;
                }
            }
            catch (Exception ex)
            {
            }
        }

        #endregion

        #region ParentCategories

        private async Task LoadParentCategories()
        {
            try
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
            catch (Exception ex)
            {
            }
        }

        private void ParentCategories_OnClicked(object sender, EventArgs e)
        {
            ParentCategoriesPicker.Unfocus();
            ParentCategoriesPicker.Focus();
        }

        private void ParentCategoriesPicker_SelectionIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (sender != null && sender is Picker)
                {
                    ParentChildCategoriesPickerGrid.IsVisible = false;
                    customControls.IsVisible = false;
                    var picker = sender as Picker;
                    SelectedParentCategory = ParentCategoryList[picker.SelectedIndex];
                    lblParentCategoriesResult.Text = SelectedParentCategory.name;
					LoadParentChildCategories(SelectedParentCategory.id);
                }
            }
            catch (Exception ex)
            {
            }
        }

        #endregion

        #region ParentChildCategories

        private async void LoadParentChildCategories(string parentId)
        {
            try
            {
                loadingControl.IsRunning = true;
                lblParentChildCategoriesResult.Text = AppResources.ServcieDesk_SelectSubCategory;
                var response = await Locator.Default.DataService.GetChildCategories(moduleName, parentId);
                loadingControl.IsRunning = false;
                if (response != null && response.ResponseStatus == AdaaMobile.DataServices.Requests.ResponseStatus.SuccessWithResult)
                {
                    if (response.Result.result != null && response.Result.result.Count() > 0)
                    {
                        ParentChildCategoriesPickerGrid.IsVisible = true;
                        ChildCategoryList = response.Result.result;
                        ParentChildCategoriesPicker.Items.Clear();
                        for (int i = 0; i < response.Result.result.Count; i++)
                        {
                            ParentChildCategoriesPicker.Items.Add(response.Result.result[i].name);
                        }
                    }
                    else
                    {
                        ParentChildCategoriesPickerGrid.IsVisible = false;
                        customControls.IsVisible = false;
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void ParentChildCategories_OnClicked(object sender, EventArgs e)
        {
            ParentChildCategoriesPicker.Unfocus();
            ParentChildCategoriesPicker.Focus();
        }

        private void ParentChildCategoriesPicker_SelectionIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (sender != null && sender is Picker)
                {
                    var picker = sender as Picker;
                    SelectedChildCategory = ChildCategoryList[picker.SelectedIndex];
                    lblParentChildCategoriesResult.Text = SelectedChildCategory.name;
                    LoadTemplates(SelectedChildCategory.id);
                }
            }
            catch (Exception ex)
            {
            }
        }

        #endregion

        #region Template
        StackLayout customControls2;
        async void LoadTemplates(string childCategroyId)
        {
            try
            {
                RenderdControlList = new List<RenderdControl>();
                int count = customControls.Children.Count;
                if (count > 0)
                    customControls.Children.RemoveAt(0);
            }
            catch (Exception ex)
            {
            }
            try
            {
                customControls2 = new StackLayout
                {
                    Orientation = StackOrientation.Vertical
                };
                customControls.Children.Add(customControls2);
                var response = await Locator.Default.DataService.GetTemplateId(childCategroyId);
                loadingControl.IsRunning = false;
                if (response != null && response.ResponseStatus == AdaaMobile.DataServices.Requests.ResponseStatus.SuccessWithResult)
                {
                    if (response.Result.result != null && response.Result.result.Count() > 0)
                    {
                        SelectedTemplate = response.Result.result[0];
                        LoadExtensions(SelectedTemplate.TemplateID);
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        #endregion

        #region Extensions

        async void LoadExtensions(string templateId)
        {
            try
            {
                loadingControl.IsRunning = true;
                var response = await Locator.Default.DataService.GetTemplateExtension(templateId);
                loadingControl.IsRunning = false;
                if (response != null && response.ResponseStatus == AdaaMobile.DataServices.Requests.ResponseStatus.SuccessWithResult)
                {
                    customControls.IsVisible = true;
                    var extensions = response.Result.Table1;
                    foreach (var extension in extensions)
                    {
                        RenderdControlList.Add(new RenderdControl() { TemplateExtension = extension, Value = "" });
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
                                await Locator.Default.DialogManager.DisplayAlert(AppResources.ApplicationName, extension.Type, AppResources.Ok);
                                break;
                        }
                    }
                }
                loadingControl.IsRunning = false;
            }
            catch (Exception ex)
            {
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
            customControls2.Children.Add(dateLabel);
            DatePicker datePicker = new DatePicker
            {
                Format = "d MMM yyyy",
                HorizontalOptions = LayoutOptions.Fill,
                //MinimumDate = DateTime.Now,
                BackgroundColor = Xamarin.Forms.Color.Transparent,
                VerticalOptions = LayoutOptions.FillAndExpand,
                BindingContext = extension,
            };
            datePicker.DateSelected += datePicker_DateSelected;
            customControls2.Children.Add(datePicker);
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
            customControls2.Children.Add(stringLabel);
            ExtendedEditor stringEditor = new ExtendedEditor
            {
                Text = extension.DefaultValue != null ? extension.DefaultValue.ToString() : "",
                HorizontalOptions = LayoutOptions.Fill,
                TextColor = Xamarin.Forms.Color.Black,
                BackgroundColor = Xamarin.Forms.Color.Transparent,
                VerticalOptions = LayoutOptions.FillAndExpand,
                MaxLength = int.Parse(extension.MaxLength),
                Keyboard = Keyboard.Text,
                BindingContext = extension,
            };
            stringEditor.TextChanged += StringEditor_TextChanged;
            customControls2.Children.Add(stringEditor);
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
            customControls2.Children.Add(doubleLabel);
            ExtendedEditor doubleEditor = new ExtendedEditor
            {
                Text = extension.DefaultValue != null ? extension.DefaultValue.ToString() : "",
                HorizontalOptions = LayoutOptions.Fill,
                TextColor = Xamarin.Forms.Color.Black,
                BackgroundColor = Xamarin.Forms.Color.Transparent,
                VerticalOptions = LayoutOptions.FillAndExpand,
                MaxLength = int.Parse(extension.MaxLength),
                Keyboard = Keyboard.Numeric,
                BindingContext = extension,
            };
            doubleEditor.TextChanged += StringEditor_TextChanged;
            customControls2.Children.Add(doubleEditor);
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
            customControls2.Children.Add(intLabel);
            ExtendedEditor intEditor = new ExtendedEditor
            {
                Text = extension.DefaultValue != null ? extension.DefaultValue.ToString() : "",
                HorizontalOptions = LayoutOptions.Fill,
                TextColor = Xamarin.Forms.Color.Black,
                BackgroundColor = Xamarin.Forms.Color.Transparent,
                VerticalOptions = LayoutOptions.FillAndExpand,
                MaxLength = int.Parse(extension.MaxLength),
                Keyboard = Keyboard.Numeric,
                BindingContext = extension,
            };
            intEditor.TextChanged += StringEditor_TextChanged;
            customControls2.Children.Add(intEditor);
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
            customControls2.Children.Add(boolLabel);
            Switch switchToggle = new Switch
            {
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.End,
                IsToggled = false,
                BindingContext = extension,
            };
            switchToggle.Toggled += switchToggle_Toggled;
            customControls2.Children.Add(switchToggle);
        }

        void RenderEnum(TemplateExtension extension)
        {

        }

        private void StringEditor_TextChanged(object sender, TextChangedEventArgs e)
        {
            ExtendedEditor editor = sender as ExtendedEditor;
            var extensionTemplate = editor.BindingContext as TemplateExtension;
            var renderControl = RenderdControlList.FirstOrDefault(a => a.TemplateExtension.ID == extensionTemplate.ID);
            renderControl.Value = e.NewTextValue;
        }

        private void switchToggle_Toggled(object sender, ToggledEventArgs e)
        {
            Switch editor = sender as Switch;
            var extensionTemplate = editor.BindingContext as TemplateExtension;
            var renderControl = RenderdControlList.FirstOrDefault(a => a.TemplateExtension.ID == extensionTemplate.ID);
            renderControl.Value = e.Value.ToString();
        }

        private void datePicker_DateSelected(object sender, DateChangedEventArgs e)
        {
            DatePicker editor = sender as DatePicker;
            var extensionTemplate = editor.BindingContext as TemplateExtension;
            var renderControl = RenderdControlList.FirstOrDefault(a => a.TemplateExtension.ID == extensionTemplate.ID);
            renderControl.Value = e.NewDate.ToString();
        }

        #endregion

        private void ShowPassword_Toggled(object sender, ToggledEventArgs e)
        {
            if (e.Value)
            {
                onBehalfOfGrid.IsVisible = true;
            }
            else
            {
                onBehalfOfGrid.IsVisible = false;
                SelectedOnBehalf = "";
            }
        }

        async void AddAttachment_Clicked(object sender, System.EventArgs e)
        {
            var _mediaPicker = DependencyService.Get<IMediaPicker>();
            await _mediaPicker.TakePhotoAsync(new CameraMediaStorageOptions { DefaultCamera = CameraDevice.Front, MaxPixelDimension = 400 }).ContinueWith(t =>
            {
                if (t.IsFaulted)
                {
                    var s = t.Exception.InnerException.ToString();
                }
                else if (t.IsCanceled)
                {
                    var canceled = true;
                }
                else
                {
                    var mediaFile = t.Result;

                    ImageSource ImageSource = ImageSource.FromStream(() => mediaFile.Source);

                    return mediaFile;
                }

                return null;
            }, null);
        }

        private async void LogIncident()
        {
            if (string.IsNullOrEmpty(TitleEditor.Text))
            {
                await Locator.Default.DialogManager.DisplayAlert(AppResources.ApplicationName, AppResources.ServiceDesk_TitleCannotBeEmpty, AppResources.Ok);
                return;
            }
            if (string.IsNullOrEmpty(DescEditor.Text))
            {
                await Locator.Default.DialogManager.DisplayAlert(AppResources.ApplicationName, AppResources.ServiceDesk_DescCannotBeEmpty, AppResources.Ok);
                return;
            }
            if (SelectedParentCategory == null)
            {
                await Locator.Default.DialogManager.DisplayAlert(AppResources.ApplicationName, AppResources.ServiceDesk_ParentCategroyCannotBeEmpty, AppResources.Ok);
                return;
            }

            //Check Validation
            string title = TitleEditor.Text;
            string descr = DescEditor.Text;
            string impact = "low";
            string urgency = "725a4cad-088c-4f55-a845-000db8872e01";
            string classification = SelectedChildCategory == null ? SelectedParentCategory.id : SelectedChildCategory.id;
            string source = "mobile";
            string affectedUser = string.IsNullOrEmpty(SelectedOnBehalf) ? LoggedUserInfo.CurrentUserProfile.DisplayName : SelectedOnBehalf;
            string createdByUser = LoggedUserInfo.CurrentUserProfile.DisplayName;
			string[] fileNames=new string[0];
            string[] files =new string[0];
            string templateId = SelectedTemplate != null ? SelectedTemplate.ID : "";
			List<string> RA_Values = new List<string>();
            foreach (var item in RenderdControlList)
            {
                if (string.IsNullOrEmpty(item.Value) && item.TemplateExtension.Required)
                {
                    await Locator.Default.DialogManager.DisplayAlert(AppResources.ApplicationName, item.TemplateExtension.DisplayName + " is requried", AppResources.Ok);
                    return;
                }
                else if (!string.IsNullOrEmpty(item.Value))
                {
					RA_Values.Add(string.Format("{0}|{1},", item.TemplateExtension.Name, item.Value));
                }
            }
           
            LogIncidentRequest request = new LogIncidentRequest();
            request.AffectedUser = "DEV\\" + affectedUser;
            request.Classification = classification;
            request.CreatedByUser = "DEV\\" + createdByUser;
            request.Description = descr;
            request.Files = files;
            request.FilesNames = fileNames;
            request.Impact = impact;
			request.RA_values = RA_Values.ToArray();
            request.Source = source;
            request.templateId = templateId;
            request.Title = title;
            request.Urgency = "725a4cad-088c-4f55-a845-000db8872e01";
            var response = await Locator.Default.DataService.LogIncident(request);
            if (response.ResponseStatus == ResponseStatus.SuccessWithResult && response.Result != null)
            {
                if (response.Result != null)
                {
                    if (response.Result.result != "")
                    {
                        Locator.Default.NavigationService.GoBack();
                    }
                }
                else
                {
                    await Locator.Default.DialogManager.DisplayAlert(AppResources.ApplicationName, AppResources.Error, AppResources.Ok);
                }
            }
        }
    }
}