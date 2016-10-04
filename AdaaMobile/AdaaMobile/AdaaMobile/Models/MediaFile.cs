using Cirrious.MvvmCross.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace PP.Core.Models
{
    public class PPMediaFile
    {
        public Byte[] data { get; set; }
        public string Extension { get; set; }

        public EventHandler<EventArgs> OnDeleted { get; set; }


        public bool isSelected { get; set; }

        private MvxCommand _DeleteCommand;
        public ICommand DeleteCommand
        {
            get
            {
                _DeleteCommand = _DeleteCommand ?? new MvxCommand(DoDeleteCommand);
                return _DeleteCommand;
            }
        }


        private void DoDeleteCommand()
        {
            isSelected = true;
            //if (OnDeleted != null)
            //    OnDeleted(this, new EventArgs());
        }

    }
}
