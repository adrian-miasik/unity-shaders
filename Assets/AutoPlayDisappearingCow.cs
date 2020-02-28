using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoPlayDisappearingCow : MonoBehaviour
{
    [SerializeField] private Material disappearingCowMaterial;
    [SerializeField] private float progressDuration = 3f;
    private float progress;
    
    private float elapsedTime;
    
    private void Update()
    {
        elapsedTime += Time.deltaTime;
        progress = elapsedTime / progressDuration;
        disappearingCowMaterial.SetFloat("Vector1_19CF4714", progress);
        
        if (elapsedTime >= progressDuration)
        {
            elapsedTime = 0;
        }
    }
}
