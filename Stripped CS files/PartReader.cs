using System;
using System.Collections.Generic;
using System.IO;
using PartToolsLib;
using UnityEngine;
using UnityEngine.Rendering;

public static class PartReader
{
	public enum ShaderPropertyType
	{
		Color,
		Vector,
		Float,
		Range,
		TexEnv
	}

	public class MaterialDummy
	{
		public List<Renderer> renderers;

		public List<KSPParticleEmitter> particleEmitters;

		public MaterialDummy()
		{
			renderers = new List<Renderer>();
			particleEmitters = new List<KSPParticleEmitter>();
		}
	}

	public class BonesDummy
	{
		public SkinnedMeshRenderer smr;

		public List<string> bones;

		public BonesDummy()
		{
			bones = new List<string>();
		}
	}

	public class TextureMaterialDummy
	{
		public Material material;

		public List<string> shaderName;

		public TextureMaterialDummy(Material material)
		{
			this.material = material;
			shaderName = new List<string>();
		}
	}

	public class TextureDummy : List<TextureMaterialDummy>
	{
		public bool Contains(Material material)
		{
			int count = base.Count;
			do
			{
				if (count-- <= 0)
				{
					return false;
				}
			}
			while (!(base[count].material == material));
			return true;
		}

		public TextureMaterialDummy Get(Material material)
		{
			int num = 0;
			int count = base.Count;
			while (true)
			{
				if (num < count)
				{
					if (base[num].material == material)
					{
						break;
					}
					num++;
					continue;
				}
				return null;
			}
			return base[num];
		}

		public void AddMaterialDummy(Material material, string shaderName)
		{
			TextureMaterialDummy textureMaterialDummy = Get(material);
			if (textureMaterialDummy == null)
			{
				Add(textureMaterialDummy = new TextureMaterialDummy(material));
			}
			if (!textureMaterialDummy.shaderName.Contains(shaderName))
			{
				textureMaterialDummy.shaderName.Add(shaderName);
			}
		}
	}

	public class TextureDummyList : List<TextureDummy>
	{
		public void AddTextureDummy(int textureID, Material material, string shaderName)
		{
			if (textureID != -1)
			{
				while (textureID >= base.Count)
				{
					Add(new TextureDummy());
				}
				base[textureID].AddMaterialDummy(material, shaderName);
			}
		}
	}

	public static int fileVersion;

	public static UrlDir.UrlFile file;

	public static List<MaterialDummy> matDummies;

	public static List<BonesDummy> boneDummies;

	public static TextureDummyList textureDummies;

	public static Shader shaderUnlit;

	public static Shader shaderDiffuse;

	public static Shader shaderSpecular;

	public static bool shaderFallback;

	public static GameObject Read(UrlDir.UrlFile file)
	{
		PartReader.file = file;
		BinaryReader binaryReader = new BinaryReader(File.Open(file.fullPath, FileMode.Open));
		if (binaryReader == null)
		{
			Debug.Log("File error");
			return null;
		}
		matDummies = new List<MaterialDummy>();
		boneDummies = new List<BonesDummy>();
		textureDummies = new TextureDummyList();
		FileType fileType = (FileType)binaryReader.ReadInt32();
		fileVersion = binaryReader.ReadInt32();
		_ = binaryReader.ReadString() + string.Empty;
		if (fileType != FileType.ModelBinary)
		{
			Debug.LogWarning("File '" + file.fullPath + "' is an incorrect type.");
			binaryReader.Close();
			return null;
		}
		GameObject gameObject = null;
		try
		{
			gameObject = ReadChild(binaryReader, null);
			if (boneDummies != null && boneDummies.Count > 0)
			{
				int i = 0;
				for (int count = boneDummies.Count; i < count; i++)
				{
					Transform[] array = new Transform[boneDummies[i].bones.Count];
					int j = 0;
					for (int count2 = boneDummies[i].bones.Count; j < count2; j++)
					{
						array[j] = FindChildByName(gameObject.transform, boneDummies[i].bones[j]);
					}
					boneDummies[i].smr.bones = array;
				}
			}
			if (shaderFallback)
			{
				Renderer[] componentsInChildren = gameObject.GetComponentsInChildren<Renderer>();
				int k = 0;
				for (int num = componentsInChildren.Length; k < num; k++)
				{
					Renderer renderer = componentsInChildren[k];
					int l = 0;
					for (int num2 = renderer.sharedMaterials.Length; l < num2; l++)
					{
						renderer.sharedMaterials[l].shader = Shader.Find("KSP/Diffuse");
					}
				}
			}
		}
		catch (Exception ex)
		{
			Debug.LogError("File error:\n" + ex.Message + "\n" + ex.StackTrace);
		}
		boneDummies = null;
		matDummies = null;
		textureDummies = null;
		binaryReader.Close();
		return gameObject;
	}

