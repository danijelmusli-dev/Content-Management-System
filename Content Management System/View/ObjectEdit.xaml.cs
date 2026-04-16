using Content_Management_System.Models;
using FontAwesome5;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Content_Management_System.View
{
    public partial class ObjectEdit : Window
    {
        public Review EditReview { get; set; } = new Review();
        public ObjectEdit(Review review)
        {
            InitializeComponent();

            this.EditReview = review;
            this.DataContext = this.EditReview;

            this.SetUpStars();
        }

        private void SetUpStars()
        {
            float rating = EditReview.Rating;

            for (int i = 0; i < StarsPanel.Children.Count; i++)
            {
                if (StarsPanel.Children[i] is Button btn && btn.Content is SvgAwesome icon)
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
        
        private void FontFamilyComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            this.FontFamilyComboBox.ItemsSource = System.Windows.Media.Fonts.SystemFontFamilies.OrderBy(f => f.Source);
            this.FontFamilyComboBox.SelectedItem = this.FontFamilyComboBox.Items[0];
        }
        private void FontSizeComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            this.FontSizeComboBox.ItemsSource = new List<double>
            { 8, 9, 10, 11, 12, 14, 16, 18, 20, 22, 24, 28, 32, 36, 48, 72 };
            this.FontSizeComboBox.SelectedItem = this.FontSizeComboBox.Items[4];
        }
        private void ColorComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            this.ColorComboBox.ItemsSource = typeof(Colors).GetProperties();
            this.ColorComboBox.SelectedItem = this.ColorComboBox.Items[0];
        }

        private void ImageAddStackPanel_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif";

            dialog.Multiselect = false;

            if (dialog.ShowDialog() == true)
            {
                try
                {
                    string filePath = dialog.FileName;
                    this.ObjectImage.Source = new BitmapImage(new Uri(filePath, UriKind.RelativeOrAbsolute));

                    this.ObjectImageIcon.Visibility = Visibility.Collapsed;
                }
                catch
                {
                    this.ObjectImageIcon.Icon = EFontAwesomeIcon.Regular_SadCry;
                    this.ObjectImageIcon.Visibility = Visibility.Visible;
                    this.ObjectImageErrTb.Text = "";
                }
            }

        }

        private void StarBtn1_Click(object sender, RoutedEventArgs e)
        {
            Button clickedBtn = sender as Button;
            SvgAwesome svgClicked = clickedBtn.Content as SvgAwesome;
            int index = this.StarsPanel.Children.IndexOf(clickedBtn);

            Point mouseDownPoint = Mouse.GetPosition(clickedBtn);
            double x = mouseDownPoint.X;

            EditReview.Rating = index;

            for (int i = 0; i < this.StarsPanel.Children.Count; i++)
            {
                Button btn = StarsPanel.Children[i] as Button;
                SvgAwesome svg = btn.Content as SvgAwesome;

                if (svg != null)
                {
                    svg.Icon = (index > i) ? EFontAwesomeIcon.Solid_Star : EFontAwesomeIcon.Regular_Star;
                }
            }

            if (svgClicked != null)
            {
                svgClicked.Icon = (x <= ((clickedBtn.Content as SvgAwesome).Width / 2)) ? EFontAwesomeIcon.Solid_StarHalfAlt : EFontAwesomeIcon.Solid_Star;
                EditReview.Rating += (x <= ((clickedBtn.Content as SvgAwesome).Width / 2)) ? 0.5f : 1;
            }
        }

        private void BoldBtn_Click(object sender, RoutedEventArgs e)
        {
            FontWeight fontWeight = (this.BoldBtn.IsChecked == true) ? FontWeights.Bold : FontWeights.Regular;
            this.DescriptionRichTextBox.Selection.ApplyPropertyValue(Inline.FontWeightProperty, fontWeight);
        }
        private void ItalicBtn_Click(object sender, RoutedEventArgs e)
        {
            FontStyle fontStyle = (this.ItalicBtn.IsChecked == true) ? FontStyles.Italic : FontStyles.Normal;
            this.DescriptionRichTextBox.Selection.ApplyPropertyValue(Inline.FontStyleProperty, fontStyle);
        }
        private void UnderlineBtn_Click(object sender, RoutedEventArgs e)
        {
            this.DescriptionRichTextBox.Selection.ApplyPropertyValue(Inline.TextDecorationsProperty, (this.UnderlineBtn.IsChecked == true) ? TextDecorations.Underline : null);
        }

        private void FontFamilyComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.FontFamilyComboBox.SelectedItem != null && !this.DescriptionRichTextBox.Selection.IsEmpty)
            {
                this.DescriptionRichTextBox.Selection.ApplyPropertyValue(Inline.FontFamilyProperty, this.FontFamilyComboBox.SelectedItem);
            }
        }
        private void FontSizeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.FontSizeComboBox.SelectedItem != null && !this.DescriptionRichTextBox.Selection.IsEmpty)
            {
                this.DescriptionRichTextBox.Selection.ApplyPropertyValue(Inline.FontSizeProperty, this.FontSizeComboBox.SelectedItem);
            }
        }
        private void ColorComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ColorComboBox.SelectedItem is PropertyInfo prop && !DescriptionRichTextBox.Selection.IsEmpty)
            {
                Color selectedColor = (Color)prop.GetValue(null);
                DescriptionRichTextBox.Selection.ApplyPropertyValue(Inline.ForegroundProperty, new SolidColorBrush(selectedColor));
            }
        }

        private void IncreaseTextSizeBtn_Click(object sender, RoutedEventArgs e)
        {
            if (this.DescriptionRichTextBox.Selection != null && (this.FontSizeComboBox.SelectedIndex + 1 < this.FontSizeComboBox.Items.Count))
            {
                int nextSizeIndex = this.FontSizeComboBox.SelectedIndex + 1;
                this.FontSizeComboBox.SelectedIndex = nextSizeIndex;
                this.DescriptionRichTextBox.Selection.ApplyPropertyValue(Inline.FontSizeProperty, this.FontSizeComboBox.Items.GetItemAt(nextSizeIndex));
            }
        }
        private void DecreaseTextSizeBtn_Click(object sender, RoutedEventArgs e)
        {
            if (this.DescriptionRichTextBox.Selection != null && (this.FontSizeComboBox.SelectedIndex - 1 >= 0))
            {
                int nextSizeIndex = this.FontSizeComboBox.SelectedIndex - 1;
                this.FontSizeComboBox.SelectedIndex = nextSizeIndex;
                this.DescriptionRichTextBox.Selection.ApplyPropertyValue(Inline.FontSizeProperty, this.FontSizeComboBox.Items.GetItemAt(nextSizeIndex));
            }
        }

        private void SaveRtxFileBtn_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                DefaultExt = ".rtf",
                Filter = "RTF Files|*.rtf"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                using (FileStream fs = new FileStream(saveFileDialog.FileName, FileMode.Create))
                {
                    TextRange textRange = new TextRange(this.DescriptionRichTextBox.Document.ContentStart, this.DescriptionRichTextBox.Document.ContentEnd);
                    textRange.Save(fs, DataFormats.Rtf);
                }
            }
        }

        private void CountWordBtn_Click(object sender, RoutedEventArgs e)
        {
            TextRange textRange = new TextRange(this.DescriptionRichTextBox.Document.ContentStart, this.DescriptionRichTextBox.Document.ContentEnd);
            int wordCount = textRange.Text
            .Split((char[])null, StringSplitOptions.RemoveEmptyEntries).Length;

            MessageWindow messageWindow = new MessageWindow($"Word Count: {wordCount}", EFontAwesomeIcon.Regular_FileWord, MessageWindow.MessageBoxCause.Info);
            messageWindow.ShowDialog();
        }

        private void DiscardObjectBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void EditObjectBtn_Click(object sender, RoutedEventArgs e)
        {
            if (this.ValidateInput())
            {
                try
                {
                    var source = this.ObjectImage.Source as BitmapSource;
                    if (source is BitmapImage bmp)
                    {
                        if (EditReview.ImagePath != bmp.UriSource.AbsolutePath)
                        {
                            EditReview.ImagePath = bmp.UriSource.LocalPath;
                        }
                    }

                    EditReview.MovieName = this.ObjectNameTb.Text;
                    EditReview.Link = this.ObjectLinkTb.Text;

                    string path = this.EditReview.DescriptionPath;

                    using (FileStream fs = new FileStream(path, FileMode.Create))
                    {
                        TextRange textRange = new TextRange(this.DescriptionRichTextBox.Document.ContentStart, this.DescriptionRichTextBox.Document.ContentEnd);
                        textRange.Save(fs, DataFormats.Rtf);
                    }
                    //EditReview.ObjectCreationTime = DateTime.Now;

                    this.DialogResult = true;
                    this.Close();
                }
                catch
                {
                    MessageWindow messageWindow = new MessageWindow("Error while editing", EFontAwesomeIcon.Solid_SadTear, MessageWindow.MessageBoxCause.Info);
                    messageWindow.ShowDialog();
                    this.DialogResult = false;
                    this.Close();
                }
            }
        }

        private void DescriptionRichTextBox_Loaded(object sender, RoutedEventArgs e)
        {
            string relativePath = this.EditReview.DescriptionPath;
            string path = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\Data\ObjectData\RtfFiles", relativePath);
            path = System.IO.Path.GetFullPath(path);

            if (File.Exists(path))
            {
                TextRange range = new TextRange(
                    DescriptionRichTextBox.Document.ContentStart,
                    DescriptionRichTextBox.Document.ContentEnd);

                using (FileStream fs = new FileStream(path, FileMode.Open))
                {
                    range.Load(fs, DataFormats.Rtf);
                }
            }
        }
        private void DescriptionRichTextBox_SelectionChanged(object sender, RoutedEventArgs e)
        {
            if (this.DescriptionRichTextBox.Selection != null)
            {
                object fontFamily = this.DescriptionRichTextBox.Selection.GetPropertyValue(Inline.FontFamilyProperty);
                object fontSize = this.DescriptionRichTextBox.Selection.GetPropertyValue(Inline.FontSizeProperty);
                object textColor = this.DescriptionRichTextBox.Selection.GetPropertyValue(Inline.ForegroundProperty);

                object fontWeight = this.DescriptionRichTextBox.Selection.GetPropertyValue(Inline.FontWeightProperty);
                object fontStyle = this.DescriptionRichTextBox.Selection.GetPropertyValue(Inline.FontStyleProperty);
                object textDecoration = this.DescriptionRichTextBox.Selection.GetPropertyValue(Inline.TextDecorationsProperty);

                this.FontFamilyComboBox.SelectedItem = fontFamily;
                this.FontSizeComboBox.SelectedItem = fontSize;

                if (textColor is SolidColorBrush brush)
                {
                    Color currentColor = brush.Color;
                    PropertyInfo prop = typeof(Colors).GetProperties()
                        .FirstOrDefault(p => (Color)p.GetValue(null) == currentColor);

                    if (prop != null)
                    {
                        ColorComboBox.SelectedItem = prop;
                    }
                }

                this.BoldBtn.IsChecked = (fontWeight.Equals(FontWeights.Bold));
                this.ItalicBtn.IsChecked = (fontStyle.Equals(FontStyles.Italic));
                this.UnderlineBtn.IsChecked = (textDecoration.Equals(TextDecorations.Underline));
            }
        }

        private bool ValidateInput()
        {
            bool result = true;

            if (this.ObjectImage.Source == null)
            {
                this.ObjectImageErrTb.Text = "Image is missing";
                result = false;
            }

            if (String.IsNullOrEmpty(this.ObjectNameTb.Text))
            {
                this.ObjectNameErrTb.Text = "Movie Name is missing";
                result = false;
            }

            if (this.EditReview.Rating == 0)
            {
                this.ObjectRatingErrTb.Text = "Movie Rating is missing";
                result = false;
            }

            // Description can be empty

            if (String.IsNullOrEmpty(this.ObjectLinkTb.Text))
            {
                this.ObjectLinkErrTb.Text = "Movie Link is missing";
                result = false;
            }

            if(!result)
                return false;

            MessageWindow messageWindow = new MessageWindow("Do you want to edit this Review?", EFontAwesomeIcon.Solid_Edit, MessageWindow.MessageBoxCause.YesNo);
            return (messageWindow.ShowDialog() == true);
            
        }

    }
}
