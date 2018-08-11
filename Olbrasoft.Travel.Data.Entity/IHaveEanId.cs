namespace Olbrasoft.Travel.Data.Entity
{
    public interface IHaveEanId<T>
    {
        T EanId { get; set; }
    }
}