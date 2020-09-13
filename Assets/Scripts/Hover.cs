using UnityEngine;

namespace AdrianMiasik
{
    public class Hover : MonoBehaviour
    {
        [SerializeField] private float amplitude = 0.075f;
        [SerializeField] private float heightOffset = 0;
        [SerializeField] private float speed = 1f;

        private float accumulatedTime = 0f;
        private Vector3 startingPosition = Vector3.zero;
        private Vector3 targetPosition = Vector3.one;
        private float offsetTime = 0f;
        private bool isInitialized = false;
        
        public void Initialize()
        {
            startingPosition = transform.localPosition;
            isInitialized = true;
        }

        private void Update()
        {
            if (!isInitialized)
            {
                return;
            }

            accumulatedTime += Time.deltaTime;
            
            targetPosition = startingPosition + Vector3.up * heightOffset;
            transform.localPosition = targetPosition + Vector3.up * (Mathf.Sin(accumulatedTime * speed + offsetTime) * amplitude);
        }

        public void SetHeightOffset(float _heightOffset)
        {
            heightOffset = _heightOffset;
        }

        public void OffsetHoverTime(float _offset)
        {
            offsetTime = _offset;
        }
    }
}