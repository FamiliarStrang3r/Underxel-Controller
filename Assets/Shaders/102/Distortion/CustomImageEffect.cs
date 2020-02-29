using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode, ImageEffectAllowedInSceneView]
public class CustomImageEffect : MonoBehaviour
{
    [SerializeField] private bool drawEffect = false;
    [SerializeField] private Material mat = null;

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (mat && drawEffect)
            Graphics.Blit(source, destination, mat);
        else
            Graphics.Blit(source, destination);
    }
}
