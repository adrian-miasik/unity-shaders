using System.Collections.Generic;
using UnityEngine;

namespace AdrianMiasik
{
    /// <summary>
    /// A class that keeps track of our current (and previous) selection in a list.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class ItemSelector<T> : MonoBehaviour
    {
        [SerializeField] private List<T> items = new List<T>();

        private int currentIndex;
        private T currentItem;
        private T lastSelectedItem;
        
        public delegate void OnSelectionChange(T _newSelection);
        public OnSelectionChange onSelectionChange;
        
        public void Initialize()
        {
            currentIndex = 0;
            currentItem = items[currentIndex];
            
            foreach (T _item in items)
            {
                OnStart(_item);
            }

            ChangeItem(currentItem);
        }
        
        protected virtual void Update()
        {
            // TODO: Remove hardcoded input logic
            
            if (Input.GetKeyDown(KeyCode.Q))
            {
                PreviousItem();
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                NextItem();
            }
        }
        
        /// <summary>
        /// Invoked for each item in the selector on initialization
        /// </summary>
        /// <param name="_spawnedItem"></param>
        protected abstract void OnStart(T _spawnedItem);
        
        /// <summary>
        /// Invoked when an item gets selected
        /// </summary>
        /// <param name="_selectedItem"></param>
        protected abstract void OnSelected(T _selectedItem);
        
        /// <summary>
        /// Invoked when an item gets deselected
        /// </summary>
        /// <param name="_deselectedItem"></param>
        protected abstract void OnDeselected(T _deselectedItem);

        public void AddItem(T _item)
        {
            items.Add(_item);
        }

        public void RemoveItem(T _item)
        {
            items.Remove(_item);
        }
        
        /// <summary>
        /// Disables the current item and enables the previous item in the list.
        /// </summary>
        private void PreviousItem()
        {
            ChangeIndex(-1);
            SetItem(currentIndex);
        }

        /// <summary>
        /// Disables the current item and enables the next item in the list.
        /// </summary>
        private void NextItem()
        {
            ChangeIndex(1);
            SetItem(currentIndex);
        }
        
        /// <summary>
        /// Increment/decrement our current index while staying within the bounds of our list by wrapping.
        /// </summary>
        /// <param name="difference"></param>
        private void ChangeIndex(int difference)
        {
            currentIndex += difference;
            currentIndex = (currentIndex + items.Count) % items.Count;
        }
        
        /// <summary>
        /// If your item is within our list, we will set the current item to it.
        /// </summary>
        /// <param name="_itemInList"></param>
        public bool SetItem(T _itemInList)
        {
            if (!items.Contains(_itemInList))
            {
                Debug.LogWarning("Item not set! Unable to find the provided item.");
                return false;
            }

            ChangeItem(_itemInList);
            return true;
        }

        /// <summary>
        /// If your index is valid, we will set the current item to that index and return true.
        /// Otherwise we will return false.
        /// </summary>
        /// <param name="_index"></param>
        /// <returns>
        /// If we were able to set the current item successfully, we will return True. Otherwise we return false.
        /// </returns>
        private bool SetItem(int _index)
        {
            if (!IsIndexValid(_index))
            {
                Debug.LogWarning("Item not set! Invalid index.");
                return false;
            }

            ChangeItem(items[_index]);
            return true;
        }

        /// <summary>
        /// Returns true if the index is within the bounds of the elements list
        /// </summary>
        /// <param name="_index"></param>
        /// <returns></returns>
        private bool IsIndexValid(int _index)
        {
            return _index >= 0 && _index < items.Count;
        }
        
        private void ChangeItem(T _itemInList)
        {
            lastSelectedItem = currentItem;
            OnDeselected(currentItem);
            currentItem = _itemInList;
            currentIndex = items.IndexOf(_itemInList);
            OnSelected(currentItem);

            onSelectionChange?.Invoke(currentItem);
        }
        
        /// <summary>
        /// Returns the current visible item
        /// </summary>
        /// <returns></returns>
        protected T GetCurrentItem()
        {
            return currentItem;
        }

        protected int GetCurrentIndex()
        {
            return currentIndex;
        }

        protected int GetCount()
        {
            return items.Count;
        }

        protected T GetLastSelectedItem()
        {
            return lastSelectedItem;
        }
    }
}