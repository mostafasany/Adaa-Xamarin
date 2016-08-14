namespace AdaaMobile.Helpers
{
    public interface IAppSettings
    {
        string SelectedCultureName { get; set; }
		string UserToken { get; set;}
        /// <summary>
        /// It converts SelectedCultureName to qualifier used by backend, arb or eng
        /// </summary>
        string Language { get; }

        bool IsCultureSet { get; }

		string NextSelectedCultureName { get; set; }
    }
}
