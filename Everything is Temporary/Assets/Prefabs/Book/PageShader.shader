Shader "Custom/PageShader" {
	Properties {
		
        _PaperTexture ("Paper Texture", 2D) = "white" {}
        
        _EmbeddingPaperMix ("Embedding Paper Blend", Range(0,1)) = 0.1
        
        _LeftPageImage ("Left Page", 2D) = "check" {}
        _RightPageImage ("Right Page", 2D) = "check" {}
        
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
        
		CGPROGRAM
        
		#pragma surface surf Cel fullforwardshadows
        
        #include "Assets/Scripts/Shaders/CelShading.cginc"
        
		sampler2D _PaperTexture;
        
        half _EmbeddingPaperMix;
        
        sampler2D _LeftPageImage;
        sampler2D _RightPageImage;
        
        half _Glossiness;
        half _Metallic;
        
		struct Input {
			float2 uv_PaperTexture;
		};
        
        float2 toPageSpace (float2 uvSpace)
        {
            const float2x2 uvTransform = {
                0.0f, 2.0f,
                1.0f, 0.0f
            };
            
            return mul(uvTransform, uvSpace);
        }
        
        float2 toLeftSpace (float2 pageSpace)
        {
            return pageSpace;
        }
        
        float2 toRightSpace (float2 pageSpace)
        {
            return float2(pageSpace.x - 1, pageSpace.y);
        }
        
        bool isOnLeftPage (float2 pageSpace)
        {
            return pageSpace.x < 1;
        }
        
        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            float2 uvPageSpace = toPageSpace(IN.uv_PaperTexture);
            fixed4 paperColor = tex2D(_PaperTexture, IN.uv_PaperTexture);
            
            if (isOnLeftPage(uvPageSpace))
            {
                float2 uvLeft = toLeftSpace(uvPageSpace);
                
                fixed4 imageColor = tex2D(_LeftPageImage, uvLeft);
                
                float blend = lerp(1, _EmbeddingPaperMix, imageColor.a);
                
                o.Albedo = lerp(imageColor, paperColor, blend);
            }
            else
            {
                float2 uvRight = toRightSpace(uvPageSpace);
                
                fixed4 imageColor = tex2D(_RightPageImage, uvRight);
                
                float blend = lerp(1, _EmbeddingPaperMix, imageColor.a);
                
                o.Albedo = lerp(imageColor, paperColor, blend);
            }
            
            o.Alpha = 1;
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
