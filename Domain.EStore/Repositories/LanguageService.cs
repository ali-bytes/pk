using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.EStore.Models;
using MRS.Models;

namespace Domain.EStore.Repositories
{
    public class LanguageService
    {
        readonly UserSession _userSession = new UserSession();

        private readonly IUnitOfWork _unitOfWork;

        public LanguageService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public LanguageTool GetLanguageToolBylanguageId(short languageId, int pageId, int toolId)
        {
            LanguageTool languageValue = null;

            LanguagePage languagePage = GetLanguageByLangIdPageId(languageId, pageId);

            if (languagePage != null)
            {
                var lng = from l in GetAllLanguagestoolValues
                          join p in GetAllLanguagePage on l.LanguagePageId equals p.LanguagePageId
                          where p.LanguagePageId == languagePage.LanguagePageId && l.ToolId == toolId
                          select l;

                languageValue = lng.FirstOrDefault();

                if (languageValue == null)
                {
                    languageValue = new LanguageTool { LanguagePageId = languagePage.LanguagePageId, ToolId = toolId };
                    SaveLanguageTool(languageValue);
                }
            }

            return languageValue;
        }

        public SeoLanguageTool GetSeoLanguageToolBylanguageId(short languageId, int pageId, int toolId)
        {
            SeoLanguageTool languageValue = null;

            LanguagePage languagePage = GetLanguageByLangIdPageId(languageId, pageId);

            if (languagePage != null)
            {
                var lng = from l in GetAllSeoLanguagestoolValues
                          join p in GetAllLanguagePage on l.LanguagePageId equals p.LanguagePageId
                          where p.LanguagePageId == languagePage.LanguagePageId && l.ToolId == toolId
                          select l;

                languageValue = lng.FirstOrDefault();

                if (languageValue == null)
                {
                    languageValue = new SeoLanguageTool { LanguagePageId = languagePage.LanguagePageId, ToolId = toolId };
                    var seoPage = GetSeoPageToolViews.First(c => c.ToolId == toolId).PageUrl;
                    if (!string.IsNullOrEmpty(seoPage))
                    {
                        var strsplir = seoPage.Split('/');
                        if (strsplir.Length > 0)
                        {

                            for (int i = 0; i < strsplir.Length; i++)
                            {
                                switch (i)
                                {
                                    case 0:
                                        languageValue.ControllerName = strsplir[0];
                                        break;
                                    case 1:
                                        languageValue.ActionName = strsplir[1];
                                        break;
                                    default:
                                        languageValue.UrlParameter += strsplir[i];
                                        break;
                                }


                            }

                            if (strsplir.Length == 1) languageValue.ActionName = "Index";

                        }
                    }

                    SaveSeoLanguageTool(languageValue);
                    //var seoMange = new SeoManager { LanguagePageId = languagePage.LanguagePageId, ToolId = toolId };
                    //SaveSeoManager(seoMange);


                }
            }

            return languageValue;
        }

        public SeoLanguageTool GetUserSeoLanguageValue(string toolName, string pageName, string pageUrl)
        {
            var pageId = GetSysPageById(pageName).PageId;

            var toolId = GetSeoPageToolIdByToolName(toolName, pageId, pageUrl);

            if (_userSession.UserLanguage == null)
            {
                _userSession.UserLanguage = GetAllActiveLanguage().First();
            }

            return GetSeoLanguageToolBylanguageId(_userSession.UserLanguage.LanguageId, pageId, toolId);
        }




        public string GetUserLanguageValue(string toolName, string pageName)
        {
            var pageId = GetSysPageById(pageName).PageId;

            var toolId = GetPageToolIdByToolName(toolName, pageId);

            if (_userSession.UserLanguage == null)
            {
                _userSession.UserLanguage = GetAllActiveLanguage().First();
            }

            var languageTool = GetLanguageToolBylanguageId(_userSession.UserLanguage.LanguageId, pageId, toolId);
            if (languageTool != null)
            {
                return !string.IsNullOrEmpty(languageTool.LanguageToolValue) ? languageTool.LanguageToolValue : toolName;
            }
            return toolName;

        }

        public SeoLanguageTool GetSeoMangerValue(string seoUrl)
        {
            var seoManger = GetSeoManagerBySeoUrl(seoUrl);


            return seoManger;

        }

        #region Language

        public static List<Language> GetAllLanguageStatic()
        {
            var unitOfWork = new UnitOfWork();
            return unitOfWork.Languages.FindAll().ToList();

        }

