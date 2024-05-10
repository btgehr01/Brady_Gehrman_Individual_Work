using System;
using NWH.VehiclePhysics2.Powertrain;
using UnityEngine;
using Random = UnityEngine.Random;

namespace NWH.VehiclePhysics2.Sound.SoundComponents
{
    /// <summary>
    ///     Shifter sound played when changing gears.
    ///     Supports multiple audio clips of which one is chosen at random each time this effect is played.
    /// </summary>
    [Serializable]
    public partial class GearChangeComponent : SoundComponent
    {
        /// <summary>
        /// True if the component sound is played as one shot.
        /// </summary>
        public override bool IsOneShot => true;


        public override void Enable()
        {
            base.Enable();

            vc.powertrain.transmission.onShift.AddListener(PlayShiftSound);
        }


        public override void Disable()
        {
            base.Disable();

            vc.powertrain.transmission.onShift.RemoveListener(PlayShiftSound);
        }


        private void PlayShiftSound(GearShift gearShift)
        {
            if (!IsEnabled || !vc.soundManager.IsEnabled)
            {
                return;
            }
            
            if (gearShift.ToGear != 0)
            {
                PlayOneShot(RandomClip, baseVolume);
            }


        }


        public override void Update()
        {

        }


        public override void FixedUpdate()
        {
        }


        public override void SetDefaults(VehicleController vc)
        {
            base.SetDefaults(vc);

            baseVolume = 0.16f;

            if (Clip == null)
            {
                AddDefaultClip("GearChange");              
            }
        }
    }
}