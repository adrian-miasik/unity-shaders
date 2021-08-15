using System.Collections.Generic;
using UnityEngine;

namespace AdrianMiasik
{
    public class CarouselManager : MonoBehaviour
    {
        [SerializeField] private List<DisplayCaseCarousel> carousels = null;
        private List<ShaderModel> allShaderModels = new List<ShaderModel>();

        [SerializeField] private float initializationStagger = 0.1f;
        private int staggerIndex;

        [SerializeField] private float animationStaggerDelay = 0.25f;
        private float waitTime;

        private void Start()
        {
            SetupCarousels();
        }

        private void SetupCarousels()
        {


            foreach (DisplayCaseCarousel _carousel in carousels)
            {
                _carousel.Initialize();
                _carousel.onSelection += OnCarouselSelection;

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

        private void OnCarouselSelection(DisplayCaseCarousel _selectedCarousel)
        {
            foreach (DisplayCaseCarousel _carousel in carousels)
            {
                // Move carousels to new index
                _carousel.MoveTo(new Vector3(
                        _carousel.staggerDisplayOffset.x * (_selectedCarousel.GetSelectedIndex() * -1),
                        _carousel.transform.position.y,
                        _carousel.transform.position.z),
                    animationStaggerDelay * CompareDistance(_carousel, _selectedCarousel));

                _carousel.Select(_selectedCarousel.GetSelectedIndex());
            }
        }

        private int CompareDistance(DisplayCaseCarousel _carouselA, DisplayCaseCarousel _carouselB)
        {
            if (carousels.Contains(_carouselA) && carousels.Contains(_carouselB))
            {
                return Mathf.Abs(carousels.IndexOf(_carouselA) - carousels.IndexOf(_carouselB));
            }

            Debug.LogAssertion("Unable to compare distances - Carousels not found with our list");
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
            foreach (DisplayCaseCarousel _carousel in carousels)
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
    }
}