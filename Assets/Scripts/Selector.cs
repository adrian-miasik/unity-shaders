using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Selector : MonoBehaviour
{
    [SerializeField] private List<Selection> allSelections = new List<Selection>();
    
    public static Selector instance { get; private set; }
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    /// <summary>
    /// Select a single object
    /// </summary>
    /// <param name="_selection"></param>
    public void Select(Selection _selection)
    {
        allSelections.Clear();
        allSelections.Add(_selection);

        Debug.Log("Selected: " + _selection.gameObject.name);
    }

    /// <summary>
    /// Safely adds your selection to the selector
    /// </summary>
    /// <param name="_selection"></param>
    public void AddSelection(Selection _selection)
    {
        if (allSelections.Contains(_selection))
        {
            Debug.LogWarning("Unable to add this selection. This selection has already been added.");
            return;
        }
        
        ForceAddSelection(_selection);
    }

    /// <summary>
    /// Forcefully adds your selection to the selector 
    /// </summary>
    /// <summary>
    /// Note: Use AddSelection() if you don't know what you are doing.
    /// </summary>
    /// <param name="_selection"></param>
    private void ForceAddSelection(Selection _selection)
    {
        allSelections.Add(_selection);
    }

    public void Deselect()
    {
        Debug.Log("All selections cleared.");
        allSelections.Clear();
    }

    /// <summary>
    /// Logs all currently selected selections
    /// </summary>
    private void LogSelections()
    {
        foreach (Selection selection in allSelections)
        {
            Debug.Log(selection.gameObject.name);
        }
        
        Debug.Log("Total selected: [" + allSelections.Count + "]");
    }
}
