#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace NWH.VehiclePhysics2.Sound.SoundComponents
{
    [CustomPropertyDrawer(typeof(TurboFlutterComponent))]
    public partial class TurboFlutterComponentDrawer : SoundComponentDrawer
    {
        public override bool OnNUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (!base.OnNUI(position, property, label))
            {
                return false;
            }

            drawer.Field("baseVolume");
            DrawClipsSection(new List<string> {"Flutter",}, 1, 10);

            drawer.EndProperty();
            return true;
        }
    }
}

#endif
