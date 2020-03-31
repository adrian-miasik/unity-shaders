﻿using UnityEngine;

namespace AdrianMiasik
{
    public class ShaderTimerProgress : TimerProgress
    {
        [SerializeField] private Renderer rend = null;
        [SerializeField] private string shaderProgressProperty = null;
        
        private MaterialPropertyBlock materialBlock;
        private int shaderProgressID;
        
        protected override bool Initialize()
        {
            shaderProgressID = Shader.PropertyToID(shaderProgressProperty);
            materialBlock = new MaterialPropertyBlock();
            
            return true;
        }
        
        protected override void OnUpdate(float _progress)
        {
            SetRendererProgress(_progress);
        }

        protected override void OnComplete()
        {
            // Nothing
        }

        private void SetRendererProgress(float _progress)
        {
            materialBlock.SetFloat(shaderProgressID, _progress);
            rend.SetPropertyBlock(materialBlock);
        }
    }
}
