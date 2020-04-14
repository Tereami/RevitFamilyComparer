using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;

namespace RevitFamilyComparer.Geometry
{
    public class GeometryCurve : GeometryElement
    {
        public string LineFormType;
        public string LineType;
        public bool IsDetailLine;
        public string SubcategoryName;
        public bool HasTangentLockEnd0;
        public bool HasTangentLockEnd1;
        public List<int> List_AdjoinedElementsEnd0;
        public List<int> List_AdjoinedElementsEnd1;

        public GeometryCurve(Autodesk.Revit.DB.CurveElement curve)
        {
            this.Id = curve.Id.IntegerValue;
            List_AdjoinedElementsEnd0 = curve.GetAdjoinedCurveElements(0).Select(i => i.IntegerValue).ToList();
            List_AdjoinedElementsEnd0 = curve.GetAdjoinedCurveElements(0).Select(i => i.IntegerValue).ToList();

            this.LineType = Enum.GetName(typeof(CurveElementType), curve.CurveElementType);

            if (curve.CurveElementType == CurveElementType.ModelCurve)
            {
                ModelCurve mcurve = curve as ModelCurve;
                this.LineFormType = mcurve.GeometryCurve.ToString();
                this.IsDetailLine = false;
                this.geomVisibility = new GeometryVisibility(mcurve.GetVisibility());
                this.HasTangentLockEnd0 = mcurve.HasTangentLocks(0);
                this.HasTangentLockEnd1 = mcurve.HasTangentLocks(1);
                //this.HostId = mcurve.LevelId.IntegerValue;
                this.SubcategoryName = mcurve.Subcategory.Name;
                this.TypeName = mcurve.LineStyle.Name;
            }
            else if(curve.CurveElementType == CurveElementType.SymbolicCurve)
            {
                SymbolicCurve mcurve = curve as SymbolicCurve;
                this.LineFormType = mcurve.GeometryCurve.ToString();
                this.IsDetailLine = false;
                this.geomVisibility = new GeometryVisibility(mcurve.GetVisibility());
                this.HasTangentLockEnd0 = mcurve.HasTangentLocks(0);
                this.HasTangentLockEnd1 = mcurve.HasTangentLocks(1);
                //this.HostId = mcurve.LevelId.IntegerValue;
                this.SubcategoryName = mcurve.Subcategory.Name;
                this.TypeName = mcurve.LineStyle.Name;
            }
        }

        public static List<GeometryCurve> CollectCurves(Document famDoc)
        {
            List<CurveElement> curveElems = new FilteredElementCollector(famDoc)
                .OfClass(typeof(CurveElement))
                .Cast<CurveElement>()
                .ToList();

            List<GeometryCurve> curves = new List<GeometryCurve>();

            foreach(CurveElement ce in curveElems)
            {
                if((ce.CurveElementType == CurveElementType.ModelCurve) 
                    || (ce.CurveElementType == CurveElementType.SymbolicCurve))
                {
                    curves.Add(new GeometryCurve(ce));
                }
            }
            return curves;
        }
    }
}
