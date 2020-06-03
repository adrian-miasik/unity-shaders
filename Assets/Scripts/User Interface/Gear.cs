using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace AdrianMiasik
{
    public class Gear : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private Animator animator = null;

        [SerializeField] private List<Graphic> gearGraphics = new List<Graphic>();

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
            ChangeColor(new Color(0.95f, 0.95f, 0.95f, 1f));
        }

        public void OnPointerUp(PointerEventData _eventData)
        {
            animator.SetBool(IsClicking, false);
            ChangeColor(Color.white);
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