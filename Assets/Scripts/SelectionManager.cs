using System;
using System.Collections.Generic;
using AdrianMiasik;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectionManager : MonoBehaviour
{
    [SerializeField] private List<Selection> allSelections = new List<Selection>();
    [SerializeField] private GameObject selectionVisual;
    
    public static SelectionManager instance { get; private set; }

    private Selection lastSelectionPoint;
    
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

    public void OnClick(Selection _selection)
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            // If we don't have this selection...
            if (!allSelections.Contains(_selection))
            {
                AddSelection(_selection);
            }
            else
            {
                RemoveSelection(_selection);
            }
        }
        else
        {
            // If we don't have this selection...
            if (!allSelections.Contains(_selection))
            {
                Select(_selection);
            }
            else
            {
                Deselect();
            }
        }
    }

    private void Select(Selection _selection)
    {
        allSelections.Clear();
        allSelections.Add(_selection);

        ShowVisual(_selection.transform.position);
    }
    
    /// <summary>
    /// Adds your selection to the selector
    /// </summary>
    /// <param name="_selection"></param>
    private void AddSelection(Selection _selection)
    {
        if (allSelections.Contains(_selection))
        {
            Debug.LogWarning("Unable to add this selection. This selection has already been added.");
            return;
        }
        
        allSelections.Add(_selection);
        UpdateVisuals();
    }
    
    private void RemoveSelection(Selection _selection)
    {
        allSelections.Remove(_selection);
        UpdateVisuals();
    }
    
    public void Deselect()
    {
        allSelections.Clear();
        HideVisual();
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

    public bool IsSelectionAdded(Selection _selection)
    {
        return allSelections.Contains(_selection);
    }

    private void UpdateVisuals()
    {
        Vector3 resultPosition = Vector3.zero;
        
        foreach (Selection selection in allSelections)
        {
            resultPosition +=  selection.transform.position;
        }

        resultPosition /= allSelections.Count;
        
        ShowVisual(resultPosition);
    }
    
    private void ShowVisual(Vector3 _position)
    {
        selectionVisual.SetActive(true);
        selectionVisual.transform.position = _position;
    }

    private void HideVisual()
    {
        selectionVisual.SetActive(false);
    }
}
