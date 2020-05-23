using UnityEngine;

namespace AdrianMiasik
{
    public class OrbitCamera : MonoBehaviour
    {
        [SerializeField] private new Camera camera; // new keyword here since Component.camera already exists
        [SerializeField] private Transform cameraTarget = null;
        [SerializeField] private Vector3 positionOffset = Vector3.zero;
        [SerializeField] private Vector2 startingRotation = new Vector2(45f,0f);
        [SerializeField] private Vector2 axisSensitivity = Vector2.one;
        [SerializeField] private float distanceFromTarget = 15f;
        [SerializeField] private float minDistanceClamp = 1;
        
        [SerializeField] private bool autoSpin = false;
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
            Vector2 _result = Vector2.zero;
            
            _result += new Vector2(Input.GetAxis("Vertical"), Input.GetAxis("Horizontal"));
            
            if (Input.GetMouseButton(1))
            {
                _result += new Vector2(Input.GetAxis("Mouse Y") * -1, Input.GetAxis("Mouse X") * -1);
            }

            if (autoSpin)
            {
                _result += spinDirection * spinSpeed;
            }

            return _result;
        }
        
        private void MoveCamera(Vector2 _inputVector)
        {
            rotation = Quaternion.Euler(_inputVector.x, -_inputVector.y, 0);
            zoom = new Vector3(0,0, -distanceFromTarget); // Top down camera

            Vector3 _targetPoint = cameraTarget.transform.position + positionOffset;
            camera.transform.position = rotation * zoom + _targetPoint;
            camera.transform.LookAt(_targetPoint);
        }

        public void SetZoomDistance(float _desiredZoom)
        {
            distanceFromTarget = _desiredZoom;
            distanceFromTarget = Mathf.Clamp(distanceFromTarget, minDistanceClamp, distanceFromTarget);
        }
    }
}