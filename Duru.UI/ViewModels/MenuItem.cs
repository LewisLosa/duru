using System.Windows;
using MahApps.Metro.Controls;

namespace Duru.UI.ViewModels;

public class MenuItem : HamburgerMenuIconItem
{
    /// <summary>Identifies the <see cref="NavigationDestination" /> dependency property.</summary>
    public static readonly DependencyProperty NavigationDestinationProperty
        = DependencyProperty.Register(
            nameof(NavigationDestination),
            typeof(Uri),
            typeof(MenuItem),
            new PropertyMetadata(default(Uri)));

    /// <summary>Identifies the <see cref="NavigationType" /> dependency property.</summary>
    public static readonly DependencyProperty NavigationTypeProperty
        = DependencyProperty.Register(
            nameof(NavigationType),
            typeof(Type),
            typeof(MenuItem),
            new PropertyMetadata(default(Type)));

    public Uri NavigationDestination
    {
        get => (Uri)GetValue(NavigationDestinationProperty);
        set => SetValue(NavigationDestinationProperty, value);
    }

    public Type NavigationType
    {
        get => (Type)GetValue(NavigationTypeProperty);
        set => SetValue(NavigationTypeProperty, value);
    }

    public bool IsNavigation => NavigationDestination != null;
}