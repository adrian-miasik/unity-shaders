﻿﻿using UnityEngine;

namespace AdrianMiasik
{
    /// <summary>
    /// A timer class
    /// </summary>
    public abstract class TimerProgress : MonoBehaviour
    {
        [SerializeField] private float duration = 3f;

        private bool isInit;
        private float progress;
        private float elapsedTime;

        /// <returns>Return true if you would like to start the timer.</returns>
        protected abstract bool Initialize();
        
        /// <param name="_normalizedPercentage">A value between 0 and 1</param>
        protected abstract void OnUpdate(float _normalizedPercentage);
        protected abstract void OnComplete();

        private void Start()
        {
            isInit = Initialize();
        }

        private void Update()
        {
            if (!isInit)
            {
                return;
            }
            
            elapsedTime += Time.deltaTime;
            OnUpdate(elapsedTime / duration);

            if (elapsedTime >= duration)
            {
                OnComplete();
                elapsedTime = 0;
            }
        }
    }
}