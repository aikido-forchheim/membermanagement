using System;
using AVF.MemberManagement.StandardLibrary.Interfaces;
using Newtonsoft.Json;

namespace AVF.MemberManagement.StandardLibrary.Tbo
{
    public class Wohnung : IIntId
    {
        public const string PrimaryKey = "ID";

        [JsonProperty(PropertyName = PrimaryKey)]
        public int Id
        {
            get;
            set;
        }

        public string PLZ
        {
            get;
            set;
        }

        public string Ort
        {
            get;
            set;
        }

        public string Strasse
        {
            get;
            set;
        }

        public string Hausnummer
        {
            get;
            set;
        }

        public string Fahrtstrecke
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
