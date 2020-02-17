using System;
using System.Collections.Generic;
using AdrianMiasik;
using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    [SerializeField] private List<Selection> allSelections = new List<Selection>();
    [SerializeField] private GameObject selectionVisual;
    
    public static SelectionManager instance { get; private set; }
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogWarning("Removing singleton instance since one already exists.");
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

        selectionVisual.transform.position = _selection.transform.position;
        
        Debug.Log("Selected: " + _selection.gameObject.name);
    }

    /// <summary>
    /// Adds your selection to the selector
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