        public List<Language> GetAllLanguage
        {
            get
            {
                try
                {
                    return _unitOfWork.Languages.FindAll().ToList();
                }
                catch (Exception)
                {
                    return new List<Language>();

                }


            }
        }

        public List<LanguageView> GetLanguageViews
        {
            get
            {
                return GetAllLanguage.Select(l => new LanguageView
                {
                    LanguageId = l.LanguageId,
                    LanguageName = l.LanguageName,
                    LanguageState = l.LanguageState,
                    LanguageType = l.LanguageType,
                    CanDelete = CanDeleted(l)

                }).ToList();
            }
        }

        private bool CanDeleted(Language language)
        {
            return !language.LanguagePages.Any();
        }

        public List<Language> GetAllActiveLanguage()
        {
            return GetAllLanguage.Where(l => l.LanguageState).ToList();
        }

        public Language GetAllLanguageByLangId(short languageId)
        {
            return GetAllLanguage.FirstOrDefault(l => l.LanguageId == languageId);
        }

        public Language GetLanguageByLangType(string languageType)
        {
            return GetAllLanguage.FirstOrDefault(l => l.LanguageType == languageType);
        }



        public bool SetLanguageActive(short languageId)
        {
            var language = GetAllLanguageByLangId(languageId);
            if (language == null) return false;
            language.LanguageState = true;

            return SaveLanguage(language);
        }

        public bool SaveLanguage(Language language)
        {
            if (language.LanguageId <= 0) return _unitOfWork.Languages.Add(language);
            if (language != GetAllActiveLanguage().First() && GetAllActiveLanguage().Count == 1)
            {
                language.LanguageState = true;
            }
            return _unitOfWork.Languages.Add(language);
        }

        public bool DeleteLanguage(short languageId)
        {

            return _unitOfWork.Languages.Remove(languageId);
        }

        #endregion

        #region SysPage
        public static List<SysPage> GetAllPagesStatic()
        {
            var unitOfWork = new UnitOfWork();
            return unitOfWork.SysPages.FindAll().ToList();

        }
        public List<SysPage> GetAllSysPage
        {
            get
            {
                return _unitOfWork.SysPages.FindAll().ToList();
            }
        }

        public List<SysPageView> GetSysPageViews
        {
            get
            {
                return GetAllSysPage.Select(p => new SysPageView
                {
                    PageId = p.PageId,
                    PageName = p.PageName,

                    CanDelete = CanDeleteSysPage(p)
                }).ToList();
            }
        }

        private bool CanDeleteSysPage(SysPage sysPage)
        {
            if (sysPage.LanguagePages.Any())
            {
                return false;
            }

            if (sysPage.PageTools.Any())
            {
                return false;
            }

            return true;
        }
        public SysPage GetSysPageBypageId(int pageId)
        {
            return GetAllSysPage.FirstOrDefault(p => p.PageId == pageId);
        }

        public SysPage GetSysPageById(string pageUrl)
        {
            var sysPage = GetAllSysPage.FirstOrDefault(p => p.PageName.Trim() == pageUrl.Trim());
            if (sysPage != null) return sysPage;
            sysPage = new SysPage { PageName = pageUrl };
            if (SavePage(sysPage))
            {
                sysPage = GetAllSysPage.FirstOrDefault(p => p.PageName.Trim() == pageUrl.Trim());
            }
            return sysPage;
        }

        public bool SavePage(SysPage sysPage)
        {
            return _unitOfWork.SysPages.Add(sysPage);
        }

        public bool DeletePage(int pageId)
        {
            return _unitOfWork.SysPages.Remove(pageId);
        }

        #endregion

        #region LanguagePage

        public List<LanguagePage> GetAllLanguagePage
        {
            get
            {
                return _unitOfWork.LanguagePages.FindAll().ToList();
            }
        }

        public List<LanguagePageView> GetLanguagePageViews
        {
            get
            {
                return GetAllLanguagePage.Select(l => new LanguagePageView
                {
                    LanguageId = l.LanguageId,
                    LanguagePageId = l.LanguagePageId,
                    PageId = l.PageId,
                    CanDelete = CanDeleteLanguagePage(l)
                }).ToList();
            }
        }

        private bool CanDeleteLanguagePage(LanguagePage languagePage)
        {
            return !languagePage.LanguageTools.Any();
        }

