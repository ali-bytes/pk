namespace MRS.Models
{
    public class PageToolView
    {
        public int ToolId { get; set; }
        public string ToolName { get; set; }
        public int PageId { get; set; }
        public bool CanDelete { get; set; }

    }
    public class SeoPageToolView
    {
        public int ToolId { get; set; }
        public string ToolName { get; set; }
        public int PageId { get; set; }
        public string PageUrl { get; set; }
        public string Keywords { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
        public bool CanDelete { get; set; }

    }
}