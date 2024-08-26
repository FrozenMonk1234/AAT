using Assessment3.Components;
using Assessment3.Models;
using Assessment3.Services;
using Microsoft.AspNetCore.Components;
using System.Reflection;

namespace Assessment3.Pages.Events
{
    public partial class Event
    {
        [Inject] IEventService service { get; set; }
        [Inject] IRegisterService registerService { get; set; }
        public static List<Models.Event> Events { get; set; } = [];
        public static List<Models.Register> UserRegisteredEvents { get; set; } = [];
        public static string SearchTerm { get; set; } = "";
        public static List<Models.Event> EventsFilterd { get; set; } = Events.Where(x => x.Name.ToLower().Contains(SearchTerm)).ToList();

        protected override async Task OnInitializedAsync()
        {
            await GetEvents();
            await GetUserRegisteredEvents();
            await base.OnInitializedAsync();
        }

        private async Task GetEvents()
        {
            IsLoading = true;
            try
            {
                var result = await service.GetAllAsync();
                Events = [.. result];
                EventsFilterd = Events;
                IsLoading = false;
            }
            catch (Exception e)
            {
                ShowError(e.Message);
            }
        }
        private async Task GetUserRegisteredEvents()
        {
            UserRegisteredEvents = await registerService.GetAllAsync(Shared.UserDetail.Id, Enums.RegisterEnums.RegisterIdType.User);
        }

        private Register RegisterModel { get; set; } = new();
        public bool showEventModal { get; set; }
        public Models.Event SelectedModel { get; set; } = new();

        private void OnClickRegister(Models.Event model)
        {
            SelectedModel = model;
            showEventModal = true;
        }
        private async Task OnSubmitRegister(Models.Event model)
        {
            showEventModal = false;
            IsLoading = true;
            try
            {
                RegisterModel.DateCreated = DateTime.Now;
                RegisterModel.DateModified = DateTime.Now;
                RegisterModel.UserId = Shared.UserDetail.Id;
                RegisterModel.EventId = model.Id;

                if (!await registerService.CreateAsync(RegisterModel))
                {
                    throw new Exception("Something went wrong. Unable to submit register.");
                }
                IsLoading = false;
            }
            catch (Exception e)
            {
                ShowError(e.Message);
            }
        }

        private static int SeatAvailable(int seatsTaken, int totalSeats)
        {
            return totalSeats - seatsTaken;
        }

        private bool IsUserRegisterd(int eventId)
        {
            return UserRegisteredEvents.Where(x => x.EventId == eventId).Count() > 0;
        }

        public bool IsLoading { get; set; }
        public bool IsError { get; set; }
        public bool IsSuccess { get; set; }
        public string SuccessMessage { get; set; } = "";
        public string ErrorMessage { get; set; } = "";

        private void ShowError(string Message)
        {
            showEventModal = false;
            ErrorMessage = Message;
            IsError = true;
        }

        private void ShowSuccess(string Message)
        {
            showEventModal = false;
            SuccessMessage = Message;
            IsSuccess = true;
        }
    }
}
