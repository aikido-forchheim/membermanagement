﻿using System;
using AVF.MemberManagement.StandardLibrary.Interfaces;
using Newtonsoft.Json;

namespace AVF.MemberManagement.StandardLibrary.Tbo
{
    public class ZuschlagKindertraining : IIntId
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

        public int Dauer
        {
            get;
            set;
        }

        public int Trainernummer
        {
            get;
            set;
        }

        public decimal Betrag
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
