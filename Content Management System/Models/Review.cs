using System;
using System.ComponentModel;

namespace Content_Management_System.Models
{
    public class Review : INotifyPropertyChanged
    {
        private string _imagePath;
        private string _movieName;
        private string _link;
        private float _rating;
        private string _descriptionPath;
        private DateTime _objectCreationTime;
        private bool _isSelected;

        public string ImagePath
        {
            get => _imagePath;
            set => _imagePath = value;
        }

        public string MovieName
        {
            get => _movieName;
            set => _movieName = value;
        }

        public string Link
        {
            get => _link;
            set => _link = value;
        }

        public float Rating
        {
            get => _rating;
            set => _rating = value;
        }

        public string DescriptionPath
        {
            get => _descriptionPath;
            set => _descriptionPath = value;
        }

        public DateTime ObjectCreationTime
        {
            get => _objectCreationTime;
            set => _objectCreationTime = value;
        }

        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;
                    OnPropertyChanged(nameof(IsSelected));
                }
            }
        }

        public Review() { }

        public Review(string movieName, float rating, string link, string description, string imagePath, DateTime objectCreationTime)
        {
            _movieName = movieName;
            _rating = rating;
            _link = link;
            _descriptionPath = description;
            _imagePath = imagePath;
            _objectCreationTime = objectCreationTime;
        }

        public Review(Review source)
        {
            if (source == null)
                return;

            _movieName = source.MovieName;
            _rating = source.Rating;
            _link = source.Link;
            _descriptionPath = source.DescriptionPath;
            _imagePath = source.ImagePath;
            _objectCreationTime = source.ObjectCreationTime;
            _isSelected = source.IsSelected;
        }

        public Review Clone()
        {
            return new Review(this);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
