using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BackButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image m_highlight;
    [SerializeField] private Color m_highlightColor;

    private void Start()
    {
        m_highlight.color = Color.clear;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        m_highlight.color = m_highlightColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        m_highlight.color = Color.clear;
    }
}
