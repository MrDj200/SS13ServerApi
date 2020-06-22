using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace IntegrationTests
{
    [TestClass]
    public class UnitTest
    {
        public async Task<string> MakeGetRequest(string uri)
        {
            string finalResponse;
            // Create a request for the URL.
            WebRequest request = WebRequest.Create(uri);
            // If required by the server, set the credentials
            request.Credentials = CredentialCache.DefaultCredentials;

            // Get the response
            WebResponse response = await request.GetResponseAsync();

            // Get the stream containing content returned by the server.
            // The using block ensures the stream is automatically closed.
            using (Stream dataStream = response.GetResponseStream())
            {
                // Open the stream using a StreamReader for easy access.
                StreamReader reader = new StreamReader(dataStream);
                // Read the content.
                finalResponse = await reader.ReadToEndAsync();
            }
            // Close the response
            response.Close();
            return finalResponse;
        }

        [TestMethod]
        public async Task TestSS13StatusDefault()
        {
            string response = await MakeGetRequest("http://localhost:5000/SS13/status");
            Console.WriteLine(response);
            Assert.IsNotNull(response);
            Assert.IsTrue(response != "");
        }
    }
}
