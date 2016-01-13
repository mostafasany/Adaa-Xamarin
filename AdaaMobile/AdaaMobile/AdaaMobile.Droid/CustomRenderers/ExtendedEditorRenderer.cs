using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using AdaaMobile.Controls;
using AdaaMobile.Droid.CustomRenderers;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Text;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
[assembly: ExportRenderer(typeof(ExtendedEditor), typeof(ExtendedEditorRenderer))]

namespace AdaaMobile.Droid.CustomRenderers
{
    public class ExtendedEditorRenderer : EditorRenderer
    {
        protected virtual ExtendedEditor FormsElement
        {
            get { return Element as ExtendedEditor; }
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Editor> e)
        {
            base.OnElementChanged(e);
            if (e.NewElement != null)
            {
                SetMaxLength(FormsElement.MaxLength);
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (e.PropertyName == ExtendedEditor.MaxLengthProperty.PropertyName)
            {
                SetMaxLength(FormsElement.MaxLength);
            }
        }

        protected void SetMaxLength(int length)
        {
            if (length <= 0)
            {
                //Clear filter if it's already added
                RemoveFilterIfExists<InputFilterLengthFilter>();
                return;
            }

            var lengthFilter = new InputFilterLengthFilter(length);
            //Add TextFlagNoSuggestions to prevent back overflow
            //Check http://stackoverflow.com/a/19222238/2086772 
            Control.InputType = Control.InputType | InputTypes.TextFlagNoSuggestions;
            AddOrReplaceFilter(lengthFilter);
            //TODO:Test change of limits scenario
        }

        /// <summary>
        /// Inpsired from this answer http://stackoverflow.com/a/10769729/2086772
        /// It tries to not lose any other filters added to the Editor.
        /// </summary>
        /// <param name="filter"></param>
        protected void AddOrReplaceFilter(IInputFilter filter)
        {
            var currentFilters = Control.GetFilters();
            if (currentFilters == null)
            {
                //There are no previous filters
                Control.SetFilters(new[] { filter });
            }
            else
            {
                //Check if the filter is already there to update it
                int foundIndex = Array.FindIndex(currentFilters, other => other.GetType() == filter.GetType());
                if (foundIndex > -1)
                {
                    currentFilters[foundIndex] = filter;
                }
                else
                {
                    //Filter doesn't exist, create new filters List with the new filter
                    var newFilters = new IInputFilter[currentFilters.Length + 1];
                    Array.Copy(currentFilters, newFilters, currentFilters.Length);
                    newFilters[newFilters.Length - 1] = filter;

                    //Set Filters
                    Control.SetFilters(newFilters);
                }
            }
        }

        /// <summary>
        /// This method attempts to remove filter if exists.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        protected void RemoveFilterIfExists<T>() where T : IInputFilter
        {
            var currentFilters = Control.GetFilters();
            //Check if there are filters
            if (currentFilters != null)
            {
                int foundIndex = Array.FindIndex(currentFilters, other => other.GetType() == typeof(T));
                if (foundIndex == -1) return; //Filter not found
                //Create new list with current length -1
                var newFilters = new IInputFilter[currentFilters.Length - 1];
                for (int oldIndex = 0, newIndex = 0; oldIndex < currentFilters.Length; oldIndex++)
                {
                    if (oldIndex == foundIndex) continue;
                    newFilters[newIndex++] = newFilters[oldIndex];
                }
                //Set Filters
                Control.SetFilters(newFilters);
            }
        }

    }
}