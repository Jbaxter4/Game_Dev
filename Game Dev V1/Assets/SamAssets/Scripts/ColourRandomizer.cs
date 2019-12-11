using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;


public class ColourRandomizer : MonoBehaviour
{
    [SerializeField]
    MeshRenderer meshRenderer;
    
  
    [Button]
    public void RandomizeColour()
    {
        
        Material mat = meshRenderer.sharedMaterial;
        mat.SetColor("_RedColour", new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f)));
        mat.SetColor("_GreenColour", new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f)));
        mat.SetColor("_BlueColour", new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f)));
    }
    public void SetColours(Color[] colors)
    {
        Material mat = Material.Instantiate( meshRenderer.sharedMaterial);
        mat.SetColor("_RedColour", colors[0]);
        mat.SetColor("_GreenColour", colors[1]);
        mat.SetColor("_BlueColour", colors[2]);
        meshRenderer.material = mat;
    }
}
