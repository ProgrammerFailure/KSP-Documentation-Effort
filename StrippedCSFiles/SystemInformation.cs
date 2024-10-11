using System.Runtime.CompilerServices;
using UnityEngine;

public class SystemInformation : MonoBehaviour
{
	public enum Renderer
	{
		OpenGL,
		DirectX
	}

	public static bool isDirectX;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public SystemInformation()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static SystemInformation()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Awake()
	{
		throw null;
	}
}
