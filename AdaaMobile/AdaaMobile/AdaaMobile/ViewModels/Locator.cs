using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using AdaaMobile.DataServices;
using AdaaMobile.Helpers;
using Autofac;

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
            cb.RegisterType<DataService>();
            cb.Register<IDataService>(c=>c.Resolve<DataService>());
            cb.RegisterType<AppSettings>().SingleInstance();
            cb.Register<IAppSettings>(c => c.Resolve<AppSettings>());
            cb.RegisterType<LoginViewModel>();
            cb.RegisterType<AttendanceViewModel>();
            cb.RegisterType<DirectoryViewModel>();
        }

        public LoginViewModel LoginViewModel
        {
            get
            {
                return Container.Resolve<LoginViewModel>();
            }
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
