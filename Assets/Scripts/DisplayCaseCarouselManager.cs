using System.Collections.Generic;
using UnityEngine;

namespace AdrianMiasik
{
    public class DisplayCaseCarouselManager : MonoBehaviour
    {
        [SerializeField] private DisplayCaseCarouselSelector carouselSelector = null;

        public void Initialize()
        {
            // Init our selector
            carouselSelector.Initialize();
            carouselSelector.onSelectionChange += OnCarouselChange;

            // Init our individual carousels
            foreach (DisplayCaseCarousel _carousel in carouselSelector.GetItems())
            {
                _carousel.Initialize();
                _carousel.onClick += OnCarouselClick;
            }
        }

        private void OnCarouselChange(DisplayCaseCarousel _previousCarousel, DisplayCaseCarousel _currentCarousel)
        {
            // Ignore clicks on the same carousel
            if (_currentCarousel == _previousCarousel)
            {
                return;
            }

            Debug.Log("Previous: " +_previousCarousel + " and current:" + _currentCarousel);
        }

        private void OnCarouselClick(DisplayCaseCarousel _clickedCarousel)
        {
            carouselSelector.Select(_clickedCarousel);
        }

        public void CleanUp()
        {
            // Clean up selector
            carouselSelector.onSelectionChange -= OnCarouselChange;
            carouselSelector.Clear();

            // Clean up individual carousels
            foreach (DisplayCaseCarousel _carousel in carouselSelector.GetItems())
            {
                _carousel.CleanUp();
            }
        }

        public IEnumerable<DisplayCaseCarousel> GetCarousels()
        {
            return carouselSelector.GetItems();
        }
    }
}