using System;
using System.Collections.Generic;
using UnityEngine;

namespace AdrianMiasik{
    public class ShaderTestStartup : MonoBehaviour
    {
        [SerializeField] private DisplayCaseCarousel standardCarousel = null;
        [SerializeField] private DisplayCaseCarousel shaderGraphCarousel = null;

        [SerializeField] private List<ShaderModel> allShaderModels = new List<ShaderModel>();
        [SerializeField] private float initializationStagger = 0.1f;

        private int index = 0;
        private float accumulatedTime = 0;
        private bool isInitialized = false;
        private bool isStaggerComplete = false;
        
        private void Start()
        {
            SetupEnvironment();
        }

        [ContextMenu("Start Environment")]
        private void SetupEnvironment()
        {
            // Initialize our carousels
            standardCarousel.Initialize();
            shaderGraphCarousel.Initialize();

            // Grab our shader model references
            GetShaderModels(standardCarousel);
            GetShaderModels(shaderGraphCarousel);
            
            isInitialized = true;
        }

        private void Update()
        {
            if (!isInitialized || isStaggerComplete)
            {
                return;
            }
            
            accumulatedTime += Time.deltaTime;

            // TODO: Stagger all hover scripts on start
            while (accumulatedTime > initializationStagger)
            {
                allShaderModels[index].Initialize();
                index++;
                accumulatedTime -= initializationStagger;

                // If there is more objects to iterate through...
                if (index < allShaderModels.Count - 1)
                {
                    continue;
                }
                
                // Stagger complete
                accumulatedTime = 0;
                isStaggerComplete = true;
            }
        }

        /// <summary>
        /// Attempts to fetch and cache the ShaderModel objects found in the provided carousel. If no ShaderModel object is
        /// found within the provided carousel, we will not cache that specific ShaderModel.
        /// </summary>
        /// <param name="_carousel"></param>
        private void GetShaderModels(DisplayCaseCarousel _carousel)
        {
            foreach (DisplayCase _displayCase in _carousel.GetDisplayCases())
            {
                ShaderModel _shaderModel = _displayCase.GetModel().GetComponent<ShaderModel>();

                if (_shaderModel != null)
                {
                    allShaderModels.Add(_shaderModel);
                }
            }
        }
        
        [ContextMenu("Quit Environment")]
        private void CleanUpEnvironment()
        {
            standardCarousel.CleanUp();
            shaderGraphCarousel.CleanUp();
        }
    }
}