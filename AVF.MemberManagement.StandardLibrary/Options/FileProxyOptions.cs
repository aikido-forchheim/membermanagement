using System.Collections.Generic;
using AVF.MemberManagement.StandardLibrary.Interfaces;
using AVF.MemberManagement.StandardLibrary.Models;

namespace AVF.MemberManagement.StandardLibrary.Options
{
    public class FileProxyOptions
    {
        public bool ShouldSimulateDelay { get; set; }

        /// <summary>
        /// <![CDATA[Dictionary<string tboName, FileProxyOptions>]]>
        /// </summary>
        public Dictionary<string, IFileProxyDelayTimes> FileProxyDelayTimes { get; set; } = new Dictionary<string, IFileProxyDelayTimes>();

        public IFileProxyDelayTimes FallBackTimes { get; set; } = new FileProxyDelayTimes { GetAsyncFullTableDelay = 5000, GetAsyncSingleEntryDelay = 1000 };
    }
}
