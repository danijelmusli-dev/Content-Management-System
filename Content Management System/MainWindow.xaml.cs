using Content_Management_System.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
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

        int ItemsCountStart = 0;
        int ItemsCountEnd = 0;
        public ObservableCollection<Review> Reviews { get; set; } = new ObservableCollection<Review>();

        public MainWindow()
        {
            InitializeComponent();

            this.DataContext = this;

            this.Reviews = new ObservableCollection<Review>(LoadObjects());
            this.ItemsCountStart = this.Reviews.Count;

            this.Visibility = Visibility.Hidden;

            var loginWindow = new View.Login();
            bool? someoneLogged = loginWindow.ShowDialog();

            if (someoneLogged == true)
            {
                this.CurrentUser = loginWindow.User;
            }

        }

        public List<Review> LoadObjects()
        {
            string path = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\Data", "Objects.xml");
            path = System.IO.Path.GetFullPath(path);

            XDocument document = XDocument.Load(path);

            var allObjects = document.Descendants("Review")
                .Select(review => new Review
                {
                    MovieName = review.Element("MovieName")?.Value,
                    Rating = float.Parse(review.Element("Rating")?.Value ?? "0"),
                    Link = review.Element("Link")?.Value,
                    Description = review.Element("Description")?.Value,
                    ImagePath = review.Element("ImagePath")?.Value,
                    ObjectCreationTime = DateTime.Now
                }).ToList();

            return allObjects;
        }
        public void SaveObjects()
        {
            string path = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\Data", "Objects.xml");
            path = System.IO.Path.GetFullPath(path);
            XDocument document = new XDocument(
                new XElement("Reviews",
                    Reviews.Select(review =>
                        new XElement("Review",
                            new XElement("MovieName", review.MovieName),
                            new XElement("Rating", review.Rating),
                            new XElement("Link", review.Link),
                            new XElement("Description", review.Description),
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
                
            }
        
        }

        private void DeleteReviewBtn_Click(object sender, RoutedEventArgs e)
        {
            List<Review> selected = Reviews.Where(x => x.IsSelected).ToList();

            foreach (Review review in selected)
            {
                Reviews.Remove(review);
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

        private void EditReviewBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.ItemsCountEnd = this.Reviews.Count;
            if (this.ItemsCountStart != this.ItemsCountEnd)
            {
                this.SaveObjects();
            }
        }

    }
}
