using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace SalesApp.Models
{
    public class MainMenuItem
    {
        public int Id { get; set; }
        public string ItemTitle { get; set; }
        public string BgImageSource { get; set; }
        public Color BorderColor { get; set; }
        public Color BackColor { get; set; }
        public Color TextColor { get; set; }
        public Color BackIconsColor { get; set; }
    }
}
