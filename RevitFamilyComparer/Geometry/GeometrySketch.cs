﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RevitFamilyComparer.Geometry
{
    public class GeometrySketch
    {
        public int Id;
        public int ReferencePlaneId;

        public List<int> List_CurveIds;


        public GeometrySketch()
        {

        }

        public GeometrySketch(int id, int refPlaneId, List<int> curveIds)
        {
            Id = id;
            ReferencePlaneId = refPlaneId;
            List_CurveIds = curveIds;
        }

        public GeometrySketch(Autodesk.Revit.DB.Sketch sk)
        {
            Id = sk.Id.IntegerValue;
            if (sk.SketchPlane == null)
                ReferencePlaneId = -1;
            else
                ReferencePlaneId = sk.SketchPlane.GetPlaneReference().ElementId.IntegerValue;

            GetCurves(sk.Profile);
        }

        public GeometrySketch(Autodesk.Revit.DB.CurveArrArray caa)
        {
            Id = -1;
            ReferencePlaneId = -1;
            GetCurves(caa);
        }

        public GeometrySketch(Autodesk.Revit.DB.Path3d p)
        {
            Id = p.Id.IntegerValue;
            ReferencePlaneId = -2;
            GetCurves(p.AllCurveLoops);
        }

        private void GetCurves(Autodesk.Revit.DB.CurveArrArray caa)
        {
            List_CurveIds = new List<int>();
            foreach (Autodesk.Revit.DB.CurveArray ar in caa)
            {
                foreach (Autodesk.Revit.DB.Curve cur in ar)
                {
                    List_CurveIds.Add(cur.Reference.ElementId.IntegerValue);
                }
            }
        }
    }
}
