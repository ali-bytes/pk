namespace Domain.EStore.Repositories
{
    public interface IUnitOfWork
    {


        #region Properties

        IRepository<SeoPageTool> SeoPageTools { get; }
        IRepository<SeoLanguageTool> SeoLanguageTools { get; }
        IRepository<SeoManager> SeoManagers { get; }

        IRepository<Branch> Branchs { get; }
        IRepository<Brand> Brands { get; }
        IRepository<Category> Categories { get; }
        IRepository<LestCategory> LestCategories { get; }
        IRepository<City> Cities { get; }
        IRepository<Enquiry> Enquiries { get; }
        IRepository<GetWay> GetWaies { get; }
        IRepository<Governate> Governates { get; }
        IRepository<HomeDetail> HomeDetails { get; }
        IRepository<MainCategory> MainCategories { get; }
        IRepository<Privilege> Privileges { get; }
        IRepository<PrivilegeAction> PrivilegeActions { get; }
        IRepository<PrivilegeCategory> PrivilegeCategories { get; }
        IRepository<Product> Products { get; }
        IRepository<ProductShow> ProductShows { get; }
        IRepository<ProductImage> ProductImages { get; }
        IRepository<Rule> Rules { get; }
        IRepository<RulePrivilege> RulePrivileges { get; }
        IRepository<SliderImage> SliderImages { get; }
        IRepository<Speciality> Specialities { get; }
        IRepository<SysEmail> SysEmails { get; }
        IRepository<SysNew> SysNews { get; }
        IRepository<User> Users { get; }
        IRepository<UserOperation> UserOperations { get; }
        IRepository<UserOperationType> UserOperationTypes { get; }
        IRepository<ViewControl> ViewControls { get; }
        IRepository<Customer> Customers { get; }
        IRepository<UserProfile> UserProfiles { get; }
        IRepository<Cart> Carts { get; }
        IRepository<CartDetail> CartDetails { get; }
        IRepository<delivery> Deliveries { get; }
        IRepository<Rating> Ratings { get; }
        IRepository<FAQ> FAQs { get; }



        #endregion
        #region Language

        IRepository<Language> Languages { get; }
        IRepository<LanguagePage> LanguagePages { get; }
        IRepository<LanguageTool> LanguageTools { get; }
        IRepository<PageTool> PageTools { get; }
        IRepository<SysPage> SysPages { get; }
    
        #endregion
    }

    public class UnitOfWork : IUnitOfWork
    {



        #region Prop

        readonly IRepository<SeoPageTool> _seoPageTool = null;
        readonly IRepository<SeoLanguageTool> _seoLanguageTool = null;
        readonly IRepository<SeoManager> _seoManager = null;
        readonly IRepository<Branch> _branchs = null;
        readonly IRepository<Brand> _brands = null;
        readonly IRepository<Category> _categories = null;
        readonly IRepository<City> _cities = null;
        readonly IRepository<Enquiry> _enquiries = null;
        readonly IRepository<GetWay> _getWaies = null;
        readonly IRepository<Governate> _governates = null;
        readonly IRepository<HomeDetail> _homeDetails = null;
        readonly IRepository<MainCategory> _mainCategories = null;
        readonly IRepository<Privilege> _privileges = null;
        readonly IRepository<PrivilegeAction> _privilegeActions = null;
        readonly IRepository<PrivilegeCategory> _privilegeCategories = null;
        readonly IRepository<Product> _products = null;
        readonly IRepository<ProductShow> _productShows = null;
        readonly IRepository<ProductImage> _productImages = null;
        readonly IRepository<Rule> _rules = null;
        readonly IRepository<RulePrivilege> _rulePrivileges = null;
        readonly IRepository<SliderImage> _sliderImages = null;
        readonly IRepository<Speciality> _specialities = null;
        readonly IRepository<SysEmail> _sysEmails = null;
        readonly IRepository<SysNew> _sysNews = null;
        readonly IRepository<User> _users = null;
        readonly IRepository<UserOperation> _userOperations = null;
        readonly IRepository<UserOperationType> _userOperationTypes = null;
        readonly IRepository<ViewControl> _viewControls = null;
        readonly IRepository<Customer> _customers = null;
        readonly IRepository<UserProfile> _UserProfile = null;
        readonly IRepository<Cart> _Carts = null;
        readonly IRepository<CartDetail> _CartDetail = null;
        readonly IRepository<delivery> _delivery = null;
        readonly IRepository<LestCategory> _LestCategory = null;
        readonly IRepository<Rating> _Rating = null;
        readonly IRepository<FAQ> _FAQ = null;

        #endregion



        #region Language
        readonly IRepository<Language> _language = null;
        readonly IRepository<LanguagePage> _languagePage = null;
        readonly IRepository<LanguageTool> _languageTool = null;
        readonly IRepository<PageTool> _pageTool = null;
        readonly IRepository<SysPage> _sysPage = null;

                public IRepository<SeoPageTool> SeoPageTools
        {
            get { return _seoPageTool ?? new Repository<SeoPageTool>("AllSeoPageTool" ); }
        }
        #endregion


        public IRepository<SeoLanguageTool> SeoLanguageTools
        {
            get { return _seoLanguageTool ?? new Repository<SeoLanguageTool>("AllSeoLanguageTools" ); }
        }


        public IRepository<SeoManager> SeoManagers
        {
            get { return _seoManager ?? new Repository<SeoManager>("AllSeoManagers" ); }
        }

        public IRepository<Language> Languages
        {
            get { return _language ?? new Repository<Language>("AllLanguages"); }
        }

        public IRepository<LanguagePage> LanguagePages
        {
            get { return _languagePage ?? new Repository<LanguagePage>("AllLanguagePages" ); }
        }

        public IRepository<LanguageTool> LanguageTools
        {
            get { return _languageTool ?? new Repository<LanguageTool>("AllLanguageTools" ); }
        }

        public IRepository<PageTool> PageTools
        {
            get { return _pageTool ?? new Repository<PageTool>("AllPageTools" ); }
        }

        public IRepository<SysPage> SysPages
        {
            get { return _sysPage ?? new Repository<SysPage>("AllSysPages" ); }
        }

        public IRepository<Branch> Branchs
        {
            get { return _branchs ?? new Repository<Branch>("AllBranchs"); }
        }

        public IRepository<Brand> Brands
        {
            get { return _brands ?? new Repository<Brand>("AllBrands"); }
        }

        public IRepository<Category> Categories
        {
            get { return _categories ?? new Repository<Category>("AllCategories"); }
        }

        public IRepository<City> Cities
        {
            get { return _cities ?? new Repository<City>("AllCities"); }
        }

        public IRepository<Enquiry> Enquiries
        {
            get { return _enquiries ?? new Repository<Enquiry>("AllEnquiries"); }
        }

        public IRepository<GetWay> GetWaies
        {
            get { return _getWaies ?? new Repository<GetWay>("AllGetWaies"); }
        }

        public IRepository<Governate> Governates
        {
            get { return _governates ?? new Repository<Governate>("AllGovernates"); }
        }

        public IRepository<HomeDetail> HomeDetails
        {
            get { return _homeDetails ?? new Repository<HomeDetail>("AllHomeDetails"); }
        }

        public IRepository<MainCategory> MainCategories
        {
            get { return _mainCategories ?? new Repository<MainCategory>("AllMainCategories"); }
        }

        public IRepository<Privilege> Privileges
        {
            get { return _privileges ?? new Repository<Privilege>("AllPrivilages"); }
        }

        public IRepository<PrivilegeAction> PrivilegeActions
        {
            get { return _privilegeActions ?? new Repository<PrivilegeAction>("AllPrivilegesActions"); }
        }

        public IRepository<PrivilegeCategory> PrivilegeCategories
        {
            get { return _privilegeCategories ?? new Repository<PrivilegeCategory>("AllPrivilegeCategories"); }
        }

        public IRepository<Product> Products
        {
            get { return _products ?? new Repository<Product>("AllProducts"); }
        }

        public IRepository<ProductShow> ProductShows { get
        {
            return _productShows ?? new Repository<ProductShow>("AllProducts");
        } }

        public IRepository<ProductImage> ProductImages
        {
            get { return _productImages ?? new Repository<ProductImage>("AllProductImages"); }
        }

        public IRepository<Rule> Rules
        {
            get { return _rules ?? new Repository<Rule>("AllRules"); }
        }

        public IRepository<RulePrivilege> RulePrivileges
        {
            get { return _rulePrivileges ?? new Repository<RulePrivilege>("AllRulePrivileges"); }
        }

        public IRepository<SliderImage> SliderImages
        {
            get { return _sliderImages ?? new Repository<SliderImage>("SliderImages"); }
        }

        public IRepository<Speciality> Specialities
        {
            get { return _specialities ?? new Repository<Speciality>("AllSpecialities"); }
        }

        public IRepository<SysEmail> SysEmails
        {
            get { return _sysEmails ?? new Repository<SysEmail>("AllSysEmails"); }
        }

        public IRepository<SysNew> SysNews
        {
            get { return _sysNews ?? new Repository<SysNew>("AllSysNews"); }
        }

        public IRepository<User> Users
        {
            get { return _users ?? new Repository<User>("AllUsers"); }
        }

        public IRepository<UserOperation> UserOperations
        {
            get { return _userOperations ?? new Repository<UserOperation>("AllUserOperations"); }
        }

        public IRepository<UserOperationType> UserOperationTypes
        {
            get { return _userOperationTypes ?? new Repository<UserOperationType>("AllUserOperationTypes"); }
        }

        



        public IRepository<ViewControl> ViewControls
        {
            get { return _viewControls ?? new Repository<ViewControl>("AllViewControls"); }
        }


        public IRepository<Customer> Customers
        {
            get { return _customers ?? new Repository<Customer>("AllCustomer"); }
        }


        public IRepository<UserProfile> UserProfiles
        {
            get { return _UserProfile ?? new Repository<UserProfile>("AllUserProfiles"); }
        }

        public IRepository<Cart> Carts
        {
            get { return _Carts ?? new Repository<Cart>("AllCarts"); }
        }

        public IRepository<CartDetail> CartDetails
        {
            get { return _CartDetail ?? new Repository<CartDetail>("AllCartDetails"); }
        }

        public IRepository<delivery> Deliveries
        {
            get { return _delivery ?? new Repository<delivery>("AllDeleveries"); }
        }


        public IRepository<LestCategory> LestCategories
        {
            get { return _LestCategory ?? new Repository<LestCategory>("AllLestCategories"); }
        }


        public IRepository<Rating> Ratings
        {
            get { return _Rating ?? new Repository<Rating>("AllRating"); }
        }


        public IRepository<FAQ> FAQs
        {
            get { return _FAQ ?? new Repository<FAQ>("ALLFaq"); }
        }
    }
}