namespace AVF.MemberManagement.StandardLibrary.Interfaces
{
    public interface IIntId : IId<int>
    {
        
    }

    public interface IId<T>
    {
        string PrimaryKeyName { get; set; }

        T Id { get; set; }
    }
}
