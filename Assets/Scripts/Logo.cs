using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Logo : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Animator animator;
    
    private static readonly int HoverEnter = Animator.StringToHash("HoverEnter");
    private static readonly int HoverExit = Animator.StringToHash("HoverExit");

    public void OnPointerEnter(PointerEventData eventData)
    {
        animator.SetTrigger(HoverEnter);
    }
    
    public void OnPointerExit(PointerEventData eventData)
    {
        animator.SetTrigger(HoverExit);
    }
}
