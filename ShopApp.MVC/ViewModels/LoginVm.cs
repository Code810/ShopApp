namespace ShopApp.MVC.ViewModels
{
    public class LoginVm
    {
        public string UserNameOrEmail { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}
