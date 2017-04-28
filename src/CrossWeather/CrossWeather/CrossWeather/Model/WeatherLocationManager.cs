using CrossWeather.Services;
using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using Microsoft.WindowsAzure.MobileServices.Sync;
using Plugin.Connectivity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossWeather.Model
{
    public class WeatherLocationManager
    {
        IMobileServiceClient client;
        IMobileServiceSyncTable<WeatherLocation> table;

        public async Task Initialize()
        {
            if (client?.SyncContext?.IsInitialized ?? false) return;

            client = new MobileServiceClient(AzureServiceKey.AzureMobileAppURL);
            var store = new MobileServiceSQLiteStore("localstore.db");
            store.DefineTable<WeatherLocation>();
            await client.SyncContext.InitializeAsync(store);

            table = client.GetSyncTable<WeatherLocation>();


        }

        public bool IsOfflineEnabled
        {
            get { return table is IMobileServiceSyncTable<WeatherLocation>; }
        }

        public async Task SyncAsync()
        {
            try
            {
                if (!CrossConnectivity.Current.IsConnected) return;
                // Push
                await client.SyncContext.PushAsync();
                // Pull
                await table.PullAsync("allweather", table.CreateQuery());
            }
            catch (MobileServiceInvalidOperationException msioe)
            {
                // 서버측에서 인증이 먼저 필요한 Method 의 경우 401 을 반환하게 된다. 
                if (msioe.Response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    throw new UnauthorizedAccessException("Unauth access", msioe);
                }
            }
            catch (MobileServicePushFailedException exc)
            {
                if (exc.PushResult != null)
                {
                    // Authentication Error when push 
                    if (exc.PushResult.Status == MobileServicePushStatus.CancelledByAuthenticationError)
                    {
                        throw new UnauthorizedAccessException("Unauth access when push", exc);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("", ex);
            }
        }

        public async Task<IEnumerable<WeatherLocation>> GetLocation()
        {
            await Initialize();
            await SyncAsync();
            return await table.OrderBy(c => c.Name).Take(3).ToEnumerableAsync();
        }

        public async Task<WeatherLocation> AddLocation(string name)
        {
            if (string.IsNullOrEmpty(name)) return null;

            await Initialize();

            var items = await table.Where(x => x.Name == name).ToListAsync();
            // 원래있으며 저장할 필요없음. 
            if (items.Count > 0) return null;

            var location = new WeatherLocation
            {
                Name = name
            };

            await table.InsertAsync(location);
            await SyncAsync();

            return location;
        }

    }
}
