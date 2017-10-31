namespace AVF.MemberManagement.StandardLibrary.Interfaces
{
    public interface ICrudRepository<T> : IRepositoryBase<T, int> where T : IIntId
    {
        bool UpdateAsync(T obj);

        bool InsertAsync(T obj);

        bool DeleteAsync(T obj);
    }
}
