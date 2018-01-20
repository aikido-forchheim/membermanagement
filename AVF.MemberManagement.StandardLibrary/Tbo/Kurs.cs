using System;
using AVF.MemberManagement.StandardLibrary.Converters;
using AVF.MemberManagement.StandardLibrary.Interfaces;
using Newtonsoft.Json;

namespace AVF.MemberManagement.StandardLibrary.Tbo
{
    public class Kurs : IIntId
    {
        public const string PrimaryKey = "ID";
        public string PrimaryKeyName { get; set; } = PrimaryKey;

        [JsonProperty(PropertyName = PrimaryKey)]
        public int Id
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
