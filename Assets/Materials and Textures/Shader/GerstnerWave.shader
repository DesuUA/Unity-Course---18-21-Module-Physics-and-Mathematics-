Shader "Custom/URP_GerstnerWaves_Complex"
{
    Properties
    {
        _BaseColor("Water Color", Color) = (0.1, 0.4, 0.6, 1)
        
        [Space(10)]
        [Header(Foam Settings)]
        _FoamColor("Foam Color", Color) = (0.9, 0.95, 1.0, 1)
        _FoamThreshold("Foam Width", Range(0.0, 2.0)) = 0.8
        _FoamIntensity("Foam Intensity", Range(0.0, 5.0)) = 2.0

        [Space(20)]
        [Header(Wave A Main Swell)]
        [Header((The main volume and global wind direction.))]
        [Space(10)]
        _WaveA_Angle("Global Wind Direction (Degrees)", Range(0, 360)) = 0.0
        _WaveA_Steepness("Steepness", Range(0, 1)) = 0.5
        _WaveA_Amp("Amplitude", Float) = 1.0
        _WaveA_Wavelength("Wavelength", Float) = 20.0
        _WaveA_Speed("Speed", Float) = 2.0

        [Space(20)]
        [Header(Wave B Medium Details)]
        [Header((Angle offset creates a cross swell pattern.))]
        [Space(10)]
        _WaveB_AngleOffset("Angle Offset from Wave A", Range(-180, 180)) = 30.0
        _WaveB_Steepness("Steepness", Range(0, 1)) = 0.4
        _WaveB_Amp("Amplitude", Float) = 0.5
        _WaveB_Wavelength("Wavelength", Float) = 10.0
        _WaveB_Speed("Speed", Float) = 3.0

        [Space(20)]
        [Header(Wave C Small Ripples)]
        [Header(Negative offset breaks up perfect crests.)]
        [Space(10)]
        _WaveC_AngleOffset("Angle Offset from Wave A", Range(-180, 180)) = -45.0
        _WaveC_Steepness("Steepness", Range(0, 1)) = 0.2
        _WaveC_Amp("Amplitude", Float) = 0.2
        _WaveC_Wavelength("Wavelength", Float) = 4.0
        _WaveC_Speed("Speed", Float) = 4.0
    }
    SubShader
    {
        Tags
        {
            "RenderType" = "Opaque"
            "RenderPipeline" = "UniversalPipeline"
            "Queue" = "Geometry"
        }

        Pass
        {
            Name "ForwardLit"
            Tags { "LightMode" = "UniversalForward" }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS   : NORMAL;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float3 positionWS : TEXCOORD0;
                float3 normalWS   : NORMAL;
                float foam        : TEXCOORD1; 
            };

            // Согласование буфера с новыми Property
            CBUFFER_START(UnityPerMaterial)
                half4 _BaseColor;
                half4 _FoamColor;
                float _FoamThreshold;
                float _FoamIntensity;

                float _WaveA_Angle, _WaveA_Steepness, _WaveA_Amp, _WaveA_Wavelength, _WaveA_Speed;
                float _WaveB_AngleOffset, _WaveB_Steepness, _WaveB_Amp, _WaveB_Wavelength, _WaveB_Speed;
                float _WaveC_AngleOffset, _WaveC_Steepness, _WaveC_Amp, _WaveC_Wavelength, _WaveC_Speed;
            CBUFFER_END

            // Функция теперь принимает вычисленный float2 вместо вектора
            float3 CalculateGerstner(float2 dir, float steepness, float amp, float wavelength, float speed, float3 originalPos, inout float3 tangent, inout float3 binormal)
            {
                float W = TWO_PI / wavelength;
                float phase = speed * W * _Time.y;
                float f = W * dot(dir, originalPos.xz) + phase;

                float Q = steepness / (W * amp);
                float WA = W * amp;
                float S = sin(f);
                float C = cos(f);

                tangent += float3(
                    -Q * dir.x * dir.x * WA * S,
                    dir.x * WA * C,
                    -Q * dir.x * dir.y * WA * S
                );

                binormal += float3(
                    -Q * dir.x * dir.y * WA * S,
                    dir.y * WA * C,
                    -Q * dir.y * dir.y * WA * S
                );

                return float3(
                    Q * amp * dir.x * C,
                    amp * S,
                    Q * amp * dir.y * C
                );
            }

            Varyings vert(Attributes input)
            {
                Varyings output;

                float3 gridPoint = input.positionOS.xyz;
                float3 p = gridPoint;

                float3 tangent = float3(1, 0, 0);
                float3 binormal = float3(0, 0, 1);

                // Вычисление радиан из градусов: Deg * (PI / 180)
                float radA = _WaveA_Angle * (PI / 180.0);
                float radB = (_WaveA_Angle + _WaveB_AngleOffset) * (PI / 180.0);
                float radC = (_WaveA_Angle + _WaveC_AngleOffset) * (PI / 180.0);

                // Формирование двумерных нормализованных векторов направления на основе углов
                float2 dirA = float2(cos(radA), sin(radA));
                float2 dirB = float2(cos(radB), sin(radB));
                float2 dirC = float2(cos(radC), sin(radC));

                // Расчет суперпозиции волн с вычисленными векторами
                p += CalculateGerstner(dirA, _WaveA_Steepness, _WaveA_Amp, _WaveA_Wavelength, _WaveA_Speed, gridPoint, tangent, binormal);
                p += CalculateGerstner(dirB, _WaveB_Steepness, _WaveB_Amp, _WaveB_Wavelength, _WaveB_Speed, gridPoint, tangent, binormal);
                p += CalculateGerstner(dirC, _WaveC_Steepness, _WaveC_Amp, _WaveC_Wavelength, _WaveC_Speed, gridPoint, tangent, binormal);

                float3 localNormal = normalize(cross(binormal, tangent));

                // Расчет Детерминанта Якоби для пены
                float J = tangent.x * binormal.z - tangent.z * binormal.x;
                float foamCoverage = saturate(1.0 - J);
                output.foam = saturate((foamCoverage - _FoamThreshold) * _FoamIntensity);

                output.positionWS = TransformObjectToWorld(p);
                output.positionCS = TransformWorldToHClip(output.positionWS);
                output.normalWS = TransformObjectToWorldNormal(localNormal);

                return output;
            }

            half4 frag(Varyings input) : SV_Target
            {
                Light mainLight = GetMainLight();
                half3 lightDir = normalize(mainLight.direction);
                half3 normal = normalize(input.normalWS);
                
                half NdotL = saturate(dot(normal, lightDir));
                half3 diffuse = mainLight.color * NdotL;
                
                half3 viewDir = normalize(GetCameraPositionWS() - input.positionWS);
                half3 reflectDir = reflect(-lightDir, normal);
                float spec = pow(max(dot(viewDir, reflectDir), 0.0), 32.0);
                half3 specular = mainLight.color * spec * 0.5;

                half3 ambient = half3(0.05, 0.1, 0.15);

                half3 waterColor = _BaseColor.rgb * (diffuse + ambient) + specular;
                half3 finalColor = lerp(waterColor, _FoamColor.rgb, input.foam);
                
                return half4(finalColor, 1.0);
            }
            ENDHLSL
        }
    }
    FallBack "Hidden/Universal Render Pipeline/FallbackError"
}