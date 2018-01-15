using System;
using System.Linq;
using System.Collections.Generic;
using AVF.MemberManagement.StandardLibrary.Interfaces;
using AVF.MemberManagement.StandardLibrary.Tbo;
using AVF.MemberManagement.Utilities;

namespace AVF.MemberManagement.Console
{
    internal class BusinessLogic
    {
        private DatabaseWrapper m_dbWrapper;

        public BusinessLogic( DatabaseWrapper db )
        {
            m_dbWrapper = db;
        }


     }
}
