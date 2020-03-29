using System.Collections.Generic;
using UnityEngine;

namespace AdrianMiasik
{
    public class DisplayCaseCarousel : MonoBehaviour
    {
        // References
        [SerializeField] private DisplayCase displayCasePrefab;
        [SerializeField] private MaterialList materials;
        [SerializeField] private DisplayCaseSelector selector;
        [SerializeField] private Vector3 targetPosition; // Position where we want the selected object to go to
        [SerializeField] private Vector3 itemOffset = Vector3.up; // How much to offset the model inside the display case?
        [SerializeField] private Vector3 displayOffset = Vector3.right; // How much to offset the entire display case?
        [SerializeField] private float animationDuration = 0.5f;
        
        private List<DisplayCase> displays = new List<DisplayCase>(); // Generated displays cache
        private bool isAnimating;
        private Vector3 startPosition;
        private Vector3 endPosition;
        private float accumulatedTime;

        /// <summary>
        /// Invoked when the DisplayModel selection changes
        /// </summary>
        /// <param name="_previousDisplay"></param>
        /// <param name="_currentDisplay"></param>
        public delegate void OnDisplayChange(DisplayCase _previousDisplay, DisplayCase _currentDisplay);
        public OnDisplayChange onDisplayChange;

        private void Start()
        {
            displays = GenerateDisplays(displayCasePrefab, materials);
            targetPosition = transform.position;
            
            selector.Initialize(displays);
            selector.onSelectionChange += OnSelectionChange;
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
            if (isAnimating)
            {
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
        }

        /// <summary>
        /// Creates and returns a list of DisplayModel, one for each material in the list
        /// </summary>
        /// <param name="_displayCasePrefab"></param>
        /// <param name="_list"></param>
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