using System;
using System.Collections.Generic;
using System.Text;

namespace ParentalControl.ValidationRules
{
    public interface IValidity
    {
        bool IsValid { get; set; }
    }
}
