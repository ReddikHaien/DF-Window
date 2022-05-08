Shader "Custom/ChunkShader"
{
    Properties
    {
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _MainTex2 ("Layer 1 (RGB)", 2D) = "white" {}
        _MainTex3 ("Layer 2 (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;
        sampler2D _MainTex2;
        sampler2D _MainTex3;
        struct Input
        {
            float2 uv_MainTex: TEXCOORD0;
            float2 uv2_MainTex2: TEXCOORD1;
            float2 uv3_MainTex3: TEXCOORD2;
            half4 vertex_color: COLOR0;
        };

        half _Glossiness;
        half _Metallic;
    
        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Albedo comes from a texture tinted by color
            fixed4 b = tex2D (_MainTex, IN.uv_MainTex);
            fixed4 l1 = tex2D (_MainTex2, IN.uv2_MainTex2);
            fixed4 l2 = tex2D (_MainTex3, IN.uv3_MainTex3);

            fixed4 c = (b * b.a) + (l1 * (1.0f - b.a));            

            c = (l2 * l1.a) + (c * (1.0f - l1.a));

            c.a = l2.a;

            c *= IN.vertex_color;

            o.Albedo = c.rgb;
            // Metallic and smoothness come from slider variables
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
