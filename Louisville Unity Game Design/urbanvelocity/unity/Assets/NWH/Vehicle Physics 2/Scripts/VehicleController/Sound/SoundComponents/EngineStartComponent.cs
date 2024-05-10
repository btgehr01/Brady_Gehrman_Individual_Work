using System;
using UnityEngine;
using UnityEngine.Audio;

namespace NWH.VehiclePhysics2.Sound.SoundComponents
{
    /// <summary>
    ///     Sound of an engine starting / stopping.
    ///     Plays while start is active.
    ///     Clip at index 0 should be an engine starting sound, clip at 1 should be an engine stopping sound (optional).
    /// </summary>
    [Serializable]
    public partial class EngineStartComponent : SoundComponent
    {
        public override bool IsOneShot => true;

        private bool _isPlayingStarting = false;

        public override void InitializeSoundComponent(AudioMixerGroup mixerGroup, GameObject sourceContainer)
        {
            base.InitializeSoundComponent(mixerGroup, sourceContainer);

            vc.powertrain.engine.OnStart.AddListener(() => { _isPlayingStarting = true; });
            vc.powertrain.engine.OnStop.AddListener(PlayStopping);
        }

        public override void Update()
        {
            if (!Active)
            {
                return;
            }

            if (_isPlayingStarting)
            {
                PlayStarting();
            }
        }

        public virtual void PlayStarting()
        {
            // Starting and stopping engine sound
            if (!IsPlaying && Clips.Count > 0)
            {
                if (vc.powertrain.engine.StarterActive)
                {
                    PlayOneShot(RandomClip, baseVolume);
                }
                else
                {
                    _isPlayingStarting = false;
                }
            }
        }

        public virtual void PlayStopping()
        {
            if (Clips.Count > 1)
            {
                PlayOneShot(RandomClip, baseVolume);
            }
        }


        public override void FixedUpdate()
        {
        }


        public override void SetDefaults(VehicleController vc)
        {
            base.SetDefaults(vc);
            baseVolume = 0.2f;

            if (Clip == null)
            {
                AddDefaultClip("EngineStart");               
            }
        }
    }
}