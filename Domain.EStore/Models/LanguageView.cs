namespace MRS.Models
{
    public class LanguageView
    {

        public short LanguageId { get; set; }
        public string LanguageName { get; set; }
        public bool LanguageState { get; set; }
        public string LanguageType { get; set; }
        public bool CanDelete { get; set; }

    }
}