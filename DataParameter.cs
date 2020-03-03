using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess
{
    class DataParameter
    {
        public Tuple<string, object> parameter;
        public DataParameter(string key, object value)
        {
            this.parameter = new Tuple<string, object>(key, value);
        }
    }
}