	public static GameObject ReadChild(BinaryReader br, Transform parent)
	{
		GameObject gameObject = new GameObject(br.ReadString());
		gameObject.transform.parent = parent;
		gameObject.transform.localPosition = new Vector3(br.ReadSingle(), br.ReadSingle(), br.ReadSingle());
		gameObject.transform.localRotation = new Quaternion(br.ReadSingle(), br.ReadSingle(), br.ReadSingle(), br.ReadSingle());
		gameObject.transform.localScale = new Vector3(br.ReadSingle(), br.ReadSingle(), br.ReadSingle());
		while (br.BaseStream.Position != br.BaseStream.Length)
		{
			switch ((EntryType)br.ReadInt32())
			{
			case EntryType.ChildTransformStart:
				ReadChild(br, gameObject.transform);
				break;
			case EntryType.Animation:
				if (fileVersion >= 3)
				{
					ReadAnimation(br, gameObject);
				}
				else
				{
					ReadAnimation(br, gameObject);
				}
				break;
			case EntryType.MeshCollider:
			{
				MeshCollider meshCollider2 = gameObject.AddComponent<MeshCollider>();
				meshCollider2.convex = br.ReadBoolean();
				if (!meshCollider2.convex)
				{
					meshCollider2.convex = true;
				}
				meshCollider2.sharedMesh = ReadMesh(br);
				break;
			}
			case EntryType.SphereCollider:
			{
				SphereCollider sphereCollider2 = gameObject.AddComponent<SphereCollider>();
				sphereCollider2.radius = br.ReadSingle();
				sphereCollider2.center = new Vector3(br.ReadSingle(), br.ReadSingle(), br.ReadSingle());
				break;
			}
			case EntryType.CapsuleCollider:
			{
				CapsuleCollider capsuleCollider2 = gameObject.AddComponent<CapsuleCollider>();
				capsuleCollider2.radius = br.ReadSingle();
				capsuleCollider2.direction = br.ReadInt32();
				capsuleCollider2.center = new Vector3(br.ReadSingle(), br.ReadSingle(), br.ReadSingle());
				break;
			}
			case EntryType.BoxCollider:
			{
				BoxCollider boxCollider2 = gameObject.AddComponent<BoxCollider>();
				boxCollider2.size = new Vector3(br.ReadSingle(), br.ReadSingle(), br.ReadSingle());
				boxCollider2.center = new Vector3(br.ReadSingle(), br.ReadSingle(), br.ReadSingle());
				break;
			}
			case EntryType.MeshFilter:
				gameObject.AddComponent<MeshFilter>().sharedMesh = ReadMesh(br);
				break;
			case EntryType.MeshRenderer:
				ReadMeshRenderer(br, gameObject);
				break;
			case EntryType.SkinnedMeshRenderer:
				ReadSkinnedMeshRenderer(br, gameObject);
				break;
			case EntryType.Materials:
			{
				int num = br.ReadInt32();
				for (int i = 0; i < num; i++)
				{
					Material material = null;
					MaterialDummy materialDummy = matDummies[i];
					material = ((fileVersion < 4) ? ReadMaterial(br) : ReadMaterial4(br));
					int count = materialDummy.renderers.Count;
					while (count-- > 0)
					{
						materialDummy.renderers[count].sharedMaterial = material;
					}
					int j = 0;
					for (int count2 = materialDummy.particleEmitters.Count; j < count2; j++)
					{
						materialDummy.particleEmitters[j].material = material;
					}
				}
				break;
			}
			case EntryType.Textures:
				ReadTextures(br, gameObject);
				break;
			case EntryType.Light:
				ReadLight(br, gameObject);
				break;
			case EntryType.TagAndLayer:
				ReadTagAndLayer(br, gameObject);
				break;
			case EntryType.MeshCollider2:
			{
				MeshCollider meshCollider = gameObject.AddComponent<MeshCollider>();
				bool isTrigger = br.ReadBoolean();
				meshCollider.convex = br.ReadBoolean();
				meshCollider.isTrigger = isTrigger;
				if (!meshCollider.convex)
				{
					meshCollider.convex = true;
				}
				meshCollider.sharedMesh = ReadMesh(br);
				break;
			}
			case EntryType.SphereCollider2:
			{
				SphereCollider sphereCollider = gameObject.AddComponent<SphereCollider>();
				sphereCollider.isTrigger = br.ReadBoolean();
				sphereCollider.radius = br.ReadSingle();
				sphereCollider.center = new Vector3(br.ReadSingle(), br.ReadSingle(), br.ReadSingle());
				break;
			}
			case EntryType.CapsuleCollider2:
			{
				CapsuleCollider capsuleCollider = gameObject.AddComponent<CapsuleCollider>();
				capsuleCollider.isTrigger = br.ReadBoolean();
				capsuleCollider.radius = br.ReadSingle();
				capsuleCollider.height = br.ReadSingle();
				capsuleCollider.direction = br.ReadInt32();
				capsuleCollider.center = new Vector3(br.ReadSingle(), br.ReadSingle(), br.ReadSingle());
				break;
			}
			case EntryType.BoxCollider2:
			{
				BoxCollider boxCollider = gameObject.AddComponent<BoxCollider>();
				boxCollider.isTrigger = br.ReadBoolean();
				boxCollider.size = new Vector3(br.ReadSingle(), br.ReadSingle(), br.ReadSingle());
				boxCollider.center = new Vector3(br.ReadSingle(), br.ReadSingle(), br.ReadSingle());
				break;
			}
			case EntryType.WheelCollider:
			{
				WheelCollider wheelCollider = gameObject.AddComponent<WheelCollider>();
				wheelCollider.mass = br.ReadSingle();
				wheelCollider.radius = br.ReadSingle();
				wheelCollider.suspensionDistance = br.ReadSingle();
				wheelCollider.center = new Vector3(br.ReadSingle(), br.ReadSingle(), br.ReadSingle());
				wheelCollider.suspensionSpring = new JointSpring
				{
					spring = br.ReadSingle(),
					damper = br.ReadSingle(),
					targetPosition = br.ReadSingle()
				};
				wheelCollider.forwardFriction = new WheelFrictionCurve
				{
					extremumSlip = br.ReadSingle(),
					extremumValue = br.ReadSingle(),
					asymptoteSlip = br.ReadSingle(),
					asymptoteValue = br.ReadSingle(),
					stiffness = br.ReadSingle()
				};
				wheelCollider.sidewaysFriction = new WheelFrictionCurve
				{
					extremumSlip = br.ReadSingle(),
					extremumValue = br.ReadSingle(),
					asymptoteSlip = br.ReadSingle(),
					asymptoteValue = br.ReadSingle(),
					stiffness = br.ReadSingle()
				};
				wheelCollider.enabled = false;
				break;
			}
			case EntryType.Camera:
				ReadCamera(br, gameObject);
				break;
			case EntryType.ParticleEmitter:
				ReadParticles(br, gameObject);
				break;
			case EntryType.ChildTransformEnd:
				return gameObject;
			}
		}
		return gameObject;
	}

