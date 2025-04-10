using System.Windows.Controls;
using System.Windows.Navigation;

namespace DataTest.UI.Utils;

public class NavigationService
{
    public event NavigatedEventHandler Navigated;

    public event NavigationFailedEventHandler NavigationFailed;

    private Frame _frame;

    public Frame Frame
    {
        get
        {
            if (_frame == null)
            {
                _frame = new Frame() { NavigationUIVisibility = NavigationUIVisibility.Hidden };
                RegisterFrameEvents();
            }

            return _frame;
        }
        set
        {
            UnregisterFrameEvents();
            _frame = value;
            RegisterFrameEvents();
        }
    }

    public bool CanGoBack => Frame.CanGoBack;

    public bool CanGoForward => Frame.CanGoForward;

    public void GoBack() => Frame.GoBack();

    public void GoForward() => Frame.GoForward();

    public bool Navigate(Uri sourcePageUri, object extraData = null)
    {
        if (Frame.CurrentSource != sourcePageUri)
        {
            return Frame.Navigate(sourcePageUri, extraData);
        }

        return false;
    }

    public bool Navigate(Type sourceType)
    {
        if (Frame.NavigationService?.Content?.GetType() != sourceType)
        {
            return Frame.Navigate(Activator.CreateInstance(sourceType));
        }

        return false;
    }

    private void RegisterFrameEvents()
    {
        if (_frame != null)
        {
            _frame.Navigated += Frame_Navigated;
            _frame.NavigationFailed += Frame_NavigationFailed;
        }
    }

    private void UnregisterFrameEvents()
    {
        if (_frame != null)
        {
            _frame.Navigated -= Frame_Navigated;
            _frame.NavigationFailed -= Frame_NavigationFailed;
        }
    }

    private void Frame_NavigationFailed(object sender, NavigationFailedEventArgs e) =>
        NavigationFailed?.Invoke(sender, e);

    private void Frame_Navigated(object sender, NavigationEventArgs e) => Navigated?.Invoke(sender, e);
}