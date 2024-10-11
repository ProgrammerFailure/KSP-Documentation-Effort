using System.Runtime.CompilerServices;
using UnityEngine;

public class PartResourceLibrary : MonoBehaviour
{
	public static int ElectricityHashcode;

	[SerializeField]
	public PartResourceDefinitionList resourceDefinitions;

	public string resourcePath;

	public string resourceExtension;

	public static PartResourceLibrary Instance
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
	public PartResourceLibrary()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static PartResourceLibrary()
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
	public PartResourceDefinition GetDefinition(string name)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public PartResourceDefinition GetDefinition(int id)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static ResourceFlowMode GetDefaultFlowMode(string resourceName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static ResourceFlowMode GetDefaultFlowMode(int resourceID)
	{
		throw null;
	}
}
