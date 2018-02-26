namespace Olbrasoft.Travel.EAN
{
    public class EanLanguage
    {

        public string LanguageCode { get; set; }

        public string Name { get; set; }

        /// <summary>
        /// https://support.ean.com/hc/en-us/articles/115005777405#locale
        /// Default: en_US
        /// </summary>
        public bool IsDefault => LanguageCode == "en_US";
    }
}
