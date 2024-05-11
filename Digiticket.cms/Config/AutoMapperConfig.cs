using AutoMapper;
using DigiticketCMS.Helpers;
using DigiticketCMS.Models;
using DigiticketCMS.Models.Banner;
using DigiticketCMS.Models.CategoryTag;
using DigiticketCMS.Models.Content;
using DigiticketCMS.Models.GroupService;
using DigiticketCMS.Models.Lib;
using DigiticketCMS.Models.Media;
using DigiticketCMS.Models.Product;
using DigiticketCMS.Models.ServicePrice;
using DigiticketCMS.Models.Supplier;
using DigiticketCMS.Models.Tags;
using DigiticketCMS.Models.Users;
using DigiticketCMS.Models.ViewModel;
using DigiticketCMS.Services.Models;
using Services;
using Services.Models;
using Services.Models.Requests.Banner;
using Services.Models.Requests.CategoryTag;
using Services.Models.Requests.Content;
using Services.Models.Requests.GroupdService;
using Services.Models.Requests.Lib;
using Services.Models.Requests.Media;
using Services.Models.Requests.Product;
using Services.Models.Requests.ServicePrice;
using Services.Models.Requests.Supplier;
using Services.Models.Requests.Tags;
using Services.Models.Responses.Banner;
using Services.Models.Responses.Content;
using Services.Models.Responses.Product;
using Services.Models.Responses.ServicePrice;
using Services.Models.Responses.Supplier;
using Services.Models.Responses.Tags;

