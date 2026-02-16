Shader "Custom/UI_PulseScan"
{
    Properties
    {
        _ColorA ("Color A", Color) = (0.08, 0.10, 0.15, 1)
        _ColorB ("Color B", Color) = (0.10, 0.35, 0.80, 1)
        _PulseSpeed ("Pulse Speed", Float) = 1.5
        _PulseAmount ("Pulse Amount", Range(0,1)) = 0.35

        _ScanSpeed ("Scan Speed", Float) = 0.6
        _ScanWidth ("Scan Width", Range(0.01, 0.5)) = 0.12
        _ScanIntensity ("Scan Intensity", Range(0, 2)) = 0.6
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "RenderType"="Transparent"
            "IgnoreProjector"="True"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

        Cull Off
        Lighting Off
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            fixed4 _ColorA;
            fixed4 _ColorB;
            float _PulseSpeed;
            float _PulseAmount;

            float _ScanSpeed;
            float _ScanWidth;
            float _ScanIntensity;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {

                float g = smoothstep(0.0, 1.0, i.uv.y);
                fixed4 col = lerp(_ColorA, _ColorB, g);


                float p = (sin(_Time.y * _PulseSpeed) * 0.5 + 0.5);
                col.rgb = lerp(col.rgb, _ColorB.rgb, p * _PulseAmount);


                float s = frac(_Time.y * _ScanSpeed);
                float band = 1.0 - smoothstep(_ScanWidth, _ScanWidth * 2.0, abs(i.uv.y - s));
                col.rgb += band * _ScanIntensity;

                return col;
            }
            ENDCG
        }
    }
}