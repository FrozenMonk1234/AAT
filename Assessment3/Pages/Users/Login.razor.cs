using Assessment3.Models;
using Assessment3.Services;
using Microsoft.AspNetCore.Components;

namespace Assessment3.Pages.Users
{
    public partial class Login
    {
        [Inject] IUserService Service { get; set; }
        [Inject] NavigationManager NavigationManager { get; set; }
        public User Model { get; set; } = new();
        private async Task OnSubmit()
        {
            try
            {
                var result = await Service.Login(Model);
                if (result == null || result.Id <= 0)
                {
                    throw new Exception();
                }

                Shared.UserDetail = result;
                NavigationManager.NavigateTo("Events");
            }
            catch (Exception)
            {
                NavigationManager.NavigateTo("Register");
            }

        }

    }
}
