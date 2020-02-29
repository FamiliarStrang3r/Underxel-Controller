using UnityEngine;

[ExecuteInEditMode, ImageEffectAllowedInSceneView]
public class BoxBlur : MonoBehaviour
{
    [SerializeField] private Material mat = null;
    [SerializeField, Range(0, 5)] private int iterations = 0;
    [SerializeField, Range(0, 4)] private int downResolution = 0;

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (mat)
        {
            int width = source.width >> downResolution;
            int height = source.height >> downResolution;

            RenderTexture temp = RenderTexture.GetTemporary(width, height);
            Graphics.Blit(source, temp);

            for (int i = 0; i < iterations; i++)
            {
                RenderTexture temp1 = RenderTexture.GetTemporary(width, height);
                Graphics.Blit(temp, temp1, mat);
                RenderTexture.ReleaseTemporary(temp);
                temp = temp1;
            }

            Graphics.Blit(temp, destination);
            RenderTexture.ReleaseTemporary(temp);
        }
        else
        {
            Graphics.Blit(source, destination);
        }
    }
}