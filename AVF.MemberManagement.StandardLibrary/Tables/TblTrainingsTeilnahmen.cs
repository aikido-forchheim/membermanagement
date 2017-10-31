﻿using System.Collections.Generic;
using AVF.MemberManagement.StandardLibrary.Interfaces;
using AVF.MemberManagement.StandardLibrary.Tbo;
using Newtonsoft.Json;

namespace AVF.MemberManagement.StandardLibrary.Tables
{
    public class TblTrainingsTeilnahmen : ITable<TrainingsTeilnahme>
    {
        public const string TableName = "tbTrainingsTeilnahme";

        public string Uri { get; set; } = TableName;

        [JsonProperty(PropertyName = TableName)]
        public List<TrainingsTeilnahme> Rows { get; set; }
    }
}
