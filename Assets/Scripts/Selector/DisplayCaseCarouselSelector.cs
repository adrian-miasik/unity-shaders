using System;
using System.Collections.Generic;
using UnityEngine;

namespace AdrianMiasik
{
    // Wrapper class created to show inside the Unity Editor
    // A class used to keep track of our currently selected DisplayCaseCarousel
    public class DisplayCaseCarouselSelector : ItemSelector<DisplayCaseCarousel>
    {
        [SerializeField] private List<DisplayCaseCarousel> startingCarousels = new List<DisplayCaseCarousel>();
        
        public void Initialize()
        {
            Initialize(startingCarousels);
        }
    }
}