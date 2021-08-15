using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace AdrianMiasik
{
    public class UIContextManager : MonoBehaviour
    {
        [SerializeField] private TMP_Text text = null;
        [SerializeField] private List<DisplayCaseCarousel> selector = null;

        public void Start()
        {
            foreach (DisplayCaseCarousel _carousel in selector)
            {
                _carousel.onDisplayChange += OnSelectionChange;
                text.text = _carousel.GetSelectedDisplayModel().GetModelRenderer().sharedMaterial.shader.ToString();
            }
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