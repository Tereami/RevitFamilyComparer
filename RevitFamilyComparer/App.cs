using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace RevitFamilyComparer
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class App : IExternalApplication
    {
        public static string assemblyPath = "";
        public static string assemblyFolder = "";
        public Result OnStartup(UIControlledApplication application)
        {
            assemblyPath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            assemblyFolder = System.IO.Path.GetDirectoryName(assemblyPath);

            string tabName = "Weandrevit";
            string solutionName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
            try { application.CreateRibbonTab(tabName); } catch { }
            RibbonPanel panel = application.CreateRibbonPanel(tabName, solutionName);
            PushButton btn = panel.AddItem(new PushButtonData(
                "GetFamilyXml",
                "GetFamilyXml",
                assemblyPath,
                solutionName + ".CommandCreateAreaRebar")
                ) as PushButton;

            return Result.Succeeded;
        }
        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }

    }
}
