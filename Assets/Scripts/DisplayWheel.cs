using System.Collections.Generic;
using UnityEngine;

namespace AdrianMiasik
{
    public class DisplayWheel : MonoBehaviour
    {
        [SerializeField] private DisplayModel displayModelPrefab;
        [SerializeField] private MaterialList materials;
        [SerializeField] private Vector3 itemOffset = Vector3.up; 
        [SerializeField] private Vector3 displayOffset = Vector3.right;
        
        [SerializeField] private DisplayModelItemSelector selector;
        
        private List<DisplayModel> displays = new List<DisplayModel>();
        private Vector3 startPosition;
        private Vector3 endPosition;
        private float accumulatedTime;
        private float duration = 0.5f;

        /// <summary>
        /// Invoked when the DisplayModel selection changes
        /// </summary>
        /// <param name="_previousDisplay"></param>
        /// <param name="_currentDisplay"></param>
        public delegate void OnDisplayChange(DisplayModel _previousDisplay, DisplayModel _currentDisplay);
        public OnDisplayChange onDisplayChange;

        private void Start()
        {
            displays = GenerateDisplays(displayModelPrefab, materials);

            selector.Initialize(displays);
            selector.onSelectionChange += OnSelectionChange;
        }
        
        private void OnSelectionChange(DisplayModel _previousDisplay, DisplayModel _currentDisplay)
        {
            // Ignore clicks on the same display model
            if (_currentDisplay == _previousDisplay)
            {
                return;
            }

            startPosition = transform.position;
            endPosition = displayOffset * (selector.GetCurrentIndex() * -1);
            accumulatedTime = 0;
            
            onDisplayChange?.Invoke(_previousDisplay, _currentDisplay);
        }

        private void Update()
        {
            accumulatedTime += Time.deltaTime;
            if (accumulatedTime > duration)
            {
                accumulatedTime = duration;
            }

            float t = accumulatedTime / duration;
            t = t * t * (3 - 2 * t); // Smoothstep formula
            transform.position = Vector3.Lerp(startPosition, endPosition, t);
        }

        /// <summary>
        /// Creates and returns a list of DisplayModel, one for each material in the list
        /// </summary>
        /// <param name="_displayModelPrefab"></param>
        /// <param name="_list"></param>
        private List<DisplayModel> GenerateDisplays(DisplayModel _displayModelPrefab, MaterialList _list)
        {
            Vector3 _currentOffset = Vector3.zero;
            List<DisplayModel> generatedDisplays = new List<DisplayModel>();
            
            foreach (Material mat in _list.materials)
            {
                // Create a display
                DisplayModel displayModel = Instantiate(_displayModelPrefab, Vector3.zero, Quaternion.identity, transform);
                displayModel.Initialize(null, itemOffset);
                displayModel.onClick += OnDisplayWheelItemClick;
                
                generatedDisplays.Add(displayModel);
                
                // Move entire display
                displayModel.GetDisplayCase().transform.position = _currentOffset;
                _currentOffset += displayOffset;
                
                // Change display item's material
                displayModel.ChangeItemMaterial(mat);
            }

            return generatedDisplays;
        }

        public DisplayModel GetSelectedDisplayModel()
        {
            return selector.GetCurrentItem();
        }

        private void OnDisplayWheelItemClick(DisplayModel _displayModel)
        {
            selector.Select(_displayModel);
        }
    }
}