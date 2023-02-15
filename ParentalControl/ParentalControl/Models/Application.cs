using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace ParentalControl.Models
{
    [Preserve(AllMembers = true)]
    public class Application
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string Name { get; set; }
        public string PackageName { get; set; }
        public string Logo { get; set; }
        public bool IsBlocked { get; set; }
        public string Action
        {
            get
            {
                if (IsBlocked)
                    return "UnBlock";
                return "Block";
            }
            
        }
        public override string ToString()
        {
            return Name;
        }
    }
}
