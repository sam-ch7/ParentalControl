using System;
using System.Collections.Generic;
using System.Text;

namespace ParentalControl.Models
{
    public class ChildDevice
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Manufacturer { get; set; }
        public string Model { get; set; }
        public string VersionString { get; set; }
        public string Platform { get; set; }
        public bool IsAccessibilityServiceEnabled { get; set; }
        public bool IsLoggedIn { get; set; }
    }
}
