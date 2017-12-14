using System;
using System.Threading.Tasks;
using AVF.MemberManagement.StandardLibrary.Interfaces;
using AVF.MemberManagement.StandardLibrary.Models;
using AVF.MemberManagement.StandardLibrary.Tbo;

namespace AVF.MemberManagement.StandardLibrary.Services
{
    public class KursModelService : IKursModelService
    {
        private readonly IRepository<Mitglied> _mitgliederRepository;
        private readonly IRepository<Wochentag> _wochentageRepository;

        public KursModelService(IRepository<Mitglied> mitgliederRepository, IRepository<Wochentag> wochentageRepository)
        {
            _mitgliederRepository = mitgliederRepository;
            _wochentageRepository = wochentageRepository;
        }

        public async Task<Class> GetAsync(Kurs kurs)
        {
            var end = kurs.Zeit + new TimeSpan(0, kurs.DauerMinuten, 0);

            var kursModel = new Class
            {
                Wochentag = await _wochentageRepository.GetAsync(kurs.WochentagID),
                Trainer = await _mitgliederRepository.GetAsync(kurs.Trainer),
                Id = kurs.Id,
                Zeit = $"{kurs.Zeit.Hours.ToString().PadLeft(2,'0')}:{kurs.Zeit.Minutes.ToString().PadLeft(2, '0')}",
                DauerMinuten = kurs.DauerMinuten,
                Ende = $"{end.Hours.ToString().PadLeft(2, '0')}:{end.Minutes.ToString().PadLeft(2, '0')}",
            };

            if (kurs.Kotrainer1 != null) kursModel.Kotrainer1 = await _mitgliederRepository.GetAsync((int)kurs.Kotrainer1);
            if (kurs.Kotrainer2 != null) kursModel.Kotrainer2 = await _mitgliederRepository.GetAsync((int)kurs.Kotrainer2);

            return kursModel;
        }
    }
}
