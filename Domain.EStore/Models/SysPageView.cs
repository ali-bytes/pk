namespace MRS.Models
{
    public class SysPageView
    {
        public int PageId { get; set; }
        public string PageName { get; set; }
        public string PageUrl { get; set; }

        public bool CanDelete { get; set; }
    }
}