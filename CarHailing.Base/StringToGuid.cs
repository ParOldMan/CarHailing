using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CarHailing.Base
{
    public class StringToGuid
    {
        /*
         * string TO guid 
         */
        private static bool ToGuid(string str)
        {
            Guid gv = new Guid();
            try
            {
                gv = new Guid(str);
            }
            catch (Exception)
            {

            }
            if (gv != Guid.Empty)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
