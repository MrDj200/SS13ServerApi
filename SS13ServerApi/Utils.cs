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
    }
}
