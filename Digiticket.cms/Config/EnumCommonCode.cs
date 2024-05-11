using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Xml.Linq;

namespace DigiticketCMS.Config
{
    public static class EnumCommonCode
    {
        public enum EnumCommissionUnit
        {
            [Display(Name = "Tiền mặt", Description = "custom-input-currency")]
            Cash = 1,
            [Display(Name = "Phần trăm (%)", Description = "custom-input-percent")]
            Percent = 2
        }
        public enum ETagsItemType
        {
            [Display(Name = "", Description = "")]
            All = 0,
            [Display(Name = "Product", Description = "")]
            Product = 1,
            [Display(Name = "GroupService", Description = "")]
            GroupService = 2,
            [Display(Name = "GroupServiceProcess", Description = "")]
            GroupServiceProcess = 3,
            [Display(Name = "ProductHighlight", Description = "")]
            ProductHighlight = 4,
            [Display(Name = "GroupServiceView ", Description = "")]
            GroupServiceView = 5,
        }
        public enum ETagsStatusProduct
        {
            [Display(Name = "Chưa thiết lập", Description = "")]
            NotSetYet = 0,
            [Display(Name = "Mở bán", Description = "")]
            Available = 1,
            [Display(Name = "Sắp mở bán", Description = "text-success")]
            Planning = 2,
            [Display(Name = "Tạm hết hàng", Description = "text-warning")]
            CurrentlyOutOfStock = 3,
            [Display(Name = "Hết hàng", Description = "text-danger")]
            SetOutOfStock = 4
        }
        public enum URLType
        {
            [Display(Name = "HasIndex", Description = "")]
            HasIndex = 1,
            [Display(Name = "NoIndex", Description = "")]
            NoIndex = 2
        }
        public enum EnumPageStatus
        {
            [Display(Name = "Public", Description = "")]
            Public = 1,
            [Display(Name = "Chưa public", Description = "")]
            UnPublic = 2
        }
        public enum EnumPageComponentStatus
        {
            [Display(Name = "Public", Description = "")]
            Public = 1,
            [Display(Name = "Chưa public", Description = "")]
            UnPublic = 2
        }
        public enum EnumPageComponentItemType
        {
            [Display(Name = "Banner_1_Medium", Description = "")]
            Banner_1_Medium = 1,
            [Display(Name = "Banner_1_In_Component_Intro", Description = "")]
            Banner_1_In_Component_Intro = 2,
            [Display(Name = "Experience_1", Description = "")]
            Experience_1 = 3,
            [Display(Name = "Product_1_6", Description = "")]
            Product_1_6 = 4,
            [Display(Name = "Product_1_Left", Description = "")]
            Product_1_Left = 5,
            [Display(Name = "Btn_Link", Description = "")]
            Btn_Link = 6,
            [Display(Name = "Product_1_8", Description = "")]
            Product_1_8 = 7,
            [Display(Name = "Video_1_Left", Description = "")]
            Video_1_Left = 8,
            [Display(Name = "Video_1_3", Description = "")]
            Video_1_3 = 9,
            [Display(Name = "Image_1_Left", Description = "")]
            Image_1_Left = 10,
            [Display(Name = "Image_1_3", Description = "")]
            Image_1_3 = 11,
            [Display(Name = "Banner_2_Big", Description = "")]
            Banner_2_Big = 12,
            [Display(Name = "Banner_2_Small", Description = "")]
            Banner_2_Small = 13,
            [Display(Name = "Banner_1_Big", Description = "")]
            Banner_1_Big = 14,
            [Display(Name = "Video_1_Right", Description = "")]
            Video_1_Right = 15,
            [Display(Name = "Image_1_Right", Description = "")]
            Image_1_Right = 16,
            [Display(Name = "Banner_3_Big", Description = "")]
            Banner_3_Big = 17,
            [Display(Name = "Banner_3_Small", Description = "")]
            Banner_3_Small = 18,
            [Display(Name = "Product_1_Right", Description = "")]
            Product_1_Right = 19,
            [Display(Name = "Banner_5_Big", Description = "")]
            Banner_5_Big = 20,
            [Display(Name = "Banner_5_Small", Description = "")]
            Banner_5_Small = 21,
            [Display(Name = "Slide_1", Description = "")]
            Slide_1 = 22,
            [Display(Name = "Product_1_4", Description = "")]
            Product_1_4 = 23,
            [Display(Name = "Banner_2_Medium", Description = "")]
            Banner_2_Medium = 24,
            [Display(Name = "Banner_1_4", Description = "")]
            Banner_1_4 = 25,
            [Display(Name = "Product_1_3", Description = "")]
            Product_1_3 = 26,
            [Display(Name = "Chọn Page", Description = "")]
            Page = 27
        }
        //[Display(Name = "BlogDetail_Tag_Sidebar", Description = "")]
        //BlogDetail_Tag_Sidebar = 27

        public enum EnumRedirectTypeTarget
        {
            [Display(Name = "Redirect301", Description = "")]
            Redirect301 = 1
        }

        public enum EnumLinkOption
        {
            [Display(Name = "Không có", Description = "")]
            no = 0,
            [Display(Name = "nofollow", Description = "")]
            nofollow = 1

        }

        #region Page type
        public enum EnumPageType
        {
            [Display(Name = "Common", Description = "")]
            Common = 1,                                             // {domain}/vi/{link}
            [Display(Name = "Blog", Description = "")]
            Blog = 2                                                // {domain}/blog/...
        }

        public enum EnumURLType
        {
            [Display(Name = "Has index", Description = "")]
            HasIndex = 1,
            [Display(Name = "No index", Description = "")]
            NoIndex = 2,
        }
        #endregion
        public static string GetDisplayNameEnum<T>(T input)
        {
            Type type = input.GetType();
            MemberInfo[] memInfo = type.GetMember(input.ToString());
            if (memInfo != null && memInfo.Length > 0)
            {
                object[] attrs = (object[])memInfo[0].GetCustomAttributes(typeof(DisplayAttribute), false);
                if (attrs != null && attrs.Length > 0)
                {
                    return ((DisplayAttribute)attrs[0]).Name;
                }
            }
            return input.ToString();
        }

        public static string GetDisplayDescriptionEnum<T>(T input)
        {
            Type type = input.GetType();
            MemberInfo[] memInfo = type.GetMember(input.ToString());
            if (memInfo != null && memInfo.Length > 0)
            {
                object[] attrs = (object[])memInfo[0].GetCustomAttributes(typeof(DisplayAttribute), false);
                if (attrs != null && attrs.Length > 0)
                {
                    return ((DisplayAttribute)attrs[0]).Description;
                }
            }
            return input.ToString();
        }
    }
    public enum EnumPriceTimeType
    {
        RangeTime = 0,
        FixedDates = 1
    }
}