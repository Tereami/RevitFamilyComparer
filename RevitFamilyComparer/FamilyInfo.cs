#region License
/*Данный код опубликован под лицензией Creative Commons Attribution-ShareAlike.
Разрешено использовать, распространять, изменять и брать данный код за основу для производных в коммерческих и
некоммерческих целях, при условии указания авторства и если производные лицензируются на тех же условиях.
Код поставляется "как есть". Автор не несет ответственности за возможные последствия использования.
Зуев Александр, 2020, все права защищены.
This code is listed under the Creative Commons Attribution-ShareAlike license.
You may use, redistribute, remix, tweak, and build upon this work non-commercially and commercially,
as long as you credit the author by linking back and license your new creations under the same terms.
This code is provided 'as is'. Author disclaims any implied warranty.
Zuev Aleksandr, 2020, all rigths reserved.*/
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Autodesk.Revit.DB;

namespace RevitFamilyComparer
{
    /// <summary>
    /// Store information of elements within Family
    /// </summary>
    [Serializable]
    public class FamilyInfo
    {
        public string Name;
        BasicFamilySettings BasicSettings;
        public List<MyFamilyParameter> List_myFamParams;
        //public List<FamilyCharacteristic> List_FamilyChars;
        public List<RefPlane> List_RefPlanes;
        public List<FamilyDimension> List_FamilyDimensions;
        public List<Geometry.GeometryCurve> List_Curves;
        public List<Geometry.FamilyGeometryForm> List_Forms;
        public List<FamilyInfo> List_NestedFamilies;

        public FamilyInfo()
        {

        }
        public FamilyInfo(Document famDoc)
        {
            Family fam = famDoc.OwnerFamily;

            if (famDoc.Title.EndsWith(".rfa"))
                Name = famDoc.Title.Substring(0, famDoc.Title.Length - 4);
            else
                Name = famDoc.Title;

            BasicSettings = new BasicFamilySettings(fam);

            List_myFamParams = MyFamilyParameter.CollectFamilyParameters(famDoc);

            //List_FamilyChars = new List<FamilyCharacteristic>();

            List_RefPlanes = RefPlane.CollectRefPlanes(famDoc);
            //List_FamilyChars.Add(new FamilyCharacteristic("ReferencePlanes", List_RefPlanes));

            List_FamilyDimensions = FamilyDimension.CollectDimensions(famDoc);
            //List_FamilyChars.Add(new FamilyCharacteristic("Dimensions", familyDimensions));

            List_Curves = Geometry.GeometryCurve.CollectCurves(famDoc);
            //List_FamilyChars.Add(new FamilyCharacteristic("Dimensions", familyDimensions));

            List_Forms = Geometry.FamilyGeometryForm.CollectForms(famDoc);
            //List_FamilyChars.Add(new FamilyCharacteristic("Forms", forms));

            List_NestedFamilies = GetNestedFamilies(famDoc, false);
            //List_FamilyChars.Add(new FamilyCharacteristic("Forms", forms));
        }


        /// <summary>
        /// Get nested families are nested in Document
        /// </summary>
        /// <param name="famDoc"></param>
        /// <param name="GetNonSharedFamilies"></param>
        /// <returns></returns>
        private List<FamilyInfo> GetNestedFamilies(Document famDoc, bool GetNonSharedFamilies)
        {
            List<FamilyInfo> fams = new List<FamilyInfo>();

            List<Family> fis = new FilteredElementCollector(famDoc)
                .WhereElementIsNotElementType()
                .OfClass(typeof(Family))
                .Cast<Family>()
                .ToList();
            foreach(Family fam in fis)
            {
                if (!fam.IsEditable) continue;
                int IsShared = fam.get_Parameter(BuiltInParameter.FAMILY_SHARED).AsInteger();
                if (!GetNonSharedFamilies && IsShared == 0) continue;

                Document nestedFamDoc = famDoc.EditFamily(fam);
                FamilyInfo nestedFi = new FamilyInfo(nestedFamDoc);
                fams.Add(nestedFi);
                nestedFamDoc.Close(false);
            }
            return fams;
        }


        


        /// <summary>
        /// Eliminate shift of Ids in case that family is opened from the project or other family
        /// </summary>
        /// <param name="idOffset"></param>
        public void ApplyIdOffset(FamilyInfo baseFi)
        {
            int dimsOffset = this.List_FamilyDimensions.First().Id - baseFi.List_FamilyDimensions.First().Id;
            foreach (FamilyDimension dim in this.List_FamilyDimensions)
            {
                dim.Id -= dimsOffset;
                dim.List_ReferenceElementIds =
                    dim.List_ReferenceElementIds.Select(i => i - dimsOffset).ToList();
            }

            int refPlanesOffset = this.List_RefPlanes.First().Id - baseFi.List_RefPlanes.First().Id;
            foreach (RefPlane rp in this.List_RefPlanes)
            {
                rp.Id -= refPlanesOffset;
            }

            int curvesOffset = this.List_Curves.First().Id - baseFi.List_Curves.First().Id;
            foreach(Geometry.GeometryCurve curve in this.List_Curves)
            {
                curve.Id -= curvesOffset;
                if (curve.List_AdjoinedElementsEnd0 != null)
                {
                    curve.List_AdjoinedElementsEnd0 =
                        curve.List_AdjoinedElementsEnd0.Select(i => i - curvesOffset).ToList();
                }
                if (curve.List_AdjoinedElementsEnd1 != null)
                {
                    curve.List_AdjoinedElementsEnd1 =
                        curve.List_AdjoinedElementsEnd1.Select(i => i - curvesOffset).ToList();
                }
            }

            int formsOffset = this.List_Forms.First().Id - baseFi.List_Forms.First().Id;
            foreach(Geometry.FamilyGeometryForm form in List_Forms)
            {
                form.Id -= formsOffset;
                foreach(Geometry.GeometrySketch profile in form.List_Profiles)
                {
                    if(profile.ReferencePlaneId != 0)
                    {
                        profile.ReferencePlaneId -= refPlanesOffset;
                    }
                    profile.CurveIds =
                        profile.CurveIds.Select(i => i - curvesOffset).ToList();
                }
            }
        }


        /// <summary>
        /// Convert class to XML string
        /// </summary>
        /// <returns>Text string includes xml</returns>
        public string SerializeToXml()
        {
            System.Xml.Serialization.XmlSerializer serializer =
                new System.Xml.Serialization.XmlSerializer(typeof(FamilyInfo));

            StringBuilder sb = new StringBuilder();
            using (System.IO.StringWriter sw = new System.IO.StringWriter(sb))
            {
                serializer.Serialize(sw, this);
            }
            string xml = sb.ToString();

            return xml;
        }
    }
}
