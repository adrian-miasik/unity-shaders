﻿﻿using UnityEngine;

namespace AdrianMiasik
{
    public abstract class TimerProgress : MonoBehaviour
    {
        [SerializeField] private float duration = 3f;

        private bool isInit;
        private float progress;
        private float elapsedTime;

        protected abstract bool Initialize();
        protected abstract void OnUpdate(float _progress);
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