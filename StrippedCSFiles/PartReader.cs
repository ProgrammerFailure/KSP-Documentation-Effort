using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine;

internal static class PartReader
{
	public enum ShaderPropertyType
	{
		Color,
		Vector,
		Float,
		Range,
		TexEnv
	}

	private class MaterialDummy
	{
		public List<Renderer> renderers;

		public List<KSPParticleEmitter> particleEmitters;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public MaterialDummy()
		{
			throw null;
		}
	}

	private class BonesDummy
	{
		public SkinnedMeshRenderer smr;

		public List<string> bones;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public BonesDummy()
		{
			throw null;
		}
	}

	private class TextureMaterialDummy
	{
		public Material material;

		public List<string> shaderName;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public TextureMaterialDummy(Material material)
		{
			throw null;
		}
	}

	private class TextureDummy : List<TextureMaterialDummy>
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		public TextureDummy()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public bool Contains(Material material)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public TextureMaterialDummy Get(Material material)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void AddMaterialDummy(Material material, string shaderName)
		{
			throw null;
		}
	}

	private class TextureDummyList : List<TextureDummy>
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		public TextureDummyList()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void AddTextureDummy(int textureID, Material material, string shaderName)
		{
			throw null;
		}
	}

	private static int fileVersion;

	private static UrlDir.UrlFile file;

	private static List<MaterialDummy> matDummies;

	private static List<BonesDummy> boneDummies;

	private static TextureDummyList textureDummies;

	private static Shader shaderUnlit;

	private static Shader shaderDiffuse;

	private static Shader shaderSpecular;

	public static bool shaderFallback;

	[MethodImpl(MethodImplOptions.NoInlining)]
	static PartReader()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static GameObject Read(UrlDir.UrlFile file)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static GameObject ReadChild(BinaryReader br, Transform parent)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void ReadTextures(BinaryReader br, GameObject o)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Texture2D NormalMapToUnityNormalMap(Texture2D tex)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void ReadMeshRenderer(BinaryReader br, GameObject o)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void ReadSkinnedMeshRenderer(BinaryReader br, GameObject o)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static Mesh ReadMesh(BinaryReader br)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static Material ReadMaterial(BinaryReader br)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static Material ReadMaterial4(BinaryReader br)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void ReadMaterialTexture(BinaryReader br, Material mat, string textureName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void ReadAnimation(BinaryReader br, GameObject o)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void ReadAnimationEvents(BinaryReader br, GameObject o)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void ReadLight(BinaryReader br, GameObject o)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void ReadTagAndLayer(BinaryReader br, GameObject o)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void ReadCamera(BinaryReader br, GameObject o)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void ReadParticles(BinaryReader br, GameObject o)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Transform FindChildByName(Transform parent, string name)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static Color ReadColor(BinaryReader br)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static Vector4 ReadVector2(BinaryReader br)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static Vector4 ReadVector3(BinaryReader br)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static Vector4 ReadVector4(BinaryReader br)
	{
		throw null;
	}
}
