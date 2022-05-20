using System;
using System.Collections;
using UI;
using UnityEngine;
using Input = UnityEngine.Input;

namespace TimeManagement
{
    public class TimeManager : MonoBehaviour
    {
        [SerializeField] private float freezedTimeScale = 0.3f;
        
        [SerializeField] private AnimationCurve freezeAnimCurve;
        [SerializeField] private float animTime = 3f;
        
        private float _timeScale = 1f;
        private bool _paused = false;
        
        public float TimeScale => _timeScale;

        public static TimeManager Inst { get; private set; }

        public void FreezeTime(float delay) => StartCoroutine(FreezeCO(true, delay));

        private void Awake()
        {
            PauseCanvasInput.OnGamePaused.AddListener(OnGamePaused);
            Inst = this;
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
                _timeScale = Mathf.Clamp(freezeAnimCurve.Evaluate(evaluate), freezedTimeScale, 1f);
                
                Time.timeScale = _timeScale;
                time += Time.unscaledDeltaTime;
                
                yield return null;
            }
            if (!start)
            {
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