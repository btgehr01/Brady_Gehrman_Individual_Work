#if UNITY_EDITOR
using NWH.NUI;
using UnityEditor;
using UnityEngine;

namespace NWH.VehiclePhysics2
{
    public partial class PowertrainComponentDrawer : NVP_NUIPropertyDrawer
    {
        public void DrawCommonProperties()
        {
            drawer.BeginSubsection("Common Properties");
            drawer.Field("name");
            drawer.Field("inertia");

            drawer.Label("Output To:");
            drawer.IncreaseIndent();
            DrawPowertrainOutputSection(ref drawer.positionRect, drawer.serializedProperty);
            drawer.DecreaseIndent();
            drawer.EndSubsection();
        }


        public virtual void DrawPowertrainOutputSection(ref Rect rect, SerializedProperty property)
        {
            drawer.Field("outputASelector");
        }


        public virtual int GetOutputCount()
        {
            return 1;
        }
    }
}

#endif
