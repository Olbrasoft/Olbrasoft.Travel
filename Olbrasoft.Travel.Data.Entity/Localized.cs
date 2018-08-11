using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Olbrasoft.Travel.Data.Entity
{
    public class Localized : CreatorInfo, ILocalized
    {
        [Key, Column(Order = 2)]
        public int LanguageId { get; set; }

        public virtual Language Language { get; set; }
    }
}