using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace ParentalControl.Models
{
    public class CurrentDevice
    {
        [PrimaryKey]
        public int ID { get; set; }
        public bool IsParentDevice { get; set; }
        public string Name { get; set; }
    }
}
