using AcademicHybridApp.Models;
using AcademicHybridApp.Services;
using Microsoft.AspNetCore.Components;

namespace AcademicHybridApp.Components.Shared
{
    public abstract class EntityFormPageBase<TForm> : ComponentBase where TForm : class, new()
    {
        [Inject]
        protected ToastService ToastService { get; set; } = default!;

        [Inject]
        protected NavigationManager Navigation { get; set; } = default!;

        protected TForm FormModel { get; } = new();

        protected bool IsLoading { get; private set; } = true;

        protected bool IsSaving { get; private set; }

        protected string? LoadErrorMessage { get; set; }

        protected abstract string ListPath { get; }

        protected abstract string SaveSuccessTitle { get; }

        protected abstract string SaveErrorTitle { get; }

        protected abstract Task LoadFormAsync();

        protected abstract Task<ServiceResult> SaveModelAsync();

        protected override Task OnInitializedAsync()
        {
            return LoadAsync();
        }

        protected async Task LoadAsync()
        {
            IsLoading = true;
            LoadErrorMessage = null;
            StateHasChanged();

            try
            {
                await LoadFormAsync();
            }
            catch (Exception)
            {
                LoadErrorMessage = ServiceMessages.Unexpected;
            }
            finally
            {
                IsLoading = false;
                StateHasChanged();
            }
        }

        protected async Task SaveAsync()
        {
            if (IsSaving)
            {
                return;
            }

            IsSaving = true;
            StateHasChanged();

            ServiceResult result;

            try
            {
                result = await SaveModelAsync();
            }
            catch (Exception)
            {
                result = ServiceResult.Fail(ServiceMessages.Unexpected);
            }

            if (result.Success)
            {
                ToastService.ShowSuccess(SaveSuccessTitle, result.Message);
                Navigation.NavigateTo(ListPath);
                return;
            }

            ToastService.ShowError(SaveErrorTitle, result.Message);
            IsSaving = false;
            StateHasChanged();
        }
    }
}
