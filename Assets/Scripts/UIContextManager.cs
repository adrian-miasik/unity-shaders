using System;
using AdrianMiasik;
using TMPro;
using UnityEngine;

public class UIContextManager : MonoBehaviour
{
    [SerializeField] private TMP_Text text;

    [SerializeField] private GameObjectSwitcher switcher;

    private void Start()
    {
        text.enabled = false;
        switcher.onGameObjectSwitch += OnGameObjectSwitch;
    }
    
    private void OnGameObjectSwitch(GameObject _gameobject)
    {
        if (gameObject == null)
        {
            text.enabled = false;
            return;
        }

        text.enabled = true;
        
        if (_gameobject.GetComponent<Renderer>())
        {
            Renderer renderer = _gameobject.GetComponent<Renderer>();
            text.text = renderer.sharedMaterial.shader.ToString();
            return;
        }
        
        text.text = _gameobject.name;
    }
}
