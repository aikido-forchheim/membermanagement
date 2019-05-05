using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AVF.MemberManagement.StandardLibrary.Interfaces;
using AVF.MemberManagement.StandardLibrary.Proxies;
using AVF.MemberManagement.StandardLibrary.Tbo;

namespace AVF.StandardLibrary.Extensions
{
    public static class TrainingsRepositoryExtensions
    {
        public static async Task<List<Training>> GetAsync(this IRepository<Training> trainingsRepository, int courseId,
            DateTime dateTime)
        {
            var filterTrainingByCourseId = new Filter
            {
                ColumnName = nameof(Training.KursID),
                MatchType = "eq",
                Value = courseId.ToString()
            };

            var filterTrainingByDate = new Filter
            {
                ColumnName = nameof(Training.Termin),
                MatchType = "eq",
                Value = dateTime.ToString("s")
            };

            var trainings = await trainingsRepository.GetAsync(new List<Filter> { filterTrainingByCourseId, filterTrainingByDate });
            return trainings;
        }
    }
}
