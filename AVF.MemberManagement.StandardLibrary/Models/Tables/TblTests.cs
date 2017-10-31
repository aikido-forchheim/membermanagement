using System.Collections.Generic;
using AVF.MemberManagement.StandardLibrary.Interfaces;
using AVF.MemberManagement.StandardLibrary.Models.Tbo;
using Newtonsoft.Json;

namespace AVF.MemberManagement.StandardLibrary.Models.Tables
{
    public class TblTests : ITable<Test>
    {
        public const string TableName = "tblTest";

        [JsonProperty(PropertyName = TableName)]
        public List<Test> Rows { get; set; }
    }
}
