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
       

        public RefPlane()
        {

        }

        public RefPlane(Autodesk.Revit.DB.ReferencePlane rp)
        {
            Id = rp.Id.IntegerValue;
            RefPlaneName = rp.Name;
        }

        public RefPlane(Autodesk.Revit.DB.Level lv)
        {
            Id = lv.Id.IntegerValue;
            RefPlaneName = lv.Name;
        }

        public RefPlane(Autodesk.Revit.DB.SketchPlane sp)
        {
            Id = sp.Id.IntegerValue;
            RefPlaneName = sp.Name;
        }

        public static List<RefPlane> CollectRefPlanes(Document famDoc)
        {
            List<RefPlane> rps = new FilteredElementCollector(famDoc)
                .OfClass(typeof(ReferencePlane))
                .Cast<ReferencePlane>()
                .Select(i => new RefPlane(i))
                .ToList();

            List<RefPlane> levels = new FilteredElementCollector(famDoc)
                .OfClass(typeof(Level))
                .Cast<Level>()
                .Select(i => new RefPlane(i))
                .ToList();
            rps.AddRange(levels);

            List<RefPlane> sketchPlanes = new FilteredElementCollector(famDoc)
                .OfClass(typeof(SketchPlane))
                .Cast<SketchPlane>()
                .Select(i => new RefPlane(i))
                .ToList();
            rps.AddRange(sketchPlanes);


            return rps;
        }
    }
}
