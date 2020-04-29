using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;

namespace RevitFamilyComparer
{
    /// <summary>
    /// Description af FamilyParameter from FamilyManager, includes units, formula, etc
    /// </summary>
    public class MyFamilyParameter
    {
        public string Name;
        public bool IsInstance;
        public bool HaveFormula;
        public string Formula;
        public bool IsReporting;
        public string Units;

        public List<MyParameter> List_AssociatedParameters;
        public MyParameterType ParamType;

        public MyFamilyParameter()
        {

        }

        public MyFamilyParameter(FamilyParameter param)
        {
            List_AssociatedParameters = new List<MyParameter>();
            InternalDefinition def = param.Definition as InternalDefinition;
            Name = def.Name;

            if (param.IsShared)
            {
                ParamType = MyParameterType.Shared;
            }
            else
            {
                if (def.BuiltInParameter == BuiltInParameter.INVALID)
                {
                    ParamType = MyParameterType.Family;
                }
                else
                {
                    ParamType = MyParameterType.Builtin;
                }
            }

            IsInstance = param.IsInstance;
            HaveFormula = param.IsDeterminedByFormula;
            Formula = param.Formula;
            IsReporting = param.IsReporting;
            foreach(Parameter asparam in param.AssociatedParameters)
            {
                MyParameter myasparam = new MyParameter(asparam, false);
                List_AssociatedParameters.Add(myasparam);
            }

            Units = Enum.GetName(typeof(StorageType), param.StorageType);
        }

        public static List<MyFamilyParameter> CollectFamilyParameters(Document famDoc)
        {
            List<MyFamilyParameter> myFamParams = new List<MyFamilyParameter>();
            foreach (FamilyParameter fp in famDoc.FamilyManager.Parameters)
            {
                MyFamilyParameter mfp = new MyFamilyParameter(fp);
                myFamParams.Add(mfp);
            }
            return myFamParams;
        }
    }
}
