using Content_Management_System.Models;
using Content_Management_System.View;
using FontAwesome5;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Navigation;

namespace Content_Management_System.UserControls
{
    /// <summary>
    /// Interaction logic for ReviewItem.xaml
    /// </summary>
    public partial class ReviewItem : UserControl
    {
        public static readonly DependencyProperty EditButtonEnabledProperty =
        DependencyProperty.Register(
            nameof(EditButtonEnabled),          // ime property-ja
            typeof(bool),                       // tip
            typeof(ReviewItem),                 // vlasnik (klasa gde se registruje)
            new PropertyMetadata(false));       // podrazumevana vrednost

        // CLR wrapper
        public bool EditButtonEnabled
        {
            get => (bool)GetValue(EditButtonEnabledProperty);
            set => SetValue(EditButtonEnabledProperty, value);
        }

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
                e.Handled = true;
            }
            catch 
            {
                e.Handled = false;
            }
        }

        private void ReviewDescriptionRtb_Loaded(object sender, RoutedEventArgs e)
        {

            string relativePath = ((Review)this.DataContext).DescriptionPath;
            string path = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\Data\ObjectData\RtfFiles", relativePath);
            path = System.IO.Path.GetFullPath(path);

            if (File.Exists(path))
            {
                TextRange range = new TextRange(
                    ReviewDescriptionRtb.Document.ContentStart,
                    ReviewDescriptionRtb.Document.ContentEnd);

                using (FileStream fs = new FileStream(path, FileMode.Open))
                {
                    range.Load(fs, DataFormats.Rtf);
                }
            }
        }

        private void ViewReviewBtn_Click(object sender, RoutedEventArgs e)
        {
            if(this.DataContext is Review)
            {
                ObjectReview objectReviewWindow = new ObjectReview(this.DataContext as Review);
                objectReviewWindow.ShowDialog();
            }
        }

        private void EditReviewBtn_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
