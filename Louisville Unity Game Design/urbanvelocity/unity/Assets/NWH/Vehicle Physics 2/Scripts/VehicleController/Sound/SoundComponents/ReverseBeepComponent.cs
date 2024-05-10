using System;
using UnityEngine;
using UnityEngine.Audio;
using System.Collections.Generic;
using System.Collections;

namespace NWH.VehiclePhysics2.Sound.SoundComponents
{
    [Serializable]
    public partial class ReverseBeepComponent : SoundComponent
    {
        public override bool IsOneShot => true;

        public bool beepOnNegativeVelocity = true;
        public bool beepOnReverseGear      = true;

        public override void Enable()
        {
            base.Enable();

            vc.StartCoroutine(BeepCoroutine());
        }

        public override void Disable()
        {
            base.Disable();

            vc.StopCoroutine(BeepCoroutine());
        }

        public override bool InitLoop => true;

        public override void Update()
        {

        }

        public override void FixedUpdate()
        {
        }


        public override void SetDefaults(VehicleController vc)
        {
            base.SetDefaults(vc);

            if (Clip == null)
            {
                AddDefaultClip("ReverseBeep");
            }
        }

        IEnumerator BeepCoroutine()
        {
            while (true)
            {
                int gear = vc.powertrain.transmission.Gear;
                if (beepOnReverseGear && gear < 0 ||
                    beepOnNegativeVelocity && vc.LocalForwardVelocity < -0.2f && gear <= 0)
                {
                    PlayOneShot(Clip, baseVolume);
                }

                yield return new WaitForSeconds(1f);
            }
        }
    }
}