using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using YueFM.Managers;


namespace YueFM.Utils
{
    public class HtmlToRichText
    {
        private SettingManager settingManager = SettingManager.GetInstance();

        private List<UIElement> view;
        private string _html;
        public string html
        {
            set
            {
                _html = value.Replace("\n", "").Replace("\r", "");
                doc = new HtmlDocument();
                doc.LoadHtml(_html);
            }
            get { return _html; }
        }

        private HtmlDocument doc;

        public double size { set; get; }
  
        public HtmlToRichText(List<UIElement> view)
        {
            this.view = view;
        }

        public void Parse()
        {
            var nodes = doc.DocumentNode.ChildNodes;

            foreach (var node in nodes)
            {
                if (node.Name == "#text")
                {
                    continue;
                }

                var block = new RichTextBox();
                if (settingManager.night_mode)
                {
                    Color f = Color.FromArgb(0xff,
                      (byte)((Double.Parse(settingManager.night_light) / 100) * 0xEE + 0x11),
                      (byte)((Double.Parse(settingManager.night_light) / 100) * 0xEE + 0x11),
                      (byte)((Double.Parse(settingManager.night_light) / 100) * 0xEE + 0x11));
                    block.Foreground = new SolidColorBrush(f);
                }
                block.Margin = new Thickness(12, 12, 12, 12);
                block.FontFamily = new FontFamily(this.settingManager.article_font);

                Paragraph p;
                Run r;
                Bold b;
                Italic i;
                switch (node.Name)
                {
                    case "p":
                        ParseExt(node);
                        break;
                    case "cite":
                        p = new Paragraph();
                        block.Blocks.Add(p);
                        r = new Run();
                        r.FontSize = size;
                        r.Text = "『" + node.InnerText.Trim() + "』";
                        p.Inlines.Add(r);
                        view.Add(block);
                        break;
                    case "q":
                        p = new Paragraph();
                        block.Blocks.Add(p);
                        r = new Run();
                        r.FontSize = size;
                        addInnerText(r,node);
                        p.Inlines.Add(r);
                        view.Add(block);
                        break;
                    case "a":
                        p = new Paragraph();
                        block.Blocks.Add(p);
                        r = new Run();
                        r.FontSize = size;
                        addInnerText(r,node);
                        p.Inlines.Add(r);
                        view.Add(block);
                        break;
                    case "u":
                        p = new Paragraph();
                        block.Blocks.Add(p);
                        r = new Run();
                        r.FontSize = size;
                        r.TextDecorations = TextDecorations.Underline;
                        addInnerText(r,node);
                        p.Inlines.Add(r);
                        view.Add(block);
                        break;
                    case "b":
                    case "strong":
                        p = new Paragraph();
                        block.Blocks.Add(p);
                        b = new Bold();
                        r = new Run();
                        r.FontSize = size;
                        addInnerText(r,node);
                        b.Inlines.Add(r);
                        p.Inlines.Add(b);
                        view.Add(block);
                        break;
                    case "i":
                        p = new Paragraph();
                        block.Blocks.Add(p);
                        i = new Italic();
                        r = new Run();
                        r.FontSize = size;
                        addInnerText(r,node);
                        i.Inlines.Add(r);
                        p.Inlines.Add(i);
                        view.Add(block);
                        break;
                    case "h1":
                        p = new Paragraph();
                        block.Blocks.Add(p);
                        r = new Run();
                        r.FontSize = size + 10;
                        addInnerText(r,node);
                        p.Inlines.Add(r);
                        view.Add(block);
                        break;
                    case "h2":
                        p = new Paragraph();
                        block.Blocks.Add(p);
                        r = new Run();
                        r.FontSize = size + 8;
                        addInnerText(r,node);
                        p.Inlines.Add(r);
                        view.Add(block);
                        break;
                    case "h3":
                        p = new Paragraph();
                        block.Blocks.Add(p);
                        r = new Run();
                        r.FontSize = size + 6;
                        addInnerText(r,node);
                        p.Inlines.Add(r);
                        view.Add(block);
                        break;
                    case "h4":
                        p = new Paragraph();
                        block.Blocks.Add(p);
                        r = new Run();
                        r.FontSize = size + 4;
                        addInnerText(r,node);
                        p.Inlines.Add(r);
                        view.Add(block);
                        break;
                    case "h5":
                        p = new Paragraph();
                        block.Blocks.Add(p);
                        r = new Run();
                        r.FontSize = size + 2;
                        addInnerText(r,node);
                        p.Inlines.Add(r);
                        view.Add(block);
                        break;
                    case "h6":
                        p = new Paragraph();
                        block.Blocks.Add(p);
                        r = new Run();
                        r.FontSize = size;
                        addInnerText(r,node);
                        p.Inlines.Add(r);
                        view.Add(block);
                        break;
                    case "li":
                    case "ui":
                        p = new Paragraph();
                        block.Blocks.Add(p);
                        r = new Run();
                        r.Text = "• " + node.InnerText.Trim();
                        p.Inlines.Add(r);
                        view.Add(block);
                        break;
                    case "img":
                        if (settingManager.article_is_image)
                        {
                            Image image = new Image();
                            image.Margin = new Thickness(6, 12, 6, 12);
                            image.Source = new BitmapImage(new Uri(node.Attributes["src"].Value));
                            image.MaxWidth = 456;
                            view.Add(image);
                        }

                        if (node.HasChildNodes)
                        {
                        p = new Paragraph();
                        block.Blocks.Add(p);
                        r = new Run();
                        addInnerText(r,node);
                        p.Inlines.Add(r);
                        view.Add(block);
                        }
                        break;
                    case "br":
                        p = new Paragraph();
                        block.Blocks.Add(p);
                        view.Add(block);
                        break;
                    case "hr":
                        break;
                    case "blockquote":
                        ParseExt(node, true);
                        break;
                    default:
                        break;
                }
            }

        }

