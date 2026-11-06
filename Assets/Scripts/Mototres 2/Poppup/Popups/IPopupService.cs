namespace PopupFramework.Popups
{
    public interface IPopupService<TData> where TData : class, IPopupData
    {
        void Show(TData data);
        void Hide();
    }
}
