using AdrianMiasik;
using UnityEngine;

public class Transform3D : MonoBehaviour
{
    [SerializeField] private Direction3D x;
    [SerializeField] private Direction3D y;
    [SerializeField] private Direction3D z;

    [SerializeField] private Direction3D currentAxis;

    private bool isInitialized;
    
    private void Start()
    {
        isInitialized = Setup();
    }

    /// <summary>
    /// Returns true if we have references to all 3 Direction3D components.
    /// </summary>
    /// <returns></returns>
    private bool Setup()
    {
        if (x != null && y != null && z != null)
        {
            x.Initialize(this, Direction3DAxis.X);
            y.Initialize(this, Direction3DAxis.Y);
            z.Initialize(this, Direction3DAxis.Z);
            
            return true;
        }
        
        Debug.LogWarning("Unable to setup this Transform3D successfully." +
                         "One or more directions are missing.", gameObject);
        return false;
    }

    public void OnClick(Direction3D _direction3D)
    {
        if (!isInitialized)
            return;

        Debug.Log("Axis changed! " + _direction3D.axis);
        currentAxis = _direction3D;
    }
}
