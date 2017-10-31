using System.Collections.Generic;
using AVF.MemberManagement.StandardLibrary.Interfaces;
using AVF.MemberManagement.StandardLibrary.Tbo;
using Newtonsoft.Json;

namespace AVF.MemberManagement.StandardLibrary.Tables
{
    public class TblZuschlaegeKindertraining : ITable<ZuschlagKindertraining>
    {
        public const string TableName = "tblZuschlagKindertraining";

        public string Uri { get; set; } = TableName;

        [JsonProperty(PropertyName = TableName)]
        public List<ZuschlagKindertraining> Rows { get; set; }
    }
}
