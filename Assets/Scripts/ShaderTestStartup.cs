using UnityEngine;

namespace AdrianMiasik{
    public class ShaderTestStartup : MonoBehaviour
    {
        [SerializeField] private DisplayCaseCarousel standardCarousel;
        [SerializeField] private DisplayCaseCarousel shaderGraphCarousel;

        private void Start()
        {
            SetupEnvironment();
        }

        [ContextMenu("Setup Environment")]
        private void SetupEnvironment()
        {
            standardCarousel.Initialize();
            shaderGraphCarousel.Initialize();
        }
    }
}