namespace DigiticketCMS.Config
{
    public class ViewModelToRequest : Profile
    {
        public ViewModelToRequest()
        {
            #region base
            CreateMap<BaseImgViewModel, BaseImg>()
                .ForMember(d => d.Name, opt => opt.MapFrom(s => s.Name.IsTrueOrFalse() ? s.Name : ""))
                .ForMember(d => d.Alt, opt => opt.MapFrom(s => s.Alt.IsTrueOrFalse() ? s.Alt : ""))
                .ForMember(d => d.Link, opt => opt.MapFrom(s => s.Link.IsTrueOrFalse() ? s.Link : ""));
            #endregion

            #region Account
            CreateMap<LoginViewModel, LoginRequest>();
            CreateMap<RegisterViewModel, RegisterRequest>();
            CreateMap<ConfirmAccountViewModel, ConfirmAccountRequest>();
            CreateMap<ResendConfirmAccountViewModel, ResendConfirmAccountRequest>();
            CreateMap<ForgotPasswordViewModel, ForgotPasswordRequest>();
            CreateMap<ResetPasswordViewModel, ResetPasswordRequest>();
            #endregion

            #region Supplier
            CreateMap<SupplierCreateViewModel, SupplierRequest>();
            CreateMap<SupplierUpdateTenantViewModel, SupplierRequest>();
            CreateMap<SupplierUpdateBaseInfoViewModel, SupplierRequest>();
            CreateMap<SupplierUpdateBookingInfoViewModel, SupplierRequest>();
            CreateMap<SupplierCreateBankViewModel, SupplierBankRequest>();
            CreateMap<SupplierDeleteBankViewModel, SupplierBankRequest>();
            CreateMap<SupplierUpdateTenantViewModel, SupplierUpdateTenantRequest>();
            #endregion

            #region Product
            CreateMap<ProductGetByPageViewModel, ProductGetByPageRequest>();
            CreateMap<ProductUpdateBaseInfoViewModel, ProductUpdateBaseInfoRequest>();
            CreateMap<FilterViewModel, FilterRequest>();
            CreateMap<ProductImportProductFromDigipostViewModel, ProductImportProductFromDigipostRequest>();
            CreateMap<ProductUpdateMainInfoViewModel, ProductUpdateMainInfoRequest>();
            CreateMap<ProductUpdateSeoInfoViewModel, ProductUpdateSeoInfoRequest>();
            CreateMap<ProductUpdateImageViewModel, ProductUpdateImageRequest>();
            CreateMap<ProductGetListByTenantFromDigipostViewModel, ProductGetListByTenantFromDigipostRequest>();
            CreateMap<ProductUpdateIdxViewModel, ProductUpdateIdxRequest>();

            CreateMap<ProductUpdateTagsViewModel, ProductUpdateTagsRequest>();
            CreateMap<TagsViewViewModel, TagsViewRequest>();
            CreateMap<TagViewModel, TagRequest>();

            CreateMap<ProductUpdateTagsGroupViewModel, ProductUpdateTagsGroupRequest>();
            CreateMap<TagsGroupViewModel, TagsGroupViewRequest>();
            CreateMap<ProductTagsResponse, TagsGroupResponse>();

            CreateMap<ProductUpdateConfigOderViewModel, ProductUpdateConfigOderRequest>();
            CreateMap<ProductUpdateStatusViewModel, ProductUpdateStatusRequest>();

            CreateMap<ProductUpdateHotViewModel, ProductUpdateHotRequest>();

            CreateMap<ProductUpdateHomeViewModel, ProductUpdateHomeRequest>();

            CreateMap<ProductTagsResponse, ProductTagsForViewResponse>()
                .ForMember(d => d.TypeResponse, o => o.Ignore());
            #endregion

            #region serviceprice 
            CreateMap<ServicePriceImportServiceFromDigiPostViewModel, ServicePriceImportServiceFromDigiPostRequest>();
            CreateMap<ServicePriceGetByPageViewModel, ServicePriceGetByPageRequest>();
            CreateMap<ServicePriceUpdateServicepriceFromDigipostViewModel, ServicePriceUpdateServicepriceFromDigipostRequest>();
            CreateMap<ServicePriceUpdateListServicepriceFromDigipostViewModel, ServicePriceUpdateListServicepriceFromDigipostRequest>();
            #endregion

            #region group service
            CreateMap<GroupServiceUpdateInfoViewModel, GroupServiceUpdateInfoRequest>();

            CreateMap<GroupServiceUpdateImageViewModel, GroupServiceUpdateImageRequest>();
            CreateMap<GroupServiceUpdateNumberTicketViewModel, GroupServiceUpdateNumberTicketRequest>();

            CreateMap<GroupServiceUpdateGroupTimeViewModel, GroupServiceUpdateGroupTimeRequest>();
            CreateMap<grouptTimeGrouptSerivceViewModel, grouptTimeGrouptSerivceRequest>();

            CreateMap<GroupServiceUpdateGroupNumberVIewModel, GroupServiceUpdateGroupNumberRequest>();
            CreateMap<grouptGroupServiceViewModel, grouptGroupServiceRequest>();

            CreateMap<GroupServiceUpdateTagsViewModel, GroupServiceUpdateTagsRequest>();
            CreateMap<GroupServiceUpdateTagsProcessViewModel, GroupServiceUpdateTagsProcessRequest>();
            CreateMap<GroupServiceUpdateTagsViewViewModel, GroupServiceUpdateTagsViewRequest>();
            CreateMap<TagsGrouptServiceViewModel, TagsGrouptServiceRequest>();
            #endregion

            #region Tags
            CreateMap<ProductTagsGetByPageViewModel, ProductTagsGetByPageRequest>();
            CreateMap<ProductTagsAddViewModel, ProductTagsAddRequest>();
            CreateMap<ProductTagsUpdateIdxViewModel, ProductTagsUpdateIdxRequest>();
            #endregion

            #region Meida
            CreateMap<MediaGetByPageViewModel, MediaGetByPageRequest>();
            CreateMap<MediaUploadViewModel, MediaUploadRequest>();
            CreateMap<MediaResizeViewModel, MediaResizeRequest>();
            #endregion

            #region base
            CreateMap<BaseViewModel, BaseRequest>();
            CreateMap<FilterViewModel, FilterRequest>();
            #endregion

            #region Banner
            CreateMap<BannerViewModel, BannerRequest>();
            CreateMap<BannerAddViewModel, BannerAddRequest>();
            CreateMap<BannerEditViewModel, BannerEditRequest>();

            CreateMap<BannerTypeViewModel, BannerTypeRequest>();
            CreateMap<BannerTypeEditViewModel, BannerTypeEditRequest>();
            CreateMap<BannerTypeAddViewModel, BannerTypeAddRequest>();
            #endregion

            #region Lib category
            CreateMap<LibCategoryUpdateViewModel, LibCategoryUpdateRequest>()
                .ForMember(d => d.Description, opt => opt.MapFrom(s => s.Description.IsTrueOrFalse() ? s.Description : ""))
                .ForMember(d => d.Url, opt => opt.MapFrom(s => s.Url.IsTrueOrFalse() ? s.Url : ""));

            CreateMap<LibDestinationViewModel, LibDestinationRequest>();
            CreateMap<LibDestinationUpdateViewModel, LibDestinationUpdateRequest>();
            #endregion

            #region Users
            CreateMap<WorkgroupGetUserViewModel, WorkgroupGetUserRequest>();
            CreateMap<WorkgroupAddUserViewModel, WorkgroupAddUserRequest>();
            #endregion

            #region Content
            CreateMap<TemplateComponentTypeEditViewModel, TemplateComponentTypeEditRequest>();

            CreateMap<TemplateComponentAddViewModel, TemplateComponentAddRequest>();
            CreateMap<TemplateComponentGetByPageViewModel, TemplateComponentGetByPageRequest>();
            CreateMap<TemplateComponentEditViewModel, TemplateComponentEditRequest>();
            CreateMap<PageTagResponse, PageTagResponseButLikeCategoryModel>()
                .ForMember(d => d.TagId, o => o.Ignore());

            CreateMap<TemplateAddViewModel, TemplateAddRequest>();
            CreateMap<TemplateGetByPageViewModel, TemplateGetByPageRequest>();
            CreateMap<TemplateEditViewModel, TemplateEditRequest>();
            CreateMap<TemplateMapViewModel, TemplateMapRequest>();
            CreateMap<TemplateMapEditViewModel, TemplateMapEditRequest>();

            CreateMap<PageAddViewModel, PageAddRequest>();
            CreateMap<PageGetByPageViewModel, PageGetByPageRequest>();
            CreateMap<PageUpdateStatusViewModel, PageUpdateStatusRequest>();
            CreateMap<PageUpdateURLTargetViewModel, PageUpdateURLTargetRequest>();
            CreateMap<PageEditViewModel, PageEditRequest>();
            CreateMap<PageComponentAddViewModel, PageComponentAddRequest>();

            CreateMap<PageComponentItemsGetByPageViewModel, PageComponentItemsGetByPageRequest>();
            CreateMap<PageTagViewModel, PageTagRequest>();
            CreateMap<PageComponentItemsAddViewModel, PageComponentItemsAddRequest>();
            CreateMap<PageComponentItemsEditViewModel, PageComponentItemsEditRequest>();
            CreateMap<PageComponentEditViewModel, PageComponentEditRequest>();
            CreateMap<PageComponentStatusEditViewModel, PageComponentStatusEditRequest>();
            CreateMap<PageComponentUpdateIdxViewModel, PageComponentUpdateIdxRequest>();
            CreateMap<PageComponentItemsUpdateIdxViewModel, PageComponentItemsUpdateIdxRequest>();
            CreateMap<PageComponentUpdateIdxArrayViewModel, PageComponentUpdateIdxArrayRequest>();
            CreateMap<PageComponentItemUpdateIdxArrayViewModel, PageComponentItemUpdateIdxArrayRequest>();
            #endregion

            #region Category Tag
            CreateMap<CategoryGetByPageViewModel, CategoryTagGetByPageRequest>();
            CreateMap<CategoryTagAddViewModel, CategoryTagAddRequest>();
            CreateMap<CategoryTagEditRequestViewModel, CategoryTagEditRequest>();
            CreateMap<CategoryTagUpdateHomeViewModel, CategoryTagUpdateHomeRequest>();
            CreateMap<CategoryTagUpdateHotViewModel, CategoryTagUpdateHotRequest>();
            CreateMap<CategoryTagEditViewModel, CategoryTagEditRequest>();

            #endregion

        }
    }
    public class ResponseToViewModel : Profile
    {
        public ResponseToViewModel()
        {
            #region Account
            CreateMap<ProfileResponse, UserCookie>()
                .ForMember(d => d.AccessToken, o => o.Ignore())
                .ForMember(d => d.Expires, o => o.Ignore())
                .ForMember(d => d.RefreshToken, o => o.Ignore());
            CreateMap<LoginResponse, UserCookie>();

            #endregion

            #region supplier
            CreateMap<SupplierResponse, SupplierViewModel>();
            #endregion

            #region service price
            CreateMap<ServicePriceResponse, ServicePriceViewToViewModel>();
            CreateMap<ListPriceResponse, ListPriceViewModel>();
            #endregion
        }
    }
}