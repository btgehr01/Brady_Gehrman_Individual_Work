#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace NWH.VehiclePhysics2.Sound.SoundComponents
{
    [CustomPropertyDrawer(typeof(BlinkerComponent))]
    public partial class BlinkerComponentComponentDrawer : SoundComponentDrawer
    {
        public override bool OnNUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (!base.OnNUI(position, property, label))
            {
                return false;
            }


            drawer.Field("baseVolume");
            DrawClipsSection(new List<string> {"Blinker On", "Blinker Off",}, 2, 2, 2);

            drawer.EndProperty();
            return true;
        }
    }
}

#endif