        public LanguagePage GetLanguagePageByPageId(int pageId)
        {

            return GetAllLanguagePage.FirstOrDefault(p => p.PageId == pageId);
        }

        public List<LanguagePage> GetLanguagePageByLanguageId(short languageId)
        {
            return GetAllLanguagePage.Where(p => p.LanguageId == languageId).ToList();
        }

        public LanguagePage GetLanguageByLangIdPageId(short languageId, int pageId)
        {
            var languagePage = GetAllLanguagePage.FirstOrDefault(p => p.LanguageId == languageId && p.PageId == pageId);
            if (languagePage == null)
            {
                languagePage = new LanguagePage { LanguageId = languageId, PageId = pageId };
                SaveLanguagePage(languagePage);
            }
            return languagePage;
        }

        public bool SaveLanguagePage(LanguagePage languagePage)
        {
            return _unitOfWork.LanguagePages.Add(languagePage);
        }

        public bool DeleteLanguagePade(int languagePageId)
        {
            return _unitOfWork.LanguagePages.Remove(languagePageId);

        }

        #endregion

        #region LanguageTool

        public List<LanguageTool> GetAllLanguagestoolValues
        {
            get
            {
                return _unitOfWork.LanguageTools.FindAll().ToList();
            }
        }

        public List<LanguageToolView> GetLanguageToolViews
        {
            get
            {
                return GetAllLanguagestoolValues.Select(l => new LanguageToolView
                {
                    CanDelete = true,
                    LanguagePageId = l.LanguagePageId,
                    LanguageToolId = l.LanguageToolId,
                    LanguageToolValue = l.LanguageToolValue,
                    ToolId = l.ToolId

                }).ToList();
            }
        }

        public List<LanguageTool> GetAllPageToolsValueByLanguagePageId(int languagePageId)
        {
            return GetAllLanguagestoolValues.Where(a => a.LanguagePageId == languagePageId).ToList();
        }

        public LanguageTool GetLanguageToolBylanguageToolId(int languageToolId)
        {
            return GetAllLanguagestoolValues.FirstOrDefault(a => a.LanguageToolId == languageToolId);
        }

        public bool SaveLanguageTool(LanguageTool languageTool)
        {
            return _unitOfWork.LanguageTools.Add(languageTool);
        }

        public bool DeleteLanguageTool(int languageToolId)
        {
            return _unitOfWork.LanguageTools.Remove(languageToolId);

        }

        #endregion

        #region PageTool

        public List<PageTool> GetAllPageTools
        {
            get
            {
                return _unitOfWork.PageTools.FindAll().ToList();
            }
        }

        public List<PageToolView> GetPageToolViews
        {
            get
            {
                return GetAllPageTools.Select(l => new PageToolView
                {
                    CanDelete = CanDeletePageTool(l),
                    PageId = l.PageId,
                    ToolId = l.ToolId,
                    ToolName = l.ToolName
                }).ToList();
            }
        }

        private bool CanDeletePageTool(PageTool pageTool)
        {
            if (pageTool.LanguageTools.Any()) return false;
            return true;
        }

        public List<PageTool> GetAllPageToolsByPageId(int pageId)
        {
            return GetAllPageTools.Where(p => p.PageId == pageId).ToList();
        }

        public PageTool GetToolByToolId(int toolId)
        {
            return GetAllPageTools.FirstOrDefault(p => p.ToolId == toolId);
        }

        public int GetPageToolIdByToolName(string toolName, int pageId)
        {
            var pageTool = GetAllPageTools.FirstOrDefault(p => p.ToolName.Trim() == toolName.Trim() && p.PageId == pageId);
            if (pageTool == null)
            {
                pageTool = new PageTool { ToolName = toolName.Trim(), PageId = pageId };
                SavePageTool(pageTool);
            }
            return pageTool.ToolId;
        }

        public bool SavePageTool(PageTool pageTool)
        {
            return _unitOfWork.PageTools.Add(pageTool);
        }

        public bool DeletePageTool(int toolId)
        {
            return _unitOfWork.PageTools.Remove(toolId);

        }

        #endregion

        #region SeoPageTool

        public List<SeoPageTool> GetAllSeoPageTools
        {
            get
            {
                return _unitOfWork.SeoPageTools.FindAll().ToList();
            }
        }

