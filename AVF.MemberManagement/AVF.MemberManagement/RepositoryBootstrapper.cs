using AVF.MemberManagement.Proxies;
using AVF.MemberManagement.StandardLibrary.Interfaces;
using AVF.MemberManagement.StandardLibrary.Models;
using AVF.MemberManagement.StandardLibrary.Options;
using AVF.MemberManagement.StandardLibrary.Proxies;
using AVF.MemberManagement.StandardLibrary.Repositories;
using AVF.MemberManagement.StandardLibrary.Tables;
using AVF.MemberManagement.StandardLibrary.Tbo;
using Microsoft.Extensions.Options;
using Prism.Ioc;
using Unity;
using Unity.Lifetime;

namespace AVF.MemberManagement
{
    public class RepositoryBootstrapper : IRepositoryBootstrapper
    {
        private readonly IContainerRegistry _container;

        public RepositoryBootstrapper(IContainerRegistry container)
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
                IOptions<FileProxyOptions> fileProxyOptions = new OptionsWrapper<FileProxyOptions>(new FileProxyOptions());
                fileProxyOptions.Value.ShouldSimulateDelay = false;
                fileProxyOptions.Value.FileProxyDelayTimes.Add(nameof(User), new FileProxyDelayTimes { GetAsyncFullTableDelay = 1, GetAsyncSingleEntryDelay = 1 });
                _container.RegisterInstance(fileProxyOptions);

                RegisterFileProxies();
            }

            _container.RegisterSingleton<IRepository<Beitragsklasse>, CachedRepository<Beitragsklasse>>();
            _container.RegisterSingleton<IRepository<Familienrabatt>, Repository<Familienrabatt>>();
            _container.RegisterSingleton<IRepository<Graduierung>, Repository<Graduierung>>();
            _container.RegisterSingleton<IRepository<Kurs>, CachedRepository<Kurs>>();
            _container.RegisterSingleton<IRepository<Mitglied>, CachedRepository<Mitglied>>();
            _container.RegisterSingleton<IRepository<Pruefung>, Repository<Pruefung>>();
            _container.RegisterSingleton<IRepositoryBase<Setting, string>, CachedSettingsRepository>();
            _container.RegisterSingleton<IRepository<Stundensatz>, Repository<Stundensatz>>();
            _container.RegisterSingleton<IRepository<Test>, Repository<Test>>();
            _container.RegisterSingleton<IRepository<TrainerErnennung>, Repository<TrainerErnennung>>();
            _container.RegisterSingleton<IRepository<TrainerStufe>, Repository<TrainerStufe>>();
            _container.RegisterSingleton<IRepository<Training>, CachedRepository<Training>>();
            _container.RegisterSingleton<IRepository<TrainingsTeilnahme>, CachedRepository<TrainingsTeilnahme>>();
            _container.RegisterSingleton<IRepository<User>, CachedRepository<User>>();
            _container.RegisterSingleton<IRepository<Wochentag>, CachedRepository<Wochentag>>();
            _container.RegisterSingleton<IRepository<Wohnung>, Repository<Wohnung>>();
            _container.RegisterSingleton<IRepository<Wohnungsbezug>, Repository<Wohnungsbezug>>();
            _container.RegisterSingleton<IRepository<ZuschlagKindertraining>, Repository<ZuschlagKindertraining>>();
        }

        private void RegisterWebProxies()
        {
            _container.Register<IProxy<Beitragsklasse>, Proxy<TblBeitragsklassen, Beitragsklasse>>();
            _container.Register<IProxy<Familienrabatt>, Proxy<TblFamilienrabatte, Familienrabatt>>();
            _container.Register<IProxy<Graduierung>, Proxy<TblGraduierungen, Graduierung>>();
            _container.Register<IProxy<Kurs>, Proxy<TblKurse, Kurs>>();
            _container.Register<IProxy<Mitglied>, Proxy<TblMitglieder, Mitglied>>();
            _container.Register<IProxy<Pruefung>, Proxy<TblPruefungen, Pruefung>>();
            _container.Register<IProxyBase<Setting, string>, ProxyBase<TblSettings, Setting, string>>();
            _container.Register<IProxy<Stundensatz>, Proxy<TblStundensaetze, Stundensatz>>();
            _container.Register<IProxy<Test>, Proxy<TblTests, Test>>();
            _container.Register<IProxy<TrainerErnennung>, Proxy<TblTrainerErnennungen, TrainerErnennung>>();
            _container.Register<IProxy<TrainerStufe>, Proxy<TblTrainerStufen, TrainerStufe>>();
            _container.Register<IProxy<Training>, Proxy<TblTrainings, Training>>();
            _container.Register<IProxy<TrainingsTeilnahme>, Proxy<TblTrainingsTeilnahmen, TrainingsTeilnahme>>();
            _container.Register<IProxy<User>, Proxy<TblUsers, User>>();
            _container.Register<IProxy<Wochentag>, Proxy<TblWochentage, Wochentag>>();
            _container.Register<IProxy<Wohnung>, Proxy<TblWohnungen, Wohnung>>();
            _container.Register<IProxy<Wohnungsbezug>, Proxy<TblWohnungsbezug, Wohnungsbezug>>();
            _container.Register<IProxy<ZuschlagKindertraining>, Proxy<TblZuschlaegeKindertraining, ZuschlagKindertraining>>();
        }

        private void RegisterFileProxies()
        {
            _container.Register<IProxy<Beitragsklasse>, FileProxy<TblBeitragsklassen, Beitragsklasse>>();
            _container.Register<IProxy<Familienrabatt>, FileProxy<TblFamilienrabatte, Familienrabatt>>();
            _container.Register<IProxy<Graduierung>, FileProxy<TblGraduierungen, Graduierung>>();
            _container.Register<IProxy<Kurs>, FileProxy<TblKurse, Kurs>>();
            _container.Register<IProxy<Mitglied>, FileProxy<TblMitglieder, Mitglied>>();
            _container.Register<IProxy<Pruefung>, FileProxy<TblPruefungen, Pruefung>>();
            _container.Register<IProxyBase<Setting, string>, FileProxyBase<TblSettings, Setting, string>>();
            _container.Register<IProxy<Stundensatz>, FileProxy<TblStundensaetze, Stundensatz>>();
            _container.Register<IProxy<Test>, FileProxy<TblTests, Test>>();
            _container.Register<IProxy<TrainerErnennung>, FileProxy<TblTrainerErnennungen, TrainerErnennung>>();
            _container.Register<IProxy<TrainerStufe>, FileProxy<TblTrainerStufen, TrainerStufe>>();
            _container.Register<IProxy<Training>, FileProxy<TblTrainings, Training>>();
            _container.Register<IProxy<TrainingsTeilnahme>, FileProxy<TblTrainingsTeilnahmen, TrainingsTeilnahme>>();
            _container.Register<IProxy<User>, FileProxy<TblUsers, User>>();
            _container.Register<IProxy<Wochentag>, FileProxy<TblWochentage, Wochentag>>();
            _container.Register<IProxy<Wohnung>, FileProxy<TblWohnungen, Wohnung>>();
            _container.Register<IProxy<Wohnungsbezug>, FileProxy<TblWohnungsbezug, Wohnungsbezug>>();
            _container.Register<IProxy<ZuschlagKindertraining>, FileProxy<TblZuschlaegeKindertraining, ZuschlagKindertraining>>();
        }
    }
}