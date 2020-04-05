using UnityEngine;

namespace AdrianMiasik
{
    public class GameObjectActiveSelector : MonoBehaviour
    {
        [SerializeField] private ItemSelector<GameObject> gameObjectSelector = null;

        private void Start()
        {
            gameObjectSelector.Initialize();
            gameObjectSelector.onSelectionChange += OnSelectionChange;
        }
        
        private static void OnSelectionChange(GameObject _previousSelection, GameObject _currentSelection)
        {
            _previousSelection.SetActive(false);
            _currentSelection.SetActive(true);
        }
    }
}