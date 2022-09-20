//=========================================================================================================================
// Optional
//=========================================================================================================================
HEADER
{
	CompileTargets = ( IS_SM_50 && ( PC || VULKAN ) );
	Description = "(Made by Nick from Landis Games) A better glow effect!";
	DevShader = false;
}

//=========================================================================================================================
// Optional
//=========================================================================================================================
FEATURES
{
    #include "common/features.hlsl"
}

//=========================================================================================================================
// Optional
//=========================================================================================================================
MODES
{
	Default();
    VrForward();													// Indicates this shader will be used for main rendering
    ToolsVis( S_MODE_TOOLS_VIS ); 									// Ability to see in the editor
    ToolsWireframe( "vr_tools_wireframe.vfx" ); 					// Allows for mat_wireframe to work
	ToolsShadingComplexity( "vr_tools_shading_complexity.vfx" ); 	// Shows how expensive drawing is in debug view
}

//=========================================================================================================================
COMMON
{
	#define USE_CUSTOM_SHADING 1
	#define STENCIL_ALREADY_SET
	#define DEPTH_STATE_ALREADY_SET
	#include "common/shared.hlsl"
	#include "postprocess/shared.hlsl"
}

//=========================================================================================================================

struct VertexInput
{
	#include "common/vertexinput.hlsl"
};

//=========================================================================================================================


struct PixelInput
{
    #include "common/pixelinput.hlsl"
};

//=========================================================================================================================

VS
{
	#include "common/vertex.hlsl"

	// Main
	//
	PixelInput MainVs( INSTANCED_SHADER_PARAMS( VS_INPUT i ) )
	{
		PixelInput o = ProcessVertex( i );
		return FinalizeVertex( o );
	}
}

//
// Used for outline
//
GS
{
	#include "common/vertex.hlsl"

	void PositionOffset (inout PixelInput input, float2 vOffsetDir, float flOutlineSize)
	{
		float2 vAspectRatio = normalize(g_vInvViewportSize);
		input.vPositionPs.xy += (vOffsetDir * 1.0) * vAspectRatio * input.vPositionPs.w * flOutlineSize;
	}
	
	//
	// Use this one if you want absolute pixel size
	//
	void PositionOffsetResolutionDependent(inout PixelInput input, float2 vOffsetDir, float flOutlineSize)
	{
		input.vPositionPs.xy += (vOffsetDir * 2.0) * g_vInvViewportSize * input.vPositionPs.w * flOutlineSize;
	}
	
    //
    // Main
    //
    [maxvertexcount(3*10)]
    void MainGs(triangle in PixelInput vertices[3], inout TriangleStream<PixelInput> triStream)
    {
		const float flOutlineSize = 0.0045f;
        const float fTwoPi = 6.28318f;
		const uint nNumIterations = 6; 

        PixelInput v[3];

        [unroll]
        for( float i = 0; i <= nNumIterations; i += 1 )
		{
			float fCycle = i / nNumIterations;

			float2 vOffset = float2( 
				( sin( fCycle * fTwoPi ) ),
				( cos( fCycle * fTwoPi ) )
			);

			for ( int i = 0; i < 3; i++ )
			{
				v[i] = vertices[i];
				PositionOffset( v[i], vOffset, flOutlineSize );


				// Todo: I will make this use a stencil mask instead of
				// positioning it backwards when moving glow logic to C# - Sam
				
				v[i].vPositionPs.z += 0.5f;


			}

			triStream.Append(v[2]);
			triStream.Append(v[0]);
			triStream.Append(v[1]);
		}
		
		// emit the vertices
		triStream.RestartStrip();
    }
}


//=========================================================================================================================

PS
{

	
	#define MSAA_NO_SAMPLER
	#include "postprocess/common.hlsl"
	#include "common/msaa.hlsl"
    #include "common/pixel.hlsl"
	
	#include "common/proceedural.hlsl"

    RenderState( DepthWriteEnable, true );
    RenderState( DepthEnable, true );
	RenderState( DepthFunc, LESS_EQUAL );

	CreateTexture2D( g_tDepthBuffer ) < Attribute( "DepthBuffer" ); 	SrgbRead( false ); Filter( MIN_MAG_MIP_POINT ); AddressU( CLAMP ); AddressV( CLAMP ); >;
	float flScale< Attribute("ResolutionScale"); Default(1.0f); >;
	
	//
	// Main
	//
	PixelOutput MainPs( PixelInput i )
	{
		
		PixelOutput o;

		// Get the current screen texture coordinates
        	float2 vScreenUv = CalculateViewportUvFromInvSize( i.vPositionSs.xy, -1.0f / g_vRenderTargetSize );
		vScreenUv *= flScale;
		// Depth Sample

		o.vColor = float4(i.vVertexColor.rgb, 1.0f);


		return o;
	}
}
