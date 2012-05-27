
struct VertexShaderInput
{
    float4 Position : POSITION0;

	float2 TexCoords : TEXCOORD0;

};

struct VertexShaderOutput
{
    float4 Position : POSITION0;

	float2 ScreenCoords : TEXCOORD0;

};

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
    VertexShaderOutput output;

    output.Position = input.Position;
	output.Position.z = 1;
	
	output.ScreenCoords = input.Position.xy;

    return output;
}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
    // TODO: add your pixel shader code here.

	float y = (1 -input.ScreenCoords.y) * 0.4 + 0.3;
    return float4(y, y, y, 1);
}

technique BigSky
{
    pass Pass1
    {
        // TODO: set renderstates here.

        VertexShader = compile vs_2_0 VertexShaderFunction();
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}
