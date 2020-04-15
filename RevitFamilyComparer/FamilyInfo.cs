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
    [Serializable]
    public class FamilyInfo
    {
        public string Name;
        BasicFamilySettings BasicSettings;
        public List<MyFamilyParameter> List_myFamParams;
        public List<FamilyCharacteristic> List_FamilyChars;
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

            List_FamilyChars = new List<FamilyCharacteristic>();

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
    }
}
