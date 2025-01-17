#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace NWH.VehiclePhysics2.Sound.SoundComponents
{
    [CustomPropertyDrawer(typeof(BrakeHissComponent))]
    public partial class BrakeHissComponentDrawer : SoundComponentDrawer
    {
        public override bool OnNUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (!base.OnNUI(position, property, label))
            {
                return false;
            }

            drawer.Field("baseVolume");
            drawer.Field("minInterval", true, "s");
            DrawClipsSection(new List<string> {"Hiss",}, 1, 10);

            drawer.EndProperty();
            return true;
        }
    }
}

#endif
