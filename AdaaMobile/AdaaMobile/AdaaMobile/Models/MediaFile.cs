using System;
using System.Threading.Tasks;
using AdaaMobile.Common;

namespace AdaaMobile.Models
{
    public class PPMediaFile
    {
        public Byte[] data { get; set; }
        public string Extension { get; set; }

        public EventHandler<EventArgs> OnDeleted { get; set; }


        public bool isSelected { get; set; }

        private AsyncExtendedCommand _DeleteCommand;
        public AsyncExtendedCommand DeleteCommand
        {
            get
            {
                _DeleteCommand = _DeleteCommand ?? new AsyncExtendedCommand(DoDeleteCommand);
                return _DeleteCommand;
            }
        }


        private Task DoDeleteCommand()
        {
            isSelected = true;
			return null;
            //if (OnDeleted != null)
            //    OnDeleted(this, new EventArgs());
        }

    }
}
