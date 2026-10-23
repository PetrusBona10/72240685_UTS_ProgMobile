namespace AcademicHybridApp.Components.Shared
{
    public enum ToastType
    {
        Success,
        Error
    }

    public sealed class ToastMessage
    {
        public Guid Id { get; } = Guid.NewGuid();

        public required ToastType Type { get; init; }

        public required string Title { get; init; }

        public required string Message { get; init; }
    }

    public sealed class ToastService
    {
        private readonly List<ToastMessage> _toasts = new();
        private readonly object _sync = new();

        public event Action? OnChange;

        public IReadOnlyList<ToastMessage> Toasts
        {
            get
            {
                lock (_sync)
                {
                    return _toasts.ToArray();
                }
            }
        }

        public void ShowSuccess(string title, string message)
        {
            Add(ToastType.Success, title, message);
        }

        public void ShowError(string title, string message)
        {
            Add(ToastType.Error, title, message);
        }

        public void Remove(Guid id)
        {
            bool removed;

            lock (_sync)
            {
                removed = _toasts.RemoveAll(toast => toast.Id == id) > 0;
            }

            if (removed)
            {
                OnChange?.Invoke();
            }
        }

        private void Add(ToastType type, string title, string message)
        {
            var toast = new ToastMessage
            {
                Type = type,
                Title = title,
                Message = message
            };

            lock (_sync)
            {
                _toasts.Add(toast);
            }

            OnChange?.Invoke();
        }
    }
}
