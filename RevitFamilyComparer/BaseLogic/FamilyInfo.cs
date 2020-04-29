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
        /// Eliminate Ids shift (in case that family is opened from the project or other family)
        /// Dummy and stupid idea, but this is the best what I invent
        /// forums.autodesk.com/t5/revit-api-forum/id-of-elements-is-changes-when-a-family-is-opened-from-a-project/
        /// </summary>
        /// <param name="idOffset"></param>
        public void ApplyIdOffset(FamilyInfo baseFi)
        {
            //old id value, id value after renumber
            Dictionary<int, int> idsRenumbInfo = new Dictionary<int, int>();

            int dimsMinCount = Math.Min(this.List_FamilyDimensions.Count, baseFi.List_FamilyDimensions.Count);
            for(int i = 0; i < dimsMinCount; i++)
            {
                idsRenumbInfo.Add(this.List_FamilyDimensions[i].Id, baseFi.List_FamilyDimensions[i].Id);
                this.List_FamilyDimensions[i].Id = baseFi.List_FamilyDimensions[i].Id;
            }

            int refPlanesMinCount = Math.Min(this.List_RefPlanes.Count, baseFi.List_RefPlanes.Count);
            for (int i = 0; i < refPlanesMinCount; i++)
            {
                idsRenumbInfo.Add(this.List_RefPlanes[i].Id, baseFi.List_RefPlanes[i].Id);
                this.List_RefPlanes[i].Id =
                    baseFi.List_RefPlanes[i].Id;
            }


            int curvesMinCount = Math.Min(this.List_Curves.Count, baseFi.List_Curves.Count);
            for (int i = 0; i < curvesMinCount; i++)
            {
                idsRenumbInfo.Add(this.List_Curves[i].Id, baseFi.List_Curves[i].Id);
                this.List_Curves[i].Id = baseFi.List_Curves[i].Id;
            }



            int formsMinCount = Math.Min(this.List_Forms.Count, baseFi.List_Forms.Count);
            for(int i = 0; i< formsMinCount; i++)
            {
                idsRenumbInfo.Add(this.List_Forms[i].Id, baseFi.List_Forms[i].Id);
                this.List_Forms[i].Id = baseFi.List_Forms[i].Id;


                int sketchesMinCount = Math.Min(
                    this.List_Forms[i].List_Profiles.Count, baseFi.List_Forms[i].List_Profiles.Count);
                for(int j = 0; j < sketchesMinCount; j++)
                {
                    idsRenumbInfo.Add(
                        this.List_Forms[i].List_Profiles[j].Id, baseFi.List_Forms[i].List_Profiles[j].Id);
                    this.List_Forms[i].List_Profiles[j].Id = baseFi.List_Forms[i].List_Profiles[j].Id;
                }
            }

            //next time, I need to renumber Ids in internal references
            foreach(FamilyDimension fdim in this.List_FamilyDimensions)
            {
                fdim.List_ReferenceElementIds = 
                    ApplyRenumbering(fdim.List_ReferenceElementIds, idsRenumbInfo);
            }

            foreach(Geometry.GeometryCurve curve in this.List_Curves)
            {
                curve.List_AdjoinedElementsEnd0 =
                    ApplyRenumbering(curve.List_AdjoinedElementsEnd0, idsRenumbInfo);
                curve.List_AdjoinedElementsEnd1 =
                    ApplyRenumbering(curve.List_AdjoinedElementsEnd1, idsRenumbInfo);
            }

            foreach(Geometry.FamilyGeometryForm form in this.List_Forms)
            {
                foreach(Geometry.GeometrySketch sketch in form.List_Profiles)
                {
                    if(idsRenumbInfo.ContainsKey(sketch.Id))
                    {
                        sketch.Id = idsRenumbInfo[sketch.Id];
                    }
                    if (idsRenumbInfo.ContainsKey(sketch.ReferencePlaneId))
                    {
                        sketch.ReferencePlaneId = idsRenumbInfo[sketch.ReferencePlaneId];
                    }
                    sketch.List_CurveIds = ApplyRenumbering(sketch.List_CurveIds, idsRenumbInfo);
                }
            }
        }



        
        public void ApplyIdOffsetVersion1(FamilyInfo baseFi)
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
            foreach (Geometry.GeometryCurve curve in this.List_Curves)
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
            foreach (Geometry.FamilyGeometryForm form in List_Forms)
            {
                form.Id -= formsOffset;
                foreach (Geometry.GeometrySketch profile in form.List_Profiles)
                {
                    if (profile.ReferencePlaneId != 0)
                    {
                        profile.ReferencePlaneId -= refPlanesOffset;
                    }
                    profile.List_CurveIds =
                        profile.List_CurveIds.Select(i => i - curvesOffset).ToList();
                }
            }
        }


        /// <summary>
        /// Replace ids to values that I found after shifting
        /// </summary>
        /// <param name="oldIds"></param>
        /// <param name="RenumbInfo"></param>
        /// <returns></returns>
        private List<int> ApplyRenumbering(List<int> oldIds, Dictionary<int,int> RenumbInfo)
        {
            List<int> newIds = new List<int>();
            if (oldIds == null) return newIds;
            foreach(int id in oldIds)
            {
                if(RenumbInfo.ContainsKey(id))
                {
                    int newId = RenumbInfo[id];
                    newIds.Add(newId);
                }
                else
                {
                    newIds.Add(id);
                }
            }
            return newIds;
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


        /// <summary>
        /// Unlucky atteampt to eliminate shift of Ids throught loading family to blank project
        /// </summary>
        /// <param name="famFilePath"></param>
        /// <param name="app"></param>
        /// <returns></returns>
        //public static FamilyInfo GetFamInfoByBlankProjectDocument(
        //    string famFilePath, Autodesk.Revit.ApplicationServices.Application app)
        //{
        //    Document blankDoc = app.NewProjectDocument(UnitSystem.Metric);
        //    FamilyInfo fi = null;
        //    using (Transaction t = new Transaction(blankDoc))
        //    {
        //        t.Start("load first family");
        //        Family fam = null;
        //        blankDoc.LoadFamily(famFilePath, out fam);
        //        t.Commit();
        //        Document famdoc1 = blankDoc.EditFamily(fam);
        //        fi = new FamilyInfo(famdoc1);
        //        famdoc1.Close(false);
        //    }
        //    string folder = System.IO.Path.GetDirectoryName(famFilePath);
        //    string projectPath = System.IO.Path.Combine(folder, "proj " + fi.Name + DateTime.Now.ToString("HHmmss") + ".rvt");
        //    blankDoc.SaveAs(projectPath);
        //    blankDoc.Close(false);
        //    return fi;
        //}
    }
}
