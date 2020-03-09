using System.Collections.Generic;
using UnityEngine;

namespace AdrianMiasik
{
    [CreateAssetMenu(fileName = "Default Material List", menuName = "Adrian Miasik/Scriptable Objects/Material List")]
    public class MaterialList : ScriptableObject
    {
        public List<Material> materials = new List<Material>();
    }
}