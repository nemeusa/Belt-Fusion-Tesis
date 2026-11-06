using PopupFramework.Core;

namespace PopupFramework.Popups
{
    public abstract class PopupActionAndCloseBinder<TData> : PopupActionBinder<TData> where TData : class, IPopupData
    {
        protected sealed override void AfterInvoke()
        {
            if (ServiceLocator.Instance.TryGetDependency(out IPopupQueue queue))
                queue.Hide();
        }
    }
}
