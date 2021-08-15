using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace AdrianMiasik
{
    public class Gear : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
    {
        // References
        [SerializeField] private Animator animator = null;
        [SerializeField] private List<Graphic> gearGraphics = new List<Graphic>();

        // Animation hashes
        private static readonly int IsHovering = Animator.StringToHash("IsHovering");
        private static readonly int IsClicking = Animator.StringToHash("IsClicking");

        // Unity Events
        public UnityEvent onShow;
        public UnityEvent onHide; 
        public UnityEvent OnClickDown;
        public UnityEvent OnClickUp;

        public void OnPointerEnter(PointerEventData _eventData)
        {
            animator.SetBool(IsHovering, true);
            onShow?.Invoke();
        }

        public void OnPointerExit(PointerEventData _eventData)
        {
            animator.SetBool(IsHovering, false);
            onHide?.Invoke();
        }

        public void OnPointerDown(PointerEventData _eventData)
        {
            animator.SetBool(IsClicking, true);
            ChangeColor(new Color(0.95f, 0.95f, 0.95f, 1f));
            OnClickDown?.Invoke();
        }

        public void OnPointerUp(PointerEventData _eventData)
        {
            animator.SetBool(IsClicking, false);
            ChangeColor(Color.white);
            
            OnClickUp?.Invoke();
        }

        private void ChangeColor(Color _color)
        {
            foreach (Graphic graphic in gearGraphics)
            {
                graphic.color = _color;
            }
        }
    }
}