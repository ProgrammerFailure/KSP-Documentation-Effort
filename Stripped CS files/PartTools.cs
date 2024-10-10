using System;
using System.Collections.Generic;
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

		public ModelPartEvent()
		{
			eventStart = "";
			code = "";
			eventFinish = "";
		}
	}

	public enum TextureFormat
	{
		TGA_Compressed,
		TGA_Uncompressed,
		TGA_Smallest,
		const_3,
		const_4,
		Smallest
	}

	public string modelName;

	public string filePath;

	public string filename;

	public string fileExt;

	public bool copyTexturesToOutputDirectory;

	public bool autoRenameTextures;

	public bool convertTextures;

	public bool production = true;

	public TextureFormat textureFormat = TextureFormat.TGA_Smallest;

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

	public void Reset()
	{
		modelName = "NewModel";
		filePath = "Parts/NewPart/";
		filename = "model";
		fileExt = ".mu";
		copyTexturesToOutputDirectory = true;
		autoRenameTextures = true;
		events = new List<ModelPartEvent>();
		SetDefaultShaders();
	}

	public void SetDefaultShaders()
	{
		shaderUnlit = Shader.Find("KSP/Unlit");
		shaderDiffuse = Shader.Find("KSP/Diffuse");
		shaderSpecular = Shader.Find("KSP/Specular");
		shaderBumped = Shader.Find("KSP/Bumped");
		shaderBumpedSpecular = Shader.Find("KSP/Bumped Specular");
		shaderEmissive = Shader.Find("KSP/Emissive/Diffuse");
		shaderEmissiveSpecular = Shader.Find("KSP/Emissive/Specular");
		shaderEmissiveBumpedSpecular = Shader.Find("KSP/Emissive/Bumped Specular");
		shaderCutout = Shader.Find("KSP/Alpha/Cutoff");
		shaderCutoutBumped = Shader.Find("KSP/Alpha/Cutoff Bumped");
		shaderAlpha = Shader.Find("KSP/Alpha/Translucent");
		shaderAlphaSpecular = Shader.Find("KSP/Alpha/Translucent Specular");
		shaderUnlitTransparent = Shader.Find("KSP/Alpha/Unlit Transparent");
	}
}
