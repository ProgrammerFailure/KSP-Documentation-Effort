using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PhysicMaterialLibrary : MonoBehaviour
{
	[SerializeField]
	public Dictionary<string, PhysicMaterialDefinition> physicMaterials;

	public string resourcePath;

	public string resourceExtension;

	public PhysicMaterialDefinition DefaultMaterial;

	public static PhysicMaterialLibrary Instance
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

	[MethodImpl(MethodImplOptions.NoInlining)]
	public PhysicMaterialLibrary()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Reset()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void LoadDefinitions()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void CreatePhysicMaterials(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void CreateDefaultPhysicMaterial()
	{
		throw null;
	}
}
