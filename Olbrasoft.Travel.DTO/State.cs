namespace Olbrasoft.Travel.DTO
{
    /// <summary>
    /// State or Province
    /// </summary>
    public class State : Geo
    {
        public int CountryId { get; set; }

        public Country Country { get; set; }
    }

}
