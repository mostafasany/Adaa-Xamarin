﻿using AdaaMobile.Common;
using System.Collections.Generic;

namespace AdaaMobile.Models
{
    public class TimeSheetFormated : BindableBase
    {
        public TimeSheetFormated()
        {
            RemainingHours = 8;
        }

        private double _LoggedInHours;
        public double LoggedInHours
        {
            get { return _LoggedInHours; }
            set { SetProperty(ref _LoggedInHours, value); }
        }

        private double _RemainingHours = 8;
        public double RemainingHours
        {
            get { return _RemainingHours; }
            set { SetProperty(ref _RemainingHours, value); }
        }

        public string LoggedInHoursFormated { get { return LoggedInHours.ToString() + " H"; } }
        public string RemainingHoursFormated { get { return RemainingHours.ToString() + " H"; } }
        public List<Project> Projects { get; set; }

    }

    public class Project : BindableBase
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public double TotalHours { get; set; }
        public string TotalHoursFormated { get { return TotalHours.ToString() + " H"; } }
        public List<ProjectTask> Tasks { get; set; }
        public override string ToString()
        {
            return Name;
        }

    }

    public class ProjectTask : BindableBase
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string AssigmentId { get; set; }
        public string AssigmentName { get; set; }
        public DayWithLoggedInHours Day { get; set; }
        public bool CanEdit { get; set; }
    }

    public class DayWithLoggedInHours : BindableBase
    {
        public double Hours { get; set; }
        public string HoursFormated { get { return Hours.ToString() + " H"; } }
        public string DayName { get; set; }
        public string Comment { get; set; }
    }
}