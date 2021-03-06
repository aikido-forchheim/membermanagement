﻿using System;
using AVF.MemberManagement.StandardLibrary.Enums;
using AVF.MemberManagement.StandardLibrary.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace AVF.MemberManagement.StandardLibrary.Tbo
{
    public class Beitragsklasse : IIntId
    {
        public const string PrimaryKey = "ID";

        [JsonIgnore]
        public string PrimaryKeyName { get; set; } = PrimaryKey;

        [JsonProperty(PropertyName = PrimaryKey)]
        public int Id { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        [JsonProperty(PropertyName = "Beitragsklasse")]
        public RomanNumeral BeitragsklasseRomanNumeral { get; set; }

        public decimal Beitrag { get; set; }

        public string Beschreibung { get; set; }

        public string Bemerkung { get; set; }

        public DateTime DatensatzAngelegtAm { get; set; }

        public string DatensatzAngelegtVon { get; set; }

        public DateTime DatensatzGeaendertAm { get; set; }

        public string DatensatzGeaendertVon { get; set; }
    }
}
