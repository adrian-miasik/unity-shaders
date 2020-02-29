using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AutoPlayDisappearingCow : MonoBehaviour
{
    [SerializeField] private Renderer rend;
    [SerializeField] private float progressDuration = 3f;
    private float progress;
    private MaterialPropertyBlock materialBlock;
    
    private float elapsedTime;
    private static readonly int Vector119Cf4714 = Shader.PropertyToID("Vector1_19CF4714");

    private void Start()
    {
        materialBlock = new MaterialPropertyBlock();
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;
        
        SetRendererProgress(elapsedTime / progressDuration);
        
        if (elapsedTime >= progressDuration)
        {
            elapsedTime = 0;
        }
    }

    private void SetRendererProgress(float _progress)
    {
        materialBlock.SetFloat(Vector119Cf4714, _progress);
        rend.SetPropertyBlock(materialBlock);
    }
}
