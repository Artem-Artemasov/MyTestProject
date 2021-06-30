using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;

namespace LinkFinder.WebApi.Logic.Request 
{ 
    public class GetTestDetailParam
    {
        /// <summary>
        /// True when URL exist in sitemap. Can be unset. 
        /// Default value = false
        /// </summary>
        public bool InSitemap { get; set; } = false;
        /// <summary>
        /// True when URL exist in html code. Can be unset.
        /// Default value = false
        /// </summary>
        public bool InHtml { get; set; } = false;
        /// <summary>
        /// Page that needed
        /// By default = 1
        /// </summary>
        public int Page { get; set; } = 1;
        /// <summary>
        /// Count of results on all pages
        /// By default = 10 items
        /// </summary>
        public int CountResultsOnPage { get; set; } = 10;
    }
}
