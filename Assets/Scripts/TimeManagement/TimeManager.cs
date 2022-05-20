using System.Collections;
using Cinemachine;
using UI;
using UnityEngine;
using UnityEngine.UI;
using Input = UnityEngine.Input;

namespace TimeManagement
{
    public class TimeManager : MonoBehaviour
    {
        [SerializeField] private float freezedTimeScale = 0.3f;
        [SerializeField] private Image shadow;
        [SerializeField] private AnimationCurve freezeAnimCurve;
        [SerializeField] private float animTime = 3f;
        [SerializeField] private float camStartDistance = 10f;
        [SerializeField] private float cameraMinDistance = 5f;

        private float _timeScale = 1f;
        private bool _paused = false;
        private CinemachineFramingTransposer _camTransposer;
        
        public float TimeScale => _timeScale;

        public static TimeManager Inst { get; private set; }

        public void FreezeTime(float delay) => StartCoroutine(FreezeCO(true, delay));

        private void Awake()
        {
            PauseCanvasInput.OnGamePaused.AddListener(OnGamePaused);
            Inst = this;
        }

        private void Start()
        {
            _camTransposer = FindObjectOfType<CinemachineVirtualCamera>().
                GetCinemachineComponent<CinemachineFramingTransposer>();
            _camTransposer.m_CameraDistance = camStartDistance;
        }

        private void Update()
        {
            if (!Input.GetKeyDown(KeyCode.C)) return;
            Debug.Log(Time.timeScale);
        }

        private void OnGamePaused(bool status)
        {
            Time.timeScale = status? 0f : _timeScale;
            _paused = status;
        }

        private IEnumerator FreezeCO(bool start, float delay)
        {
            var time = 0f;
            while (time < animTime)
            {
                if (_paused)
                {
                    yield return null;
                    continue;
                }
                var evaluate = !start ? (time / animTime) : (1f - time / animTime);
                var curveValue = freezeAnimCurve.Evaluate(evaluate);
                
                _timeScale = Mathf.Lerp(freezedTimeScale, 1f, curveValue);
                _camTransposer.m_CameraDistance = Mathf.Lerp(cameraMinDistance, camStartDistance, curveValue);
                shadow.color = new Color(shadow.color.r, shadow.color.g, shadow.color.b, 1 - curveValue);
                
                Time.timeScale = _timeScale;
                time += Time.unscaledDeltaTime;
                
                yield return null;
            }
            if (!start)
            {
                shadow.color = new Color(shadow.color.r, shadow.color.g, shadow.color.b, 0);
                _camTransposer.m_CameraDistance = camStartDistance;
                Time.timeScale = 1f;
                yield break;
            }

            time = 0f;
            
            while (time < delay)
            {
                yield return null;
                if (_paused) continue;

                time += Time.unscaledDeltaTime;
            }
            StartCoroutine(FreezeCO(false, delay));
        }
    }
}