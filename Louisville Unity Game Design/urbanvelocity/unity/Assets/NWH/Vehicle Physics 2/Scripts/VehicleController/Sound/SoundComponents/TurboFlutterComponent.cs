using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace NWH.VehiclePhysics2.Sound.SoundComponents
{
    /// <summary>
    ///     Sound of a wastegate releasing air on turbocharged vehicles.
    /// </summary>
    [Serializable]
    public partial class TurboFlutterComponent : SoundComponent
    {
        public override bool IsOneShot => true;


        public override void Update()
        {
            if (!Active)
            {
                return;
            }

            if (!IsPlaying && Clip != null && vc.powertrain.engine.forcedInduction.useForcedInduction &&
                vc.powertrain.engine.forcedInduction.hasWastegate)
            {
                if (vc.powertrain.engine.forcedInduction.wastegateFlag)
                {
                    float newVolume = baseVolume * vc.powertrain.engine.forcedInduction.wastegateBoost * Random.Range(0.7f, 1.3f);
                    newVolume = newVolume < 0 ? 0 : newVolume > 1 ? 1 : newVolume;
                    PlayOneShot(RandomClip, newVolume);

                    vc.powertrain.engine.forcedInduction.wastegateFlag = false;
                }
            }
        }


        public override void FixedUpdate()
        {
        }


        public override void SetDefaults(VehicleController vc)
        {
            base.SetDefaults(vc);

            baseVolume           = 0.015f;

            if (Clip == null)
            {
                AddDefaultClip("TurboFlutter");
            }
        }
    }
}