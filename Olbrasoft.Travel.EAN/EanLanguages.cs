using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Olbrasoft.Travel.EAN
{
    /// <summary>
    /// https://support.ean.com/hc/en-us/articles/115005813145
    /// </summary>
    public class EanLanguages
    {

        private static IEnumerable<EanLanguage> _eanLanguages;

        public static IEnumerable<EanLanguage> GetLanguages()
        {
            return _eanLanguages ?? (_eanLanguages = BuildEanLanguages());
        }
        
        private static IEnumerable<EanLanguage> BuildEanLanguages()
        {
            var result = new List<EanLanguage>()
            {
                new EanLanguage{LanguageCode="fr_CA",Name="French(Canada)"},
                new EanLanguage{LanguageCode="hu_HU",Name="Hungarian"},
                new EanLanguage{LanguageCode="hr_HR",Name="Croatian"},
                new EanLanguage{LanguageCode="in_ID",Name="Indonesian"},
                new EanLanguage{LanguageCode="is_IS",Name="Icelandic"},
                new EanLanguage{LanguageCode="it_IT",Name="Italian"},
                new EanLanguage{LanguageCode="ja_JP",Name="Japanese"},
                new EanLanguage{LanguageCode="ko_KR",Name="Korean"},
                new EanLanguage{LanguageCode="no_NO",Name="Norwegian"},
                new EanLanguage{LanguageCode="ms_MY",Name="Malay"},
                new EanLanguage{LanguageCode="lv_LV",Name="Latvian"},
                new EanLanguage{LanguageCode="lt_LT",Name="Lithuanian"},
                new EanLanguage{LanguageCode="nl_NL",Name="Dutch"},
                new EanLanguage{LanguageCode="ar_SA",Name="Arabic"},
                new EanLanguage{LanguageCode="en_US",Name="English"},
                new EanLanguage{LanguageCode="cs_CZ",Name="Czech"},
                new EanLanguage{LanguageCode="da_DK",Name="Danish"},
                new EanLanguage{LanguageCode="de_DE",Name="German"},
                new EanLanguage{LanguageCode="el_GR",Name="Greek"},
                new EanLanguage{LanguageCode="es_ES",Name="Spanish(Spain)"},
                new EanLanguage{LanguageCode="es_MX",Name="Spanish(Mexico)"},
                new EanLanguage{LanguageCode="et_EE",Name="Estonian"},
                new EanLanguage{LanguageCode="fi_FI",Name="Finnish"},
                new EanLanguage{LanguageCode="fr_FR",Name="French"},
                new EanLanguage{LanguageCode="pl_PL",Name="Polish"},
                new EanLanguage{LanguageCode="pt_BR",Name="Portuguese (Brazil)"},
                new EanLanguage{LanguageCode="pt_PT",Name="Portuguese (Portugal)"},
                new EanLanguage{LanguageCode="ru_RU",Name="Russian"},
                new EanLanguage{LanguageCode="sv_SE",Name="Swedish"},
                new EanLanguage{LanguageCode="sk_SK",Name="Slovak"},
                new EanLanguage{LanguageCode="th_TH",Name="Thai"},
                new EanLanguage{LanguageCode="tr_TR",Name="Turkish"},
                new EanLanguage{LanguageCode="uk_UA",Name="Ukranian"},
                new EanLanguage{LanguageCode="vi_VN",Name="Vietnamese"},
                new EanLanguage{LanguageCode="zh_TW",Name="Traditional Chinese"},
                new EanLanguage{LanguageCode="zh_CN",Name="Simplified Chinese"},
            };

            return result;
        }


        public static bool TryParseCultureInfo(string languageCode, out CultureInfo cultureInfo)
        {
            //TODO must Api test
            //https://stackoverflow.com/questions/44245959/android-generating-wrong-language-code-for-indonesia
            //in_ID->id_ID
            //http://i18n.skolelinux.no/localekoder.txt
            //no_NO->nb_NO
            languageCode = languageCode
                .Replace("in_ID", "id_ID")
                .Replace("no_NO", "nn_NO");

            cultureInfo =
             CultureInfo
            .GetCultures(CultureTypes.AllCultures)
            .FirstOrDefault(cul => cul.Parent.IsNeutralCulture && cul.Name == languageCode.Replace("_", "-"));

            return cultureInfo != null;
        }
        

        public IEnumerable<CultureInfo> GetSupportedCultures()
        {
            List<CultureInfo> cultures = new List<CultureInfo>();
            foreach (var language in GetLanguages())
            {

                if (TryParseCultureInfo(language.LanguageCode, out var cultureInfo))
                {
                    cultures.Add(cultureInfo);
                }
                else
                {
                    throw new Exception(nameof(language));
                }
            }

            return cultures;
        }

    }
}
