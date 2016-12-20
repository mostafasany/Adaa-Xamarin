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

		string GetVersion();

		void ComposeMail (string recipient, string subject, string messagebody = null, Action<bool> completed = null);

		bool ComposeMailWithAttachment (string recipient, string subject, string fileName,byte[] imageData, string messagebody = null);

        /// <summary>
        /// Opens oracle app through app package name on Android and store app on iOS
        /// </summary>
        void OpenOracleApp();

        void OpenCrosspondenceApp();

        string SavePictureToDisk (string filename, byte[] imageData,bool addToGallery=true);

	}
}

