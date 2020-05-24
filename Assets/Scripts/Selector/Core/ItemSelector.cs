﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace AdrianMiasik
{
    /// <summary>
    /// A class that keeps track of our current (and previous) selection in a collection
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ItemSelector<T> : MonoBehaviour
    {
        [SerializeField] protected Collection<T> items = new Collection<T>();

        private T currentItem;
        private int currentIndex;
        
        private T lastSelectedItem;
        
        /// <summary>
        /// Invoked when the selection changes
        /// </summary>
        /// <param name="_previousSelection"></param>
        /// <param name="_currentSelection"></param>
        public delegate void OnSelectionChange(T _previousSelection, T _currentSelection);
        public OnSelectionChange onSelectionChange;
        
        /// <summary>
        /// Invoked when an item gets selected
        /// </summary>
        /// <param name="_selectedItem"></param>
        public delegate void OnSelected(T _selectedItem);
        public OnSelected onSelected;
        
        /// <summary>
        /// Invoked when an item gets deselected
        /// </summary>
        /// <param name="_deselectedItem"></param>
        public delegate void OnDeselected(T _deselectedItem);
        public OnDeselected onDeselected;

        /// <summary>
        /// Initializes with the serialized list
        /// </summary>
        private void Initialize()
        {
            currentIndex = 0;
            currentItem = items[currentIndex];
        }

        /// <summary>
        /// Initializes with a specific set of objects
        /// </summary>
        /// <param name="_allSelectionItems"></param>
        public void Initialize(IEnumerable<T> _allSelectionItems)
        {
            foreach (T _item in _allSelectionItems)
            {
                items.Add(_item);
            }

            Initialize();
        }

        public void Clear()
        {
            items.Clear();
            currentItem = default;
        }
        
        public void AddItem(T _item)
        {
            items.Add(_item);
        }

        public void RemoveItem(T _item)
        {
            items.Remove(_item);
        }
        
        /// <summary>
        /// Selects the previous item in the collection
        /// </summary>
        public void PreviousItem()
        {
            ChangeIndex(-1);
            Select(currentIndex);
        }

        /// <summary>
        /// Selects the next item in the collection
        /// </summary>
        public void NextItem()
        {
            ChangeIndex(1);
            Select(currentIndex);
        }
        
        /// <summary>
        /// Increment/decrement our current index while staying within the bounds of our list by wrapping.
        /// </summary>
        /// <param name="_difference"></param>
        private void ChangeIndex(int _difference)
        {
            currentIndex += _difference;
            currentIndex = (currentIndex + items.Count) % items.Count;
        }
        
        /// <summary>
        /// If your item is within our list, we will set the current item to it.
        /// </summary>
        /// <param name="_itemInList"></param>
        public bool Select(T _itemInList)
        {
            if (!items.Contains(_itemInList))
            {
                Debug.LogWarning("Item not set! Unable to find the provided item.");
                return false;
            }

            ChangeSelection(_itemInList);
            onSelectionChange?.Invoke(lastSelectedItem, currentItem);
            return true;
        }

        /// <summary>
        /// If your index is valid, we will set the current item to the object at the provided index and return true.
        /// Otherwise we will only return false. (No current item change)
        /// </summary>
        /// <param name="_index"></param>
        public bool Select(int _index)
        {
            if (!IsIndexValid(_index))
            {
                Debug.LogWarning("Item not set! Invalid index.");
                return false;
            }

            ChangeSelection(items[_index]);
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

        private void ChangeSelection(T _itemInList)
        {
            // Deselect
            lastSelectedItem = currentItem;
            onDeselected?.Invoke(currentItem);

            // Swap
            currentItem = _itemInList;
            currentIndex = items.IndexOf(_itemInList);

            // Select
            onSelected?.Invoke(currentItem);
        }
        
        /// <summary>
        /// Returns the current visible item
        /// </summary>
        /// <returns></returns>
        public T GetCurrentItem()
        {
            return currentItem;
        }

        public int GetCurrentIndex()
        {
            return currentIndex;
        }

        public int GetCount()
        {
            return items.Count;
        }

        public T GetLastSelectedItem()
        {
            return lastSelectedItem;
        }

        public Collection<T> GetItems()
        {
            return items;
        }
    }
}