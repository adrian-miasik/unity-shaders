using UnityEngine;
using UnityEngine.EventSystems;

namespace AdrianMiasik
{
    public class OpenURLOnClick : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private string url = string.Empty;
        
        public void OnPointerClick(PointerEventData _eventData)
        {
            Application.OpenURL(url);
        }
    }
}
