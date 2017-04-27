// Helpers/Settings.cs
using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace CrossWeather.Helpers
{
    /// <summary>
    /// This is the Settings static class that can be used in your Core solution or in any
    /// of your client applications. All settings are laid out the same exact way with getters
    /// and setters. 
    /// </summary>
    public static class Settings
    {
        private static ISettings AppSettings
        {
            get
            {
                return CrossSettings.Current;
            }
        }

        #region Setting Constants

        private const string CityKey = "city_key";
        private static readonly string CityDefault = "����";

        private const string CountryKey = "country_key";
        private static readonly string CountryDefault = "������";

        private const string VillageKey = "village_key";
        private static readonly string VillageDefault = "ȭ�";

        #endregion

        public static string City
        {
            get { return AppSettings.GetValueOrDefault<string>(CityKey, CityDefault); }
            set { AppSettings.AddOrUpdateValue<string>(CityKey, value); }
        }

        public static string Country
        {
            get { return AppSettings.GetValueOrDefault<string>(CountryKey, CountryDefault); }
            set { AppSettings.AddOrUpdateValue<string>(CountryKey, value); }
        }

        public static string Village
        {
            get { return AppSettings.GetValueOrDefault<string>(VillageKey, VillageDefault); }
            set { AppSettings.AddOrUpdateValue<string>(VillageKey, value); }
        }

    }
}