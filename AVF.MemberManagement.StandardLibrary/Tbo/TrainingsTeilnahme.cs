using System;
using AVF.MemberManagement.StandardLibrary.Converters;
using AVF.MemberManagement.StandardLibrary.Interfaces;
using Newtonsoft.Json;

namespace AVF.MemberManagement.StandardLibrary.Tbo
{
    public class TrainingsTeilnahme : IIntId
    {
        public const string PrimaryKey = "ID";
        public string PrimaryKeyName { get; set; } = PrimaryKey;

        [JsonProperty(PropertyName = PrimaryKey)]
        public int Id
        {
            get;
            set;
        }

        public int TrainingID
        {
            get;
            set;
        }

        public int MitgliedID
        {
            get;
            set;
        }

        public string Bemerkung
        {
            get;
            set;
        }

        [JsonConverter(typeof(DateTimeStringToNullableDateTimeConverter))]
        public DateTime? DatensatzAngelegtAm
        {
            get;
            set;
        }

        public int DatensatzAngelegtVon
        {
            get;
            set;
        }

        [JsonConverter(typeof(DateTimeStringToNullableDateTimeConverter))]
        public DateTime? DatensatzGeaendertAm
        {
            get;
            set;
        }

        public int DatensatzGeaendertVon
        {
            get;
            set;
        }
    }
}
