using System;
using System.Collections.Generic;
using NWH.VehiclePhysics2.Powertrain;
using NWH.WheelController3D;
using UnityEngine;
using UnityEngine.Audio;
using Random = UnityEngine.Random;

namespace NWH.VehiclePhysics2.Sound.SoundComponents
{
    /// <summary>
    ///     Sound of wheel hitting the surface or obstracle.
    /// </summary>
    [Serializable]
    public partial class SuspensionBumpComponent : SoundComponent
    {
        public override bool IsOneShot => true;

        private List<bool> _prevHasHits = new List<bool>();
        private int _wheelCount;


        public override void InitializeSoundComponent(AudioMixerGroup mixerGroup, GameObject sourceContainer)
        {
            _wheelCount = vc.Wheels.Count;
            for (int index = 0; index < _wheelCount; index++)
            {
                _prevHasHits.Add(true);
            }

            base.InitializeSoundComponent(mixerGroup, sourceContainer);
        }
       

        public override void Update()
        {
            if (!Active)
            {
                return;
            }

            if (Clip != null)
            {
                int wheelCount = vc.Wheels.Count;
                for (int i = 0; i < wheelCount; i++)
                {
                    WheelController wc = vc.Wheels[i].wheelController;

                    float forwardAngle = wc.wheelHit.angleForward;

                    bool playOnGrounded = wc.isGrounded && _prevHasHits[i] == false;
                    bool playOnHit = (wc.forwardFriction.speed > 0.8f || wc.forwardFriction.speed < -0.8f) &&
                        (forwardAngle > 6f || forwardAngle < -6f);

                    if (playOnGrounded || playOnHit)
                    {
                        float newPitch = Random.Range(0.8f, 1.2f);
                        float absDamperForce = wc.damperForce < 0 ? -wc.damperForce : wc.damperForce;
                        float newVolume = baseVolume *
                                            Mathf.Clamp01(absDamperForce * 3f / Mathf.Max(wc.damper.bumpForce,
                                                            wc.damper.reboundForce));

                        PlayOneShot(RandomClip, newVolume);
                    }

                    _prevHasHits[i] = wc.isGrounded;
                }
            }
        }


        public override void FixedUpdate()
        {
        }


        public override void SetDefaults(VehicleController vc)
        {
            base.SetDefaults(vc);

            baseVolume = 0.12f;

            if (Clip == null)
            {
                AddDefaultClip("SuspensionBump");
            }
        }
    }
}