	public static void ReadTextures(BinaryReader br, GameObject o)
	{
		int num = br.ReadInt32();
		if (num != textureDummies.Count)
		{
			Debug.LogError("TextureError: " + num + " " + textureDummies.Count);
			return;
		}
		for (int i = 0; i < num; i++)
		{
			string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(br.ReadString());
			TextureType textureType = (TextureType)br.ReadInt32();
			string url = file.parent.url + "/" + fileNameWithoutExtension;
			Texture2D texture = GameDatabase.Instance.GetTexture(url, textureType == TextureType.NormalMap);
			if (texture == null)
			{
				Debug.LogError("Texture '" + file.parent.url + "/" + fileNameWithoutExtension + "' not found!");
				continue;
			}
			int j = 0;
			for (int count = textureDummies[i].Count; j < count; j++)
			{
				TextureMaterialDummy textureMaterialDummy = textureDummies[i][j];
				int k = 0;
				for (int count2 = textureMaterialDummy.shaderName.Count; k < count2; k++)
				{
					string name = textureMaterialDummy.shaderName[k];
					textureMaterialDummy.material.SetTexture(name, texture);
				}
			}
		}
	}

	public static Texture2D NormalMapToUnityNormalMap(Texture2D tex)
	{
		int width = tex.width;
		int height = tex.height;
		Texture2D texture2D = new Texture2D(width, height, TextureFormat.RGBA32, mipChain: true);
		texture2D.wrapMode = TextureWrapMode.Repeat;
		Color color = new Color(1f, 1f, 1f, 1f);
		for (int i = 0; i < width; i++)
		{
			for (int j = 0; j < height; j++)
			{
				Color pixel = tex.GetPixel(i, j);
				color.r = pixel.g;
				color.g = pixel.g;
				color.b = pixel.g;
				color.a = pixel.r;
				texture2D.SetPixel(i, j, color);
			}
		}
		texture2D.Apply(updateMipmaps: true, makeNoLongerReadable: true);
		return texture2D;
	}

	public static void ReadMeshRenderer(BinaryReader br, GameObject o)
	{
		MeshRenderer meshRenderer = o.AddComponent<MeshRenderer>();
		if (fileVersion >= 1)
		{
			if (br.ReadBoolean())
			{
				meshRenderer.shadowCastingMode = ShadowCastingMode.On;
			}
			else
			{
				meshRenderer.shadowCastingMode = ShadowCastingMode.Off;
			}
			meshRenderer.receiveShadows = br.ReadBoolean();
		}
		int num = br.ReadInt32();
		for (int i = 0; i < num; i++)
		{
			int num2 = br.ReadInt32();
			while (num2 >= matDummies.Count)
			{
				matDummies.Add(new MaterialDummy());
			}
			matDummies[num2].renderers.Add(meshRenderer);
		}
	}

