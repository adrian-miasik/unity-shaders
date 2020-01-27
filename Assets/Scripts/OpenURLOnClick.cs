using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace AdrianMiasik
{
    public class OpenURLOnClick : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private string url;
        
        public void OnPointerClick(PointerEventData eventData)
        {
            Application.OpenURL(url);
        }
    }
}
