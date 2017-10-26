using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnvifIPClib.utils
{
  public  class CommonUtils
    {
        public static int str2int(string str,int defaultInt)
        {
            try
            {
                return Int32.Parse(str);
            }catch(Exception ex)
            {
                return defaultInt;
            }
        }
    }
}
