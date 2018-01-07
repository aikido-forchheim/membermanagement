namespace AVF.MemberManagement
{
    public interface IRepositoryBootstrapper
    {
        void RegisterRepositories(bool useFileProxies);
    }
}