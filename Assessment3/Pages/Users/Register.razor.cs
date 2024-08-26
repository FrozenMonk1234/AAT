using Assessment3.Models;
using Assessment3.Services;
using Microsoft.AspNetCore.Components;

namespace Assessment3.Pages.Users
{
    public partial class Register
    {
        [Inject] IUserService Service { get; set; }
        [Inject] NavigationManager NavigationManager { get; set; }
        public User Model { get; set; } = new();

        private async Task OnSubmit()
        {
            IsLoading = true;
            try
            {
                var result = await Service.Login(Model);
                if (result == null || result.Id <= 0)
                {
                    Model.DateCreated = DateTime.Now;
                    Model.DateModified = DateTime.Now;
                    if (await Service.CreateAsync(Model))
                    {
                        IsLoading = false;
                        result = await Service.Login(Model);
                        Shared.UserDetail = result;
                        NavigationManager.NavigateTo("Events");
                    }
                    else
                        throw new Exception("Something went wrong. Unable to login.");

                }
                else
                    NavigationManager.NavigateTo("Events");
            }
            catch (Exception e)
            {
                ShowError(e.Message);
            }
        }
        private void OnCancel() => NavigationManager.NavigateTo("/");

        public bool IsLoading { get; set; }
        public bool IsError { get; set; }
        public string ErrorMessage { get; set; } = "";

        private void ShowError(string Message)
        {
            ErrorMessage = Message;
            IsError = true;
        }

    }
}
