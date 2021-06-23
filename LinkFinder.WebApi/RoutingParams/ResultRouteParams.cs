namespace LinkFinder.WebApi.RoutingParams
{
    public class ResultRouteParams
    {
        /// <summary>
        /// Test id the results of which must be obtained 
        /// </summary>
        public int? Id { get; set; }
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
