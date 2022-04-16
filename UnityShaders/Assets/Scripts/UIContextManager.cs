using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace AdrianMiasik
{
    public class UIContextManager : MonoBehaviour
    {
        [SerializeField] private List<DisplayCaseCarousel> selector = null;
        [SerializeField] private TitleLabel sourceTitleLabelPrefab = null;
        
        // Cache
        private TitleLabel currentLabel = null;
        private Shader previousShader;

        public void Start()
        {
            foreach (DisplayCaseCarousel _carousel in selector)
            {
                // Subscribe to selection changes
                _carousel.onDisplayChange += OnSelectionChange;
            }

            Shader shader = selector[selector.Count - 1].GetShader();
            
            // Spawn a label based on the last added carousel
            currentLabel = SpawnLabel(shader.ToString());

            previousShader = shader;
        }
        
        private void OnSelectionChange(DisplayCase _previousCase, DisplayCase _currentCase)
        {
            Shader shader = _currentCase.GetShader();
            
            // If the selected shader is actually a different shader...
            if (shader != previousShader)
            {
                currentLabel.Hide();
                
                // Create and cache a new label
                currentLabel = SpawnLabel(shader.ToString());
            }

            // Cache shader
            previousShader = shader;
        }

        // TODO: Spawner
        private TitleLabel SpawnLabel(string _message)
        {
            TitleLabel _label = Instantiate(sourceTitleLabelPrefab, transform);
            _label.Initialize(_message);
            return _label;
        }
    }
}