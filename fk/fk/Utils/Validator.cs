using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace fk
{
    public class Validator
    {

        public static bool ValidateDigit(string s, int from, int to)
        {
            try
            {
                int c = int.Parse(s);
                if (c < from || c > to)
                    return false;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool ValidateEmail(string s)
        {
            try
            {
                new MailAddress(s);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
            catch (Exception)
            {
                return true;
            }
        }
    }
}
