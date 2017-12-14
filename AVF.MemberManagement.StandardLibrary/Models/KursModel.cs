using System;
using System.Collections.Generic;
using System.Text;
using AVF.MemberManagement.StandardLibrary.Tbo;

namespace AVF.MemberManagement.StandardLibrary.Models
{
    public class Class : Kurs
    {
        public Wochentag Wochentag { get; set; }

        public new Mitglied Trainer { get; set; }

        public new Mitglied Kotrainer1 { get; set; }

        public new Mitglied Kotrainer2 { get; set; }

        public new string Zeit { get; set; }

        public string Ende { get; set; }

        public string Time
        {
            get { return $"{Zeit}-{Ende}"; }
            set { }
        }
    }
}
