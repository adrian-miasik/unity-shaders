using UnityEngine;
using UnityEngine.SceneManagement;

namespace AdrianMiasik
{
    public class QuickSceneLoad : MonoBehaviour
    {
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.F5))
            {
                SceneManager.LoadScene(0);
            }
        }
    }
}