using System;

namespace AdaaMobile
{
	public interface IPhoneService
	{
		/// <summary>
		/// Opens native dialog to dial the specified number.
		/// </summary>
		/// <param name="number">Number to dial.</param>
		void DialNumber(string number);

		void ComposeMail (string recipient, string subject, string messagebody = null, Action<bool> completed = null);

        /// <summary>
        /// Opens oracle app through app package name on Android and store app on iOS
        /// </summary>
        void OpenOracleApp();
	}
}

