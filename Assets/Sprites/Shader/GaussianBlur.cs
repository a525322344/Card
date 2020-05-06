using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GaussianBlur : PostEffectBase
{
    //模糊半径
    public float BlurRadius = 1.0f;
    //分辨率下降
    public int downSample = 2;
    //迭代次数
    public int iteration = 1;
    // Start is called before the first frame update

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (_Material)
        {
            RenderTexture rt1 = RenderTexture.GetTemporary(source.width >> downSample, source.height >> downSample, 0,source.format);
            RenderTexture rt2 = RenderTexture.GetTemporary(source.width >> downSample, source.height >> downSample, 0,source.format); ;

            Graphics.Blit(source, rt1);
            for(int i = 0; i < iteration; i++)
            {
                _Material.SetVector("_OffSets", new Vector4(0, BlurRadius, 0, 0));
                Graphics.Blit(rt1, rt2, _Material);
                _Material.SetVector("_OffSets", new Vector4(BlurRadius,0, 0, 0));
                Graphics.Blit(rt2, rt1, _Material);
            }

            Graphics.Blit(rt1, destination);
            RenderTexture.ReleaseTemporary(rt1);
            RenderTexture.ReleaseTemporary(rt2);
        }
    }
}
