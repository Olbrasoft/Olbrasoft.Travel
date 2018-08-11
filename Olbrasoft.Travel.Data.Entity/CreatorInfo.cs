namespace Olbrasoft.Travel.Data.Entity
{
    public class CreatorInfo : CreationInfo
    {
        public int CreatorId { get; set; }

        public User Creator { get; set; }

    }
}