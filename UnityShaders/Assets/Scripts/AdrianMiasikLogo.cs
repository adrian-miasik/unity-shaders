using UnityEngine;
using UnityEngine.EventSystems;

namespace AdrianMiasik
{
    public class AdrianMiasikLogo : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Animator animator = null;
        private static readonly int HoverEnter = Animator.StringToHash("HoverEnter");
        private static readonly int HoverExit = Animator.StringToHash("HoverExit");

        public void OnPointerEnter(PointerEventData _eventData)
        {
            animator.SetTrigger(HoverEnter);
        }

        public void OnPointerExit(PointerEventData _eventData)
        {
            animator.SetTrigger(HoverExit);
        }
    }
}