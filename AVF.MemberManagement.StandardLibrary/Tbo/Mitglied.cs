using System;
using AVF.MemberManagement.StandardLibrary.Converters;
using AVF.MemberManagement.StandardLibrary.Interfaces;
using Newtonsoft.Json;

namespace AVF.MemberManagement.StandardLibrary.Tbo
{
    public class Mitglied : IIntId
    {
        public const string PrimaryKey = "Mitgliedsnummer";

        [JsonIgnore]
        public string PrimaryKeyName { get; set; } = PrimaryKey;

        [JsonProperty(PropertyName = PrimaryKey)]
        public int Id
        {
            get;
            set;
        }

        public string Nachname
        {
            get;
            set;
        }

        public string Vorname
        {
            get;
            set;
        }

        public string Rufname
        {
            get;
            set;
        }

        [JsonIgnore]
        public string Name
        {
            get
            {
                var vorname = Vorname;
                if (!string.IsNullOrEmpty(Rufname))
                {
                    vorname = Rufname;
                }

                return $"{vorname} {Nachname}";
            }
        }

        [JsonIgnore]
        public string FirstName
        {
            get
            {
                var vorname = Vorname;
                if (!string.IsNullOrEmpty(Rufname))
                {
                    vorname = Rufname;
                }

                return $"{vorname}";
            }
        }

        public DateTime Geburtsdatum
        {
            get;
            set;
        }

        public string Geburtsort
        {
            get;
            set;
        }

        public int BeitragsklasseID
        {
            get;
            set;
        }

        public DateTime? Eintritt
        {
            get;
            set;
        }

        [JsonConverter(typeof(DateTimeStringToNullableDateTimeConverter))]
        public DateTime? Austritt
        {
            get;
            set;
        }

        public int Familienmitglied
        {
            get;
            set;
        }

        public int Faktor
        {
            get;
            set;
        }

        public string KontoinhaberNachname
        {
            get;
            set;
        }

        public string KontoinhaberVorname
        {
            get;
            set;
        }

        public string Email1
        {
            get;
            set;
        }

        public string Email2
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

        public int AikidoBeginn
        {
            get;
            set;
        }

        public string Festnetz
        {
            get;
            set;
        }

        public string Mobil
        {
            get;
            set;
        }

        public string Rudname
        {
            get;
            set;
        }

        public override string ToString()
        {
            return $"[Mitglied: Id={Id}, Nachname={Nachname}, Vorname={Vorname}]";
        }
    }
}
