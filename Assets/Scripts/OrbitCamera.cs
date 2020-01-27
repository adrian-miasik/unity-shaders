using System;
using UnityEngine;

namespace AdrianMiasik
{
    public class OrbitCamera : MonoBehaviour
    {
        [SerializeField] private Camera camera;
        [SerializeField] private Transform cameraTarget;
        [SerializeField] private float distanceFromTarget = 15f;
        [SerializeField] private Vector2 axisSensitivity = Vector2.one;
        
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
            MoveCamera(resultVector);
        }

        private Vector2 FetchInput()
        {
            return new Vector2(Input.GetAxis("Vertical"), Input.GetAxis("Horizontal"));
        }
        
        private void MoveCamera(Vector2 _inputVector)
        {
            rotation = Quaternion.Euler(_inputVector.x, -_inputVector.y, 0);
            zoom = new Vector3(0,0, -distanceFromTarget); // Top down camera
            
            camera.transform.position = rotation * zoom + cameraTarget.transform.position;
            camera.transform.LookAt(cameraTarget);
        }
    }
}