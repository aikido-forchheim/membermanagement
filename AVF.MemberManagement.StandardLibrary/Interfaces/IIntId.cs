namespace AVF.MemberManagement.StandardLibrary.Interfaces
{
    public interface IIntId : IId<int>
    {
        
    }

    public interface IId<T>
    {
        T Id { get; set; }
    }
}
