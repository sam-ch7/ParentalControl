using System;
using System.Collections.Generic;
using System.Text;

namespace ParentalControl.Models
{
    public static class SelectedChildDevice
    {
        public static string Name { get; set; }
        public static string Manufacturer { get; set; }
        public static string Model { get; set; }
        public static string VersionString { get; set; }
        public static string Platform { get; set; }
        public static string ID { get; set; }
        public static bool IsAccessibilityServiceEnabled { get; set; }
        public static bool IsLoggedIn { get; set; }
    }
}
