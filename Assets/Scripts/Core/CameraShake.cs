using Cinemachine;
using UnityEngine;

namespace Core
{
    public class CameraShake : MonoBehaviour 
    {
        [SerializeField] private float amplitude = 1f;
        private CinemachineVirtualCamera _vcam;
        private CinemachineBasicMultiChannelPerlin _noise;
        private float _timeAtCurrentFrame;
        private float _timeAtLastFrame;

        public static CameraShake Inst;

        void Awake()
        {
            Inst = this;
            _vcam = GetComponent<CinemachineVirtualCamera>();
            _noise = _vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            _noise.m_AmplitudeGain = 0f;
        }
 
        public void Shake(float duration)
        {
            CancelInvoke();
            _noise.m_AmplitudeGain = amplitude;
            Invoke(nameof(StopShake), duration);
        } 
        
        public void Shake()
        {
            CancelInvoke();
            _noise.m_AmplitudeGain = amplitude;
        }

        private void StopShake() => _noise.m_AmplitudeGain = 0f;
    }
}