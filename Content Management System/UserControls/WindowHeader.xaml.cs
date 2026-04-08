using System.Windows;
using System.Windows.Controls;

namespace Content_Management_System.UserControls
{
    /// <summary>
    /// Interaction logic for WindowHeader.xaml
    /// </summary>
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
        }


        public WindowHeader()
        {
            this.InitializeComponent();
            UpdateVisibility();
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
    }
}
