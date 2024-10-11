using System.Runtime.CompilerServices;
using UnityEngine;

public abstract class VersioningBase : MonoBehaviour
{
	protected static VersioningBase instance;

	public static VersioningBase Instance
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected VersioningBase()
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

	protected abstract void OnAwake();

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static string GetVersionString()
	{
		throw null;
	}

	protected abstract string GetVersion();
}
