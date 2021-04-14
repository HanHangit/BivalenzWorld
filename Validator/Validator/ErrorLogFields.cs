using System;
using System.Collections.Generic;
using System.Text;

namespace Validator
{
    internal class ErrorLogFields
    {
        public const string VALIDATION_FREEVARIABLES = "This formula contains a free variable.";
        public const string VALIDATION_ARGUMENTUNKNOWN = "This formula contains an unknown symbol.";
        public const string VALIDATION_CONSTANTNOTINWORLD = "The constant symbol is not assigned in the world.";
    }
}
