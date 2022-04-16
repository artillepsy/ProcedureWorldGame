using System.Collections.Generic;
using UnityEngine;

namespace Audios
{
    public class RepeatableAudio : MonoBehaviour
    {
        [SerializeField] private List<AudioClip> audios;
        [SerializeField] private float playRate = 0.4f;
        [Range(0,1)]
        [SerializeField] private float volume = 1f;
        [SerializeField] private bool playWhenWalk = true;
        private float _timeSincePlay;
        private AudioSource _audioSrc;
        private Vector3 _prevPos = Vector3.zero;

        private void Awake()
        {
            _audioSrc = GetComponent<AudioSource>();
            _prevPos = transform.position;
        }

        private void Update()
        {
            if (!ShouldPlay()) return;
            if (playWhenWalk && _prevPos == transform.position)
            {
                _prevPos = transform.position;
                return;
            }

            _prevPos = transform.position;
            _audioSrc.PlayOneShot(audios[Random.Range(0, audios.Count)], volume);
            _timeSincePlay = 0;
        }

        private bool ShouldPlay()
        {
            if (_timeSincePlay >= playRate) return true;
            _timeSincePlay += Time.deltaTime;
            return false;
        }
    }
}