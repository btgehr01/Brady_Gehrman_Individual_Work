using System;
using UnityEngine;

namespace NWH.VehiclePhysics2.Sound.SoundComponents
{
    /// <summary>
    ///     Sound of turbocharger or supercharger.
    /// </summary>
    [Serializable]
    public partial class TurboWhistleComponent : SoundComponent
    {
        /// <summary>
        ///     Pitch range that will be added to the base pitch depending on turbos's RPM.
        /// </summary>
        [Range(0, 5)]
        [Tooltip("    Pitch range that will be added to the base pitch depending on turbos's RPM.")]
        public float pitchRange = 0.9f;

        public override bool InitLoop => true;


        public override void Update()
        {
            if (!Active)
            {
                return;
            }

            if (Clip != null && vc.powertrain.engine.IsRunning &&
                vc.powertrain.engine.forcedInduction.useForcedInduction)
            {
                SetVolume(Mathf.Clamp01(baseVolume 
                                        * vc.powertrain.engine.forcedInduction.boost * vc.powertrain.engine.forcedInduction.boost));
                SetPitch(pitchRange * vc.powertrain.engine.forcedInduction.boost);
                Play();
            }
            else
            {
                if (Source != null)
                {
                    SetVolume(0);
                    if (Source.isPlaying) Stop();
                }
            }
        }


        public override void FixedUpdate()
        {
        }


        public override void SetDefaults(VehicleController vc)
        {
            base.SetDefaults(vc);

            baseVolume = 0.015f;
            pitchRange = 0.8f;

            if (Clip == null)
            {
                AddDefaultClip("TurboWhistle");
            }
        }
    }
}