	public static void ReadSkinnedMeshRenderer(BinaryReader br, GameObject o)
	{
		SkinnedMeshRenderer skinnedMeshRenderer = o.AddComponent<SkinnedMeshRenderer>();
		int num = br.ReadInt32();
		for (int i = 0; i < num; i++)
		{
			int num2 = br.ReadInt32();
			while (num2 >= matDummies.Count)
			{
				matDummies.Add(new MaterialDummy());
			}
			matDummies[num2].renderers.Add(skinnedMeshRenderer);
		}
		skinnedMeshRenderer.localBounds = new Bounds(new Vector3(br.ReadSingle(), br.ReadSingle(), br.ReadSingle()), new Vector3(br.ReadSingle(), br.ReadSingle(), br.ReadSingle()));
		skinnedMeshRenderer.quality = (SkinQuality)br.ReadInt32();
		skinnedMeshRenderer.updateWhenOffscreen = br.ReadBoolean();
		int num3 = br.ReadInt32();
		BonesDummy bonesDummy = new BonesDummy();
		bonesDummy.smr = skinnedMeshRenderer;
		for (int j = 0; j < num3; j++)
		{
			bonesDummy.bones.Add(br.ReadString());
		}
		boneDummies.Add(bonesDummy);
		skinnedMeshRenderer.sharedMesh = ReadMesh(br);
	}

	public static Mesh ReadMesh(BinaryReader br)
	{
		Mesh mesh = new Mesh();
		EntryType entryType = (EntryType)br.ReadInt32();
		if (entryType != EntryType.MeshStart)
		{
			Debug.LogError("Mesh Error");
			return null;
		}
		int num = br.ReadInt32();
		br.ReadInt32();
		Vector3[] array = null;
		Vector3[] array2 = null;
		Vector2[] array3 = null;
		Vector2[] array4 = null;
		Vector4[] array5 = null;
		BoneWeight[] array6 = null;
		Color32[] array7 = null;
		int[] array8 = null;
		int num2 = 0;
		while ((entryType = (EntryType)br.ReadInt32()) != EntryType.MeshEnd)
		{
			switch (entryType)
			{
			case EntryType.MeshVertexColors:
			{
				array7 = new Color32[num];
				for (int k = 0; k < num; k++)
				{
					array7[k] = new Color32(br.ReadByte(), br.ReadByte(), br.ReadByte(), br.ReadByte());
				}
				mesh.colors32 = array7;
				break;
			}
			case EntryType.MeshVerts:
			{
				array = new Vector3[num];
				for (int num6 = 0; num6 < num; num6++)
				{
					array[num6] = new Vector3(br.ReadSingle(), br.ReadSingle(), br.ReadSingle());
				}
				mesh.vertices = array;
				break;
			}
			case EntryType.MeshUV:
			{
				array3 = new Vector2[num];
				for (int j = 0; j < num; j++)
				{
					array3[j] = new Vector2(br.ReadSingle(), br.ReadSingle());
				}
				mesh.uv = array3;
				break;
			}
			case EntryType.MeshUV2:
			{
				array4 = new Vector2[num];
				for (int n = 0; n < num; n++)
				{
					array4[n] = new Vector2(br.ReadSingle(), br.ReadSingle());
				}
				mesh.uv2 = array4;
				break;
			}
			case EntryType.MeshNormals:
			{
				array2 = new Vector3[num];
				for (int num7 = 0; num7 < num; num7++)
				{
					array2[num7] = new Vector3(br.ReadSingle(), br.ReadSingle(), br.ReadSingle());
				}
				mesh.normals = array2;
				break;
			}
			case EntryType.MeshTangents:
			{
				array5 = new Vector4[num];
				for (int m = 0; m < num; m++)
				{
					array5[m] = new Vector4(br.ReadSingle(), br.ReadSingle(), br.ReadSingle(), br.ReadSingle());
				}
				mesh.tangents = array5;
				break;
			}
			case EntryType.MeshTriangles:
			{
				int num4 = br.ReadInt32();
				array8 = new int[num4];
				for (int num5 = 0; num5 < num4; num5++)
				{
					array8[num5] = br.ReadInt32();
				}
				if (mesh.subMeshCount == num2)
				{
					mesh.subMeshCount++;
				}
				mesh.SetTriangles(array8, num2);
				num2++;
				break;
			}
			case EntryType.MeshBoneWeights:
			{
				array6 = new BoneWeight[num];
				for (int l = 0; l < num; l++)
				{
					array6[l] = default(BoneWeight);
					array6[l].boneIndex0 = br.ReadInt32();
					array6[l].weight0 = br.ReadSingle();
					array6[l].boneIndex1 = br.ReadInt32();
					array6[l].weight1 = br.ReadSingle();
					array6[l].boneIndex2 = br.ReadInt32();
					array6[l].weight2 = br.ReadSingle();
					array6[l].boneIndex3 = br.ReadInt32();
					array6[l].weight3 = br.ReadSingle();
				}
				mesh.boneWeights = array6;
				break;
			}
			case EntryType.MeshBindPoses:
			{
				int num3 = br.ReadInt32();
				Matrix4x4[] array9 = new Matrix4x4[num3];
				for (int i = 0; i < num3; i++)
				{
					array9[i] = default(Matrix4x4);
					array9[i].m00 = br.ReadSingle();
					array9[i].m01 = br.ReadSingle();
					array9[i].m02 = br.ReadSingle();
					array9[i].m03 = br.ReadSingle();
					array9[i].m10 = br.ReadSingle();
					array9[i].m11 = br.ReadSingle();
					array9[i].m12 = br.ReadSingle();
					array9[i].m13 = br.ReadSingle();
					array9[i].m20 = br.ReadSingle();
					array9[i].m21 = br.ReadSingle();
					array9[i].m22 = br.ReadSingle();
					array9[i].m23 = br.ReadSingle();
					array9[i].m30 = br.ReadSingle();
					array9[i].m31 = br.ReadSingle();
					array9[i].m32 = br.ReadSingle();
					array9[i].m33 = br.ReadSingle();
				}
				mesh.bindposes = array9;
				break;
			}
			}
		}
		mesh.RecalculateBounds();
		return mesh;
	}

