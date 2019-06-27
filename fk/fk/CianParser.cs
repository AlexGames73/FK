using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace fk
{
    class CianParser : IParser
    {
        private Parameters parameters;

        public void GetURL()
        {
            string url = "https://cian.ru/cat.php" + parameters.ToGET();
        }
    }
}
