using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace AdrianMiasik
{
    public class UIContextManager : MonoBehaviour
    {
        [SerializeField] private RectTransform container = null;
        [SerializeField] private ContentSizeFitter contentFitter = null;
        [SerializeField] private TMP_Text text = null;
        [SerializeField] private List<DisplayCaseCarousel> selector = null;
        [SerializeField] private int preferredFontSize = 38;
        
        private void Refresh()
        {
            if (text.text == string.Empty)
            {
                contentFitter.horizontalFit = ContentSizeFitter.FitMode.MinSize;
            }
            
            // Change fit and update ui elements (font before layout group)
            text.enableAutoSizing = false;
            text.fontSize = preferredFontSize;
            contentFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
            LayoutRebuilder.ForceRebuildLayoutImmediate(container);

            Debug.LogWarning(container.sizeDelta.x);
            
            // if content is overflowing...
            if (container.sizeDelta.x > 0)
            {
                contentFitter.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;
                container.Resize();
                text.enableAutoSizing = true;
            }
            // else content is within bounds...
            else
            {
                contentFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
                text.enableAutoSizing = false;
                text.fontSize = preferredFontSize;
            }
        }
        
        public void Start()
        {
            foreach (DisplayCaseCarousel _carousel in selector)
            {
                _carousel.onDisplayChange += OnSelectionChange;
                text.text = _carousel.GetSelectedDisplayModel().GetModelRenderer().sharedMaterial.shader.ToString();
            }
            
            Refresh();
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
            Refresh();
        }
    }
}