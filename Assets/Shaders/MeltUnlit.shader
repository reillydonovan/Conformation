Shader "Unlit/MeltUnlit"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
	_SpeedX("SpeedX", Float) = 0.0
	_AmountX("AmountX", Float) = 0.0
	_DistanceX("DistanceX", Float) = 0.0

		_SpeedY("SpeedY", Float) = 0.0
		_AmountY("AmountY", Float) = 0.0
		_DistanceY("DistanceY", Float) = 0.0

		_SpeedZ("SpeedZ", Float) = 0.0
		_AmountZ("AmountZ", Float) = 0.0
		_DistanceZ("DistanceZ", Float) = 0.0
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"


			
			float _SpeedX = 1;
			float _AmountX = 5;
			float _DistanceX = 0.1;

			float _SpeedY = 1;
			float _AmountY = 5;
			float _DistanceY = 0.1;

			float _SpeedZ = 1;
			float _AmountZ = 5;
			float _DistanceZ = 0.1;

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			
			v2f vert (appdata v)
			{
				v2f o;
				v.vertex.x += sin(_Time.y * _SpeedX + v.vertex.y * _AmountX) * _DistanceX;
				//v.vertex.y += sin(_Time.z * _Speed + v.vertex.z * _Amount) * _Distance;
				v.vertex.y += sin(_Time.y * _SpeedY + v.vertex.y * _AmountY) * _DistanceY;
				v.vertex.z += sin(_Time.z * _SpeedZ + v.vertex.z * _AmountZ) * _DistanceZ;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv);
				// apply fog
				UNITY_APPLY_FOG(i.fogCoord, col);
				return col;
			}
			ENDCG
		}
	}
}