	public static Material ReadMaterial(BinaryReader br)
	{
		string name = br.ReadString();
		ShaderType shaderType = (ShaderType)br.ReadInt32();
		Material material = null;
		switch (shaderType)
		{
		default:
			material = new Material(Shader.Find("KSP/Diffuse"));
			ReadMaterialTexture(br, material, "_MainTex");
			break;
		case ShaderType.Specular:
			material = new Material(Shader.Find("KSP/Specular"));
			ReadMaterialTexture(br, material, "_MainTex");
			material.SetColor("_SpecColor", ReadColor(br));
			material.SetFloat("_Shininess", br.ReadSingle());
			break;
		case ShaderType.Bumped:
			material = new Material(Shader.Find("KSP/Bumped"));
			ReadMaterialTexture(br, material, "_MainTex");
			ReadMaterialTexture(br, material, "_BumpMap");
			break;
		case ShaderType.BumpedSpecular:
			material = new Material(Shader.Find("KSP/Bumped Specular"));
			ReadMaterialTexture(br, material, "_MainTex");
			ReadMaterialTexture(br, material, "_BumpMap");
			material.SetColor("_SpecColor", ReadColor(br));
			material.SetFloat("_Shininess", br.ReadSingle());
			break;
		case ShaderType.Emissive:
			material = new Material(Shader.Find("KSP/Emissive/Diffuse"));
			ReadMaterialTexture(br, material, "_MainTex");
			ReadMaterialTexture(br, material, "_Emissive");
			material.SetColor(PropertyIDs._EmissiveColor, ReadColor(br));
			break;
		case ShaderType.EmissiveSpecular:
			material = new Material(Shader.Find("KSP/Emissive/Specular"));
			ReadMaterialTexture(br, material, "_MainTex");
			material.SetColor("_SpecColor", ReadColor(br));
			material.SetFloat("_Shininess", br.ReadSingle());
			ReadMaterialTexture(br, material, "_Emissive");
			material.SetColor(PropertyIDs._EmissiveColor, ReadColor(br));
			break;
		case ShaderType.EmissiveBumpedSpecular:
			material = new Material(Shader.Find("KSP/Emissive/Bumped Specular"));
			ReadMaterialTexture(br, material, "_MainTex");
			ReadMaterialTexture(br, material, "_BumpMap");
			material.SetColor("_SpecColor", ReadColor(br));
			material.SetFloat("_Shininess", br.ReadSingle());
			ReadMaterialTexture(br, material, "_Emissive");
			material.SetColor(PropertyIDs._EmissiveColor, ReadColor(br));
			break;
		case ShaderType.AlphaCutout:
			material = new Material(Shader.Find("KSP/Alpha/Cutoff"));
			ReadMaterialTexture(br, material, "_MainTex");
			material.SetFloat("_Cutoff", br.ReadSingle());
			break;
		case ShaderType.AlphaCutoutBumped:
			material = new Material(Shader.Find("KSP/Alpha/Cutoff Bumped"));
			ReadMaterialTexture(br, material, "_MainTex");
			ReadMaterialTexture(br, material, "_BumpMap");
			material.SetFloat("_Cutoff", br.ReadSingle());
			break;
		case ShaderType.Alpha:
			material = new Material(Shader.Find("KSP/Alpha/Translucent"));
			ReadMaterialTexture(br, material, "_MainTex");
			break;
		case ShaderType.AlphaSpecular:
			material = new Material(Shader.Find("KSP/Alpha/Translucent Specular"));
			ReadMaterialTexture(br, material, "_MainTex");
			material.SetFloat("_Gloss", br.ReadSingle());
			material.SetColor("_SpecColor", ReadColor(br));
			material.SetFloat("_Shininess", br.ReadSingle());
			break;
		case ShaderType.AlphaUnlit:
			material = new Material(Shader.Find("KSP/Alpha/Unlit Transparent"));
			ReadMaterialTexture(br, material, "_MainTex");
			material.SetColor("_Color", ReadColor(br));
			break;
		case ShaderType.Unlit:
			material = new Material(Shader.Find("KSP/Unlit"));
			ReadMaterialTexture(br, material, "_MainTex");
			material.SetColor("_Color", ReadColor(br));
			break;
		case ShaderType.ParticleAlpha:
			material = new Material(Shader.Find("KSP/Particles/Alpha Blended"));
			ReadMaterialTexture(br, material, "_MainTex");
			material.SetColor("_Color", ReadColor(br));
			material.SetFloat("_InvFade", br.ReadSingle());
			break;
		case ShaderType.ParticleAdditive:
			material = new Material(Shader.Find("KSP/Particles/Additive"));
			ReadMaterialTexture(br, material, "_MainTex");
			material.SetColor("_Color", ReadColor(br));
			material.SetFloat("_InvFade", br.ReadSingle());
			break;
		case ShaderType.BumpedSpecularMap:
			material = new Material(Shader.Find("KSP/Bumped Specular (Mapped)"));
			ReadMaterialTexture(br, material, "_MainTex");
			ReadMaterialTexture(br, material, "_BumpMap");
			ReadMaterialTexture(br, material, "_SpecMap");
			material.SetFloat("_SpecTint", br.ReadSingle());
			material.SetFloat("_Shininess", br.ReadSingle());
			break;
		}
		if (material != null)
		{
			material.name = name;
		}
		return material;
	}

