namespace Olbrasoft.Travel.DTO
{
    public class CreatorInfo : CreationInfo
    {
        public int CreatorId { get; set; }

        public User Creator { get; set; }

    }
}