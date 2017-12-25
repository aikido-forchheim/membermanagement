using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AVF.MemberManagement.StandardLibrary.Interfaces;
using AVF.MemberManagement.StandardLibrary.Tbo;

namespace AVF.MemberManagement.Console
{
    class Pruefungsliste
    {
        private DatabaseWrapper m_dbWrapper;

        public async Task Main()
        {
            m_dbWrapper = new DatabaseWrapper();

            await m_dbWrapper.ReadTables();

            foreach ( Mitglied mitglied in m_dbWrapper.Mitglieder() )
            {
 //               List<Pruefung> pruefungen = new List<Pruefung>;

                foreach ( Pruefung pruefung in m_dbWrapper.Pruefungen() )
                {

                }
            }
        }
    }
}
