using UnityEngine;

namespace AdrianMiasik
{
    public class GameObjectActiveSelector : MonoBehaviour
    {
        [SerializeField] private ItemSelector<GameObject> _selector;

        private void Start()
        {
            _selector.Initialize();
            _selector.onSelectionChange += OnSelectionChange;
        }
        
        private void OnSelectionChange(GameObject _previousSelection, GameObject _currentSelection)
        {
            _previousSelection.SetActive(false);
            _currentSelection.SetActive(true);
        }
    }
}