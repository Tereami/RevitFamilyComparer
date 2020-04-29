using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;

namespace RevitFamilyComparer
{
    /// <summary>
    /// Definition of Dimension element
    /// </summary>
    public class FamilyDimension
    {
        public int Id;
        public string ViewName;
        public string DimTypeName;
        public bool IsEQ;
        public List<int> List_ReferenceElementIds;
        public int SegmentsCount;
        

        public FamilyDimension()
        {

        }

        public FamilyDimension(Autodesk.Revit.DB.Dimension dim)
        {
            Id = dim.Id.IntegerValue;

            if (dim.View != null)
                ViewName = dim.View.Name;
            else
                ViewName = "null";

            DimTypeName = dim.DimensionType.Name; ;
            IsEQ = dim.AreSegmentsEqual;
            SegmentsCount = dim.Segments.Size;

            List_ReferenceElementIds = new List<int>();
            foreach(Reference rf in dim.References)
            {
                List_ReferenceElementIds.Add(rf.ElementId.IntegerValue);
            }
        }

        /// <summary>
        /// Get all Dimension in a Document
        /// </summary>
        /// <param name="familyDocument"></param>
        /// <returns></returns>
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