        public List<SeoPageToolView> GetSeoPageToolViews
        {
            get
            {
                return GetAllSeoPageTools.Select(l => new SeoPageToolView
                {
                    CanDelete = CanDeletePageTool(l),
                    PageId = l.PageId,
                    ToolId = l.ToolId,
                    ToolName = l.ToolName,
                    PageUrl = l.PageUrl

                }).ToList();
            }
        }

        private bool CanDeletePageTool(SeoPageTool pageTool)
        {
            if (pageTool.SeoLanguageTools.Any()) return false;
            return true;
        }

        public List<SeoPageTool> GetAllSeoPageToolsByPageId(int pageId)
        {
            return GetAllSeoPageTools.Where(p => p.PageId == pageId).ToList();
        }

        public SeoPageTool GetSeoToolByToolId(int toolId)
        {
            return GetAllSeoPageTools.FirstOrDefault(p => p.ToolId == toolId);
        }

        public int GetSeoPageToolIdByToolName(string toolName, int pageId, string pageurl = null)
        {
            var pageTool = GetAllSeoPageTools.FirstOrDefault(p => p.ToolName.Trim() == toolName.Trim() && p.PageId == pageId);
            if (pageTool == null)
            {
                pageTool = new SeoPageTool { ToolName = toolName.Trim(), PageId = pageId, PageUrl = pageurl };
                SaveSeoPageTool(pageTool);


            }
            else
            {
                if (pageTool.PageUrl == null && pageurl != null)
                {
                    pageTool.PageUrl = pageurl;
                    SaveSeoPageTool(pageTool);
                }
            }
            return pageTool.ToolId;
        }

        public bool SaveSeoPageTool(SeoPageTool pageTool)
        {
            return _unitOfWork.SeoPageTools.Add(pageTool);
        }

        public bool DeleteSeoPageTool(int toolId)
        {
            return _unitOfWork.SeoPageTools.Remove(toolId);

        }

        #endregion

        #region SeoLanguageTool

        public List<SeoLanguageTool> GetAllSeoLanguagestoolValues
        {
            get
            {
                return _unitOfWork.SeoLanguageTools.FindAll().ToList();
            }
        }

        public List<SeoLanguageToolView> GetSeoLanguageToolViews
        {
            get
            {
                return GetAllSeoLanguagestoolValues.Select(l => new SeoLanguageToolView
                {
                    CanDelete = true,
                    LanguagePageId = l.LanguagePageId,
                    LanguageToolId = l.LanguageToolId,
                    Author = l.Author,
                    Description = l.Description,
                    Keywords = l.Keywords,
                    ToolId = l.ToolId


                }).ToList();
            }
        }

        public List<SeoLanguageTool> GetAllSeoPageToolsValueByLanguagePageId(int languagePageId)
        {
            return GetAllSeoLanguagestoolValues.Where(a => a.LanguagePageId == languagePageId).ToList();
        }

        public SeoLanguageTool GetSeoLanguageToolBylanguageToolId(int languageToolId)
        {
            return GetAllSeoLanguagestoolValues.FirstOrDefault(a => a.LanguageToolId == languageToolId);
        }

        public bool SaveSeoLanguageTool(SeoLanguageTool languageTool)
        {
            return _unitOfWork.SeoLanguageTools.Add(languageTool);
        }

        public bool DeleteSeoLanguageTool(int languageToolId)
        {
            return _unitOfWork.SeoLanguageTools.Remove(languageToolId);

        }

        #endregion

        #region SeoManger

        public List<SeoManager> GetAllSeoManagers
        {
            get
            {
                return _unitOfWork.SeoManagers.FindAll().ToList();
            }
        }

        public List<SeoManager> GetAllSeoManagersByToolId(int toolId)
        {
            return GetAllSeoManagers.Where(p => p.ToolId == toolId).ToList();
        }

        public SeoManager GetSeoManagerById(int id)
        {
            return GetAllSeoManagers.FirstOrDefault(p => p.Id == id);
        }

        public SeoLanguageTool GetSeoManagerBySeoUrl(string seoUrl)
        {
            return GetAllSeoLanguagestoolValues.FirstOrDefault(p => p.SeoUrl != null && p.SeoUrl.ToLower() == seoUrl.ToLower());
        }

        public bool SaveSeoManager(SeoManager seoManager)
        {
            return _unitOfWork.SeoManagers.Add(seoManager);
        }

        public bool DeleteseoManager(int id)
        {
            return _unitOfWork.SeoManagers.Remove(id);

        }

        #endregion

    }
}
