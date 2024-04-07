Shader "Terastal/Test"
{
    Properties
    {
        _BaseColor ("Base Color", Color) = (1, 1, 1, 1)
    }
	SubShader
    {
        Tags 
		{ 
			"RenderType" = "Opaque"
			"Queue" = "Geometry"
			"RenderPipeline" = "UniversalPipeline"
		}

        Pass
        {
			Tags
			{
				"LightMode" = "UniversalForward"
			}

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

			float rand(float2 seed, float min, float max)
			{
				float r = frac(sin(dot(seed, float2(12.9898, 78.2333))) * 43758.5453);
				return lerp(min, max, r);
			}

            struct appdata
            {
                float4 positionOS : Position;
				//uint primitiveID : SV_PrimitiveID;
            };

            struct v2f
            {
                float4 positionCS : SV_Position;
				//nointerpolation uint primitiveID : SV_PrimitiveID;
            };

			CBUFFER_START(UnityPerMaterial)
				float4 _BaseColor;
			CBUFFER_END

            v2f vert (appdata v)
            {
                v2f o;
                o.positionCS = TransformObjectToHClip(v.positionOS.xyz);
				//o.primitiveID = v.primitiveID;
                return o;
            }

            float4 frag (v2f i, uint primitiveID : SV_PrimitiveID) : SV_Target
            {
				return rand(primitiveID, 0.0f, 1.0f);
            }
            ENDHLSL
        }
    }
}
