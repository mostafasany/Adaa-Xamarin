﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QueryExtensions;

namespace AdaaMobile.Models.Request
{
    public class LoginQParameters
    {
        [QueryParameter("server")]
        public string Server { get; set; }

        [QueryParameter("url")]
        public string Url
        {
            get { return "funcname=validateLogin"; }
        }

        [QueryParameter("langid")]
        public string Langid { get; set; }
    }
}
