using System.Collections.Generic;
using UnityEngine;

namespace AdrianMiasik{
    public class ShaderTestStartup : MonoBehaviour
    {
        [SerializeField] private DisplayCaseCarousel standardCarousel = null;
        [SerializeField] private DisplayCaseCarousel shaderGraphCarousel = null;

        [SerializeField] private List<ShaderModel> allShaderModels = new List<ShaderModel>();
        [SerializeField] private float initializationStagger = 0.1f;

        private int index;

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

            StaggerShaderModels();
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

        /// <summary>
        /// Staggers all our cached shader models so they all hover at different times on init
        /// </summary>
        private void StaggerShaderModels()
        {
            foreach (ShaderModel _shaderModel in allShaderModels)
            {
                _shaderModel.Initialize();
                _shaderModel.SetTimeOffset(initializationStagger * index);
                index++;
            }
        }

        [ContextMenu("Quit Environment")]
        private void CleanUpEnvironment()
        {
            standardCarousel.CleanUp();
            shaderGraphCarousel.CleanUp();
            
            allShaderModels.Clear();
            index = 0;
        }

        [ContextMenu("Rebuild Environment")]
        private void RebuildEnvironment()
        {
            CleanUpEnvironment();
            SetupEnvironment();
        }
    }
}