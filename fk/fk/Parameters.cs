using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace fk
{
    public class Parameters
    {
        private Dictionary<string, string> parameters;

        public void SetParam(string key, string value)
        {
            parameters.Add(key, value);
        }

        public string ToGET()
        {
            string res = "?";
            foreach (string key in parameters.Keys)
                res += key + "=" + parameters[key] + "&";
            return res.Substring(0, res.Length - 1);
        }
    }
}
