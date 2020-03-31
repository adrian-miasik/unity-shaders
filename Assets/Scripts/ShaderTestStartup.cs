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

        [ContextMenu("Setup Environment")]
        private void SetupEnvironment()
        {
            standardCarousel.Initialize();
            shaderGraphCarousel.Initialize();
        }
    }
}