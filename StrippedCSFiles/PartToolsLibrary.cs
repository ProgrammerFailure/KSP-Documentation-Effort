using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[AddComponentMenu("KSP/Part Tools Library")]
public class PartToolsLibrary : MonoBehaviour
{
	public List<PartTools> partPrefabs;

	public string loaderLevelName;

	public bool forceTextureFormat;

	public PartTools.TextureFormat textureFormat;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public PartToolsLibrary()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Awake()
	{
		throw null;
	}
}
