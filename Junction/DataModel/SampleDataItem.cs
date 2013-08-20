using System;

namespace Junction.DataModel
{
    public class SampleDataItem : SampleDataCommon
    {
        public SampleDataItem(String uniqueId, String title, String subtitle, String imagePath, String description, String content, SampleDataGroup group)
            : base(uniqueId, title, subtitle, imagePath, description)
        {
            _content = content;
            _group = group;
        }

        private string _content = string.Empty;
        public string Content
        {
            get { return _content; }
            set { SetProperty(ref _content, value); }
        }

        private SampleDataGroup _group;
        public SampleDataGroup Group
        {
            get { return _group; }
            set { SetProperty(ref _group, value); }
        }
    }
}