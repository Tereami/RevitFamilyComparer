using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitFamilyComparer.Geometry
{

    public abstract class GeometryElement
    {
        public int Id;
        public string TypeName;
        //public string HostId;

        public GeometryVisibility geomVisibility;
    }
}
