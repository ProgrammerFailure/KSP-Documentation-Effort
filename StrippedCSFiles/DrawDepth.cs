using System.Runtime.CompilerServices;
using UnityEngine;

[RequireComponent(typeof(Camera))]
[ExecuteInEditMode]
public class DrawDepth : MonoBehaviour
{
	protected bool isSupported;

	protected bool supportHDRTextures;

	public Shader depthShader;

	private Material depthMaterial;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public DrawDepth()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnEnable()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool CheckSupport()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDisable()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool CheckResources()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool CheckSupport(bool needDepth)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool CheckSupport(bool needDepth, bool needHdr)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ReportAutoDisable()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool CheckShader(Shader s)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void NotSupported()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void LateUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void DoDepthRender()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private Material CheckShaderAndCreateMaterial(Shader s, Material m2Create)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private Material CreateMaterial(Shader s, Material m2Create)
	{
		throw null;
	}
}
