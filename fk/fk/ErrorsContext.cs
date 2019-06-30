using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fk
{
    public class ErrorsContext
    {
        public const string EMAIL_ERROR = "Неправильный адрес электронной почты";

        public string ErrorEmail { get; set; }
        public string ErrorDigit { get; set; }
        public string MsgSubmit { get; set; }
    }
}
