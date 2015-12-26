using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using AdaaMobile.DataServices;
using AdaaMobile.DataServices.Requests;
using AdaaMobile.Helpers;
using Autofac;
using Xamarin.Forms;

namespace AdaaMobile.ViewModels
{
    public class Locator
    {
        public static IContainer Container { get; set; }

        public static Locator Default { set; get; }

        public IContainer CreateContainer()
        {
            var containerBuilder = new ContainerBuilder();
            RegisterDependencies(containerBuilder);
            return containerBuilder.Build();
        }

        /// <summary>
        /// Registers differende dependencies.
        /// Override in platform dependent to add other/or override registerations
        /// </summary>
        /// <param name="cb"></param>
        protected virtual void RegisterDependencies(ContainerBuilder cb)
        {
            cb.RegisterType<AppSettings>().SingleInstance();
            cb.Register<IAppSettings>(c => c.Resolve<AppSettings>());
            cb.RegisterType<NetworkHelper>().SingleInstance();
            cb.Register<INetworkHelper>(c => c.Resolve<NetworkHelper>());
            cb.RegisterType<DialogManager>().SingleInstance();
            cb.Register<IDialogManager>(c => c.Resolve<DialogManager>());
            cb.RegisterType<RequestMessageResolver>().SingleInstance();
            cb.Register<IRequestMessageResolver>(c => c.Resolve<RequestMessageResolver>());
            cb.RegisterType<BaseRequest>();
            cb.RegisterType<DataService>();
            //            cb.RegisterType<MockDataService>();
            cb.Register<IDataService>(c => c.Resolve<DataService>());
            cb.RegisterType<NavigationService>();
            cb.Register<INavigationService>(c => c.Resolve<NavigationService>());
            cb.RegisterType<LoginViewModel>();
            cb.RegisterType<HomeViewModel>();
            cb.RegisterType<AttendanceViewModel>();
            cb.RegisterType<DirectoryViewModel>();
            cb.RegisterType<ProfileViewModel>();
            cb.RegisterType<SettingsViewModel>();
            cb.RegisterType<UserAccountServicesViewModel>();
            cb.RegisterType<ChangePasswordViewModel>();
            cb.RegisterType<DirectoryViewModel>();
			cb.RegisterType<DayPassViewModel>();
			cb.RegisterType<DelegationViewModel>();
        }



        public AttendanceViewModel AttendanceViewModel
        {
            get
            {
                return Container.Resolve<AttendanceViewModel>();
            }
        }

        public DirectoryViewModel DirectoryViewModel
        {
            get
            {
                return Container.Resolve<DirectoryViewModel>();
            }
        }

        public ProfileViewModel ProfileViewModel
        {
            get
            {
                return Container.Resolve<ProfileViewModel>();
            }
        }

        public SettingsViewModel SettingsViewModel
        {
            get
            {
                return Container.Resolve<SettingsViewModel>();
            }
        }

        public UserAccountServicesViewModel UserAccountServicesViewModel
        {
            get
            {
                return Container.Resolve<UserAccountServicesViewModel>();
            }
        }

        public ChangePasswordViewModel ChangePasswordViewModel
        {
            get
            {
                return Container.Resolve<ChangePasswordViewModel>();
            }
        }

        public DayPassViewModel DayPassViewModel
        {
            get
            {
                return Container.Resolve<DayPassViewModel>();
            }
        }

        public DelegationViewModel DelegationViewModel
        {
            get
            {
                return Container.Resolve<DelegationViewModel>();
            }
        }



        public IAppSettings AppSettings
        {
            get
            {
                return Container.Resolve<IAppSettings>();
            }
        }

        public INavigationService NavigationService
        {
            get
            {
                return Container.Resolve<INavigationService>();
            }
        }
    }
}
