using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Models.Responses.Product
{
    public class PlaceResponse
    {

    }

    #region ssid
    public class ProductPlaceGetSSIDResponse
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public string Data { get; set; }
    }
    #endregion

    #region suggest


    public class ProductPlaceSuggestResponse
    {
        public List<Prediction> Predictions { get; set; }
        public string RawJson { get; set; }
        public string RawQueryString { get; set; }
        public string Status { get; set; }
        public object Error_message { get; set; }
        public object Html_attributions { get; set; }
    }
    public class Prediction
    {
        public string Place_id { get; set; }
        public string Description { get; set; }
        public StructuredFormatting Structured_formatting { get; set; }
        public List<Term> Terms { get; set; }
        public List<string> Types { get; set; }
        public List<MatchedSubstring> Matched_substrings { get; set; }
    }
    public class StructuredFormatting
    {
        public string Main_text { get; set; }
        public List<MainTextMatchedSubstring> Main_text_matched_substrings { get; set; }
        public string Secondary_text { get; set; }
        public object Secondary_text_matched_substrings { get; set; }
    }
    public class MainTextMatchedSubstring
    {
        public string Offset { get; set; }
        public string Length { get; set; }
    }
    public class Term
    {
        public string Value { get; set; }
        public string Offset { get; set; }
    }
    public class MatchedSubstring
    {
        public string Offset { get; set; }
        public string Length { get; set; }
    }
    #endregion

    #region 
    public class ProductPlaceDetailResponse
    {
        public object Place_id { get; set; }
        public string Address { get; set; }
        public List<AddressComponent> Address_components { get; set; }
        public object Location { get; set; }
        public object Viewport { get; set; }
        public int Level { get; set; }
        public string Parent_placeid { get; set; }
        public int Status { get; set; }
        public string Name { get; set; }
        public object Description { get; set; }
        public string Lang_code { get; set; }
    }
    public class AddressComponent
    {
        public string Short_name { get; set; }
        public string Long_Name { get; set; }
        public List<string> Types { get; set; }
    }
    #endregion
}
