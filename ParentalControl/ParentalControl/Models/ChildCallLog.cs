using System;
using System.Collections.Generic;
using System.Text;

namespace ParentalControl.Models
{
    public class ChildCallLog
    {
        public string ContactName { get; set; }
        public string CallNumber { get; set; }
        public string CallDate { get; set; }
        public string CallType { get; set; }
        public string Duration { get; set; }
        public override string ToString()
        {
            return CallNumber;
        }
    }
}
