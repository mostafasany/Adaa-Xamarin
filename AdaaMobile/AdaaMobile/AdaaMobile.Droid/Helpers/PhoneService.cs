﻿
using System.Globalization;
using AdaaMobile.Droid.Helpers;
using AdaaMobile.Helpers;
using Java.Util;
using Xamarin.Forms;
using AdaaMobile.Droid;
using Android.Net;
using Android.Content;
using System;
using Android.Content.PM;

[assembly: Dependency(typeof(PhoneService))]
namespace AdaaMobile.Droid
{
	public class PhoneService : IPhoneService
    {
        /// <summary>
        /// Opens native dialog to dial the specified number.
        /// </summary>
        /// <param name="number">Number to dial.</param>
        public void DialNumber(string number)
        {
            var instance = MainActivity.Instance;
            if (instance != null)
            {
                var uri = Android.Net.Uri.Parse("tel:" + number);
                instance.StartActivity(new Intent(Intent.ActionDial, uri));
            }
        }
        public void ComposeMail(string recipient, string subject, string messagebody = null, Action<bool> completed = null)
        {
            Device.OpenUri(new System.Uri(string.Format("mailto:{0}?subject={1}&body=", recipient, subject)));
        }

        public void OpenOracleApp()
        {
           // string packageName = "com.mostafa.cairometrobeta";
            string packageName = "com.ADDOF";
            //Intent intent = new Intent(Intent.ActionMain);
            //intent.SetComponent(new ComponentName("com.mostafa.cairometrobeta", "com.mostafa.cairometrobeta.MainActivity"));
            //StartActivity(intent);


            var instance = MainActivity.Instance;
            if (instance != null)
            {
                Intent intent = instance.PackageManager.GetLaunchIntentForPackage(packageName);

                //Application exists on device, open it directly
                if (intent != null)
                {
                    instance.StartActivity(intent);
                }
                else
                {
                    OpenApplicationInPlayStore(packageName);
                }
            }

        }

        private static void OpenApplicationInPlayStore(string packageName)
        {
            //try to open application on play store
            //If failed open it inside browser
            try
            {
                MainActivity.Instance.StartActivity(new Intent(Intent.ActionView, Android.Net.Uri.Parse("market://details?id=" + packageName)));
            }
            catch (Android.Content.ActivityNotFoundException)
            {
                MainActivity.Instance.StartActivity(new Intent(Intent.ActionView, Android.Net.Uri.Parse("https://play.google.com/store/apps/details?id=" + packageName)));
            }
        }

		public void SavePictureToDisk (string filename, byte[] imageData)
		{
			var dir = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDcim);
			var pictures = dir.AbsolutePath;
			//adding a time stamp time file name to allow saving more than one image... otherwise it overwrites the previous saved image of the same name
			string name = filename + System.DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".jpg";
			string filePath = System.IO.Path.Combine(pictures, name);
			try
			{
				System.IO.File.WriteAllBytes(filePath, imageData);
				//mediascan adds the saved image into the gallery
				var mediaScanIntent = new Intent(Intent.ActionMediaScannerScanFile);
				mediaScanIntent.SetData(Android.Net.Uri.FromFile(new Java.IO.File(filePath)));
				Xamarin.Forms.Forms.Context.SendBroadcast(mediaScanIntent);
			}
			catch(System.Exception e)
			{
				System.Console.WriteLine(e.ToString());
			}
		}
    }
}

