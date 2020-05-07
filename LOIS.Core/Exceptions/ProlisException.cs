using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOIS.Core.Exceptions
{
    public class ProlisException : Exception
    {
        public ProlisException()
        {

        }

        public ProlisException(string message) : base(message)
        {

        }

        public ProlisException(string message, Exception inner) : base(message, inner)
        {

        }
    }
}
