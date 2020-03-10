using TMPro;
using UnityEngine;

namespace AdrianMiasik
{
    public class UIContextManager : MonoBehaviour
    {
        [SerializeField] private TMP_Text text;
        [SerializeField] private DisplayWheel _selector;

        public void Start()
        {
            _selector.onDisplayChange += OnSelectionChange;

            text.text = _selector.GetSelectedDisplayModel().FetchRenderer().sharedMaterial.shader.ToString();
        }
        
        private void OnSelectionChange(DisplayModel _previousModel, DisplayModel _currentModel)
        {
            if (_currentModel == null)
            {
                text.enabled = false;
                return;
            }

            text.enabled = true;
            text.text = _currentModel.FetchRenderer().sharedMaterial.shader.ToString();
        }
    }
}