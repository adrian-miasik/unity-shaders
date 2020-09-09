using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace AdrianMiasik
{
    public class TitleLabel : MonoBehaviour
    {
        [SerializeField] private RectTransform container = null;
        [SerializeField] private ContentSizeFitter contentFitter = null;
        [SerializeField] private TMP_Text text = null;
        [SerializeField] private int preferredFontSize = 38;

        public void Initialize(string _message)
        {
            text.text = _message;
            Refresh();
        }

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
            
            // if content is overflowing...
            if (container.sizeDelta.x > 0)
            {
                Debug.Log("Scale to fit.", gameObject);
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
        
        public void Hide()
        {
            Debug.LogWarning("TODO: Animate context label out");
            Destroy(gameObject);
        }
    }
}