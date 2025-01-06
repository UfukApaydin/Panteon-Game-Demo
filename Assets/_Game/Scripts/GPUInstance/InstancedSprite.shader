Shader "Custom/InstancedSprite"
{
    Properties
    {
        _MainTex ("Sprite Texture", 2D) = "white" {}
    }

    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 100

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            Cull Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_instancing

            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;

                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            sampler2D _MainTex;

            // Declare instanced properties explicitly
            UNITY_INSTANCING_BUFFER_START(Props)
                float4 _InstanceColor;          // Per-instance color
                float4x4 _InstanceTransform;   // Per-instance transformation
            UNITY_INSTANCING_BUFFER_END(Props)

            v2f vert(appdata_t v)
            {
                v2f o;

                // Set up instancing ID
                UNITY_SETUP_INSTANCE_ID(v);

                // Apply instanced transformation
                float4 worldPos = mul(UNITY_ACCESS_INSTANCED_PROP(Props, _InstanceTransform), float4(v.vertex.xy, 0.0, 1.0));
                o.vertex = UnityObjectToClipPos(worldPos);

                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // Set up instancing ID
                UNITY_SETUP_INSTANCE_ID(i);

                // Get instance color and multiply with texture
                fixed4 col = tex2D(_MainTex, i.uv) * UNITY_ACCESS_INSTANCED_PROP(Props, _InstanceColor);
                return col;
            }
            ENDCG
        }
    }
}
