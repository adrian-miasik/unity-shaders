using UnityEngine;

namespace AdrianMiasik
{
    public class Zoom : MonoBehaviour
    {
        [SerializeField] private OrbitCamera orbitCamera = null;
        [SerializeField] private float sensitivity = 3f;
        [SerializeField] private float currentZoom = 20f;

        private void Start()
        {
            if (orbitCamera == null)
            {
                Debug.LogWarning("Missing reference to orbitCamera. (An OrbitCamera.cs Component)");
            }
        }

        private void Update()
        {
            currentZoom += FetchInput() * sensitivity;
            currentZoom = Mathf.Clamp(currentZoom, 0, currentZoom);
            
            orbitCamera.SetZoomDistance(currentZoom);
        }

        private static float FetchInput()
        {
            return Input.GetAxis("Mouse ScrollWheel") * -1;
        }
    }
}