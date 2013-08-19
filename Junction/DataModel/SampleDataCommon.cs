using System;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Junction.Common;

namespace Junction.DataModel
{
    [Windows.Foundation.Metadata.WebHostHidden]
    public abstract class SampleDataCommon : BindableBase
    {
        private static readonly Uri _baseUri = new Uri("ms-appx:///");

        public SampleDataCommon(String uniqueId, String title, String subtitle, String imagePath, String description)
        {
            _uniqueId = uniqueId;
            _title = title;
            _subtitle = subtitle;
            _description = description;
            _imagePath = imagePath;
        }

        private string _uniqueId = string.Empty;
        public string UniqueId
        {
            get { return _uniqueId; }
            set { SetProperty(ref _uniqueId, value); }
        }

        private string _title = string.Empty;
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        private string _subtitle = string.Empty;
        public string Subtitle
        {
            get { return _subtitle; }
            set { SetProperty(ref _subtitle, value); }
        }

        private string _description = string.Empty;
        public string Description
        {
            get { return _description; }
            set { SetProperty(ref _description, value); }
        }

        private ImageSource _image = null;
        private String _imagePath = null;
        public ImageSource Image
        {
            get
            {
                if (_image == null && _imagePath != null)
                {
                    _image = new BitmapImage(new Uri(SampleDataCommon._baseUri, _imagePath));
                }
                return _image;
            }

            set
            {
                _imagePath = null;
                SetProperty(ref _image, value);
            }
        }

        public void SetImage(String path)
        {
            _image = null;
            _imagePath = path;
            OnPropertyChanged("Image");
        }

        public override string ToString()
        {
            return Title;
        }
    }
}