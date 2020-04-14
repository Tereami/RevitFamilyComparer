using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;

namespace RevitFamilyComparer
{
    public class FamilyDimension
    {
        public int Id;
        public string ViewName;
        public string DimTypeName;
        public bool IsEQ;
        public List<int> List_ReferenceElementIds;
        public int SegmentsCount;
        
        public FamilyDimension(Autodesk.Revit.DB.Dimension dim)
        {
            Id = dim.Id.IntegerValue;
            ViewName = dim.View.Name;
            DimTypeName = dim.Name;
            IsEQ = dim.AreSegmentsEqual;
            SegmentsCount = dim.Segments.Size;

            List_ReferenceElementIds = new List<int>();
            foreach(Reference rf in dim.References)
            {
                List_ReferenceElementIds.Add(rf.ElementId.IntegerValue);
            }
        }

        public static List<FamilyDimension> CollectDimensions(Document familyDocument)
        {
            List<FamilyDimension> dims = new FilteredElementCollector(familyDocument)
                .WhereElementIsNotElementType()
                .OfClass(typeof(Dimension))
                .Cast<Dimension>()
                .Select(i => new FamilyDimension(i))
                .ToList();

            return dims;
        }
    }
}
