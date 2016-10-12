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
using AdaaMobile.Models.Response;
using System.IO;
using Newtonsoft.Json;

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

            if (!string.IsNullOrEmpty(Locator.Default.ServiceDeskHomeViewModel.Module))
            {
                moduleName = Locator.Default.ServiceDeskHomeViewModel.Module;
                if (moduleName == "Service%20request%20area")
                    Title = AppResources.ServiceDesk_RequestITService;
                else
                {
                    Title = AppResources.ServiceDesk_LogAnIncident;
                }
            }

            HandleArabicLanguageFlowDirection();
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
            LoadOnBelhaf();
            LoadParentCategories();
        }

        void HandleArabicLanguageFlowDirection()
        {
            if (Locator.Default.AppSettings.SelectedCultureName.Contains("ar"))
            {
                ShowPasswordLabel.HorizontalOptions = LayoutOptions.End;
                PasswordToggle.HorizontalOptions = LayoutOptions.Start;


                lblOnBelHalf.HorizontalOptions = LayoutOptions.End;
                lblOnBelHalfResult.HorizontalOptions = LayoutOptions.End;
                Grid.SetColumn(imageOnBelHalfType, 0);
                imageOnBelHalfType.RotationY = 180;

                lblParentCategories.HorizontalOptions = LayoutOptions.End;
                lblParentCategoriesResult.HorizontalOptions = LayoutOptions.End;
                lblDesc.HorizontalOptions = LayoutOptions.End;
                lblTitle.HorizontalOptions = LayoutOptions.End;
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
        byte[] attachment;
        string attachmentName = "";
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
                        SelectedChildCategory = ChildCategoryList[0];
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

        async void RenderEnum(TemplateExtension extension)
        {
            var response = await Locator.Default.DataService.GetParentCategories(extension.EnumListName);
            loadingControl.IsRunning = false;
            if (response != null && response.ResponseStatus == AdaaMobile.DataServices.Requests.ResponseStatus.SuccessWithResult)
            {
                if (response.Result.result != null && response.Result.result.Count() > 0)
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
                    Picker picker = new Picker
                    {
                        VerticalOptions = LayoutOptions.Center,
                        HorizontalOptions = LayoutOptions.End,
                        BindingContext = extension,
                        WidthRequest = 400,
                        BackgroundColor = Xamarin.Forms.Color.Transparent,
                    };
                    for (int i = 0; i < response.Result.result.Count; i++)
                    {
                        picker.Items.Add(response.Result.result[i].name);
                    }
                    picker.SelectedIndexChanged += picker_SelectionIndexChanged;
                    customControls2.Children.Add(picker);
                }
            }
        }

        private void picker_SelectionIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (sender != null && sender is Picker)
                {
                    var picker = sender as Picker;
                    var extensionTemplate = picker.BindingContext as TemplateExtension;
                    var renderControl = RenderdControlList.FirstOrDefault(a => a.TemplateExtension.ID == extensionTemplate.ID);
                    renderControl.Value = picker.Items[picker.SelectedIndex];
                }
            }
            catch (Exception ex)
            {
            }
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
            try
            {
                //return;
                loadingControl.IsRunning = true;

                var mediaPicker = DependencyService.Get<IMediaPicker>();
                var mediaFile = await mediaPicker.SelectPhotoAsync(new CameraMediaStorageOptions { });
                if (mediaFile != null)
                {
                    attachmentGrid.IsVisible = true;
                    attachment = ReadFully(mediaFile.Source);
                    attachmentName = Path.GetFileName(mediaFile.Path);
                    lblFileName.Text = Path.GetFileName(mediaFile.Path);
                    // await Locator.Default.DialogManager.DisplayAlert(attachment.Length.ToString(), mediaFile.Path, AppResources.Ok);
                    //var imageSource = ImageSource.FromStream(() => mediaFile.Source);
                }
                else
                {
                    attachmentGrid.IsVisible = false;
                    attachment = new byte[0];
                    attachmentName = "";
                    lblFileName.Text = "";
                    //await Locator.Default.DialogManager.DisplayAlert(attachment.Length.ToString(), mediaFile.Path, AppResources.Ok);
                }

            }
            catch (Exception ex)
            {

            }
            finally
            {
                loadingControl.IsRunning = false;
            }
        }

        public static byte[] ReadFully(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }

        private async void LogIncident()
        {
            try
            {
                loadingControl.IsRunning = true;

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
                List<string> fileNames = new List<string>();
                List<byte[]> files = new List<byte[]>();

                if (string.IsNullOrEmpty(attachmentName))
                    fileNames.Add(attachmentName);

                if (attachment.Length > 0)
                    files.Add(attachment);

                string templateId = SelectedTemplate != null ? SelectedTemplate.ID : "";
                List<string> RA_Values = new List<string>();
                if (RenderdControlList != null)
                {
                    foreach (var item in RenderdControlList)
                    {
                        if (string.IsNullOrEmpty(item.Value) && item.TemplateExtension.Required)
                        {
                            await Locator.Default.DialogManager.DisplayAlert(AppResources.ApplicationName, item.TemplateExtension.DisplayName + " is requried", AppResources.Ok);
                            return;
                        }
                        else if (!string.IsNullOrEmpty(item.Value))
                        {
                            RA_Values.Add(string.Format("{0}|{1}", item.TemplateExtension.Name, item.Value));
                        }
                    }
                }

                if (moduleName == "Incident%20Classification")
                {
                    LogIncidentRequest request = new LogIncidentRequest();
                    request.AffectedUser = "DEV\\" + affectedUser;
                    request.Classification = classification;
                    request.CreatedByUser = "DEV\\" + createdByUser;
                    request.Description = descr;
                    request.Files = files.ToArray();
                    request.FilesNames = fileNames.ToArray();
                    request.Impact = impact;
                    request.RA_values = RA_Values.ToArray();
                    request.Source = source;
                    request.templateId = templateId;
                    request.Title = title;
                    request.Urgency = "725a4cad-088c-4f55-a845-000db8872e01";
                    var result = JsonConvert.SerializeObject(request);
                    await Locator.Default.DialogManager.DisplayAlert(AppResources.ApplicationName, result, AppResources.Ok);
                    DependencyService.Get<IPhoneService>().ComposeMailWithAttachment("mostafasany.khodeir@Hotmail.com", "Test Email", "adaa_greeting_card.jpg", files[0], result);
                    ResponseWrapper<AcceptAndReject> response = await Locator.Default.DataService.LogIncident(request);
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
                else
                {
                    NewServiceRequest request = new NewServiceRequest();
                    request.affectedUserField = "DEV\\" + createdByUser;
                    request.calssificationIDField = classification;
                    request.createdByUserField = "DEV\\" + createdByUser;
                    request.descriptionField = descr;
                    request.pocoStatusField = "New";
                    request.filesBytesField = files.ToArray();
                    request.filesNamesField = fileNames.ToArray();
                    request.rA_ValuesField = RA_Values.ToArray();
                    request.templateIDField = templateId;
                    request.titleField = title;
                    var result = JsonConvert.SerializeObject(request);
                    await Locator.Default.DialogManager.DisplayAlert(AppResources.ApplicationName, result, AppResources.Ok);
                    DependencyService.Get<IPhoneService>().ComposeMailWithAttachment("mostafasany.khodeir@Hotmail.com", "Test Email", "adaa_greeting_card.jpg", files[0], result);
                    ResponseWrapper<string> response = await Locator.Default.DataService.NewServiceRequest(request);
                    if (response.ResponseStatus == ResponseStatus.SuccessWithResult && response.Result != null)
                    {
                        if (response.Result != "")
                        {
                            Locator.Default.NavigationService.GoBack();
                        }
                        else
                        {
                            await Locator.Default.DialogManager.DisplayAlert(AppResources.ApplicationName, AppResources.Error, AppResources.Ok);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                await Locator.Default.DialogManager.DisplayAlert(AppResources.ApplicationName, AppResources.Error, AppResources.Ok);
            }
            finally
            {
                loadingControl.IsRunning = false;
            }

        }
    }
}