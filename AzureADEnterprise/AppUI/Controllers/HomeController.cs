using AppUI.Models;
using CommonLibrary;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace AppUI.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public async Task<IActionResult> Index()
        {
            return View();
        }

        public async Task<IActionResult> About()
        {
            string userObjectID = (User.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier"))?.Value;
            AuthenticationContext authContext = new AuthenticationContext(AzureADSettings.AzureSettings.Authority, true, new TokenSessionCache(userObjectID, HttpContext.Session));
            ClientCredential credential = new ClientCredential(AzureADSettings.AzureSettings.ClientId, AzureADSettings.AzureSettings.ClientSecret);
            var result = await authContext.AcquireTokenSilentAsync(AzureADSettings.AzureSettings.ResourceId, credential, new UserIdentifier(userObjectID, UserIdentifierType.UniqueId));
            HttpClient client = new HttpClient();
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, AzureADSettings.AzureSettings.ResourceBasePath + "/api/values");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", result.AccessToken);
            HttpResponseMessage response = await client.SendAsync(request);
            ViewData["Message"] = await response.Content.ReadAsStringAsync();

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
