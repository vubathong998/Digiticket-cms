using DigiticketCMS.Services.Models;
using Infrastructure.Config;
using Services.Interfaces;
using Services.Models;
using Services.Models.Requests.Tags;
using Services.Models.Responses.Tags;
using System;
using System.Collections.Generic;
using System.Linq;
using Utilities.Caching;

namespace Services.Implement
{
    public class TagsServices : ITagsServices
    {
        private BaseServices _baseService;
        public TagsServices()
        {
            _baseService = new BaseServices(APIConfig.API_SERVICES_URL);
        }
        public BaseResponse<PagingResult<ProductTagsResponse>> GetByPage(ProductTagsGetByPageRequest model, string token)
        {
            var result = _baseService.PostApi<PagingResult<ProductTagsResponse>>("/api/tag/get-by-page", model, token);
            return result;
        }
        public BaseResponse<int> Add(ProductTagsAddRequest model, string token)
        {
            var result = _baseService.PostApi<int>("/api/tag", model, token);
            return result;
        }
        public BaseResponse<string> UpdateIdx(ProductTagsUpdateIdxRequest model, string token)
        {
            var result = _baseService.PutApi<string>($"/api/tag/{model.Id}/update-idx", model, token);
            return result;
        }
        public BaseResponse<string> Delete(int id, string token)
        {
            var result = _baseService.DeleteApi<string>($"/api/tag/{id}", null, token);
            return result;
        }
        public BaseResponse<ProductTagsResponse> GetById(int id, string token)
        {
            var result = _baseService.GetApi<ProductTagsResponse>($"/api/tag/{id}", null, token);
            return result;
        }
        public BaseResponse<List<ProductTagsResponse>> GetList(TagsGetListRequest model, string token)
        {
            var result = _baseService.GetApi<List<ProductTagsResponse>>($"api/tag/get-list", model, token);
            return result;
        }
        public GetListTagsAndSubTagsCache GetListTagsAndSubTagsCache(int typeParent, string token)
        {
            var keyToken = CacheNameConfig.GroupServiceTags;
            var cache = CacheUtility.GetValue(keyToken) as GetListTagsAndSubTagsCache;

            if (cache == null || cache.ParentTags == null || cache.SubTags == null)
            {
                var result = new GetListTagsAndSubTagsCache();
                var request = new ProductTagsGetByPageRequest()
                {
                    PageIndex = 1,
                    PageSize = 10,
                    FieldName = "CreatedDate",
                    Orderby = "Desc",
                    Filter = new List<FilterRequest>()
                            {
                                new FilterRequest()
                                {
                                    Opt = FilterOptionOpt.AND,
                                    Name = "type",
                                    Opt1 = FilterOptionOpt1.EqualTo,
                                    Type = FilterOptionType.Number,
                                    Values = typeParent.ToString()
                                },
                                new FilterRequest()
                                {
                                    Opt = FilterOptionOpt.AND,
                                    Name = "parentId",
                                    Opt1 = FilterOptionOpt1.EqualTo,
                                    Type = FilterOptionType.Number,
                                    Values = "0"
                                }
                            }
                };
                var parentTags = _baseService.PostApi<PagingResult<ProductTagsResponse>>("/api/tag/get-by-page", request, token);
                if (parentTags.IsOK() && parentTags.Data != null)
                {
                    result.ParentTags = parentTags.Data.Result.ToList();
                    foreach (var item in result.ParentTags)
                    {
                        var sibRequest = new TagsGetListRequest()
                        {
                            Type = 2,
                            ParentId = item.Id,
                        };
                        var subTag = _baseService.GetApi<List<ProductTagsResponse>>($"api/tag/get-list", sibRequest, token);
                        if (subTag.IsOK() && subTag.Data != null)
                        {
                            foreach (var subItem in subTag.Data)
                            {
                                result.SubTags.Add(subItem);
                            }
                        }
                    }
                }
                CacheUtility.Add(keyToken, result, DateTimeOffset.Now.AddMonths(1));
                return result;
            }
            return cache;
        }

        public GetListTagsCache GetListTagsCache(int typeParent, string token)
        {
            var keyToken = CacheNameConfig.GroupServiceTags;
            var cache = CacheUtility.GetValue(keyToken) as GetListTagsCache;

            if (cache == null || cache.Tags == null)
            {
                var result = new GetListTagsCache();
                var request = new ProductTagsGetByPageRequest()
                {
                    PageIndex = 1,
                    PageSize = 20,
                    FieldName = "CreatedDate",
                    Orderby = "Desc",
                    Filter = new List<FilterRequest>()
                    {
                        new FilterRequest()
                        {
                            Opt = FilterOptionOpt.AND,
                            Name = "type",
                            Opt1 = FilterOptionOpt1.EqualTo,
                            Type = FilterOptionType.Number,
                            Values = typeParent.ToString()
                        }
                    }
                };
                var TagsRespone = _baseService.PostApi<PagingResult<ProductTagsResponse>>("/api/tag/get-by-page", request, token);
                if (TagsRespone.IsOK() && TagsRespone.Data != null)
                {
                    result.Tags = TagsRespone.Data.Result.ToList();
                    CacheUtility.Add(keyToken, result, DateTimeOffset.Now.AddMonths(1));
                }
                return result;
            }

            return cache;
        }
    }
}