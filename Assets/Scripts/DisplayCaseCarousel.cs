using System.Collections.Generic;
using UnityEngine;

namespace AdrianMiasik
{
    public class DisplayCaseCarousel : MonoBehaviour
    {
        // References
        [SerializeField] private DisplayCase displayPrefab = null;
        [SerializeField] private MaterialList materials = null;
        [SerializeField] private GameObject modelPrefab = null;
        [SerializeField] private DisplayCaseSelector displayCaseSelector = null;
        
        // TODO: Create inspector which only shows this after init
        [SerializeField] private Vector3 targetPosition; // Position where we want the selected object to go to 
        
        [SerializeField] private Vector3 itemOffset = Vector3.up; // How much to offset the model inside the display case?
        [SerializeField] private Vector3 displayOffset = Vector3.right; // How much to offset the entire display case?
        [SerializeField] private float animationDuration = 0.5f;
        
        // TODO: Create inspector which makes this a read-only
        private bool isInitialized; 
        
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
        
        [ContextMenu("Initialize")]
        public void Initialize()
        {
            if (isInitialized)
            {
                Debug.LogWarning("This carousel has already been initialized");
                return;
            }
            
            // Generate displays
            GenerateDisplays(displayPrefab, modelPrefab, materials.materials.Count);

            // Swap materials on each model
            for (int i = 0; i < displays.Count; i++)
            {
                DisplayCase display = displays[i];
                display.ChangeModelMaterial(materials.materials[i]);
            }

            targetPosition = transform.position;
            
            displayCaseSelector.Initialize(displays);
            displayCaseSelector.onSelectionChange += OnSelectionChange;
            
            isInitialized = true;
        }

        /// <summary>
        /// Destroys any cached game objects
        /// </summary>
        [ContextMenu("Clean Up")]
        public void CleanUp()
        {
            foreach (DisplayCase displayCase in displays)
            {
                DestroyImmediate(displayCase.gameObject);
            }
            
            Clear();
        }

        private void Clear()
        {
            displays.Clear();
            targetPosition = Vector3.zero;
            
            displayCaseSelector.onSelectionChange -= OnSelectionChange;
            displayCaseSelector.Clear();
            
            isInitialized = false;
        }
        
        private void OnSelectionChange(DisplayCase _previousDisplay, DisplayCase _currentDisplay)
        {
            // Ignore clicks on the same display case
            if (_currentDisplay == _previousDisplay)
            {
                return;
            }

            startPosition = transform.position;
            endPosition = displayOffset * (displayCaseSelector.GetCurrentIndex() * -1);
            accumulatedTime = 0;

            isAnimating = true;
            onDisplayChange?.Invoke(_previousDisplay, _currentDisplay);
        }

        private void Update()
        {
            if (!isInitialized)
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
        /// Creates and caches a list of DisplayCases
        /// </summary>
        /// <param name="_displayPrefab">What display case do you want to generate?</param>
        /// <param name="_modelToSpawnInside">What GameObject do you want to generate inside the display case?</param>
        /// <param name="_quantity">How many displays would you like to generate?</param>
        /// <returns></returns>
        private void GenerateDisplays(DisplayCase _displayPrefab, GameObject _modelToSpawnInside, int _quantity)
        {
            displays.Clear();

            for (int i = 0; i < _quantity; i++)
            {
                // Create a display
                DisplayCase displayCase = Instantiate(_displayPrefab, transform);
                displayCase.Initialize(_modelToSpawnInside, itemOffset);
                displayCase.GetDisplay().transform.position += displayOffset * i;
                displayCase.onClick += OnDisplayWheelItemClick;

                // Cache display
                displays.Add(displayCase);
            }
        }
        
        public void NextDisplay()
        {
            displayCaseSelector.NextItem();
        }

        public void PreviousDisplay()
        {
            displayCaseSelector.PreviousItem();
        }
        
        public DisplayCase GetSelectedDisplayModel()
        {
            return displayCaseSelector.GetCurrentItem();
        }

        private void OnDisplayWheelItemClick(DisplayCase _displayCase)
        {
            displayCaseSelector.Select(_displayCase);
        }
    }
}