using System.Collections.Generic;
using AVF.MemberManagement.StandardLibrary.Interfaces;
using AVF.MemberManagement.StandardLibrary.Tbo;
using Newtonsoft.Json;

namespace AVF.MemberManagement.StandardLibrary.Tables
{
    public class TblTrainerErnennungen : ITable<TrainerErnennung>
    {
        public const string TableName = "tbTrainerErnennung";

        public string Uri { get; set; } = TableName;

        [JsonProperty(PropertyName = TableName)]
        public List<TrainerErnennung> Rows { get; set; }
    }
}
