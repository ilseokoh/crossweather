using CrossWeather.Services;
using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using Microsoft.WindowsAzure.MobileServices.Sync;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossWeather.Model
{
    public class WeatherLocationManager
    {
        static WeatherLocationManager defaultInstance = new WeatherLocationManager();
        IMobileServiceClient client;
        IMobileServiceSyncTable<WeatherLocation> eventTable;

        private WeatherLocationManager()
        {
            client = new MobileServiceClient(AzureServiceKey.AzureMobileAppURL);
            var store = new MobileServiceSQLiteStore("localstore.db");
            store.DefineTable<WeatherLocation>();
            client.SyncContext.InitializeAsync(store);

            eventTable = client.GetSyncTable<WeatherLocation>();
        }

        public static WeatherLocationManager DefaultManager
        {
            get
            {
                return defaultInstance;
            }
            private set
            {
                defaultInstance = value;
            }
        }

        public IMobileServiceClient CurrentClient
        {
            get { return client; }
        }

        public bool IsOfflineEnabled
        {
            get { return eventTable is IMobileServiceSyncTable<WeatherLocation>; }
        }



    }
}
