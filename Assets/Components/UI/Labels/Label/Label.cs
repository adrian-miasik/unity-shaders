using UnityEngine;
using UnityEngine.EventSystems;

namespace AdrianMiasik
{
    public class Label : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private Animator animator = null;
        
        private static readonly int IsHovering = Animator.StringToHash("IsHovering");
        private static readonly int IsClicking = Animator.StringToHash("IsClicking");

        public void OnPointerEnter(PointerEventData _eventData)
        {
            animator.SetBool(IsHovering, true);
        }

        public void OnPointerExit(PointerEventData _eventData)
        {
            animator.SetBool(IsHovering, false);
        }

        public void OnPointerDown(PointerEventData _eventData)
        {
            animator.SetBool(IsClicking, true);
        }

        public void OnPointerDown()
        {
            OnPointerDown(null);
        }
        
        public void OnPointerUp(PointerEventData _eventData)
        {
            animator.SetBool(IsClicking, false);
        }
        
        public void OnPointerUp()
        {
            OnPointerUp(null);
        }

        public void Show()
        {
            animator.SetBool(IsHovering, true);
        }
        
        public void Hide()
        {
            animator.SetBool(IsHovering, false);
        }
    }
}