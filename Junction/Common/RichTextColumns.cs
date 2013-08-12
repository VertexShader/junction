using System;
using System.Collections.Generic;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Junction.Common
{
    [Windows.UI.Xaml.Markup.ContentProperty(Name = "RichTextContent")]
    public sealed class RichTextColumns : Panel
    {
        public static readonly DependencyProperty RichTextContentProperty =
            DependencyProperty.Register("RichTextContent", typeof(RichTextBlock),
            typeof(RichTextColumns), new PropertyMetadata(null, ResetOverflowLayout));

        public static readonly DependencyProperty ColumnTemplateProperty =
            DependencyProperty.Register("ColumnTemplate", typeof(DataTemplate),
            typeof(RichTextColumns), new PropertyMetadata(null, ResetOverflowLayout));

        public RichTextColumns()
        {
            this.HorizontalAlignment = HorizontalAlignment.Left;
        }

        public RichTextBlock RichTextContent
        {
            get { return (RichTextBlock)GetValue(RichTextContentProperty); }
            set { SetValue(RichTextContentProperty, value); }
        }

        public DataTemplate ColumnTemplate
        {
            get { return (DataTemplate)GetValue(ColumnTemplateProperty); }
            set { SetValue(ColumnTemplateProperty, value); }
        }

        private static void ResetOverflowLayout(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var target = d as RichTextColumns;
            if (target != null)
            {
                target._overflowColumns = null;
                target.Children.Clear();
                target.InvalidateMeasure();
            }
        }

        private List<RichTextBlockOverflow> _overflowColumns;

        protected override Size MeasureOverride(Size availableSize)
        {
            if (RichTextContent == null) return new Size(0, 0);

            if (_overflowColumns == null)
            {
                Children.Add(RichTextContent);
                _overflowColumns = new List<RichTextBlockOverflow>();
            }

            RichTextContent.Measure(availableSize);
            var maxWidth = RichTextContent.DesiredSize.Width;
            var maxHeight = RichTextContent.DesiredSize.Height;
            var hasOverflow = RichTextContent.HasOverflowContent;

            var overflowIndex = 0;
            while (hasOverflow && maxWidth < availableSize.Width && this.ColumnTemplate != null)
            {
                RichTextBlockOverflow overflow;
                if (_overflowColumns.Count > overflowIndex)
                {
                    overflow = _overflowColumns[overflowIndex];
                }
                else
                {
                    overflow = (RichTextBlockOverflow)ColumnTemplate.LoadContent();
                    _overflowColumns.Add(overflow);
                    Children.Add(overflow);
                    if (overflowIndex == 0)
                    {
                        RichTextContent.OverflowContentTarget = overflow;
                    }
                    else
                    {
                        _overflowColumns[overflowIndex - 1].OverflowContentTarget = overflow;
                    }
                }

                overflow.Measure(new Size(availableSize.Width - maxWidth, availableSize.Height));
                maxWidth += overflow.DesiredSize.Width;
                maxHeight = Math.Max(maxHeight, overflow.DesiredSize.Height);
                hasOverflow = overflow.HasOverflowContent;
                overflowIndex++;
            }

            if (_overflowColumns.Count > overflowIndex)
            {
                if (overflowIndex == 0)
                {
                    RichTextContent.OverflowContentTarget = null;
                }
                else
                {
                    _overflowColumns[overflowIndex - 1].OverflowContentTarget = null;
                }
                while (_overflowColumns.Count > overflowIndex)
                {
                    _overflowColumns.RemoveAt(overflowIndex);
                    Children.RemoveAt(overflowIndex + 1);
                }
            }

            return new Size(maxWidth, maxHeight);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            double maxWidth = 0;
            double maxHeight = 0;
            foreach (var child in Children)
            {
                child.Arrange(new Rect(maxWidth, 0, child.DesiredSize.Width, finalSize.Height));
                maxWidth += child.DesiredSize.Width;
                maxHeight = Math.Max(maxHeight, child.DesiredSize.Height);
            }
            return new Size(maxWidth, maxHeight);
        }
    }
}
