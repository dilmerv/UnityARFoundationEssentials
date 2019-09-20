using UnityEngine;

namespace DilmerGames.Core.Utilities
{
    public static class MaterialUtils 
    {
        public static Material CreateMaterial(Color color, string name, string shaderName = "Standard")
        {
            Material material = new Material(Shader.Find(shaderName));
            material.name = name;
            material.color = color;
            material.EnableKeyword("_EMISSION");
            material.SetColor("_EmissionColor", color);
            return material;
        }
    }
}