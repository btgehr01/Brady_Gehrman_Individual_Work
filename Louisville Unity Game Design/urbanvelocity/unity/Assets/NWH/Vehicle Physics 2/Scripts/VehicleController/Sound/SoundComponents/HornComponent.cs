using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

namespace NWH.VehiclePhysics2.Sound.SoundComponents
{
    /// <summary>
    ///     Vehicle horn sound.
    /// </summary>
    [Serializable]
    public partial class HornComponent : SoundComponent
    {
        public override bool IsOneShot => true;

        public override bool InitLoop => true;

        private float _clipLength = 1f;

        public override void InitializeSoundComponent(AudioMixerGroup mixerGroup, GameObject sourceContainer)
        {
            base.InitializeSoundComponent(mixerGroup, sourceContainer);

            if (Clip != null)
            {
                _clipLength = Clip.length;
            }
        }

        public override void Update()
        {
            if (!Active)
            {
                return;
            }

            if (Clip != null)
            {
                if (vc.input.Horn)
                {
                    if (!IsPlaying) vc.StartCoroutine(PlayRepeating());
                }
                else
                {
                    if (IsPlaying) vc.StopCoroutine(PlayRepeating());
                }
            }
        }


        public override void FixedUpdate()
        {
        }


        public override void SetDefaults(VehicleController vc)
        {
            base.SetDefaults(vc);

            if (Clip == null)
            {
                AddDefaultClip("BlinkerOn");
                AddDefaultClip("BlinkerOff");
            }
        }

        public IEnumerator PlayRepeating()
        {
            while (vc.input.Horn)
            {
                PlayOneShot(RandomClip, baseVolume);
                yield return new WaitForSeconds(_clipLength);
            }
        }
    }
}