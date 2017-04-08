// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Shader Forge/NewShader" {
    Properties {
        _scale ("scale", Range(50, 5000)) = 50
        _col ("col", Color) = (0.5,0.5,0.5,1)
        _brightness ("brightness", Range(0, 10)) = 0
    }
    SubShader {
        Tags {
            "RenderType"="Opaque"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            uniform float4 _TimeEditor;
            uniform float _scale;
            uniform float4 _col;
            uniform float _brightness;
            struct VertexInput {
                float4 vertex : POSITION;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float4 screenPos : TEXCOORD0;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.pos = UnityObjectToClipPos(v.vertex );
                o.screenPos = o.pos;
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.screenPos = float4( i.screenPos.xy / i.screenPos.w, 0, 0 );
                i.screenPos.y *= _ProjectionParams.x;
////// Lighting:
////// Emissive:
                float node_9202 = (_scale*5.0);
                float2 node_9692_tc_rcp = float2(1.0,1.0)/float2( node_9202, node_9202 );
                float node_9692_ty = floor(node_9202 * node_9692_tc_rcp.x);
                float node_9692_tx = node_9202 - node_9202 * node_9692_ty;
                float4 node_6396 = _Time + _TimeEditor;
                float node_716_ang = node_6396.g;
                float node_716_spd = 1.0;
                float node_716_cos = cos(node_716_spd*node_716_ang);
                float node_716_sin = sin(node_716_spd*node_716_ang);
                float2 node_716_piv = float2(0.5,0.5);
                float2 node_716 = (mul(i.screenPos.rg-node_716_piv,float2x2( node_716_cos, -node_716_sin, node_716_sin, node_716_cos))+node_716_piv);
                float2 node_9692 = (node_716 + float2(node_9692_tx, node_9692_ty)) * node_9692_tc_rcp;
                float2 node_4091_skew = node_9692 + 0.2127+node_9692.x*0.3713*node_9692.y;
                float2 node_4091_rnd = 4.789*sin(489.123*(node_4091_skew));
                float node_4091 = frac(node_4091_rnd.x*node_4091_rnd.y*(1+node_4091_skew.x));
                float2 node_3304_tc_rcp = float2(1.0,1.0)/float2( _scale, _scale );
                float node_3304_ty = floor(_scale * node_3304_tc_rcp.x);
                float node_3304_tx = _scale - _scale * node_3304_ty;
                float2 node_3304 = (node_716 + float2(node_3304_tx, node_3304_ty)) * node_3304_tc_rcp;
                float2 node_7855_skew = node_3304 + 0.2127+node_3304.x*0.3713*node_3304.y;
                float2 node_7855_rnd = 4.789*sin(489.123*(node_7855_skew));
                float node_7855 = frac(node_7855_rnd.x*node_7855_rnd.y*(1+node_7855_skew.x));
                float4 node_6889 = _Time + _TimeEditor;
                float3 node_2702 = ((float3(float2(node_4091,node_7855),node_7855)*saturate(3.0*abs(1.0-2.0*frac(sin(node_7855)+float3(0.0,-1.0/3.0,1.0/3.0)))-1)*node_6889.r)*_col.rgb);
                float3 emissive = clamp(node_2702,0.0,_brightness);
                float3 finalColor = emissive;
                return fixed4(finalColor,1);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}