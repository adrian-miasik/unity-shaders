using TMPro;
using UnityEngine;

namespace AdrianMiasik
{
    public class UIContextManager : MonoBehaviour
    {
        [SerializeField] private TMP_Text text = null;
        [SerializeField] private DisplayCaseCarousel _selector = null;

        public void Start()
        {
            _selector.onDisplayChange += OnSelectionChange;

            text.text = _selector.GetSelectedDisplayModel().GetModelRenderer().sharedMaterial.shader.ToString();
        }
        
        private void OnSelectionChange(DisplayCase _previousCase, DisplayCase _currentCase)
        {
            if (_currentCase == null)
            {
                text.enabled = false;
                return;
            }

            text.enabled = true;
            text.text = _currentCase.GetModelRenderer().sharedMaterial.shader.ToString();
        }
    }
}