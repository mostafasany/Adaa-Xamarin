﻿using System;
using System.Globalization;

namespace AdaaMobile.Helpers
{
    public interface ILocalize
    {
        CultureInfo GetCurrentCultureInfo();
		void UpdateCultureInfo (string cultureName);
    }
}
