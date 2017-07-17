 Shader "Custom/Bubbling" {
     Properties {

         _Color ("Color", Color) = (1,1,1,1)
         _MainTex ("Albedo (RGB)", 2D) = "white" {}
         _Glossiness ("Smoothness", Range(0,1)) = 0.5
         _Metallic ("Metallic", Range(0,1)) = 0.0
         _Amount ("Amount", Range(-100,100)) = 1.0
         _Value1 ("Value1", Range(-100,100)) = 1.0
         _Value2 ("Value2", Range(-100,100)) = 1.0
         _Value3 ("Value3", Range(-100,100)) = 1.0
     }
     SubShader {
         Tags { "RenderType"="Opaque" }
         LOD 200
        			Cull Off

         CGPROGRAM
         // Physically based Standard lighting model, and enable shadows on all light types
        // #pragma surface surf Standard fullforwardshadows
 		#pragma surface surf Standard vertex:vert fullforwardshadows
         // Use shader model 3.0 target, to get nicer looking lighting
         #pragma target 3.0
 
         sampler2D _MainTex;
 
         struct Input {
             float2 uv_MainTex;
         };
 
         half _Glossiness;
         half _Metallic;
         fixed4 _Color;
         float _Amount;
         float _Value1;
         float _Value2;
         float _Value3;
 
         void surf (Input IN, inout SurfaceOutputStandard o) {
             // Albedo comes from a texture tinted by color
             fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
             o.Albedo = c.rgb;
             // Metallic and smoothness come from slider variables
             o.Metallic = _Metallic;
             o.Smoothness = _Glossiness;
             o.Alpha = c.a;
         }
        
         void vert (inout appdata_full v) {
           //  v.vertex.xz += v.normal.xz * _Amount * abs(sin(_Time * 10)) * v.color.x;
           //  v.vertex.y += _Amount * abs(sin(_Time * 200)) * v.color.y;

           //Fat Mesh
         //  v.vertex.xyz += v.normal * _Value1;

           //Wave Mesh
        //   v.vertex.x += sin((v.vertex.y + _Time * _Value3 ) * _Value2 ) * _Value1;
     //   v.vertex.x += sin((v.vertex.x + _Time * _Value3 ) * _Value2 ) * _Value1;
    //     v.vertex.y += sin((v.vertex.y + _Time * _Value3 ) * _Value2 ) * _Value1;
    //     v.vertex.z += sin((v.vertex.z + _Time * _Value3 ) * _Value2 ) * _Value1;

       //Bubbling Mesh
       v.vertex.xyz += v.normal * ( sin((v.vertex.x + _Time * _Value3) * _Value2) + cos((v.vertex.z + _Time* _Value3) * _Value2)) * _Value1;

         }
 
         ENDCG
     }
     FallBack "Diffuse"
     }