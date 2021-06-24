namespace LinkFinder.WebApi.RoutingParams
{
    public class TestDetailParam
    {
        /// <summary>
        /// True when URL exist in sitemap. Can be unset. Default value = false
        /// </summary>
        public bool InSitemap { get; set; } = false;
        /// <summary>
        /// True when URL exist in html code. Can be unset. Default value = false
        /// </summary>
        public bool InHtml { get; set; } = false;
    }
}
