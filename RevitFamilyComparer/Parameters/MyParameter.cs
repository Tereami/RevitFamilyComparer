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
using Autodesk.Revit.DB;

namespace RevitFamilyComparer
{
    public enum MyParameterType { Builtin, Family, Shared }
    public class MyParameter
    {
        public string Name;
        public string Value;
        public string Formula;
        public string Units;
        public MyParameterType ParamType;

        public MyParameter()
        {

        }
        public MyParameter(string name, string value)
        {
            Name = name;
            Value = value;
        }

        public MyParameter(Parameter param, bool UseValue)
        {
            InternalDefinition def = param.Definition as InternalDefinition;

            Name = def.Name;

            if (param.IsShared)
            {
                ParamType = MyParameterType.Shared;
            }
            else
            {
                if(def.BuiltInParameter == BuiltInParameter.INVALID)
                {
                    ParamType = MyParameterType.Family;
                }
                else
                {
                    ParamType = MyParameterType.Builtin;
                }
            }

            Units = Enum.GetName(typeof(Autodesk.Revit.DB.UnitType), param.Definition.UnitType);

            if (!UseValue || !param.HasValue)
            {
                Value = "NO VALUE";
            }
            else if (def.ParameterType == ParameterType.YesNo)
            {
                int intval = param.AsInteger();
                Value = "false";
                if (intval == 1) Value = "true";
            }
            else
            {
                switch (param.StorageType)
                {
                    case StorageType.None:
                        Value = "NONE STORAGE TYPE";
                        break;
                    case StorageType.Integer:
                        Value = param.AsInteger().ToString();
                        break;
                    case StorageType.Double:
                        Value = param.AsDouble().ToString("G");
                        break;
                    case StorageType.String:
                        Value = param.AsString();
                        break;
                    case StorageType.ElementId:
                        Value = param.AsValueString();
                        break;
                    default:
                        break;
                }
            }

            
        }
    }
}
