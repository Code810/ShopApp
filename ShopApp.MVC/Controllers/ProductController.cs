using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ShopApp.MVC.ViewModels;
using System.Net.Http.Headers;

namespace ShopApp.MVC.Controllers
{
    public class ProductController : Controller
    {
        public async Task<IActionResult> Index(int page)
        {
            using HttpClient client = new();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Request.Cookies["token"]);
            HttpResponseMessage response = await client.GetAsync("http://localhost:5113/api/Product?page=" + page);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<ProductListVm>(data);
                return View(result);
            }
            return BadRequest();
        }
    }
}
