float4x4 World;
float4x4 View;
float4x4 Projection;

// TODO: add effect parameters here.

struct VertexShaderInput
{
    float4 Position : POSITION0;
	float2 TexCoord : TEXCOORD0;

    // TODO: add input channels such as texture
    // coordinates and vertex colors here.
};

struct VertexShaderOutput
{
    float4 Position : POSITION0;

	float2 TexCoord : TEXCOORD0;
	float3 Pworld : TEXCOORD1;
	float3 Pscreen : TEXCOORD2;
};

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
    VertexShaderOutput output;

    float4 worldPosition = mul(input.Position, World);
    float4 viewPosition = mul(worldPosition, View);
    output.Position = mul(viewPosition, Projection);

    output.TexCoord = input.TexCoord;
	output.Pworld = worldPosition;
	output.Pscreen = output.Position.xyz;

    return output;
}

float4 Pass1PixelShaderFunction(VertexShaderOutput input) : COLOR0
{

	float depth = input.Pscreen.z;

	float fogStart = 140.0f;
	float fogEnd = 8000.0f;

	float3 fogColor = 0.5f ;

	float l = saturate((depth - fogStart) / (fogEnd - fogStart));

	float noise = input.Pscreen.x + input.Pscreen.y*100;

	float3 offset = float3(cos(noise),cos(noise + 2.1),cos(noise + 4.2));
    float4 outColor = float4(lerp(0.0f,0.5f,l*l).xxx + offset * 0.00390625f, 1);

	return outColor;
}

float4 Pass2PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
	float depth = input.Pscreen.z;

	float fogStart = 140.0f;
	float fogEnd = 8000.0f;

	float3 fogColor = 0.5f ;

	float l = saturate((depth - fogStart) / (fogEnd - fogStart));

    float x = 2*(input.TexCoord.x - 0.5f);
	float y = 2*(input.TexCoord.y - 0.5f);

	float r = sqrt(x*x + y*y);
	float i = clamp(1-r,0,1);
	
	float4 outColor = float4(lerp(i*i,0.5f,l).xxx,0.5f*i*i);
	return outColor;
}

technique Houses
{
    pass Pass1
    {
		ZWriteEnable = true;
		AlphaBlendEnable = false;

        VertexShader = compile vs_3_0 VertexShaderFunction();
        PixelShader = compile ps_3_0 Pass1PixelShaderFunction();
    }
}

technique Lights
{
	

    pass Pass1
    {
		ZWriteEnable = false; 
		AlphaBlendEnable = TRUE;
		SrcBlend = SrcAlpha;
		DestBlend = InvSrcAlpha;

        VertexShader = compile vs_3_0 VertexShaderFunction();
        PixelShader = compile ps_3_0 Pass2PixelShaderFunction();
    }
}