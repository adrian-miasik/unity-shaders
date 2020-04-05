using UnityEngine;

namespace AdrianMiasik
{
    public class ShaderModel : MonoBehaviour
    {
        [Header("Optional")]
        [SerializeField] private Hover hover = null;
            
        public void Initialize()
        {
            if (hover != null)
            {
                hover.Initialize();
            }
        }
    }
}