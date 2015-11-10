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
            cb.RegisterType<BaseRequest>();
            cb.RegisterType<DataService>();
            cb.RegisterType<MockDataService>();
            cb.Register<IDataService>(c => c.Resolve<DataService>());
            cb.RegisterType<LoginViewModel>();
            //cb.Register<LoginViewModel>((c, prms) =>
            //new LoginViewModel(
            //    c.Resolve<IDataService>()
            //    , c.Resolve<IDialogManager>()
            //    , prms.TypedAs<INavigation>())
            //    );
            cb.RegisterType<AttendanceViewModel>();
            cb.RegisterType<DirectoryViewModel>();
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

        public IAppSettings AppSettings
        {
            get
            {
                return Container.Resolve<IAppSettings>();
            }
        }
    }
}
