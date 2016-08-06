using AdaaMobile.DataServices;
using AdaaMobile.DataServices.Requests;
using AdaaMobile.Helpers;
using Autofac;

namespace AdaaMobile.ViewModels
{
	public class Locator
	{
		public static IContainer Container { get; set; }

		public static Locator Default { set; get; }

		public IContainer CreateContainer ()
		{
			var containerBuilder = new ContainerBuilder ();
			RegisterDependencies (containerBuilder);
			return containerBuilder.Build ();
		}

		/// <summary>
		/// Registers differende dependencies.
		/// Override in platform dependent to add other/or override registerations
		/// </summary>
		/// <param name="cb"></param>
		protected virtual void RegisterDependencies (ContainerBuilder cb)
		{
			cb.RegisterType<AppSettings> ().SingleInstance ();
			cb.Register<IAppSettings> (c => c.Resolve<AppSettings> ());
			cb.RegisterType<NetworkHelper> ().SingleInstance ();
			cb.Register<INetworkHelper> (c => c.Resolve<NetworkHelper> ());
			cb.RegisterType<DialogManager> ().SingleInstance ();
			cb.Register<IDialogManager> (c => c.Resolve<DialogManager> ());
			cb.RegisterType<RequestMessageResolver> ().SingleInstance ();
			cb.Register<IRequestMessageResolver> (c => c.Resolve<RequestMessageResolver> ());
			cb.RegisterType<BaseRequest> ();
			cb.RegisterType<DataService> ();
	
			cb.Register<IDataService> (c => c.Resolve<DataService> ());
			cb.RegisterType<NavigationService> ();
			cb.Register<INavigationService> (c => c.Resolve<NavigationService> ());
			cb.RegisterType<UserSelectionService> ();
			cb.RegisterType<EquipmentsSelectionService> ();
			cb.RegisterType<LoginViewModel> ();
			cb.RegisterType<HomeViewModel> ();
			cb.RegisterType<AttendanceViewModel> ();
			cb.RegisterType<DirectoryViewModel> ().SingleInstance ();
			cb.RegisterType<ProfileViewModel> ();
			cb.RegisterType<SettingsViewModel> ();
			cb.RegisterType<UserAccountServicesViewModel> ();
			cb.RegisterType<ChangePasswordViewModel> ();

			cb.RegisterType<DayPassViewModel> ().SingleInstance ();
			cb.RegisterType<NewDayPassViewModel> ();
			cb.RegisterType<DelegationViewModel> ().SingleInstance ();
			cb.RegisterType<DelegationDetailsViewModel> ();
			cb.RegisterType<NewDelegationViewModel> ();
			cb.RegisterType<TaskDetailsViewmodel> ();
			cb.RegisterType<EServicesViewModel> ();
			cb.RegisterType<RequestDriverViewModel> ();
			cb.RegisterType<OfficeMaintenanceViewModel> ();
			cb.RegisterType<GreetingCardsViewModel> ();
			cb.RegisterType<EquipmentsSelectionViewModel> ();
			cb.RegisterType<MyRequestsViewModel> ().SingleInstance ();
			cb.RegisterType<TimeSheetViewModel> ();
			cb.RegisterType<MyTimeSheetViewModel> ();
			cb.RegisterType<SelectTaskViewModel> ().SingleInstance ();
            cb.RegisterType<MyPendingTasksViewModel>().SingleInstance();
			cb.RegisterType<MyAssigmentsViewModel>().SingleInstance ();
        }


        public MyAssigmentsViewModel MyAssigmentsViewModel
        {
            get
            {
                return Container.Resolve<MyAssigmentsViewModel>();
            }
        }

        public MyPendingTasksViewModel MyPendingTasksViewModel
        {
            get
            {
                return Container.Resolve<MyPendingTasksViewModel>();
            }
        }

        public SelectTaskViewModel SelectTaskViewModel {
			get {
				return Container.Resolve<SelectTaskViewModel> ();
			}
		}

		public MyTimeSheetViewModel MyTimeSheetViewModel {
			get {
				return Container.Resolve<MyTimeSheetViewModel> ();
			}
		}


		public TimeSheetViewModel TimeSheetViewModel {
			get {
				return Container.Resolve<TimeSheetViewModel> ();
			}
		}


		public GreetingCardsViewModel GreetingCardsViewModel {
			get {
				return Container.Resolve<GreetingCardsViewModel> ();
			}
		}

		public AttendanceViewModel AttendanceViewModel {
			get {
				return Container.Resolve<AttendanceViewModel> ();
			}
		}

		public DirectoryViewModel DirectoryViewModel {
			get {
				return Container.Resolve<DirectoryViewModel> ();
			}
		}

		public ProfileViewModel ProfileViewModel {
			get {
				return Container.Resolve<ProfileViewModel> ();
			}
		}

		public SettingsViewModel SettingsViewModel {
			get {
				return Container.Resolve<SettingsViewModel> ();
			}
		}

		public UserAccountServicesViewModel UserAccountServicesViewModel {
			get {
				return Container.Resolve<UserAccountServicesViewModel> ();
			}
		}

		public ChangePasswordViewModel ChangePasswordViewModel {
			get {
				return Container.Resolve<ChangePasswordViewModel> ();
			}
		}

		public DayPassViewModel DayPassViewModel {
			get {
				return Container.Resolve<DayPassViewModel> ();
			}
		}

		public NewDayPassViewModel NewDayPassViewModel {
			get {
				return Container.Resolve<NewDayPassViewModel> ();
			}
		}

		public DelegationViewModel DelegationViewModel {
			get {
				return Container.Resolve<DelegationViewModel> ();
			}
		}

		public DelegationDetailsViewModel DelegationDetailsViewModel {
			get {
				return Container.Resolve<DelegationDetailsViewModel> ();
			}
		}

		public NewDelegationViewModel NewDelegationViewModel {
			get {
				return Container.Resolve<NewDelegationViewModel> ();
			}
		}

		public TaskDetailsViewmodel TaskDetailsViewmodel {
			get {
				return Container.Resolve<TaskDetailsViewmodel> ();
			}
		}


		public EServicesViewModel EServicesViewModel {
			get {
				return Container.Resolve<EServicesViewModel> ();
			}
		}

		public MyRequestsViewModel MyRequestsViewModel {
			get {
				return Container.Resolve<MyRequestsViewModel> ();
			}
		}

		public RequestDriverViewModel RequestDriverViewModel {
			get {
				return Container.Resolve<RequestDriverViewModel> ();
			}
		}

		public OfficeMaintenanceViewModel OfficeMaintenanceViewModel {
			get {
				return Container.Resolve<OfficeMaintenanceViewModel> ();
			}
		}


		public EquipmentsSelectionViewModel EquipmentsSelectionViewModel {
			get {
				return Container.Resolve<EquipmentsSelectionViewModel> ();
			}
		}


		public IAppSettings AppSettings {
			get {
				return Container.Resolve<IAppSettings> ();
			}
		}

		public INavigationService NavigationService {
			get {
				return Container.Resolve<INavigationService> ();
			}
		}

		public IDialogManager DialogManager {
			get {
				return Container.Resolve<IDialogManager> ();
			}
		}
	}
}
