using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ShopApp.MVC.ViewModels;
using System.Text;

namespace ShopApp.MVC.Controllers
{
    public class AuthController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginVm loginVm)
        {
            using HttpClient client = new HttpClient();
            StringContent content = new StringContent(JsonConvert.SerializeObject(loginVm), Encoding.UTF8, "application/json");
            var responce = await client.PostAsync("http://localhost:5113/api/Auth/login", content);
            if (responce.IsSuccessStatusCode)
            {
                var data = await responce.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<UserTokenVm>(data);
                Response.Cookies.Append("token", result.Token.ToString());
                return Content("success");
            }
            return BadRequest();
        }
    }
}
