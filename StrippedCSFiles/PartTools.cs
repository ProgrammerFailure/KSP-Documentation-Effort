using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[AddComponentMenu("KSP/Part Tools")]
public class PartTools : MonoBehaviour
{
	[Serializable]
	public class ModelPartEvent
	{
		public string eventStart;

		public string code;

		public string eventFinish;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public ModelPartEvent()
		{
			throw null;
		}
	}

	public enum TextureFormat
	{
		TGA_Compressed,
		TGA_Uncompressed,
		TGA_Smallest,
		MBM,
		PNG,
		Smallest
	}

	public string modelName;

	public string filePath;

	public string filename;

	public string fileExt;

	public bool copyTexturesToOutputDirectory;

	public bool autoRenameTextures;

	public bool convertTextures;

	public bool production;

	public TextureFormat textureFormat;

	public Shader shaderUnlit;

	public Shader shaderDiffuse;

	public Shader shaderSpecular;

	public Shader shaderBumped;

	public Shader shaderBumpedSpecular;

	public Shader shaderEmissive;

	public Shader shaderEmissiveSpecular;

	public Shader shaderEmissiveBumpedSpecular;

	public Shader shaderCutout;

	public Shader shaderCutoutBumped;

	public Shader shaderAlpha;

	public Shader shaderAlphaSpecular;

	public Shader shaderUnlitTransparent;

	public int eventSelected;

	public List<ModelPartEvent> events;

	public Material material;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public PartTools()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Reset()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetDefaultShaders()
	{
		throw null;
	}
}
