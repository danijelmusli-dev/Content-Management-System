using FontAwesome5;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Diagnostics;
using System.Windows.Navigation;

namespace Content_Management_System.UserControls
{
    /// <summary>
    /// Interaction logic for ReviewItem.xaml
    /// </summary>
    public partial class ReviewItem : UserControl
    {

        public ReviewItem()
        {
            InitializeComponent();
        }

        private void UserControl_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue is Models.Review oldReview)
                oldReview.PropertyChanged -= Review_PropertyChanged;

            if (e.NewValue is Models.Review newReview)
                newReview.PropertyChanged += Review_PropertyChanged;

            SetUpStars();
        }

        private void Review_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Models.Review.IsSelected))
            {
                // Forsiraj refresh CheckBox-a
                ReviewCheckBox.IsChecked = ((Models.Review)sender).IsSelected;
            }
        }

        private void SetUpStars()
        {
            if (DataContext is Content_Management_System.Models.Review review)
            {
                float rating = review.Rating;

                for (int i = 0; i < StarsPanel.Children.Count; i++)
                {
                    if (StarsPanel.Children[i] is SvgAwesome icon)
                    {
                        if (rating >= 1)
                        {
                            icon.Icon = EFontAwesomeIcon.Solid_Star;
                        }
                        else if (rating >= 0.5)
                        {
                            icon.Icon = EFontAwesomeIcon.Solid_StarHalfAlt;
                        }
                        else
                        {
                            icon.Icon = EFontAwesomeIcon.Regular_Star;
                        }
                    }
                    rating -= 1;
                }
            }
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            try
            {
                Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri) { UseShellExecute = true });
            }
            catch { }
            e.Handled = true;
        }

    }
}
