using UnityEngine;

public class BackgroundScenarioManager : MonoBehaviour
{
    [SerializeField] private Camera currentCamera;

    private enum ScenarioTypes
    {
        Skybox,
        SolidColor
    }

    [SerializeField] private ScenarioTypes scenario = ScenarioTypes.Skybox;
    
    private readonly Color[] solidColors = new[] {Color.black, Color.grey, Color.white};
    private int currentColor;
    
    private void Start()
    {
        SetupScenario(scenario);
    }

    private void Update()
    {
        // Skybox
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SetupScenario(ScenarioTypes.Skybox);
        }
        
        // Solid Color
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SetupScenario(ScenarioTypes.SolidColor);
        }

        if (scenario == ScenarioTypes.SolidColor)
        {
            // Next Color
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                ChangeColorSelection(1);
            }
        }
    }
    
    private void SetupScenario(ScenarioTypes _scenario)
    {
        scenario = _scenario;
        
        switch (scenario)
        {
            // Use scenario to manipulate the camera component
            case ScenarioTypes.Skybox:
                InitializeSkybox();
                break;
            case ScenarioTypes.SolidColor:
                InitializeSolidColor(GetSolidColor(currentColor));
                break;
            default:
                Debug.LogWarning("This scenario is not fully supported.");
                break;
        }
    }

    /// <summary>
    /// Updates the camera flags
    /// </summary>
    private void InitializeSkybox()
    {
        currentCamera.clearFlags = CameraClearFlags.Skybox;
    }
    
    /// <summary>
    /// Updates camera flags and sets the RenderSettings skybox to be desiredSkybox
    /// </summary>
    /// <param name="desiredSkybox"></param>
    private void InitializeSkybox(Material desiredSkybox)
    {
        InitializeSkybox();
        RenderSettings.skybox = desiredSkybox;
    }
    
    private void InitializeSolidColor(Color _color)
    {
        currentCamera.clearFlags = CameraClearFlags.SolidColor;
        SetCameraBackground(_color);
    }

    private void SetCameraBackground(Color _color)
    {
        currentCamera.backgroundColor = _color;
    }
    
    private Color GetSolidColor(int _index)
    {
        return solidColors[_index];
    }

    /// <summary>
    /// Increment/decrement our currentColor index while staying within the bounds of our array by wrapping.
    /// </summary>
    /// <param name="difference"></param>
    private void ChangeColorSelection(int difference)
    {
        currentColor += difference;
        currentColor = (currentColor + solidColors.Length) % solidColors.Length;

        SetCameraBackground(GetSolidColor(currentColor));
    }
}