using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;

namespace RevitFamilyComparer
{
    public class BasicFamilySettings
    {
        public List<MyParameter> List_OrderParameters;

        public BasicFamilySettings(Family fam)
        {
            foreach(Parameter p in fam.GetOrderedParameters())
            {
                MyParameter mp = new MyParameter(p, true);
                List_OrderParameters.Add(mp);
            }
        }
    }
}
