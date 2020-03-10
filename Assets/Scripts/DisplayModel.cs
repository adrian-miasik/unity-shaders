using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace AdrianMiasik
{
    /// <summary>
    /// A display model is used to showcase a specific object inside a display case.
    /// </summary>
    [Serializable]
    public class DisplayModel : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private GameObject displayCase;
        private GameObject displayModel;
        
        private new Renderer renderer;
        private const PrimitiveType primitiveFallback = PrimitiveType.Sphere;
        private bool hasInitialized;

        public delegate void OnClick(DisplayModel clickedModel);
        public OnClick onClick;
        
        public void Initialize(GameObject _itemToDisplay, Vector3 _itemPositionOffset)
        {
            if (_itemToDisplay == null)
            {
                displayModel = GameObject.CreatePrimitive(primitiveFallback);
                displayModel.transform.localPosition = _itemPositionOffset;
                displayModel.transform.SetParent(displayCase.transform);
            }
            else
            {
                displayModel = Instantiate(_itemToDisplay, _itemPositionOffset, Quaternion.identity,
                    displayCase.transform);
            }

            if (displayModel != null)
            {
                hasInitialized = true;
            }
        }
        
        public void ChangeItemMaterial(Material _material)
        {
            FetchRenderer().sharedMaterial = _material;
        }
        
        public GameObject GetDisplayCase()
        {
            return displayCase;
        }

        public GameObject GetDisplayModel()
        {
            return displayModel;
        }
        
        public Renderer FetchRenderer()
        {
            if (!hasInitialized)
            {
                Debug.LogWarning("Please invoke Initialize() before FetchRenderer()");
                return null;
            }
            
            // If we have a renderer...
            if (renderer != null)
            {
#if DISPLAY_MODEL_DEBUG_MODE
                Debug.Log("Found a cached renderer component!");
#endif
                return renderer;
            }
            
#if DISPLAY_MODEL_DEBUG_MODE
            Debug.LogWarning("Unable to find a cached renderer. Attempting to look for one now...");
#endif
            
            // Grab renderer
            renderer = displayModel.GetComponent<Renderer>();
            if (renderer != null)
            {
#if DISPLAY_MODEL_DEBUG_MODE
                Debug.Log("Found a renderer component within our spawned object!");
#endif
                return renderer;
            }
            
#if DISPLAY_MODEL_DEBUG_MODE
            Debug.LogWarning("Unable to find a renderer component. Attempting to create one now...");
#endif            

            // Create renderer
            renderer = displayModel.AddComponent<Renderer>();
            if (renderer != null)
            {
#if DISPLAY_MODEL_DEBUG_MODE
                Debug.Log("A renderer has been successfully created!");
#endif
                return renderer;
            }
    
            Debug.LogAssertion("Failed to fetch you a renderer.");
            return null;
        }
        
        public void OnPointerClick(PointerEventData eventData)
        {
            onClick?.Invoke(this);
        }
    }
}