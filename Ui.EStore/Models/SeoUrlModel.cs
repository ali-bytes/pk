using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MRS.Models
{
    public class SeoUrlModel
    {
        public int Id { get; set; }
        public int LanguageToolId { get; set; }
        public int ToolId { get; set; }
        public int LanguagePageId { get; set; }
        public string Keywords { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
        public string Title { get; set; }
        public string SeoUrl { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public string UrlParameter { get; set; }
        public string PageUrl { get; set; }
    }
}