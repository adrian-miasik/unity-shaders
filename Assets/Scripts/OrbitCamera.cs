using System;
using UnityEngine;

namespace AdrianMiasik
{
    [ExecuteInEditMode]
    public class OrbitCamera : MonoBehaviour
    {
        [SerializeField] private Camera camera;
        [SerializeField] private Transform cameraTarget;
        [SerializeField] private Vector3 positionOffset;
        [SerializeField] private Vector2 startingRotation = new Vector2(45f,0f);
        [SerializeField] private Vector2 axisSensitivity = Vector2.one;
        [SerializeField] private float distanceFromTarget = 15f;
        
        
        
        [SerializeField] private bool autoSpin;
        [SerializeField] private Vector2 spinDirection = Vector2.up;
        [SerializeField] private float spinSpeed = 0.1f;
        
        private Vector3 zoom;
        private Quaternion rotation;
        
        private Vector2 resultVector;
        
        private void Reset()
        {
            // Quickly fetch references
            camera = Camera.main;
        }
        
        private void Start()
        {
            if (camera == null)
            {
                Debug.LogWarning("Missing reference to camera. (A Camera Component)");
            }
            
            if (cameraTarget == null)
            {
                Debug.LogWarning("Missing reference to cameraTarget. (A Transform Component)");
            }
        }

        private void Update()
        {
            // Sample input w/ sensitivity
            resultVector += FetchInput() * axisSensitivity;
            
            // Translate & rotate camera
            MoveCamera(resultVector + startingRotation);
        }

        private Vector2 FetchInput()
        {
            Vector2 result = Vector2.zero;
            
            result += new Vector2(Input.GetAxis("Vertical"), Input.GetAxis("Horizontal"));
            
            if (Input.GetMouseButton(1))
            {
                result += new Vector2(Input.GetAxis("Mouse Y") * -1, Input.GetAxis("Mouse X") * -1);
            }

            if (autoSpin)
            {
                result += spinDirection * spinSpeed;
            }

            return result;
        }
        
        private void MoveCamera(Vector2 _inputVector)
        {
            rotation = Quaternion.Euler(_inputVector.x, -_inputVector.y, 0);
            zoom = new Vector3(0,0, -distanceFromTarget); // Top down camera

            Vector3 targetPoint = cameraTarget.transform.position + positionOffset;
            camera.transform.position = rotation * zoom + targetPoint;
            camera.transform.LookAt(targetPoint);
        }

        public void SetZoomDistance(float _desiredZoom)
        {
            distanceFromTarget = _desiredZoom;
        }
    }
}