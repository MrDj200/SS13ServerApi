using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace SS13ServerApi
{
    public static class Utils
    {
        public static async Task<bool> IsValidAddress(string address)
        {
            // Checks if the given IP is a valid IP and is not a loopback
            return !(await Dns.GetHostAddressesAsync(address)).Any(IP => IPAddress.IsLoopback(IP));
        }

        public static async Task<string> MakeGetRequest(string uri)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            using HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync();
            using Stream stream = response.GetResponseStream();
            using StreamReader reader = new StreamReader(stream);
            return await reader.ReadToEndAsync();
        }

    }
}
