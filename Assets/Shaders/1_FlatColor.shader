// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Tutorial/1_FlatColor"{
	Properties{
		_Color ("Color", Color) = (0.3,0.6,0.4,1.0)

	}
	Subshader{
		Pass{
			CGPROGRAM	

			//pragmas
			#pragma vertex vert
			#pragma fragment frag

			//user defined variables
			float4 _Color;

			//base input structs
			struct vertexInput{
				float4 vertex : POSITION;
			};

			struct vertexOutput{
				float4 pos : SV_POSITION;
			};
			//vertex function
			vertexOutput vert(vertexInput v){
				vertexOutput o;
				o.pos = UnityObjectToClipPos(v.vertex);

				return o;
			}
			//fragment function
			float4 frag(vertexOutput i) : COLOR {
				return _Color;
			}

			ENDCG
		}
	}
	//fallback if shader fails
	Fallback "Diffuse"
}