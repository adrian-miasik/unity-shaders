using UnityEngine;
using UnityEngine.EventSystems;

namespace AdrianMiasik
{
    /// <summary>
    /// Opens the URL in a web browser / whatever environment is appropriate
    /// </summary>
    public class OpenURLOnClick : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private string url = string.Empty;
        
        public void OnPointerClick(PointerEventData _eventData)
        {
            Application.OpenURL(url);
        }
    }
}
