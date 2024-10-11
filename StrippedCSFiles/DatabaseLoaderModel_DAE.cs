using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Xml;
using UnityEngine;

[DatabaseLoaderAttrib(new string[] { "dae", "DAE" })]
public class DatabaseLoaderModel_DAE : DatabaseLoader<GameObject>
{
	public class DAE
	{
		private class SceneGeo
		{
			public string ParentNode;

			public string GID;

			public bool isInstance;

			public Matrix4x4 Gm;

			public Quaternion Totalrot;

			public List<GMaterialID> GMaterial;

			[MethodImpl(MethodImplOptions.NoInlining)]
			public SceneGeo()
			{
				throw null;
			}
		}

		private class GMaterialID
		{
			public string id;

			public string symbol;

			[MethodImpl(MethodImplOptions.NoInlining)]
			public GMaterialID()
			{
				throw null;
			}
		}

		private class SceneTransform
		{
			public string NodeName;

			public Vector3 pos;

			public Quaternion rot;

			public Vector3 scl;

			public bool hasgeometry;

			public string ParentNode;

			[MethodImpl(MethodImplOptions.NoInlining)]
			public SceneTransform()
			{
				throw null;
			}
		}

		private class DataID
		{
			public string ID;

			public string Semantic;

			[MethodImpl(MethodImplOptions.NoInlining)]
			public DataID()
			{
				throw null;
			}
		}

		private class DAETextureID
		{
			public string id;

			public string path;

			[MethodImpl(MethodImplOptions.NoInlining)]
			public DAETextureID()
			{
				throw null;
			}
		}

		private class MaterialData
		{
			public string ID;

			public string name;

			public Color ambient;

			public Color diffuse;

			public Color specular;

			public Color emmisive;

			public Color alphacolor;

			public float shininess;

			public float alpha;

			public string diffuseTexPath;

			public string emmisiveTexPath;

			public string specularTexPath;

			public string alphaTexPath;

			public Texture2D DiffTexture;

			public Texture2D EmmisiveTexture;

			public string ShaderName;

			[MethodImpl(MethodImplOptions.NoInlining)]
			public MaterialData()
			{
				throw null;
			}
		}

		public string objPath;

		public bool MakeCollider;

		public float objmaxsize;

		public float objminsize;

		public bool EnforceSingleObj;

		private bool initval;

		private float MasterScale;

		private float minvpos;

		private float maxvpos;

		private float minvxpos;

		private float maxvxpos;

		private float minvypos;

		private float maxvypos;

		private float minvzpos;

		private float maxvzpos;

		private float MasterOffsetx;

		private float MasterOffsety;

		private float MasterOffsetz;

		private Vector3 MainOffset;

		private GeometryBuffer buffer;

		private Texture2D[] TMPTextures;

		private XmlDocument xdoc;

		private int Voffset;

		private int Noffset;

		private int Uoffset;

		private int instanceVoffset;

		private int instanceNoffset;

		private int instanceUoffset;

		private int up_axis;

		private bool normalvertexgrouped;

		private bool treatasoneobject;

		private bool firstobject;

		private bool hasmaterials;

		private string currentgname;

		private int stwithoutgeometry;

		private GameObject gameObject;

		private List<SceneGeo> sceneGeo;

		private List<SceneTransform> sceneTransforms;

		private List<DAETextureID> daeTextures;

		private Dictionary<string, int> TextureList;

		private List<MaterialData> materialData;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public DAE()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public GameObject Load(UrlDir.UrlFile urlFile, FileInfo file)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private bool LoadDAE(UrlDir.UrlFile urlFile, FileInfo file)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public static void AddReducedMeshColliders(GameObject[] gs, string method)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private void GetUpAxis()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private void ReadDAE(string gdata)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private Vector3 UpPosConv(Vector3 v)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private Vector3 UpScaleConv(Vector3 v)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private Vector3 UpRotDirConv(Vector3 v)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private bool GetSceneGeo()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private string CanParseGeometry(XmlElement georoot)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private void pushtooffsetscale(Vector3 v)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private bool GetGeometryData(XmlElement georoot)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private string[] FormatFaceData(XmlElement FaceSection, int IndiceCount, int FaceCount)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private bool GetFaceData(XmlElement georoot, string gtype)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private void CheckScale()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private void GetDAEMaterials()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private Color DAEGetColor(XmlElement elem)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private string GetValidTexturePath(string ID, XmlElement fx)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private void SolveMaterials()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private Material GetMaterial(MaterialData md)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private void Build()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private float cf(string v)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private int ci(string v)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private static string RemoveWS(string p)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private Texture2D NormalMap(Texture2D source, float strength)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private Texture2D ScaleTexture(Texture2D source, int targetWidth, int targetHeight)
		{
			throw null;
		}
	}

	[CompilerGenerated]
	private sealed class _003CLoad_003Ed__1 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public UrlDir.UrlFile urlFile;

		public FileInfo file;

		public DatabaseLoaderModel_DAE _003C_003E4__this;

		object IEnumerator<object>.Current
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			[DebuggerHidden]
			get
			{
				throw null;
			}
		}

		object IEnumerator.Current
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			[DebuggerHidden]
			get
			{
				throw null;
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DebuggerHidden]
		public _003CLoad_003Ed__1(int _003C_003E1__state)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private bool MoveNext()
		{
			throw null;
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public DatabaseLoaderModel_DAE()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[IteratorStateMachine(typeof(_003CLoad_003Ed__1))]
	public override IEnumerator Load(UrlDir.UrlFile urlFile, FileInfo file)
	{
		throw null;
	}
}
