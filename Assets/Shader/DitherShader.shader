Shader "Custom/DitherShader_SceneColors" {
    Properties {
        _MainTex("Texture", 2D) = "white" {}
        _Scale("Scale", Float) = 1
        _Threshold("Threshold", Float) = 0.5 // Adjusts the point at which shading switches between colors
    }
    SubShader {
        Tags { "RenderType" = "Opaque" }
        LOD 100

        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float2 uv : TEXCOORD0;
                float4 pos : SV_POSITION;
            };

            sampler2D _MainTex;
            float _Scale;
            float _Threshold;

            v2f vert(appdata_t v) {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float4 frag(v2f i) : SV_Target {
                uint matSize = 4;
                float4x4 mat = float4x4(
                    1,  8,  3, 11,
                    12, 4, 14,  6,
                    3, 10,  2,  9,
                    15, 7, 13,  5
                ) / 16.0;

                int pxIndex = int(i.pos.x * _Scale);
                int pyIndex = int(i.pos.y * _Scale);
                float4 base = tex2D(_MainTex, i.uv);

                int mati = pxIndex % matSize;
                int matj = pyIndex % matSize;

                // Dither each color channel individually
                float red = base.r - mat[mati][matj] + _Threshold;
                float green = base.g - mat[mati][matj] + _Threshold;
                float blue = base.b - mat[mati][matj] + _Threshold;

                // Clamp values to 0-1 range
                red = red > 0.5 ? base.r : 0.0;
                green = green > 0.5 ? base.g : 0.0;
                blue = blue > 0.5 ? base.b : 0.0;

                return float4(red, green, blue, base.a); // Preserve the original alpha
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
