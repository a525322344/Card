using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
public class CardOutlineShaderCS : MonoBehaviour
{
    public Vector4 Sides;
    public Color color;
    public float OutlineWidth;
    public float CardOpacity;
    public float AlphaMult;
    [Range(0,1)]
    public float trimDistance;
    [Range(0,1)]
    public float smoothDistance;
    public float smoothSpeed;
    public float noiseFreq;
    public float noiseSpeed;
    public float noiseMult;
    public float noiseThreshold;
    public float noiseDistance;
    public float noiseOffset;
    public float seed;
    private Material material;
    // Update is called once per frame
    void Update()
    {
        if (material)
        {
            material.SetVector("_Sides", Sides);
            material.SetFloat("_Width", OutlineWidth);
            material.SetFloat("_AlphaMult", AlphaMult);
            material.SetFloat("_CardOpacity", CardOpacity);
            material.SetColor("_Color", color);
            material.SetFloat("_TrimOffset", trimDistance);
            material.SetFloat("_Smooth", smoothDistance);
            material.SetFloat("_SmoothSpeed", smoothSpeed);
            material.SetFloat("_NoiseFreq", noiseFreq);
            material.SetFloat("_NoiseSpeed", noiseSpeed);
            material.SetFloat("_NoiseMult", noiseMult);
            material.SetFloat("_NoiseThreshold", noiseThreshold);
            material.SetFloat("_NoiseDistance", noiseDistance);
            material.SetFloat("_NoiseOffset", noiseOffset);
            material.SetFloat("_Seed", seed);
        }
        else
        {
            material = GetComponent<MeshRenderer>().material;
        }
    }
}
