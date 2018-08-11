using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Attribute = Olbrasoft.Travel.Data.Entity.Attribute;

namespace Olbrasoft.Travel.Data.Entity
{
    public class AccommodationToAttribute
    {
        [Key, Column(Order = 1)]
        public int AccommodationId { get; set; }

        [Key, Column(Order = 2)]
        public int AttributeId { get; set; }

        [Key, Column(Order = 3)]
        public int LanguageId { get; set; }

        public int CreatorId { get; set; }
        
        [StringLength(800)]
        public string Text { get; set; }

        public DateTime DateAndTimeOfCreation { get; set; }

        public User Creator { get; set; }
        public Accommodation Accommodation { get; set; }
        public Attribute Attribute { get; set; }
        public Language Language { get; set; }

    }
}
