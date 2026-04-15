using Content_Management_System.Models;
using Content_Management_System.View;
using FontAwesome5;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Navigation;

namespace Content_Management_System.UserControls
{

    public partial class ReviewItem : UserControl
    {
        public static readonly DependencyProperty EditButtonEnabledProperty =
        DependencyProperty.Register(nameof(EditButtonEnabled), typeof(bool), 
            typeof(ReviewItem), new PropertyMetadata(false));

        public static readonly RoutedEvent ReviewItemEditedEvent =
            EventManager.RegisterRoutedEvent(
                "ReviewItemEditedEvent",
                RoutingStrategy.Bubble,
                typeof(RoutedEvent),
                typeof(ReviewItem));
        
        public event RoutedEventHandler ReviewEdited
        {
            add { AddHandler(ReviewItemEditedEvent, value); }
            remove { RemoveHandler(ReviewItemEditedEvent, value); }
        }

        public bool EditButtonEnabled
        {
            get => (bool)GetValue(EditButtonEnabledProperty);
            set => SetValue(EditButtonEnabledProperty, value);
        }

        public ReviewItem()
        {
            InitializeComponent();
            SetUpStars();
        }

        private void UserControl_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            SetUpStars();
        }

        private void Review_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Review.IsSelected))
            {
                ReviewCheckBox.IsChecked = ((Review)sender).IsSelected;
            }
        }

        private void SetUpStars()
        {
            if (DataContext is Review review)
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
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\Data\ObjectData\RtfFiles", relativePath);
            path = Path.GetFullPath(path);

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
            if (this.DataContext is Review original)
            {

                Review copy = original.Clone();
                ObjectEdit objectEdit = new ObjectEdit(copy);

                if (objectEdit.ShowDialog() == true)
                {
                    
                    original.MovieName = objectEdit.EditReview.MovieName;
                    original.Rating = objectEdit.EditReview.Rating;
                    original.Link = objectEdit.EditReview.Link;
                    original.DescriptionPath = objectEdit.EditReview.DescriptionPath;
                    original.ImagePath = objectEdit.EditReview.ImagePath;

                    this.SetUpStars();
                    this.ReviewDescriptionRtb_Loaded(this, new RoutedEventArgs());

                    RaiseEvent(new RoutedEventArgs(ReviewItemEditedEvent, this.DataContext as Review));

                }

            }
        }


    }
}
