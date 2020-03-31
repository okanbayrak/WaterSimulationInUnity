using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlurEffect : MonoBehaviour
{
    public int iterationCount;
    public float blurSpreadingConstant = 0.4f;
    public Shader blurShader;

    private static Material _material = null;
    protected Material material
    {
        get
        {
            if (_material == null)
            {
                _material = new Material(blurShader) { hideFlags = HideFlags.DontSave };
            }
            return _material;
        }
    }
    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        int width = source.width / 4;
        int height = source.height / 4;
        RenderTexture buffer = RenderTexture.GetTemporary(width, height, 0);
        Blur(source, buffer, 1);
        //Blur effect
        for (int i = 0; i < iterationCount; i++)
        {
            RenderTexture bluredTexture = RenderTexture.GetTemporary(width, height, 0);
            Blur(buffer, bluredTexture, i);
            RenderTexture.ReleaseTemporary(buffer);
            buffer = bluredTexture;
        }
        Graphics.Blit(buffer, destination);
        RenderTexture.ReleaseTemporary(buffer);
    }

    private void Blur(RenderTexture source, RenderTexture destination, int iteration)
    {
        float offset = iteration * blurSpreadingConstant;
        Vector2[] offsets = new Vector2[] {
            new Vector2(-offset, -offset),
            new Vector2(-offset, offset),
            new Vector2(offset, offset),
            new Vector2(offset, -offset)
        };
        Graphics.BlitMultiTap(source, destination, material, offsets);
    }
}
