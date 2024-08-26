using Assessment3.Services;
using Microsoft.AspNetCore.Components;

namespace Assessment3.Pages.Events
{
    public partial class ManageEvents
    {
        [Inject] IEventService service { get; set; }
        public static List<Models.Event> Events { get; set; } = [];
        public static string SearchTerm { get; set; } = "";
        public static List<Models.Event> EventsFilterd { get; set; } = Events.Where(x => x.Name.ToLower().Contains(SearchTerm)).ToList();

        protected override async Task OnInitializedAsync()
        {
            await GetEvents();
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

        private async Task OnClickDelete(Models.Event model)
        {
            try
            {
                showUpdateModal = false;
                IsLoading = true;
                if (!await service.DeleteByIdAsync(model.Id))
                {
                    throw new Exception();
                }
                IsLoading = false;
            }
            catch (Exception e)
            {
                ShowError(e.Message);
            }
        }
        public bool showUpdateModal { get; set; }
        public Models.Event SelectedModel { get; set; } = new();
        private void OnClickUpdate(Models.Event model)
        {
            SelectedModel = model;
            showUpdateModal = true;
        }

        private async Task OnSubmitUpdate(Models.Event model)
        {
            showUpdateModal = false;
            IsLoading = true;
            try
            {
                if (!await service.UpdateAsync(model))
                {
                    throw new Exception();
                }
                IsLoading = false;
            }
            catch (Exception e)
            {
                ShowError(e.Message);
            }
        }

        public bool IsLoading { get; set; }
        public bool IsError { get; set; }
        public bool IsSuccess { get; set; }
        public string SuccessMessage { get; set; } = "";
        public string ErrorMessage { get; set; } = "";

        private void ShowError(string Message)
        {
            IsLoading = false;
            showUpdateModal = false;
            ErrorMessage = Message;
            IsError = true;
        }

        private void ShowSuccess(string Message)
        {
            IsLoading = false;
            showUpdateModal = false;
            SuccessMessage = Message;
            IsSuccess = true;
        }
    }
}
