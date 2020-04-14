using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;

namespace RevitFamilyComparer
{
    
    public class RefPlane
    {
        public int Id;
        public string RefPlaneName;
       

        public RefPlane(Autodesk.Revit.DB.ReferencePlane rp)
        {
            Id = rp.Id.IntegerValue;
            RefPlaneName = rp.Name;
        }

        public static List<RefPlane> CollectRefPlanes(Document famDoc)
        {
            List<RefPlane> rps = new FilteredElementCollector(famDoc)
                .OfClass(typeof(ReferencePlane))
                .Cast<ReferencePlane>()
                .Select(i => new RefPlane(i))
                .ToList();
            return rps;
        }
    }
}
