using System;
using NWH.VehiclePhysics2.Effects;
using UnityEngine;
using UnityEngine.Audio;

namespace NWH.VehiclePhysics2.Sound.SoundComponents
{
    /// <summary>
    ///     Click-clack of the working blinker.
    ///     Accepts two clips, first is for the blinker turning on and the second is for blinker turning off.
    /// </summary>
    [Serializable]
    public partial class BlinkerComponent : SoundComponent
    {
        public override bool IsOneShot => true;

        public override void InitializeSoundComponent(AudioMixerGroup mixerGroup, GameObject sourceContainer)
        {
            base.InitializeSoundComponent(mixerGroup, sourceContainer);

            if (vc.VehicleMultiplayerInstanceType == Vehicle.MultiplayerInstanceType.Local)
            {
                if (vc.effectsManager.lightsManager.leftBlinkers.lightSources.Count > 0)
                {
                    LightSource ls = vc.effectsManager.lightsManager.leftBlinkers.lightSources[0];
                    ls.onLightTurnedOn.AddListener(PlayBlinkerOn);
                    ls.onLightTurnedOff.AddListener(PlayBlinkerOff);
                }

                if (vc.effectsManager.lightsManager.rightBlinkers.lightSources.Count > 0)
                {
                    LightSource ls = vc.effectsManager.lightsManager.rightBlinkers.lightSources[0];
                    ls.onLightTurnedOn.AddListener(PlayBlinkerOn);
                    ls.onLightTurnedOff.AddListener(PlayBlinkerOff);
                }
            }
        }

        private void PlayBlinkerOn()
        {
            PlayOneShot(Clips[1], baseVolume);
        }

        private void PlayBlinkerOff()
        {
            PlayOneShot(Clips[0], baseVolume);
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

            baseVolume = 0.8f;

            if (Clip == null)
            {
                AddDefaultClip("BlinkerOn");
                AddDefaultClip("BlinkerOff");
            }
        }
    }
}