        private void addInnerText(Run r, HtmlNode node)
        {
            if (node.InnerText.Trim().Length >0)
                r.Text = node.InnerText.Trim();
        }

        private void ParseExt(HtmlNode nodes, bool backquote = false)
        {
            RichTextBox block = new RichTextBox();
            if (settingManager.night_mode)
            {
                Color f = Color.FromArgb(0xff,
                               (byte)((Double.Parse(settingManager.night_light) / 100) * 0xEE + 0x11),
                               (byte)((Double.Parse(settingManager.night_light) / 100) * 0xEE + 0x11),
                               (byte)((Double.Parse(settingManager.night_light) / 100) * 0xEE + 0x11));
                block.Foreground = new SolidColorBrush(f);
            }

            block.FontFamily = new FontFamily(this.settingManager.article_font);

            Paragraph p = new Paragraph();
            block.Blocks.Add(p);
            view.Add(block);

            if (backquote)
            {
                block.Margin = new Thickness(32, 18, 12, 18);
            }
            else
            {
                block.Margin = new Thickness(12, 12, 12, 12);
            }
             
            foreach (var node in nodes.ChildNodes)
            {
                
                Run r;
                Bold b;
                Italic i;
                switch (node.Name)
                {
                    case "#text":
                    case "p":
                    case "q": r = new Run();
                        r.FontSize = size;
                        addInnerText(r,node);
                        p.Inlines.Add(r);
                        break;
                    case "cite":
                        r = new Run();
                        r.FontSize = size;
                        r.Text = "『" + node.InnerText.Trim() + "』";
                        p.Inlines.Add(r);
                        break;
                    case "blockquote":
                        r = new Run();
                        r.FontSize = size;
                        r.Text = node.InnerText;
                        p.Inlines.Add(r);
                        break;
                    case "u":
                        r = new Run();
                        r.FontSize = size;
                        r.Text = node.InnerText;
                        r.TextDecorations = TextDecorations.Underline;
                        p.Inlines.Add(r);
                        break;
                    case "b":
                    case "strong":
                        b = new Bold();
                        r = new Run();
                        r.FontSize = size;
                        r.Text = node.InnerText;
                        b.Inlines.Add(r);
                        p.Inlines.Add(b);
                        break;
                    case "i":
                        i = new Italic();
                        r = new Run();
                        r.FontSize = size;
                        r.Text = node.InnerText;
                        i.Inlines.Add(r);
                        p.Inlines.Add(i);
                        break;
                    case "h1":
                        r = new Run();
                        r.FontSize = size + 10;
                        r.Text = node.InnerText;
                        p.Inlines.Add(r);
                        break;
                    case "h2":
                        r = new Run();
                        r.FontSize = size + 8;
                        r.Text = node.InnerText;
                        p.Inlines.Add(r);
                        break;
                    case "h3":
                        r = new Run();
                        r.FontSize = size + 6;
                        r.Text = node.InnerText;
                        p.Inlines.Add(r);
                        break;
                    case "h4":
                        r = new Run();
                        r.FontSize = size + 4;
                        r.Text = node.InnerText;
                        p.Inlines.Add(r);
                        break;
                    case "h5":
                        r = new Run();
                        r.FontSize = size + 2;
                        r.Text = node.InnerText;
                        p.Inlines.Add(r);
                        break;
                    case "h6":
                        r = new Run();
                        r.FontSize = size;
                        r.Text = node.InnerText;
                        p.Inlines.Add(r);
                        break;
                    case "br":
                        p = new Paragraph();
                        block.Blocks.Add(p);
                        break;
                    case "hr":
                        break;
                    case "li":
                    case "ui":
                        r = new Run();
                        r.Text = "• " + node.InnerText.Trim();
                        p.Inlines.Add(r);
                        break;
                    case "img":
                        if (settingManager.article_is_image)
                        {
                            Image image = new Image();
                            image.Source = new BitmapImage(new Uri(node.Attributes["src"].Value));
                            image.Margin = new Thickness(6, 12, 6, 24);
                            image.MaxWidth = 456;
                            view.Add(image);
                        }

                        if (node.HasChildNodes)
                        {
                            r = new Run();
                            addInnerText(r,node);
                            p.Inlines.Add(r);
                        }
                        break;
                    default:
                        break;
                }

            }
        }
    }
}