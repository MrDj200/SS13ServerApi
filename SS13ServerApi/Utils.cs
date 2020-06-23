using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace SS13ServerApi
{
    public static class Utils
    {
        public static async Task<bool> IsValidAddress(string address)
        {
            return !(await Dns.GetHostAddressesAsync(address)).Any(IP => IPAddress.IsLoopback(IP));
        }
    }
}
