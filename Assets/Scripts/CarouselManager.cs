using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace AdrianMiasik
{
    // TODO: Clean up
    public class CarouselManager : MonoBehaviour
    {
        [SerializeField] private DisplayCaseCarouselSelector _carouselSelector = null;
        private List<ShaderModel> allShaderModels = new List<ShaderModel>();

        [SerializeField] private float initializationStagger = 0.1f;
        private int staggerIndex;

        [SerializeField] private float animationStaggerDelay = 0.25f;
        private float waitTime;

        private bool isInitialized = false;
        [SerializeField] private bool isMovingOnX = false;
        private float longestXDelayTime = 0f;
        private float accumulatedTime = 0f;

        // TODO: Unsub properly
        
        private void Start()
        {
            SetupCarousels();
            isInitialized = true;
        }

        private void SetupCarousels()
        {
            _carouselSelector.Initialize();

            staggerIndex = 0;
            foreach (DisplayCaseCarousel _carousel in _carouselSelector.GetItems())
            {
                _carousel.Initialize(this);

                // Query for shader models in the carousel
                foreach (ShaderModel _shaderModel in FetchShaderModels(_carousel))
                {
                    // Initialize each shader model
                    _shaderModel.Initialize();

                    // Staggers all our cached shader models so they all hover at different times on init
                    _shaderModel.SetTimeOffset(initializationStagger * staggerIndex);
                    staggerIndex++;
                }
            }
        }
        
        private int GetIndexDistance(Collection<DisplayCaseCarousel> _collection, DisplayCaseCarousel _itemA, DisplayCaseCarousel _itemB)
        {
            if (_collection.Contains(_itemA) && _collection.Contains(_itemB))
            {
                return Mathf.Abs(_collection.IndexOf(_itemA) - _collection.IndexOf(_itemB));
            }
        
            Debug.LogAssertion("Unable to compare distances - DisplayCaseCarousel not found with our list");
            return 0;
        }

        /// <summary>
        /// Attempts to fetch and cache the ShaderModel objects found in the provided carousel. If no ShaderModel object is
        /// found within the provided carousel, we will not cache that specific ShaderModel.
        /// </summary>
        /// <param name="_carousel"></param>
        private IEnumerable<ShaderModel> FetchShaderModels(DisplayCaseCarousel _carousel)
        {
            foreach (DisplayCase _displayCase in _carousel.GetDisplayCases())
            {
                ShaderModel _shaderModel = _displayCase.GetModel().GetComponent<ShaderModel>();

                if (_shaderModel != null)
                {
                    allShaderModels.Add(_shaderModel);
                }
            }

            return allShaderModels;
        }

        [ContextMenu("Quit Carousels")]
        private void CleanUpCarousels()
        {
            foreach (DisplayCaseCarousel _carousel in _carouselSelector.GetItems())
            {
                _carousel.CleanUp();
            }

            allShaderModels.Clear();
            staggerIndex = 0;
        }

        [ContextMenu("Rebuild Carousels")]
        private void RebuildCarousels()
        {
            CleanUpCarousels();
            SetupCarousels();
        }

        public void OnSelected(DisplayCaseCarousel _selectedCarousel)
        {
            // Reset X movement (for interruptions)
            isMovingOnX = false;
            accumulatedTime = 0f;
            
            _carouselSelector.Select(_selectedCarousel);

            longestXDelayTime = 0f;

            // X-axis movement
            foreach (DisplayCaseCarousel _carousel in _carouselSelector.GetItems())
            {
                Vector3 desiredPosition = _carousel.transform.position;
                desiredPosition.x = _carousel.staggerDisplayOffset.x * (_selectedCarousel.GetSelectedIndex() * -1);

                float delayTime = animationStaggerDelay * GetIndexDistance(_carouselSelector.GetItems(),
                    _carousel, _selectedCarousel);

                if (longestXDelayTime <= delayTime + _carousel.GetMovementDuration())
                {
                    longestXDelayTime = delayTime + _carousel.GetMovementDuration();
                }
                
                _carousel.MoveTo(desiredPosition, delayTime);
            }

            isMovingOnX = true;
        }

        private void Update()
        {
            if (!isInitialized)
            {
                return;
            }

            if (isMovingOnX)
            {
                accumulatedTime += Time.deltaTime;

                if (accumulatedTime > longestXDelayTime)
                {
                    isMovingOnX = false;
                    accumulatedTime = 0f;
                    Debug.Log("TODO: Z-Axis Movement");
                }
            }                
        }
    }
}