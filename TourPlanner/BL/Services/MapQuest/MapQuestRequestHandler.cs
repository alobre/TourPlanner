using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Drawing;
using System.Drawing.Imaging;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows;
using TourPlanner.BL.Services.Logging;

namespace TourPlanner.BL.Services.MapQuest
{
    public class MapQuestRequestHandler
    {

        
        public static async Task<(Rootobject? route, BitmapImage? image, string URL)> GetRouteAsync(MapQuestRequestData start, MapQuestRequestData dest)
        {
            try
            {

                using var client = new HttpClient();
                string URI = $"http://www.mapquestapi.com/directions/v2/route?key=y0MJ3PFzkkliOvNfcFm49CPFMpKj65t5&from=sd{start.GetString()}&to={dest.GetString()}";
                var res = await client.GetAsync(URI);

                var json = await res.Content.ReadAsStringAsync();
                var imgres = GetRouteImageAsync(start, dest).Result;
                return (JsonConvert.DeserializeObject<Rootobject>(json), imgres.img, imgres.URL);
            }
            catch (Exception ex)
            {
                Log.LogError(ex.Message);
                MessageBox.Show("Could not create a new tour. Please check your input and your internet connection");
                return (null, null, "");
            }

        }

        public static async Task<(Rootobject? route, BitmapImage? image, string URL)> GetRouteAsync(MapQuestRequestData start, MapQuestRequestData dest, string url)
        {
            try
            {

                using var client = new HttpClient();
                var res = await client.GetAsync(url);

                var json = await res.Content.ReadAsStringAsync();
                var imgres = GetRouteImageAsync(start, dest).Result;
                return (JsonConvert.DeserializeObject<Rootobject>(json), imgres.img, imgres.URL);
            }
            catch (Exception ex)
            {
                Log.LogError(ex.Message);
                MessageBox.Show("Could not create a new tour. Please check your input and your internet connection");
                return (null, null, "");
            }

        }

        public static async Task<(BitmapImage? img, string URL)> GetRouteImageAsync(MapQuestRequestData start, MapQuestRequestData dest)
        {
            try
            {
                WebClient client = new WebClient();
                string URL = $"https://www.mapquestapi.com/staticmap/v5/map?key=y0MJ3PFzkkliOvNfcFm49CPFMpKj65t5&start={start.GetString()}&end={dest.GetString()}";
                Stream stream = client.OpenRead(URL);
                Bitmap bitmap; bitmap = new Bitmap(stream);

                stream.Flush();
                stream.Close();
                client.Dispose();

                return (ToBitmapImage(bitmap), URL);

            }
            catch (Exception ex)
            {
                Log.LogError(ex.Message);
                MessageBox.Show(ex.Message);
                return (null, "");
            }
        }

        public static async Task<(BitmapImage? img, string URL)> GetRouteImageAsync(MapQuestRequestData start, MapQuestRequestData dest, string url)
        {
            try
            {
                WebClient client = new WebClient();
                Stream stream = client.OpenRead(url);
                Bitmap bitmap; bitmap = new Bitmap(stream);

                stream.Flush();
                stream.Close();
                client.Dispose();

                return (ToBitmapImage(bitmap), url);

            }
            catch (Exception ex)
            {
                Log.LogError(ex.Message);
                MessageBox.Show(ex.Message);
                return (null, "");
            }
        }

        public static BitmapImage ToBitmapImage(Bitmap bitmap)
        {
            using (var memory = new MemoryStream())
            {
                bitmap.Save(memory, ImageFormat.Png);
                memory.Position = 0;

                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                bitmapImage.Freeze();

                return bitmapImage;
            }
        }
    }
}
