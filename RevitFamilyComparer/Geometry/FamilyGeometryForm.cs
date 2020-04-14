using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;

namespace RevitFamilyComparer.Geometry
{
    public enum FormTypeEnum { Extrusion, Blend, Revolution, Sweep, SweptBlend, Freeform }
    public class FamilyGeometryForm : GeometryElement
    {
        public FormTypeEnum FormType;
        public bool IsSolidGeometry;
        public string SubcategoryName;
        public List<GeometrySketch> Profiles;

        public FamilyGeometryForm (Autodesk.Revit.DB.GenericForm form)
        {
            Id = form.Id.IntegerValue;
            this.geomVisibility = new GeometryVisibility(form.GetVisibility());

            //Autodesk.Revit.DB.Parameter sketchPlaneParam = form.get_Parameter(Autodesk.Revit.DB.BuiltInParameter.SKETCH_PLANE_PARAM);
            //if(sketchPlaneParam != null)
            //{
            //    this.
            //}

            IsSolidGeometry = form.IsSolid;
            if (form.Subcategory == null)
                SubcategoryName = "";
            else
                SubcategoryName = form.Subcategory.Name;

            

            if(form is Extrusion)
            {
                FormType = FormTypeEnum.Extrusion;
                Extrusion ex = form as Extrusion;
                Sketch extrusionProfile = ex.Sketch;
                GeometrySketch gs = new GeometrySketch(extrusionProfile);
                Profiles = new List<GeometrySketch> { gs };
            }
            else if(form is Blend)
            {
                FormType = FormTypeEnum.Blend;
                Blend bl = form as Blend;
                GeometrySketch TopProfile = new GeometrySketch(bl.TopSketch);
                GeometrySketch BottomProfile = new GeometrySketch(bl.BottomSketch);
                Profiles = new List<GeometrySketch> { TopProfile, BottomProfile };
            }
            else if(form is Revolution)
            {
                FormType = FormTypeEnum.Revolution;
                Revolution rev = form as Revolution;
                Sketch revProfile = rev.Sketch;
                GeometrySketch gs = new GeometrySketch(revProfile);

                GeometrySketch axis = new GeometrySketch(rev.Axis.Id.IntegerValue, -1, null);
                Profiles = new List<GeometrySketch> { gs, axis };
            }
            else if(form is Sweep)
            {
                FormType = FormTypeEnum.Sweep;
                Sweep sw = form as Sweep;
                Profiles = new List<GeometrySketch>();
                if (sw.Path3d != null)
                {
                    GeometrySketch gs = new GeometrySketch(sw.Path3d);
                    Profiles.Add(gs);
                }
                else if(sw.PathSketch != null)
                {
                    GeometrySketch gs = new GeometrySketch(sw.PathSketch);
                    Profiles.Add(gs);
                }

                if(sw.ProfileSymbol != null)
                {
                    GeometrySketch gs = new GeometrySketch(sw.ProfileSymbol.Profile.Id.IntegerValue, -1, null);
                }
                else if(sw.ProfileSketch != null)
                {
                    GeometrySketch gs = new GeometrySketch(sw.ProfileSketch);
                    Profiles.Add(gs);
                }
            }
            else if(form is SweptBlend)
            {
                FormType = FormTypeEnum.SweptBlend;
                SweptBlend sb = form as SweptBlend;
                Profiles = new List<GeometrySketch>();
                if (sb.SelectedPath != null)
                {
                    Curve path = sb.SelectedPath;
                    GeometrySketch gs = new GeometrySketch(path.Reference.ElementId.IntegerValue, -1, null);
                    Profiles.Add(gs);
                }
                else if (sb.PathSketch != null)
                {
                    GeometrySketch gs = new GeometrySketch(sb.PathSketch);
                    Profiles.Add(gs);
                }

                GeometrySketch gs1 = new GeometrySketch(sb.TopProfile);
                GeometrySketch gs2 = new GeometrySketch(sb.BottomProfile);
                Profiles.Add(gs1);
                Profiles.Add(gs2);

            }

        }


        public static List<FamilyGeometryForm> CollectForms(Document famDoc)
        {
            List<FamilyGeometryForm> forms = new List<FamilyGeometryForm>();

            List<GenericForm> gforms = new FilteredElementCollector(famDoc)
                .OfClass(typeof(GenericForm))
                .Cast<GenericForm>()
                .ToList();

            foreach(GenericForm gf in gforms)
            {
                if (!gf.IsValidObject) continue;

                FamilyGeometryForm fgf = new FamilyGeometryForm(gf);
                forms.Add(fgf);
            }
            return forms;
        }
    }
}
