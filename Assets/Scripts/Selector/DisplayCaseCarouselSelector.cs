using System.Collections.Generic;
using UnityEngine;

namespace AdrianMiasik
{
    // Wrapper class created to show inside the Unity Editor
    public class DisplayCaseCarouselSelector : ItemSelector<DisplayCaseCarousel>
    {
        [SerializeField] private List<DisplayCaseCarousel> startingCarousels = new List<DisplayCaseCarousel>();

        /// <summary>
        /// Initializes this selector using the serialized set of objects
        /// </summary>
        public void Initialize()
        {
            Initialize(startingCarousels);
        }
    }
}