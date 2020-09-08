using UnityEngine;

namespace AdrianMiasik
{
    public static class UIExtensions
    {
        public static void Resize(this RectTransform _rectTransform)
        {
            _rectTransform.anchorMin = Vector2.zero;
            _rectTransform.anchorMax = Vector2.one;
            _rectTransform.offsetMin = Vector2.zero;
            _rectTransform.offsetMax = Vector2.zero;
        }
    }
}