namespace Domain.EStore.Models
{
    public class LanguageToolView
    {
        public int LanguageToolId { get; set; }
        public int ToolId { get; set; }
        public int LanguagePageId { get; set; }
        public string LanguageToolValue { get; set; }
        public bool CanDelete { get; set; }

    }
    public class SeoLanguageToolView
    {
        public int LanguageToolId { get; set; }
        public int ToolId { get; set; }
        public int LanguagePageId { get; set; }
        public string Keywords { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
        public bool CanDelete { get; set; }

    }
}