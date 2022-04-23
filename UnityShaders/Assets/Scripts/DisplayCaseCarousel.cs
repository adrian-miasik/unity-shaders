using System;
using System.Collections.Generic;
using UnityEngine;

namespace AdrianMiasik
{
    [ExecuteInEditMode]
    public class DisplayCaseCarousel : MonoBehaviour
    {
        // References
        [SerializeField] private DisplayCaseSelector displayCaseSelector = null;
        [SerializeField] private DisplayCase displayCasePrefab = null;
        [SerializeField] private GameObject shaderModelPrefab = null;
        [SerializeField] private MaterialList materials = null;
        [SerializeField] private bool disableRendererShadows = false;

        [Tooltip("How much to offset the model inside the display case?")]
        [SerializeField] private Vector3 itemOffset = Vector3.up;

        [SerializeField] public Vector3 staggerDisplayOffset = Vector3.right;
        [SerializeField] private Vector3 individualDisplayOffset = Vector3.up;

        [SerializeField, HideInInspector] private bool isInitialized;
        [SerializeField, HideInInspector] private List<DisplayCase> displays = new List<DisplayCase>();

        // Animations
        [SerializeField] private float movementDuration = 1;
        [SerializeField] private AnimationCurve movementCurve = AnimationCurve.Linear(0,0,1,1);

        // Optional
        private CarouselManager carouselManager;
        
        private bool isMoving;
        private bool isDelayed;
        private Vector3 startPosition;
        private Vector3 endPosition;
        private float totalAccumulatedTime;
        private float accumulatedWaitTime;
        private float delayTime = 1;
        
        public delegate void OnDisplayChange(DisplayCase _previousDisplay, DisplayCase _currentDisplay);
        public OnDisplayChange onDisplayChange;
        
        [ContextMenu("Initialize")]
        public void Initialize(CarouselManager _carouselManager)
        {
            if (isInitialized)
            {
                Debug.Log("Building Carousel.");
                CleanUp();
            }

            if(_carouselManager != null)
            {
                carouselManager = _carouselManager;
            }
            
            GenerateDisplays(displayCasePrefab, shaderModelPrefab, materials.materials.Count);

            // Swap materials on each model
            for (int _i = 0; _i < displays.Count; _i++)
            {
                DisplayCase _display = displays[_i];
                _display.ChangeModelMaterial(materials.materials[_i]);
                _display.gameObject.name = "Display Case: " + _display.GetShader();
            }

            startPosition = transform.position;
            
            displayCaseSelector.Initialize(displays);
            displayCaseSelector.onSelected += OnSelected;

            isInitialized = true;
        }
        
        private void OnSelected(DisplayCase _selectedItem)
        {
            if (carouselManager != null)
            {
                carouselManager.OnSelected(this, _selectedItem);
            }
            
            onDisplayChange?.Invoke(displayCaseSelector.GetLastSelectedItem(), _selectedItem);
        }

        /// <summary>
        /// Destroys any cached game objects
        /// </summary>
        [ContextMenu("Clean Up")]
        public void CleanUp()
        {
            foreach (DisplayCase _displayCase in displays)
            {
                // It's possible our display case was deleted in the editor
                if (_displayCase != null)
                {
                    DestroyImmediate(_displayCase.gameObject);
                }
            }
            
            Clear();
        }

        [ContextMenu("Rebuild")]
        public void Rebuild()
        {
            CleanUp();
            Initialize(null);
        }

        private void Clear()
        {
            displays.Clear();
            
            displayCaseSelector.onSelected -= OnSelected;
            displayCaseSelector.Clear();
            
            isInitialized = false;
        }

        private void Update()
        {
            if (!isInitialized)
            {
                return;
            }

            if (!isMoving)
            {
                return;
            }

            totalAccumulatedTime += Time.deltaTime;

            // Early exit condition
            isDelayed = totalAccumulatedTime <= delayTime;
            if (isDelayed)
            {
                return;
            }

            accumulatedWaitTime += Time.deltaTime;
            float _t = accumulatedWaitTime / movementDuration;

            if (_t > 1)
            {
                transform.position = endPosition;
                isMoving = false;
                return;
            }

            _t = movementCurve.Evaluate(_t);
            transform.position = Vector3.Lerp(startPosition, endPosition, _t);
        }

        // TODO: Ability to queue movements
        public void MoveTo(Vector3 _targetPosition, float _delay)
        {
            startPosition = transform.position;
            endPosition = _targetPosition;

            delayTime = _delay;
            totalAccumulatedTime = 0;
            accumulatedWaitTime = 0;

            if (_delay > 0)
            {
                isDelayed = true;
            }

            isMoving = true;
        }

        public float GetMovementDuration()
        {
            return movementDuration;
        }

        // TODO: Spawner
        /// <summary>
        /// Creates and caches a list of DisplayCases
        /// </summary>
        /// <param name="_display">What display case do you want to generate?</param>
        /// <param name="_modelToSpawnInside">What GameObject do you want to generate inside the display case?</param>
        /// <param name="_quantity">How many displays would you like to generate?</param>
        /// <returns></returns>
        private void GenerateDisplays(DisplayCase _display, GameObject _modelToSpawnInside, int _quantity)
        {
            displays.Clear();

            for (int _i = 0; _i < _quantity; _i++)
            {
                // Create a display
                DisplayCase _displayCase = Instantiate(_display, transform);
                _displayCase.Initialize(_modelToSpawnInside, itemOffset, disableRendererShadows);
                _displayCase.SetDisplayPosition(_displayCase.GetDisplayWorldPosition() + staggerDisplayOffset * _i + individualDisplayOffset);
                _displayCase.onClick += OnDisplayCaseClick;

                // Cache display
                displays.Add(_displayCase);
            }
        }

        public Shader GetShader()
        {
            return GetSelectedDisplayModel().GetModelRenderer().sharedMaterial.shader;
        }

        public DisplayCase GetSelectedDisplayModel()
        {
            return displayCaseSelector.GetCurrentItem();
        }

        /// <summary>
        /// Invoked when the user clicks on the display case
        /// </summary>
        /// <param name="_displayCase"></param>
        private void OnDisplayCaseClick(DisplayCase _displayCase)
        {
            // TODO: Include carousel
            displayCaseSelector.Select(_displayCase);
        }

        /// <summary>
        /// Returns the cached display cases
        /// </summary>
        /// <returns></returns>
        public IEnumerable<DisplayCase> GetDisplayCases()
        {
            return displays;
        }

        public int GetSelectedIndex()
        {
            return displayCaseSelector.GetCurrentIndex();
        }

        public bool IsMoving()
        {
            return isMoving;
        }
    }
}