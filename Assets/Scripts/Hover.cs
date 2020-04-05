using UnityEngine;

namespace AdrianMiasik
{
    public class Hover : MonoBehaviour
    {
        [SerializeField] private float amplitude;
        [SerializeField] private float heightOffset = 0;
        [SerializeField] private float speed = 1f;

        private float accumulatedTime;
        private Vector3 startingPosition;
        private Vector3 targetPosition;
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
            
            targetPosition = startingPosition + (Vector3.up * heightOffset);
            transform.localPosition = targetPosition + (Vector3.up * (Mathf.Sin(accumulatedTime * speed) * amplitude));
        }

        public void SetHeightOffset(float _heightOffset)
        {
            heightOffset = _heightOffset;
        }
    }
}