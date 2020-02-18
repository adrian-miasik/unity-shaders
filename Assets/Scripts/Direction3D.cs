using System.Collections;
using System.Collections.Generic;
using AdrianMiasik;
using UnityEngine;
using UnityEngine.EventSystems;

public class Direction3D : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] public Direction3DAxis axis = Direction3DAxis.NONE;
    
    private Transform3D transform3D;
    private bool isInitialized;
    
    public void Initialize(Transform3D _parent)
    {
        transform3D = _parent;
        isInitialized = true;
    }

    public void Initialize(Transform3D _parent, Direction3DAxis _axis)
    {
        transform3D = _parent;
        axis = _axis;
        isInitialized = true;
    }

    public bool IsInitialized()
    {
        return isInitialized;
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isInitialized)
            return;
        
        SetHoverState(true);
    }
    
    public void OnPointerExit(PointerEventData eventData)
    {
        if (!isInitialized)
            return;

        SetHoverState(false);
    }

    private void SetHoverState(bool hasPointerEntered)
    {
        if (hasPointerEntered)
        {
            Highlight();
        }
        else
        {
            UnHighlight();
        }
    }

    private void Highlight()
    {
        // Enable shader
    }

    private void UnHighlight()
    {
        // Disable shader
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        if (!isInitialized)
            return;
        
        transform3D.OnClick(this);
    }
}
