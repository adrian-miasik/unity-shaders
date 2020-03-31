using System.Collections.Generic;
using UnityEngine;

namespace AdrianMiasik
{
    public class DisplayCaseCarousel : MonoBehaviour
    {
        // References
        [SerializeField] private DisplayCase displayCasePrefab = null;
        [SerializeField] private MaterialList materials = null;
        [SerializeField] private DisplayCaseSelector selector = null;
        
        // TODO: Create inspector which only shows this after init
        [SerializeField] private Vector3 targetPosition; // Position where we want the selected object to go to 
        
        [SerializeField] private Vector3 itemOffset = Vector3.up; // How much to offset the model inside the display case?
        [SerializeField] private Vector3 displayOffset = Vector3.right; // How much to offset the entire display case?
        [SerializeField] private float animationDuration = 0.5f;
        
        // TODO: Create inspector which makes this a read-only
        private bool isInit; 
        
        private List<DisplayCase> displays = new List<DisplayCase>(); 
        private bool isAnimating;
        private Vector3 startPosition;
        private Vector3 endPosition;
        private float accumulatedTime;

        /// <summary>
        /// Invoked when the DisplayModel selection changes
        /// </summary>
        public delegate void OnDisplayChange(DisplayCase _previousDisplay, DisplayCase _currentDisplay);
        public OnDisplayChange onDisplayChange;
        
        public void Initialize()
        {
            if (isInit)
            {
                Debug.LogWarning("This carousel has already been initialized");
                return;
            }
            
            displays = GenerateDisplays(displayCasePrefab, materials);
            targetPosition = transform.position;
            
            selector.Initialize(displays);
            selector.onSelectionChange += OnSelectionChange;

            isInit = true;
        }

        private void OnSelectionChange(DisplayCase _previousDisplay, DisplayCase _currentDisplay)
        {
            // Ignore clicks on the same display case
            if (_currentDisplay == _previousDisplay)
            {
                return;
            }

            startPosition = transform.position;
            endPosition = displayOffset * (selector.GetCurrentIndex() * -1);
            accumulatedTime = 0;

            isAnimating = true;
            onDisplayChange?.Invoke(_previousDisplay, _currentDisplay);
        }

        private void Update()
        {
            if (!isInit)
            {
                return;
            }

            if (!isAnimating)
            {
                return;
            }

            accumulatedTime += Time.deltaTime;
            if (accumulatedTime > animationDuration)
            {
                accumulatedTime = animationDuration;
                isAnimating = false;
            }

            float t = accumulatedTime / animationDuration;
            t = t * t * (3 - 2 * t); // Smoothstep formula
            transform.position = Vector3.Lerp(startPosition, targetPosition + endPosition, t);
        }

        /// <summary>
        /// Creates and returns a list of DisplayModel, one for each material in the list
        /// </summary>
        /// <param name="_displayCasePrefab">The type of display you want to generate</param>
        /// <param name="_list">The list of materials you want to apply for each model in each display case</param>
        private List<DisplayCase> GenerateDisplays(DisplayCase _displayCasePrefab, MaterialList _list)
        {
            List<DisplayCase> generatedDisplays = new List<DisplayCase>();

            for (int i = 0; i < _list.materials.Count; i++)
            {
                // Create a display
                DisplayCase displayCase = Instantiate(_displayCasePrefab, transform);
                displayCase.Initialize(null, itemOffset);
                displayCase.GetDisplay().transform.position += displayOffset * i;
                displayCase.onClick += OnDisplayWheelItemClick;

                // Cache display
                generatedDisplays.Add(displayCase);

                // Change display item's material
                displayCase.SwapMaterialOnModel(_list.materials[i]);
            }

            return generatedDisplays;
        }

        public DisplayCase GetSelectedDisplayModel()
        {
            return selector.GetCurrentItem();
        }

        private void OnDisplayWheelItemClick(DisplayCase _displayCase)
        {
            selector.Select(_displayCase);
        }
    }
}