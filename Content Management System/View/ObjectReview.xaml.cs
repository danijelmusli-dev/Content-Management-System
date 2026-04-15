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

namespace Content_Management_System.View
{
    /// <summary>
    /// Interaction logic for ObjectReview.xaml
    /// </summary>
    public partial class ObjectReview : Window
    {
        private Review LookReview { get; set; }
        public ObjectReview(Review review)
        {
            InitializeComponent();

            this.LookReview = review;
            this.DataContext = this.LookReview;
            this.SetUpStars();
        }


        private void SetUpStars()
        {
            float rating = this.LookReview.Rating;

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

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            try
            {
                Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri) { UseShellExecute = true });
            }
            catch { }
            e.Handled = true;
        }

        private void ReviewDescriptionRtb_Loaded(object sender, RoutedEventArgs e)
        {
            string relativePath = this.LookReview.DescriptionPath;
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

    }
}