	public static Material ReadMaterial4(BinaryReader br)
	{
		string name = br.ReadString();
		string name2 = br.ReadString();
		int num = br.ReadInt32();
		Material material = new Material(Shader.Find(name2));
		material.name = name;
		for (int i = 0; i < num; i++)
		{
			string text = br.ReadString();
			switch ((ShaderPropertyType)br.ReadInt32())
			{
			case ShaderPropertyType.Color:
				material.SetColor(text, ReadColor(br));
				break;
			case ShaderPropertyType.Vector:
				material.SetVector(text, ReadVector4(br));
				break;
			case ShaderPropertyType.Float:
				material.SetFloat(text, br.ReadSingle());
				break;
			case ShaderPropertyType.Range:
				material.SetFloat(text, br.ReadSingle());
				break;
			case ShaderPropertyType.TexEnv:
				ReadMaterialTexture(br, material, text);
				break;
			}
		}
		return material;
	}

	public static void ReadMaterialTexture(BinaryReader br, Material mat, string textureName)
	{
		textureDummies.AddTextureDummy(br.ReadInt32(), mat, textureName);
		Vector2 value = Vector3.zero;
		value.x = br.ReadSingle();
		value.y = br.ReadSingle();
		mat.SetTextureScale(textureName, value);
		value.x = br.ReadSingle();
		value.y = br.ReadSingle();
		mat.SetTextureOffset(textureName, value);
	}

	public static void ReadAnimation(BinaryReader br, GameObject o)
	{
		Animation animation = o.AddComponent<Animation>();
		int num = br.ReadInt32();
		bool flag = false;
		for (int i = 0; i < num; i++)
		{
			AnimationClip animationClip = new AnimationClip();
			animationClip.legacy = true;
			string newName = br.ReadString();
			animationClip.localBounds = new Bounds(new Vector3(br.ReadSingle(), br.ReadSingle(), br.ReadSingle()), new Vector3(br.ReadSingle(), br.ReadSingle(), br.ReadSingle()));
			animationClip.wrapMode = (WrapMode)br.ReadInt32();
			int num2 = br.ReadInt32();
			for (int j = 0; j < num2; j++)
			{
				string text = br.ReadString();
				string text2 = br.ReadString();
				Type type = null;
				switch ((AnimationType)br.ReadInt32())
				{
				case AnimationType.Transform:
					type = typeof(Transform);
					break;
				case AnimationType.Material:
					type = typeof(Material);
					break;
				case AnimationType.Light:
					type = typeof(Light);
					break;
				case AnimationType.AudioSource:
					type = typeof(AudioSource);
					break;
				}
				WrapMode preWrapMode = (WrapMode)br.ReadInt32();
				WrapMode postWrapMode = (WrapMode)br.ReadInt32();
				int num3 = br.ReadInt32();
				Keyframe[] array = new Keyframe[num3];
				for (int k = 0; k < num3; k++)
				{
					array[k] = default(Keyframe);
					array[k].time = br.ReadSingle();
					array[k].value = br.ReadSingle();
					array[k].inTangent = br.ReadSingle();
					array[k].outTangent = br.ReadSingle();
					br.ReadInt32();
				}
				AnimationCurve animationCurve = new AnimationCurve(array);
				animationCurve.preWrapMode = preWrapMode;
				animationCurve.postWrapMode = postWrapMode;
				if (text != null && !(type == null) && text2 != null && animationCurve != null)
				{
					animationClip.SetCurve(text, type, text2, animationCurve);
					continue;
				}
				Debug.LogWarning(((text == null) ? "Null curvePath" : text) + ", " + ((type == null) ? "Null curveType" : type.ToString()) + ", " + ((text2 == null) ? "Null curveProperty" : text2) + ", " + ((animationCurve == null) ? "Null curve" : animationCurve.ToString()));
				flag = true;
			}
			if (!flag)
			{
				animation.AddClip(animationClip, newName);
			}
		}
		string text3 = br.ReadString();
		if (text3 != string.Empty && !flag)
		{
			animation.clip = animation.GetClip(text3);
		}
		animation.playAutomatically = br.ReadBoolean();
	}

