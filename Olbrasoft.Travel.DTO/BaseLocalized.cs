using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Olbrasoft.Travel.DTO
{
    public class BaseLocalized : Creator, ILocalized
    {
        [Key, Column(Order = 2)]
        public int LanguageId { get; set; }

    }
}