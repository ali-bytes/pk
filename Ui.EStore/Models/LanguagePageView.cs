namespace MRS.Models
{
    public class LanguagePageView
    {
        public int LanguagePageId { get; set; }
        public short LanguageId { get; set; }
        public int PageId { get; set; }
        public bool CanDelete { get; set; }

    }
}