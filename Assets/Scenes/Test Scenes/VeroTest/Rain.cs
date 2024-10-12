using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Rain : MonoBehaviour
{
    public Material rainMaterial; // Arrastra el material del shader aquí.

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (rainMaterial != null)
        {
            Graphics.Blit(source, destination, rainMaterial);
        }
        else
        {
            Graphics.Blit(source, destination);
        }
    }
}
