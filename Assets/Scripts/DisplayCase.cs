using System;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.EventSystems;

namespace AdrianMiasik
{
    public class DisplayCase : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private GameObject display = null;
        [SerializeField] private PositionConstraint modelConstraintContainer = null;
        
        private GameObject model;
        
        private new Renderer renderer;
        private const PrimitiveType primitiveFallback = PrimitiveType.Sphere;
        private bool hasInitialized;

        public delegate void OnClick(DisplayCase _clickedCase);
        public OnClick onClick;
        
        public void Initialize(GameObject _modelToSpawn, Vector3 _newPosition)
        {
            // Spawn model
            if (_modelToSpawn == null)
            {
                // Use fallback model
                model = GameObject.CreatePrimitive(primitiveFallback);
                model.transform.SetParent(modelConstraintContainer.transform);
                model.transform.localPosition = _newPosition;
            }
            else
            {
                // Use desired model
                model = Instantiate(_modelToSpawn, _newPosition, Quaternion.identity, modelConstraintContainer.transform);
            }

            if (model != null)
            {
                hasInitialized = true;
            }
            else
            {
                Debug.LogAssertion("Failed to initialize.", gameObject);
            }
        }

        /// <summary>
        /// Switch the material on the model
        /// </summary>
        /// <param name="_material"></param>
        public void SwapMaterialOnModel(Material _material)
        {
            GetModelRenderer().sharedMaterial = _material;
        }
        
        public GameObject GetDisplay()
        {
            return display;
        }

        public GameObject GetModel()
        {
            return model;
        }
        
        /// <summary>
        /// Returns the renderer component on the model GameObject.
        /// </summary>
        /// <summary>If a cached renderer is found, we will return that.</summary>
        /// <summary>If a cached renderer is not found, we will attempt to get and return the renderer on the model
        /// using GetComponent.</summary>
        /// <summary>If the GetComponent renderer is not found, we will attempt to create and return a new renderer
        /// on the model using AddComponent.</summary>
        /// <summary>If the created renderer is not created successfully, we will return null and log an assertion.</summary>
        /// <returns></returns>
        public Renderer GetModelRenderer()
        {
            if (!hasInitialized)
            {
                Debug.LogWarning("Please invoke Initialize() before FetchModelRenderer()");
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
            renderer = model.GetComponent<Renderer>();
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
            renderer = model.AddComponent<Renderer>();
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