using System;
using System.Collections.Generic;
using System.Text;

namespace ParentalControl.Repository
{
    public interface ISpecialDeviceInfo
    {
        string GetDeviceID();
        bool IsAccessibilityServiceEnabled();
    }
}
