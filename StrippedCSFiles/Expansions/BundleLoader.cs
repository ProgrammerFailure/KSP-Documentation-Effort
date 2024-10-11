using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Networking;

namespace Expansions;

public class BundleLoader : MonoBehaviour
{
	public class ABAssetInfo
	{
		public string assetName;

		public bool isScene;

		public string bundleName;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public ABAssetInfo(string name, string bundleName, bool isScene)
		{
			throw null;
		}
	}

	public class ABInfo
	{
		public AssetBundle bundle;

		public string path;

		public byte[] hash;

		public string BundleName
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				throw null;
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public ABInfo(AssetBundle bundle, string path, byte[] hash)
		{
			throw null;
		}
	}

	[CompilerGenerated]
	private sealed class _003CLoadAssetBundle_003Ed__16 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public string bundleName;

		public string folderPath;

		private byte[] _003Chash_003E5__2;

		private AssetBundle _003Cbundle_003E5__3;

		private UnityWebRequest _003CwebRequest_003E5__4;

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
		public _003CLoadAssetBundle_003Ed__16(int _003C_003E1__state)
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
		private void _003C_003Em__Finally1()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw null;
		}
	}

	public static Dictionary<string, ABInfo> loadedBundles;

	public static Dictionary<string, ABAssetInfo> loadedAssets;

	public static BundleLoader Instance
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		private set
		{
			throw null;
		}
	}

	public Dictionary<string, ABAssetInfo> LoadedAssets
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public Dictionary<string, ABInfo> LoadedBundles
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public BundleLoader()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static BundleLoader()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static bool IsBundleLoaded(string bundleName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static void LoadScene(GameScenes scene, string bundleName, string sceneUrl)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static UnityEngine.Object LoadAsset<T>(string bundleName, string assetUrl) where T : MonoBehaviour
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static UnityEngine.Object LoadAsset(string bundleName, string assetUrl)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static Texture2D LoadTextureAsset(string bundleName, string assetUrl)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[IteratorStateMachine(typeof(_003CLoadAssetBundle_003Ed__16))]
	internal static IEnumerator LoadAssetBundle(string bundleName, string folderPath)
	{
		throw null;
	}
}
