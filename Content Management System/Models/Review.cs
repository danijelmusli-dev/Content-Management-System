using System;
using System.ComponentModel;

namespace Content_Management_System.Models
{
    public class Review
    {

        private string _imagePath;
        private string _movieName;
        private string _link;
        private float _rating;
        private string _description;
        private DateTime _objectCreationTime;
        private bool _isSelected;

        public string ImagePath
        {
            get => _imagePath;
            set
            {
                if (_imagePath != value)
                {
                    _imagePath = value;
                    OnPropertyChanged(nameof(ImagePath));
                }
            }
        }

        public string MovieName
        {
            get => _movieName;
            set
            {
                if (_movieName != value)
                {
                    _movieName = value;
                    OnPropertyChanged(nameof(MovieName));
                }
            }
        }

        public string Link
        {
            get => _link;
            set
            {
                if (_link != value)
                {
                    _link = value;
                    OnPropertyChanged(nameof(Link));
                }
            }
        }

        public float Rating
        {
            get => _rating;
            set
            {
                if (_rating != value)
                {
                    _rating = value;
                    OnPropertyChanged(nameof(Rating));
                }
            }
        }

        public string Description
        {
            get => _description;
            set
            {
                if (_description != value)
                {
                    _description = value;
                    OnPropertyChanged(nameof(Description));
                }
            }
        }

        public DateTime ObjectCreationTime
        {
            get => _objectCreationTime;
            set
            {
                if (_objectCreationTime != value)
                {
                    _objectCreationTime = value;
                    OnPropertyChanged(nameof(ObjectCreationTime));
                }
            }
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

        public Review()
        {
            _objectCreationTime = DateTime.Now;
        }
        public Review(string movieName, float rating, string link, string description, string imagePath, DateTime objectCreationTime)
        {
            _rating = rating;
            _movieName = movieName;
            _link = link;
            _description = description;
            _imagePath = imagePath;
            _objectCreationTime = objectCreationTime;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    }
}
