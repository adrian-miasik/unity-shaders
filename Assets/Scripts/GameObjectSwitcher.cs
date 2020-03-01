using System.Collections.Generic;
using UnityEngine;

namespace AdrianMiasik
{
    public class GameObjectSwitcher : MonoBehaviour
    {
        [SerializeField] private List<GameObject> allGameObjects = new List<GameObject>();

        private int currentIndex;
        private GameObject currentGO;

        public delegate void OnObjectSwitch(GameObject _gameObject);
        public OnObjectSwitch onGameObjectSwitch;

        private void Start()
        {
            currentIndex = 0;
            currentGO = allGameObjects[currentIndex];
            
            foreach (GameObject _go in allGameObjects)
            {
                _go.SetActive(false);
            }

            ChangeGameObject(currentGO);
        }
        
        /// <summary>
        /// If your gameobject is within our list, we will set the current gameobject to it.
        /// </summary>
        /// <param name="_gameObjectInList"></param>
        private bool SetGameObject(GameObject _gameObjectInList)
        {
            if (!allGameObjects.Contains(_gameObjectInList))
            {
                Debug.LogWarning("Gameobject not set! Unable to find the provided gameobject.", _gameObjectInList);
                return false;
            }

            ChangeGameObject(_gameObjectInList);
            return true;
        }

        /// <summary>
        /// If your index is valid, we will set the current gameobject to that index and return true.
        /// Otherwise we will return false.
        /// </summary>
        /// <param name="_index"></param>
        /// <returns>
        /// If we were able to set the current gameobject successfully, we will return True. Otherwise we return false.
        /// </returns>
        public bool SetGameObject(int _index)
        {
            if (!IsIndexValid(_index))
            {
                Debug.LogWarning("Gameobject not set! Invalid index.");
                return false;
            }

            ChangeGameObject(allGameObjects[_index]);
            return true;
        }

        /// <summary>
        /// Returns true if the index is within the bounds of the allGameObjects list
        /// </summary>
        /// <param name="_index"></param>
        /// <returns></returns>
        private bool IsIndexValid(int _index)
        {
            return _index >= 0 && _index < allGameObjects.Count;
        }

        private void ChangeGameObject(GameObject _gameObjectInList)
        {
            currentGO.SetActive(false);
            currentGO = _gameObjectInList;
            currentGO.SetActive(true);
            
            onGameObjectSwitch?.Invoke(currentGO);
        }

        /// <summary>
        /// Disables the current gameobject and enables the next gameobject in the list.
        /// </summary>
        public void NextGameObject()
        {
            ChangeIndex(1);
            SetGameObject(currentIndex);
        }

        /// <summary>
        /// Disables the current gameobject and enables the previous gameobject in the list.
        /// </summary>
        public void PreviousGameObject()
        {
            ChangeIndex(-1);
            SetGameObject(currentIndex);
        }

        /// <summary>
        /// Increment/decrement our current index while staying within the bounds of our list by wrapping.
        /// </summary>
        /// <param name="difference"></param>
        private void ChangeIndex(int difference)
        {
            currentIndex += difference;
            currentIndex = (currentIndex + allGameObjects.Count) % allGameObjects.Count;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                PreviousGameObject();
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                NextGameObject();
            }
        }

        /// <summary>
        /// Returns the current visible gameobject
        /// </summary>
        /// <returns></returns>
        public GameObject GetCurrentGameObject()
        {
            return currentGO;
        }
    }
}