using UnityEngine;
using UnityEngine.SceneManagement;

namespace AdrianMiasik
{
    public class QuickSceneLoad : MonoBehaviour
    {
        [SerializeField] private int sceneIndex = 0;

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.F5))
            {
                SceneManager.LoadScene(sceneIndex);
            }
        }
    }
}