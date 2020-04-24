Shader "Custom/cardOutline"
{
    Properties
    {
        _Color("Color",Color)=(1,1,1,1)
    }
    SubShader
    {
        Tags
        {
            "IgnoreProjector" = "True"
            "Queue" = "Transparent"
            "RenderType" = "Transparent"
            "DisableBatching" = "True"      //禁止使用批处理，这样就可以对内部空间操作了
        }
        Pass{
            Lighting Off
            Fog{ Mode Off}
            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha

            CGPROGRAM
            #pragma target 3.0
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            #include "noiseSimplex.cginc"

            uniform float4 _Sides;
            uniform float _Width;
            uniform float _AlphaMult;
            uniform float _CardOpacity;
            uniform float4 _Color;
            uniform float _TrimOffset;
			uniform float _Smooth;
			uniform float _SmoothSpeed;
			uniform float _NoiseFreq;
			uniform float _NoiseSpeed;
			uniform float _NoiseMult;
			uniform float _NoiseThreshold;
			uniform float _NoiseDistance;
			uniform float _NoiseOffset;
			uniform float _Seed;

            struct a2v{
                float4 vertex:POSITION;
                float2 texcoord0:TEXCOORD0;
            };
            struct v2f{
                float4 pos:SV_POSITION;
                float2 uv:TEXCOORD0;
            };

            v2f vert(a2v v){
                v2f o;
                o.uv=v.texcoord0;
                float4 w=v.vertex;
                w.x+=normalize(v.vertex).x*_Width;
                w.y+=normalize(v.vertex).y*_Width;
                o.pos=UnityObjectToClipPos(w);
                return o;
            }
            float4 frag(v2f i):SV_Target{
                float4 col=float4(1,1,1,1);
                //对颜色亮度调整
                col.xyz=_Color*(_AlphaMult*pow(_CardOpacity,6));
                //裁剪透明 渐变
                float left=smoothstep(0.0, _TrimOffset, i.uv.x);
                float right=1 - smoothstep(1 - _TrimOffset, 1.0, i.uv.x);
                float down=smoothstep(0.0, _TrimOffset, i.uv.y);
                float up=1 - smoothstep(1 - _TrimOffset, 1.0, i.uv.y);
                float trim=1-lerp(1,left,_Sides.x)*lerp(1,right,_Sides.y)*lerp(1,up,_Sides.z)*lerp(1,down,_Sides.w);
                col.a=lerp(0,1,trim);
                //削弱最外层
                float sm=1 - smoothstep(0.0, _Smooth, i.uv.x) * (1 - smoothstep(1 - _Smooth, 1.0, i.uv.x)) * smoothstep(0.0, _Smooth, i.uv.y) * (1 - smoothstep(1 - _Smooth, 1.0, i.uv.y));
                col.a=max(0,col.a-sm*_SmoothSpeed);

                //噪音
                float2 nuv=i.uv*_NoiseFreq;
                nuv.y+=_Time.x*_NoiseSpeed+_Seed;
                float ns=snoise(nuv+i.uv)*_NoiseMult+_NoiseOffset;

                ns=lerp(0,_NoiseThreshold,ns);

                float noise=1-smoothstep(0,_NoiseDistance,i.uv.x) * (1 - smoothstep(1 - _NoiseDistance, 1.0, i.uv.x)) * smoothstep(0.0, _NoiseDistance, i.uv.y) * (1 - smoothstep(1 - _NoiseDistance, 1.0, i.uv.y));
            	col*=lerp(1,ns,noise);
            	col.a*=_CardOpacity;
            	return col;
            }
            ENDCG
        }
 
    }
    FallBack "Diffuse"
}
