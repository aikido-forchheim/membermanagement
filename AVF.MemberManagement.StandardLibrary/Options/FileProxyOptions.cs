using System.Collections.Generic;
using AVF.MemberManagement.StandardLibrary.Models;

namespace AVF.MemberManagement.StandardLibrary.Options
{
    public class FileProxyOptions
    {
        public bool ShouldSimulateDelay { get; set; }

        /// <summary>
        /// <![CDATA[Dictionary<string tboName, FileProxyOptions>]]>
        /// </summary>
        public Dictionary<string, FileProxyDelayTimes> FileProxyDelayTimes { get; set; }

        public FileProxyDelayTimes FallBackTimes { get; set; }
    }
}
