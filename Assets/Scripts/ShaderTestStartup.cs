using UnityEngine;

namespace AdrianMiasik{
    public class ShaderTestStartup : MonoBehaviour
    {
        [SerializeField] private DisplayCaseCarousel standardCarousel = null;
        [SerializeField] private DisplayCaseCarousel shaderGraphCarousel = null;

        private void Start()
        {
            SetupEnvironment();
        }

        [ContextMenu("Start Environment")]
        private void SetupEnvironment()
        {
            standardCarousel.Initialize();
            shaderGraphCarousel.Initialize();
        }
        
        [ContextMenu("Quit Environment")]
        private void CleanUpEnvironment()
        {
            standardCarousel.CleanUp();
            shaderGraphCarousel.CleanUp();
        }
    }
}