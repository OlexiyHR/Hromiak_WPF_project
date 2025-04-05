using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hromiak_WPF_project.Exceptions
{
    public class FutureBirthDateException : Exception
    {
        public FutureBirthDateException()
            : base("Дата народження не може бути у майбутньому.")
        {
        }

        public FutureBirthDateException(string message)
            : base(message)
        {
        }
    }

    public class ExcessivelyOldBirthDateException : Exception
    {
        public ExcessivelyOldBirthDateException()
            : base("Дата народження занадто давня.")
        {
        }

        public ExcessivelyOldBirthDateException(string message)
            : base(message)
        {
        }
    }

    public class InvalidEmailException : Exception
    {
        public InvalidEmailException()
            : base("Невірна адреса електронної пошти.")
        {
        }

        public InvalidEmailException(string message)
            : base(message)
        {
        }
    }
}
