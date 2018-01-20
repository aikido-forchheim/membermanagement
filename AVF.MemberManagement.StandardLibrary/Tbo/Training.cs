using System;
using AVF.MemberManagement.StandardLibrary.Converters;
using AVF.MemberManagement.StandardLibrary.Interfaces;
using AVF.MemberManagement.StandardLibrary.Services;
using Newtonsoft.Json;

namespace AVF.MemberManagement.StandardLibrary.Tbo
{
    public class Training : IIntId
    {
        public const string PrimaryKey = "ID";

        [JsonIgnore]
        public string PrimaryKeyName { get; set; } = PrimaryKey;

        [JsonProperty(PropertyName = PrimaryKey)]
        public int Id
        {
            get;
            set;
        }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? KursID
        {
            get;
            set;
        }

        [JsonConverter(typeof(DateFormatConverter), "yyyy-MM-dd")]
        public DateTime Termin
        {
            get;
            set;
        }

        public int WochentagID
        {
            get;
            set;
        }

        public TimeSpan Zeit
        {
            get;
            set;
        }

        public int DauerMinuten
        {
            get;
            set;
        }

        [JsonConverter(typeof(LongToBooleanConverter))]
        public bool Kindertraining
        {
            get;
            set;
        }

        public bool VHS
        {
            get;
            set;
        }

        public int Trainer
        {
            get;
            set;
        }

        public int? Kotrainer1
        {
            get;
            set;
        }

        public int? Kotrainer2
        {
            get;
            set;
        }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Bemerkung
        {
            get;
            set;
        }

        public DateTime DatensatzAngelegtAm
        {
            get;
            set;
        }

        public int DatensatzAngelegtVon
        {
            get;
            set;
        }

        public DateTime DatensatzGeaendertAm
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
