Shader "VacuumShaders/Curved World/Particles/Multiply" 
{
	Properties 
	{
		[CurvedWorldGearMenu] V_CW_Label_Tag("", float) = 0

		_MainTex("Particle Texture", 2D) = "white" {}
	    _InvFade("Soft Particles Factor", Range(0.01,3.0)) = 1.0

		//CurvedWorld Options
		[CurvedWorldRangeFade] V_CW_RangeFadeDrawer("Range Fade", float) = 0
	}

	Category 
	{
		Tags { "Queue"="Transparent" 
			   "IgnoreProjector"="True" 
			   "RenderType"="Transparent" 
			   "PreviewType" = "Plane"
		       "CurvedWorldTag"="Particles/Multiply" 
			   "CurvedWorldNoneRemoveableKeywords"="" 
			   "CurvedWorldAvailableOptions"="V_CW_RANGE_FADE;" 
			 } 
			Blend Zero SrcColor
			Cull Off Lighting Off ZWrite Off
	
		SubShader 
		{
        Pass {

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0
            #pragma multi_compile_particles
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

			#pragma shader_feature _ V_CW_RANGE_FADE


			#include "../cginc/CurvedWorld_Base.cginc"
			#include "../cginc/CurvedWorld_RangeFade.cginc"


            sampler2D _MainTex;
            fixed4 _TintColor;

            struct appdata_t {
                float4 vertex : POSITION;
                fixed4 color : COLOR;
                float2 texcoord : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f {
                float4 vertex : SV_POSITION;
                fixed4 color : COLOR;
                float2 texcoord : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                #ifdef SOFTPARTICLES_ON
                float4 projPos : TEXCOORD2;
                #endif
                UNITY_VERTEX_OUTPUT_STEREO

				//Curved World Distance Fade
				CURVEDWORLD_RANGE_FADE_COORDINATE(4)
            };

            float4 _MainTex_ST;

            v2f vert (appdata_t v)
            {
                v2f o;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);


				//Curved World Distance Fade
				CURVEDWORLD_RANGE_FADE_SETUP(o, v.vertex)

				V_CW_TransformPoint(v.vertex);


                o.vertex = UnityObjectToClipPos(v.vertex);
                #ifdef SOFTPARTICLES_ON
                o.projPos = ComputeScreenPos (o.vertex);
                COMPUTE_EYEDEPTH(o.projPos.z);
                #endif
                o.color = v.color;
                o.texcoord = TRANSFORM_TEX(v.texcoord,_MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            UNITY_DECLARE_DEPTH_TEXTURE(_CameraDepthTexture);
            float _InvFade;

            fixed4 frag (v2f i) : SV_Target
            {
				CURVEDWORLD_RANGE_FADE_CALCULATE(i) 


                #ifdef SOFTPARTICLES_ON
                float sceneZ = LinearEyeDepth (SAMPLE_DEPTH_TEXTURE_PROJ(_CameraDepthTexture, UNITY_PROJ_COORD(i.projPos)));
                float partZ = i.projPos.z;
                float fade = saturate (_InvFade * (sceneZ-partZ));
                i.color.a *= fade;
                #endif

                half4 prev = i.color * tex2D(_MainTex, i.texcoord);
                fixed4 col = lerp(half4(1,1,1,1), prev, prev.a);
                UNITY_APPLY_FOG_COLOR(i.fogCoord, col, fixed4(1,1,1,1)); // fog towards white due to our blend mode


                col = lerp(col, 1, CURVEDWORLD_RANGE_FADE_VALUE);

                return col;
            }
            ENDCG
        }
		}
	}
}
