using UnityEngine;

[ExecuteInEditMode]
public class HLSLLitLambert : MonoBehaviour
{
    [SerializeField] private Light directionalLight;
    
    private Renderer renderer;
    private bool isInitialized;
    private static readonly int WorldSpaceLightPosition = Shader.PropertyToID("_WorldSpaceLightPosition");
    private static readonly int WorldSpaceLightColor = Shader.PropertyToID("_WorldSpaceLightColor");

    private void Awake()
    {
        renderer = GetComponent<Renderer>();

        if (renderer == null)
        {
            Debug.LogWarning("Unable to fetch renderer.");
        }
        else
        {
            isInitialized = true;
        }
    }

    private void Update()
    {
        if (isInitialized)
        {
            renderer.material.SetVector(WorldSpaceLightPosition, directionalLight.transform.localEulerAngles);
            renderer.material.SetColor(WorldSpaceLightColor, directionalLight.color);
            Debug.Log(directionalLight.transform.position);
        }
    }
}
