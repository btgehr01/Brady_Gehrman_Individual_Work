using System;
using UnityEngine;

namespace NWH.VehiclePhysics2.Sound.SoundComponents
{
    /// <summary>
    ///     EngineFanComponent is used to imitate engine fan running, the sound especially prominent in commercial vehicles and
    ///     off-road vehicles with clutch driven fan.
    ///     Can also be used to mimic induction noise.
    /// </summary>
    [Serializable]
    public partial class EngineFanComponent : SoundComponent
    {
        /// <summary>
        /// Starting sound pitch at idle RPM.
        /// </summary>
        [UnityEngine.Tooltip("Starting sound pitch at idle RPM.")]
        public float basePitch = 1f;

        /// <summary>
        /// Pitch range, redline pitch equals basePitch + pitchRange.
        /// </summary>
        [Range(0, 4)]
        [UnityEngine.Tooltip("Pitch range, redline pitch equals basePitch + pitchRange.")]
        public float pitchRange = 0.5f;

        public override bool InitLoop => true;

        public override void Update()
        {
            if (!Active)
            {
                return;
            }

            if (vc.powertrain.engine.IsRunning)
            {
                if (!Source.isPlaying)
                {
                    Play();
                }

                float rpmPercent = vc.powertrain.engine.RPMPercent;
                SetVolume(rpmPercent * rpmPercent * baseVolume);
                SetPitch(basePitch + pitchRange * rpmPercent);
            }
            else
            {
                if (Source.isPlaying)
                {
                    Stop();
                }
            }
        }


        public override void FixedUpdate()
        {
        }


        public override void SetDefaults(VehicleController vc)
        {
            base.SetDefaults(vc);

            baseVolume = 0.05f;

            if (Clip == null)
            {
                AddDefaultClip("EngineFan");              
            }
        }
    }
}