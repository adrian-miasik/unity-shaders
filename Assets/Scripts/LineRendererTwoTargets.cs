using UnityEngine;

namespace AdrianMiasik
{
    [ExecuteInEditMode]
    public class LineRendererTwoTargets : MonoBehaviour
    {
        [SerializeField] private LineRenderer lineRenderer;

        [Header("Target Points")]
        [SerializeField] private Transform startTarget;
        [SerializeField] private Transform endTarget;

        private void Reset()
        {
            // Quickly fetch references
            lineRenderer = GetComponent<LineRenderer>();
            startTarget = gameObject.transform;
        }

        private void Start()
        {
            if (lineRenderer == null)
            {
                Debug.LogWarning("Missing reference to lineRenderer. (A LineRenderer Component)", gameObject);
            }
        }

        private void Update()
        {
            // Define the length of our lineRender positions
            lineRenderer.positionCount = 2; // One for startTarget and another for endTarget.

            // Connect startTarget and endTarget using our line renderer
            lineRenderer.SetPosition(0, startTarget.position);
            lineRenderer.SetPosition(1, endTarget.position);
        }
    }
}