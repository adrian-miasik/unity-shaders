using System;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;

namespace AdrianMiasik
{
    public class DisplayCase : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private GameObject display = null;
        [SerializeField] private PositionConstraint modelConstraintContainer = null;

        [Header("Optional")]
        private GameObject model;

        private new Renderer renderer;
        private const PrimitiveType primitiveFallback = PrimitiveType.Sphere; // TODO: Store in user settings
        private bool hasInitialized;

        public delegate void OnClick(DisplayCase _clickedCase);
        public OnClick onClick;

        private bool disableRendererShadows;
        
        public void Initialize(GameObject _modelToSpawn, Vector3 _modelLocalPos, bool disableRendererShadows = false)
        {
            this.disableRendererShadows = disableRendererShadows;
            SpawnModel(_modelToSpawn, _modelLocalPos);

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
        /// Spawns provided gameobject at a specific local position
        /// </summary>
        /// <summary>
        /// Note: If no model is provided we will use a fallback gameobject instead
        /// </summary>
        /// <param name="_modelToSpawn">What gameobject would you like to spawn inside the display case?</param>
        /// <param name="_modelLocalPos">New local position of the model inside the display case</param>
        private void SpawnModel(GameObject _modelToSpawn, Vector3 _modelLocalPos)
        {
            if (_modelToSpawn == null)
            {
                Debug.LogWarning("No model found. Falling back to primitive model.");
                
                // Use fallback model
                model = GameObject.CreatePrimitive(primitiveFallback);
                model.transform.SetParent(modelConstraintContainer.transform);
            }
            else
            {
                // Use desired model
                model = Instantiate(_modelToSpawn, modelConstraintContainer.transform);

                if (disableRendererShadows)
                {
                    if (model.GetComponent<Renderer>())
                    {
                        model.GetComponent<Renderer>().shadowCastingMode = ShadowCastingMode.Off;
                    }
                }
            }

            model.transform.localPosition = _modelLocalPos;
        }
        
        /// <summary>
        /// Switch the material on the model
        /// </summary>
        /// <param name="_material">The material you want to switch to</param>
        public void ChangeModelMaterial(Material _material)
        {
            GetModelRenderer().sharedMaterial = _material;
        }
        
        public void SetDisplayPosition(Vector3 _worldPosition)
        {
            display.transform.position = _worldPosition;
        }

        public Vector3 GetDisplayWorldPosition()
        {
            return display.transform.position;
        }
        
        public GameObject GetModel()
        {
            return model;
        }
        
        /// <summary>
        /// Returns the renderer component on the model GameObject
        /// </summary>
        public Renderer GetModelRenderer()
        {
            if (!hasInitialized)
            {
                Debug.LogWarning("Please invoke Initialize() before GetModelRenderer()");
                return null;
            }
            
            if (renderer != null)
            {
                // Return cached renderer
                return renderer;
            }
            
            renderer = model.GetComponent<Renderer>();
            if (renderer != null)
            {
                // Return found renderer
                return renderer;
            }
            
            // Fallback to mesh renderer
            // TODO: Better fallback support
            renderer = model.AddComponent<MeshRenderer>();
            if (renderer != null)
            {
                // Return created renderer
                return renderer;
            }
    
            Debug.LogAssertion("Failed to fetch you a renderer.");
            return null;
        }

        public Shader GetShader()
        {
            return GetModelRenderer().sharedMaterial.shader;
        }
        
        public void OnPointerClick(PointerEventData _eventData)
        {
            onClick?.Invoke(this);
        }
    }
}