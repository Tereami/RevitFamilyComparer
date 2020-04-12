using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitFamilyComparer.Geometry
{
    public enum FormTypeEnum { Extrusion, Blend, Revolution, Sweep, SweepBlend }
    public class FamilyGeometryForm : GeometryElement
    {
        public FormTypeEnum FormType;
        public bool IsVoidGeometry;
        public string SubcategoryName;


    }
}
