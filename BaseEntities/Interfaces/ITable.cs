namespace BaseEntities.Interfaces
{
    public interface ITable<T> : IIsDeleted, IChangedBy, IChangeDate, IIdentityEntity<T>
    {
    }
}
