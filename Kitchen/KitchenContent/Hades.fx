float4x4 World;
float4x4 View;
float4x4 Projection;

// TODO: add effect parameters here.

struct VertexShaderInput
{
    float4 Position : POSITION0;

    // TODO: add input channels such as texture
    // coordinates and vertex colors here.
};

struct VertexShaderOutput
{
    float4 Position : POSITION0;

	float3 Pscreen : TEXCOORD0;
	float3 Pworld : TEXCOORD1;

    // TODO: add vertex shader outputs such as colors and texture
    // coordinates here. These values will automatically be interpolated
    // over the triangle, and provided as input to your pixel shader.
};

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
    VertexShaderOutput output;

    float4 worldPosition = mul(input.Position, World);
    float4 viewPosition = mul(worldPosition, View);
    output.Position = mul(viewPosition, Projection);

	output.Pworld = worldPosition;
	output.Pscreen = output.Position.xyz;

    // TODO: add your vertex shader code here.

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
	
	float4 outColor = float4((1.0f).xxx,1);
	return outColor;
}

technique Hades1
{
    pass Pass1
    {

        VertexShader = compile vs_2_0 VertexShaderFunction();
        PixelShader = compile ps_2_0 Pass1PixelShaderFunction();
    }

    pass Pass2
    {

        VertexShader = compile vs_2_0 VertexShaderFunction();
        PixelShader = compile ps_2_0 Pass2PixelShaderFunction();
    }

}
