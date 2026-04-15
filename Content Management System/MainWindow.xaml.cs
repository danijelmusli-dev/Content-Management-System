using Content_Management_System.Models;
using Content_Management_System.UserControls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media.Animation;
using System.Xml.Linq;

namespace Content_Management_System
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public User CurrentUser { get; set; }
        public User AddedUser { get; set; }
        public Review AddedReview { get; set; }
        public bool IsAdminLogged { get; set; }

        private bool IsReviewsChanged { get; set; } = false;
        public ObservableCollection<Review> Reviews { get; set; } = new ObservableCollection<Review>();

        public MainWindow()
        {
            InitializeComponent();

            this.DataContext = this;

            this.Reviews = new ObservableCollection<Review>(LoadObjects());

            this.Visibility = Visibility.Hidden;

            var loginWindow = new View.Login();
            bool? someoneLogged = loginWindow.ShowDialog();

            if (someoneLogged == true)
            {
                this.CurrentUser = loginWindow.User;
                this.IsAdminLogged = (this.CurrentUser.Role == UserRole.Role.Admin);
            }

        }

        public List<Review> LoadObjects()
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\Data\ObjectData\XmlFiles", "Objects.xml");
            path = Path.GetFullPath(path);

            XDocument document = XDocument.Load(path);

            var allObjects = document.Descendants("Review")
                .Select(review => new Review
                {
                    MovieName = review.Element("MovieName")?.Value,
                    Rating = float.Parse(review.Element("Rating")?.Value ?? "0"),
                    Link = review.Element("Link")?.Value,
                    DescriptionPath = review.Element("DescriptionPath")?.Value,
                    ImagePath = review.Element("ImagePath")?.Value,
                    ObjectCreationTime = DateTime.Parse(
                        review.Element("ObjectCreationTime")?.Value ?? DateTime.MinValue.ToString() )
                }).ToList();

            return allObjects;
        }
        public void SaveObjects()
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\Data\ObjectData\XmlFiles", "Objects.xml");
            path = Path.GetFullPath(path);
            XDocument document = new XDocument(
                new XElement("Reviews",
                    Reviews.Select(review =>
                        new XElement("Review",
                            new XElement("MovieName", review.MovieName),
                            new XElement("Rating", review.Rating),
                            new XElement("Link", review.Link),
                            new XElement("DescriptionPath", review.DescriptionPath),
                            new XElement("ImagePath", review.ImagePath),
                            new XElement("ObjectCreationTime", review.ObjectCreationTime.ToString("o")
                        )
                    )
                )
            ));
            document.Save(path);
        }

        private void AddReviewBtn_Click(object sender, RoutedEventArgs e)
        {
            var addObjectWindow = new View.ObjectAdd();
            bool? result = addObjectWindow.ShowDialog();

            if (result == true)
            { 
                AddedReview = addObjectWindow.NewReview;
                this.Reviews.Insert(0, AddedReview);

                this.IsReviewsChanged = true;
            }
        
        }

        private void DeleteReviewBtn_Click(object sender, RoutedEventArgs e)
        {
            List<Review> selected = Reviews.Where(x => x.IsSelected).ToList();

            int itemStartCount = this.Reviews.Count;

            foreach (Review review in selected)
            {
                string relativePath = review.DescriptionPath;
                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\Data\ObjectData\RtfFiles", relativePath);
                path = Path.GetFullPath(path);

                if (File.Exists(path))
                {
                    try
                    {
                        File.Delete(path);
                    }
                    catch (Exception re)
                    {
                        MessageBox.Show(re.Message);
                    }
                }

                Reviews.Remove(review);
            }

            if (itemStartCount != this.Reviews.Count) 
            {
                this.IsReviewsChanged = true;
            }

        }

        private void CheckAllCb_Checked(object sender, RoutedEventArgs e)
        {
            bool checkState = (this.CheckAllCb.IsChecked == true);

            foreach (Review review in Reviews)
            {
                review.IsSelected = checkState;
            }

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (this.IsReviewsChanged)
            {
                this.SaveObjects();
            }
        }

        private void ReviewItem_ReviewEdited(object sender, RoutedEventArgs e)
        {
            if (sender is ReviewItem item && item.DataContext is Review original)
            {
                if (e.Source is ReviewItem revItem && item.DataContext is Review edited)
                {
                    int index = Reviews.IndexOf(original);
                    if (index >= 0)
                    {
                        Reviews.RemoveAt(index);
                        this.Reviews.Insert(index, edited);

                        this.IsReviewsChanged = true;
                    }
                }
            }
        }
    }
}
