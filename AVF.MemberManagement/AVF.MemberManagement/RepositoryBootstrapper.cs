using AVF.MemberManagement.Proxies;
using AVF.MemberManagement.StandardLibrary.Interfaces;
using AVF.MemberManagement.StandardLibrary.Proxies;
using AVF.MemberManagement.StandardLibrary.Repositories;
using AVF.MemberManagement.StandardLibrary.Tables;
using AVF.MemberManagement.StandardLibrary.Tbo;
using Microsoft.Practices.Unity;

namespace AVF.MemberManagement
{
    public class RepositoryBootstrapper
    {
        private readonly IUnityContainer _container;

        public RepositoryBootstrapper(IUnityContainer container)
        {
            _container = container;
        }

        public void RegisterRepositories(bool useFileProxies)
        {
            if (!useFileProxies)
            {
                RegisterWebProxies();
            }
            else
            {
                RegisterFileProxies();
            }

            _container.RegisterType<IRepository<Beitragsklasse>, Repository<Beitragsklasse>>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IRepository<Familienrabatt>, Repository<Familienrabatt>>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IRepository<Graduierung>, Repository<Graduierung>>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IRepository<Kurs>, Repository<Kurs>>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IRepository<Mitglied>, Repository<Mitglied>>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IRepository<Pruefung>, Repository<Pruefung>>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IRepositoryBase<Setting, string>, CachedSettingsRepository>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IRepository<Stundensatz>, Repository<Stundensatz>>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IRepository<Test>, Repository<Test>>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IRepository<TrainerErnennung>, Repository<TrainerErnennung>>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IRepository<TrainerStufe>, Repository<TrainerStufe>>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IRepository<Training>, Repository<Training>>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IRepository<TrainingsTeilnahme>, Repository<TrainingsTeilnahme>>();
            _container.RegisterType<IRepository<User>, CachedRepository<User>>();
            _container.RegisterType<IRepository<Wochentag>, Repository<Wochentag>>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IRepository<Wohnung>, Repository<Wohnung>>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IRepository<Wohnungsbezug>, Repository<Wohnungsbezug>>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IRepository<ZuschlagKindertraining>, Repository<ZuschlagKindertraining>>(new ContainerControlledLifetimeManager());
        }

        private void RegisterWebProxies()
        {
            _container.RegisterType<IProxy<Beitragsklasse>, Proxy<TblBeitragsklassen, Beitragsklasse>>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IProxy<Familienrabatt>, Proxy<TblFamilienrabatte, Familienrabatt>>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IProxy<Graduierung>, Proxy<TblGraduierungen, Graduierung>>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IProxy<Kurs>, Proxy<TblKurse, Kurs>>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IProxy<Mitglied>, Proxy<TblMitglieder, Mitglied>>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IProxy<Pruefung>, Proxy<TblPruefungen, Pruefung>>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IProxyBase<Setting, string>, ProxyBase<TblSettings, Setting, string>>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IProxy<Stundensatz>, Proxy<TblStundensaetze, Stundensatz>>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IProxy<Test>, Proxy<TblTests, Test>>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IProxy<TrainerErnennung>, Proxy<TblTrainerErnennungen, TrainerErnennung>>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IProxy<TrainerStufe>, Proxy<TblTrainerStufen, TrainerStufe>>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IProxy<Training>, Proxy<TblTrainings, Training>>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IProxy<TrainingsTeilnahme>, Proxy<TblTrainingsTeilnahmen, TrainingsTeilnahme>>();
            _container.RegisterType<IProxy<User>, Proxy<TblUsers, User>>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IProxy<Wochentag>, Proxy<TblWochentage, Wochentag>>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IProxy<Wohnung>, Proxy<TblWohnungen, Wohnung>>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IProxy<Wohnungsbezug>, Proxy<TblWohnungsbezug, Wohnungsbezug>>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IProxy<ZuschlagKindertraining>, Proxy<TblZuschlaegeKindertraining, ZuschlagKindertraining>>(new ContainerControlledLifetimeManager());
        }

        private void RegisterFileProxies()
        {
            _container.RegisterType<IProxy<Beitragsklasse>, FileProxy<TblBeitragsklassen, Beitragsklasse>>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IProxy<Familienrabatt>, FileProxy<TblFamilienrabatte, Familienrabatt>>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IProxy<Graduierung>, FileProxy<TblGraduierungen, Graduierung>>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IProxy<Kurs>, FileProxy<TblKurse, Kurs>>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IProxy<Mitglied>, FileProxy<TblMitglieder, Mitglied>>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IProxy<Pruefung>, FileProxy<TblPruefungen, Pruefung>>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IProxyBase<Setting, string>, FileProxyBase<TblSettings, Setting, string>>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IProxy<Stundensatz>, FileProxy<TblStundensaetze, Stundensatz>>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IProxy<Test>, FileProxy<TblTests, Test>>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IProxy<TrainerErnennung>, FileProxy<TblTrainerErnennungen, TrainerErnennung>>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IProxy<TrainerStufe>, FileProxy<TblTrainerStufen, TrainerStufe>>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IProxy<Training>, FileProxy<TblTrainings, Training>>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IProxy<TrainingsTeilnahme>, FileProxy<TblTrainingsTeilnahmen, TrainingsTeilnahme>>();
            _container.RegisterType<IProxy<User>, FileProxy<TblUsers, User>>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IProxy<Wochentag>, FileProxy<TblWochentage, Wochentag>>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IProxy<Wohnung>, FileProxy<TblWohnungen, Wohnung>>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IProxy<Wohnungsbezug>, FileProxy<TblWohnungsbezug, Wohnungsbezug>>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IProxy<ZuschlagKindertraining>, FileProxy<TblZuschlaegeKindertraining, ZuschlagKindertraining>>(new ContainerControlledLifetimeManager());
        }
    }
}