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
        public List<FamilyCharacteristic> List_FamilyChars;

        public FamilyInfo(Document famDoc)
        {
            Family fam = famDoc.OwnerFamily;
            Name = fam.Name;
            BasicFamilySettings bfc = new BasicFamilySettings(fam);
            List_FamilyChars.Add(new FamilyCharacteristic("BasicFamilySettings", bfc));

            List<MyFamilyParameter> List_myFamParams = MyFamilyParameter.CollectFamilyParameters(famDoc);
            List_FamilyChars.Add(new FamilyCharacteristic("FamilyParameters", List_myFamParams));


            List<RefPlane> List_RefPlanes = RefPlane.CollectRefPlanes(famDoc);
            List_FamilyChars.Add(new FamilyCharacteristic("ReferencePlanes", List_RefPlanes));

            List<FamilyDimension> familyDimensions = FamilyDimension.CollectDimensions(famDoc);
            List_FamilyChars.Add(new FamilyCharacteristic("Dimensions", familyDimensions));

            List<Geometry.GeometryCurve> curves = Geometry.GeometryCurve.CollectCurves(famDoc);
            List_FamilyChars.Add(new FamilyCharacteristic("Dimensions", familyDimensions));

            List<Geometry.FamilyGeometryForm> forms = Geometry.FamilyGeometryForm.CollectForms(famDoc);
            List_FamilyChars.Add(new FamilyCharacteristic("Forms", forms));
        }
    }
}