	public static void ReadAnimationEvents(BinaryReader br, GameObject o)
	{
		Animation animation = o.AddComponent<Animation>();
		int num = br.ReadInt32();
		for (int i = 0; i < num; i++)
		{
			AnimationClip animationClip = new AnimationClip();
			string newName = br.ReadString();
			animationClip.localBounds = new Bounds(new Vector3(br.ReadSingle(), br.ReadSingle(), br.ReadSingle()), new Vector3(br.ReadSingle(), br.ReadSingle(), br.ReadSingle()));
			animationClip.wrapMode = (WrapMode)br.ReadInt32();
			int num2 = br.ReadInt32();
			for (int j = 0; j < num2; j++)
			{
				string relativePath = br.ReadString();
				string propertyName = br.ReadString();
				Type type = null;
				switch ((AnimationType)br.ReadInt32())
				{
				case AnimationType.Transform:
					type = typeof(Transform);
					break;
				case AnimationType.Material:
					type = typeof(Material);
					break;
				case AnimationType.Light:
					type = typeof(Light);
					break;
				case AnimationType.AudioSource:
					type = typeof(AudioSource);
					break;
				}
				WrapMode preWrapMode = (WrapMode)br.ReadInt32();
				WrapMode postWrapMode = (WrapMode)br.ReadInt32();
				int num3 = br.ReadInt32();
				Keyframe[] array = new Keyframe[num3];
				for (int k = 0; k < num3; k++)
				{
					array[k] = default(Keyframe);
					array[k].time = br.ReadSingle();
					array[k].value = br.ReadSingle();
					array[k].inTangent = br.ReadSingle();
					array[k].outTangent = br.ReadSingle();
					br.ReadInt32();
				}
				AnimationCurve animationCurve = new AnimationCurve(array);
				animationCurve.preWrapMode = preWrapMode;
				animationCurve.postWrapMode = postWrapMode;
				animationClip.SetCurve(relativePath, type, propertyName, animationCurve);
				int num4 = br.ReadInt32();
				Debug.Log("EVTS: " + num4);
				for (int l = 0; l < num4; l++)
				{
					AnimationEvent animationEvent = new AnimationEvent();
					animationEvent.time = br.ReadSingle();
					animationEvent.functionName = br.ReadString();
					animationEvent.stringParameter = br.ReadString();
					animationEvent.intParameter = br.ReadInt32();
					animationEvent.floatParameter = br.ReadSingle();
					animationEvent.messageOptions = (SendMessageOptions)br.ReadInt32();
					Debug.Log(animationEvent.time);
					Debug.Log(animationEvent.functionName);
					Debug.Log(animationEvent.stringParameter);
					Debug.Log(animationEvent.intParameter);
					Debug.Log(animationEvent.floatParameter);
					Debug.Log(animationEvent.messageOptions);
					animationClip.AddEvent(animationEvent);
				}
			}
			animation.AddClip(animationClip, newName);
		}
		string text = br.ReadString();
		if (text != string.Empty)
		{
			animation.clip = animation.GetClip(text);
		}
		animation.playAutomatically = br.ReadBoolean();
	}

	public static void ReadLight(BinaryReader br, GameObject o)
	{
		Light light = o.AddComponent<Light>();
		light.type = (LightType)br.ReadInt32();
		light.intensity = br.ReadSingle();
		light.range = br.ReadSingle();
		light.color = ReadColor(br);
		light.cullingMask = br.ReadInt32();
		if (fileVersion > 1)
		{
			light.spotAngle = br.ReadSingle();
		}
	}

	public static void ReadTagAndLayer(BinaryReader br, GameObject o)
	{
		o.tag = br.ReadString();
		o.layer = br.ReadInt32();
	}

	public static void ReadCamera(BinaryReader br, GameObject o)
	{
		Camera camera = o.AddComponent<Camera>();
		camera.clearFlags = (CameraClearFlags)br.ReadInt32();
		camera.backgroundColor = ReadColor(br);
		camera.cullingMask = br.ReadInt32();
		camera.orthographic = br.ReadBoolean();
		camera.fieldOfView = br.ReadSingle();
		camera.nearClipPlane = br.ReadSingle();
		camera.farClipPlane = br.ReadSingle();
		camera.depth = br.ReadSingle();
		camera.allowHDR = false;
		camera.enabled = false;
	}

