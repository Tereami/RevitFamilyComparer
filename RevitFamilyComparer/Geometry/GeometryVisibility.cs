using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;

namespace RevitFamilyComparer.Geometry
{
    /// <summary>
    /// Description of Visibility Settings of Element if family
    /// </summary>
    public class GeometryVisibility
    {
        public bool VisibleInLowLeveL;
        public bool VisibleInMiddleLevel;
        public bool VisibleInHighLevel;
        public bool VisibleFrontBack;
        public bool VisibleLeftRight;
        public bool VisibleTopBottom;
        public bool VisibleOnPlans;
        public bool VisibleOnlyWhenCut;
        public string VisibilityType;


        public GeometryVisibility()
        {

        }

        public GeometryVisibility(FamilyElementVisibility fev)
        {
            VisibleInLowLeveL = fev.IsShownInCoarse;
            VisibleInMiddleLevel = fev.IsShownInMedium;
            VisibleInHighLevel = fev.IsShownInFine;
            VisibleFrontBack = fev.IsShownInFrontBack;
            VisibleLeftRight = fev.IsShownInLeftRight;
            VisibleTopBottom = fev.IsShownInTopBottom;
            VisibleOnPlans = fev.IsShownInPlanRCPCut;
            VisibleOnlyWhenCut = fev.IsShownOnlyWhenCut;

            VisibilityType = Enum.GetName(typeof(FamilyElementVisibilityType), fev.VisibilityType);
        }
    }
}
