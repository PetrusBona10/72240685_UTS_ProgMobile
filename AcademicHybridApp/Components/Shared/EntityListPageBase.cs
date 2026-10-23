using AcademicHybridApp.Models;
using AcademicHybridApp.Services;
using Microsoft.AspNetCore.Components;

namespace AcademicHybridApp.Components.Shared
{
    public abstract class EntityListPageBase<T> : ComponentBase where T : class
    {
        [Inject]
        protected ToastService ToastService { get; set; } = default!;

        [Inject]
        protected NavigationManager Navigation { get; set; } = default!;

        protected List<T> Items { get; private set; } = new();

        protected bool IsLoading { get; private set; } = true;

        protected string? ErrorMessage { get; private set; }

        protected bool ConfirmVisible { get; private set; }

        protected bool IsDeleting { get; private set; }

        private T? ItemToDelete { get; set; }

        protected abstract string EntityName { get; }

        protected abstract string AddPath { get; }

        protected abstract Task<ServiceResult<List<T>>> LoadItemsAsync();

        protected abstract Task<ServiceResult> DeleteItemAsync(T item);

        protected abstract string GetItemName(T item);

        protected abstract string GetEditPath(T item);

        protected string DeleteMessage => ItemToDelete is null
            ? $"Data {EntityName.ToLowerInvariant()} akan dihapus permanen dan tidak dapat dikembalikan."
            : $"Data {EntityName.ToLowerInvariant()} {GetItemName(ItemToDelete)} akan dihapus permanen dan tidak dapat dikembalikan.";

        protected override Task OnInitializedAsync()
        {
            return LoadAsync();
        }

        protected async Task LoadAsync()
        {
            IsLoading = true;
            ErrorMessage = null;
            StateHasChanged();

            await FetchAsync();

            IsLoading = false;
            StateHasChanged();
        }

        private async Task RefreshAsync()
        {
            await FetchAsync();
            StateHasChanged();
        }

        private async Task FetchAsync()
        {
            try
            {
                var result = await LoadItemsAsync();

                if (result.Success)
                {
                    Items = result.Data ?? new List<T>();
                    ErrorMessage = null;
                }
                else
                {
                    ErrorMessage = result.Message;
                }
            }
            catch (Exception)
            {
                ErrorMessage = ServiceMessages.Unexpected;
            }
        }

        protected void NavigateToAdd()
        {
            Navigation.NavigateTo(AddPath);
        }

        protected void NavigateToEdit(T item)
        {
            Navigation.NavigateTo(GetEditPath(item));
        }

        protected void OpenDeleteDialog(T item)
        {
            ItemToDelete = item;
            IsDeleting = false;
            ConfirmVisible = true;
        }

        protected void CancelDelete()
        {
            if (IsDeleting)
            {
                return;
            }

            ConfirmVisible = false;
            ItemToDelete = null;
        }

        protected async Task ConfirmDeleteAsync()
        {
            if (ItemToDelete is null || IsDeleting)
            {
                return;
            }

            var item = ItemToDelete;
            IsDeleting = true;
            StateHasChanged();

            ServiceResult result;

            try
            {
                result = await DeleteItemAsync(item);
            }
            catch (Exception)
            {
                result = ServiceResult.Fail(ServiceMessages.Unexpected);
            }

            IsDeleting = false;
            ConfirmVisible = false;
            ItemToDelete = null;

            if (result.Success)
            {
                ToastService.ShowSuccess($"{EntityName} berhasil dihapus", result.Message);
                StateHasChanged();
                await RefreshAsync();
            }
            else
            {
                ToastService.ShowError($"Gagal menghapus {EntityName.ToLowerInvariant()}", result.Message);
                StateHasChanged();
            }
        }
    }
}
