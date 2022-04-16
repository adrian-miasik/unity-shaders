using UnityEngine;
using UnityEngine.UI;

public class QuitButton : MonoBehaviour
{
    [SerializeField] private Button quit;

    private void Start()
    {
        quit.onClick.AddListener(QuitApplication);
    }

    private void QuitApplication()
    {
        Debug.Log("Closing Application");
        Application.Quit();
    }
}
