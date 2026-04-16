using FontAwesome5;
using System;
using System.Linq;
using System.Security.Policy;
using System.Windows;
using System.Windows.Controls;

namespace Content_Management_System.UserControls
{
    public partial class WindowHeader : UserControl
    {

        public static readonly DependencyProperty MinimizedButtonProperty =
        DependencyProperty.Register(nameof(MinimizedButton), typeof(bool), typeof(WindowHeader),
            new PropertyMetadata(true, OnButtonVisibilityChanged));

        public static readonly DependencyProperty MaximizedButtonProperty =
            DependencyProperty.Register(nameof(MaximizedButton), typeof(bool), typeof(WindowHeader),
                new PropertyMetadata(true, OnButtonVisibilityChanged));

        public static readonly DependencyProperty CloseButtonProperty =
            DependencyProperty.Register(nameof(CloseButton), typeof(bool), typeof(WindowHeader),
                new PropertyMetadata(true, OnButtonVisibilityChanged));

        public static readonly DependencyProperty DarkModeButtonProperty =
            DependencyProperty.Register(nameof(DarkModeButton), typeof(bool), typeof(WindowHeader),
                new PropertyMetadata(true, OnButtonVisibilityChanged));

        public bool MinimizedButton
        {
            get => (bool)GetValue(MinimizedButtonProperty);
            set => SetValue(MinimizedButtonProperty, value);
        }

        public bool MaximizedButton
        {
            get => (bool)GetValue(MaximizedButtonProperty);
            set => SetValue(MaximizedButtonProperty, value);
        }

        public bool CloseButton
        {
            get => (bool)GetValue(CloseButtonProperty);
            set => SetValue(CloseButtonProperty, value);
        }

        public bool DarkModeButton
        {
            get => (bool)GetValue(DarkModeButtonProperty);
            set => SetValue(DarkModeButtonProperty, value);
        }

        private static void OnButtonVisibilityChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (WindowHeader)d;
            control.UpdateVisibility();
        }

        private void UpdateVisibility()
        {
            WindowMinimizeBtn.Visibility = MinimizedButton ? Visibility.Visible : Visibility.Collapsed;
            WindowMaximizeBtn.Visibility = MaximizedButton ? Visibility.Visible : Visibility.Collapsed;
            WindowCloseBtn.Visibility = CloseButton ? Visibility.Visible : Visibility.Collapsed;
            DarkModeBtn.Visibility = DarkModeButton ? Visibility.Visible : Visibility.Collapsed;
        }

        public WindowHeader()
        {
            this.InitializeComponent();
            UpdateVisibility();

            bool isDarkMode = (bool)Application.Current.Resources["IsDarkMode"];
            if (this.DarkModeBtn.Content is SvgAwesome icon)
            {
                icon.Icon = (isDarkMode) ? EFontAwesomeIcon.Regular_Sun : EFontAwesomeIcon.Regular_Moon;
            }
        }

        private void WindowCloseBtn_Click(object sender, RoutedEventArgs e)
        {
            var parentWindow = Window.GetWindow(this);
            parentWindow?.Close();
        }

        private void WindowMaximizeBtn_Click(object sender, RoutedEventArgs e)
        {
            var parentWindow = Window.GetWindow(this);
            if (parentWindow != null)
            {
                parentWindow.WindowState = (parentWindow.WindowState != WindowState.Maximized) ? WindowState.Maximized : WindowState.Normal;
            }
        }

        private void WindowMinimizeBtn_Click(object sender, RoutedEventArgs e)
        {
            var parentWindow = Window.GetWindow(this);
            if (parentWindow != null)
            {
                parentWindow.WindowState = WindowState.Minimized;
            }
        }

        private void DarkModeBtn_Click(object sender, RoutedEventArgs e)
        {
            if (this.DarkModeBtn.Content is SvgAwesome icon)
            {
                bool isDarkMode = (bool)Application.Current.Resources["IsDarkMode"];

                string currentMode = (isDarkMode)  ? "Dark" : "Light";
                string nextMode    = (!isDarkMode) ? "Dark" : "Light";
                icon.Icon = (!isDarkMode) ? EFontAwesomeIcon.Regular_Sun : EFontAwesomeIcon.Regular_Moon;

                var dictionaries = Application.Current.Resources.MergedDictionaries;
                var currentResource = dictionaries.FirstOrDefault(x => x.Source != null && x.Source.OriginalString.Contains(currentMode));

                if (currentResource != null)
                {
                    dictionaries.Remove(currentResource);

                    ResourceDictionary nextResource = new ResourceDictionary();
                    nextResource.Source = new Uri($"Styles/BaseColors{nextMode}Theme.xaml", UriKind.Relative);

                    dictionaries.Add(nextResource);
                    Application.Current.Resources["IsDarkMode"] = !isDarkMode;
                }
            }
        }


    }
}
