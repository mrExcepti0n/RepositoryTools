namespace BaseEntities.Interfaces
{
    public interface IIdentityEntity<T>
    {
        T Id { get; set; }
    }
}
