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
        public string Units;
        public MyParameterType ParamType;

        public MyParameter(string name, string value)
        {
            Name = name;
            Value = value;
        }

        public MyParameter(Parameter param)
        {

            Name = param.Definition.Name;

            if (param.IsShared)
            {
                ParamType = MyParameterType.Shared;
            }
            else
            {
                InternalDefinition def = param.Definition as InternalDefinition;
                if(def.BuiltInParameter == BuiltInParameter.INVALID)
                {
                    ParamType = MyParameterType.Family;
                }
                else
                {
                    ParamType = MyParameterType.Builtin;
                }
            }


            if (!param.HasValue)
            {
                Value = "NO VALUE";
            }
            else if (param.Definition.ParameterType == ParameterType.YesNo)
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

            Units = Enum.GetName(typeof(Autodesk.Revit.DB.UnitType), param.Definition.UnitType);
        }
    }
}