	public static void ReadParticles(BinaryReader br, GameObject o)
	{
		KSPParticleEmitter kSPParticleEmitter = o.AddComponent<KSPParticleEmitter>();
		kSPParticleEmitter.emit = br.ReadBoolean();
		kSPParticleEmitter.shape = (KSPParticleEmitter.EmissionShape)br.ReadInt32();
		kSPParticleEmitter.shape3D.x = br.ReadSingle();
		kSPParticleEmitter.shape3D.y = br.ReadSingle();
		kSPParticleEmitter.shape3D.z = br.ReadSingle();
		kSPParticleEmitter.shape2D.x = br.ReadSingle();
		kSPParticleEmitter.shape2D.y = br.ReadSingle();
		kSPParticleEmitter.shape1D = br.ReadSingle();
		kSPParticleEmitter.color = ReadColor(br);
		kSPParticleEmitter.useWorldSpace = br.ReadBoolean();
		kSPParticleEmitter.minSize = br.ReadSingle();
		kSPParticleEmitter.maxSize = br.ReadSingle();
		kSPParticleEmitter.minEnergy = br.ReadSingle();
		kSPParticleEmitter.maxEnergy = br.ReadSingle();
		kSPParticleEmitter.minEmission = br.ReadInt32();
		kSPParticleEmitter.maxEmission = br.ReadInt32();
		kSPParticleEmitter.worldVelocity.x = br.ReadSingle();
		kSPParticleEmitter.worldVelocity.y = br.ReadSingle();
		kSPParticleEmitter.worldVelocity.z = br.ReadSingle();
		kSPParticleEmitter.localVelocity.x = br.ReadSingle();
		kSPParticleEmitter.localVelocity.y = br.ReadSingle();
		kSPParticleEmitter.localVelocity.z = br.ReadSingle();
		kSPParticleEmitter.rndVelocity.x = br.ReadSingle();
		kSPParticleEmitter.rndVelocity.y = br.ReadSingle();
		kSPParticleEmitter.rndVelocity.z = br.ReadSingle();
		kSPParticleEmitter.emitterVelocityScale = br.ReadSingle();
		kSPParticleEmitter.angularVelocity = br.ReadSingle();
		kSPParticleEmitter.rndAngularVelocity = br.ReadSingle();
		kSPParticleEmitter.rndRotation = br.ReadBoolean();
		kSPParticleEmitter.doesAnimateColor = br.ReadBoolean();
		kSPParticleEmitter.colorAnimation = new Color[5];
		for (int i = 0; i < 5; i++)
		{
			kSPParticleEmitter.colorAnimation[i] = ReadColor(br);
		}
		kSPParticleEmitter.worldRotationAxis.x = br.ReadSingle();
		kSPParticleEmitter.worldRotationAxis.y = br.ReadSingle();
		kSPParticleEmitter.worldRotationAxis.z = br.ReadSingle();
		kSPParticleEmitter.localRotationAxis.x = br.ReadSingle();
		kSPParticleEmitter.localRotationAxis.y = br.ReadSingle();
		kSPParticleEmitter.localRotationAxis.z = br.ReadSingle();
		kSPParticleEmitter.sizeGrow = br.ReadSingle();
		kSPParticleEmitter.rndForce.x = br.ReadSingle();
		kSPParticleEmitter.rndForce.y = br.ReadSingle();
		kSPParticleEmitter.rndForce.z = br.ReadSingle();
		kSPParticleEmitter.force.x = br.ReadSingle();
		kSPParticleEmitter.force.y = br.ReadSingle();
		kSPParticleEmitter.force.z = br.ReadSingle();
		kSPParticleEmitter.damping = br.ReadSingle();
		kSPParticleEmitter.castShadows = br.ReadBoolean();
		kSPParticleEmitter.recieveShadows = br.ReadBoolean();
		kSPParticleEmitter.lengthScale = br.ReadSingle();
		kSPParticleEmitter.velocityScale = br.ReadSingle();
		kSPParticleEmitter.maxParticleSize = br.ReadSingle();
		switch (br.ReadInt32())
		{
		default:
			kSPParticleEmitter.particleRenderMode = ParticleSystemRenderMode.Billboard;
			break;
		case 3:
			kSPParticleEmitter.particleRenderMode = ParticleSystemRenderMode.Stretch;
			break;
		case 4:
			kSPParticleEmitter.particleRenderMode = ParticleSystemRenderMode.HorizontalBillboard;
			break;
		case 5:
			kSPParticleEmitter.particleRenderMode = ParticleSystemRenderMode.VerticalBillboard;
			break;
		}
		kSPParticleEmitter.uvAnimationXTile = br.ReadInt32();
		kSPParticleEmitter.uvAnimationYTile = br.ReadInt32();
		kSPParticleEmitter.uvAnimationCycles = br.ReadInt32();
		int num = br.ReadInt32();
		while (num >= matDummies.Count)
		{
			matDummies.Add(new MaterialDummy());
		}
		matDummies[num].particleEmitters.Add(kSPParticleEmitter);
	}

	public static Transform FindChildByName(Transform parent, string name)
	{
		if (parent.name == name)
		{
			return parent;
		}
		foreach (Transform item in parent)
		{
			Transform transform = FindChildByName(item, name);
			if (transform != null)
			{
				return transform;
			}
		}
		return null;
	}

	public static Color ReadColor(BinaryReader br)
	{
		return new Color(br.ReadSingle(), br.ReadSingle(), br.ReadSingle(), br.ReadSingle());
	}

	public static Vector4 ReadVector2(BinaryReader br)
	{
		return new Vector2(br.ReadSingle(), br.ReadSingle());
	}

	public static Vector4 ReadVector3(BinaryReader br)
	{
		return new Vector3(br.ReadSingle(), br.ReadSingle(), br.ReadSingle());
	}

	public static Vector4 ReadVector4(BinaryReader br)
	{
		return new Vector4(br.ReadSingle(), br.ReadSingle(), br.ReadSingle(), br.ReadSingle());
	}
}
