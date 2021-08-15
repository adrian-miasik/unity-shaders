using UnityEngine;

namespace AdrianMiasik
{
    public static class UIExtensions
    {
        /// <summary>
        /// Resets the rect transform anchor bounds to use their max available space. (such as full screen)
        /// Moves each anchor to their respective corner and resets our size & position.
        /// </summary>
        /// <param name="_rectTransform"></param>
        public static void Resize(this RectTransform _rectTransform)
        {
            _rectTransform.anchorMin = Vector2.zero;
            _rectTransform.anchorMax = Vector2.one;
            _rectTransform.offsetMin = Vector2.zero;
            _rectTransform.offsetMax = Vector2.zero;
        }
    }
}