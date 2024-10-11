using System.Runtime.CompilerServices;
using UnityEngine;

namespace KSP.UI.Rendering;

[RequireComponent(typeof(Camera))]
public class CameraShaderReplacement : MonoBehaviour
{
	[SerializeField]
	private Shader shader;

	[SerializeField]
	private string replacementTag;

	[SerializeField]
	private Camera cam;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public CameraShaderReplacement()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Reset()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[ContextMenu("Reset Shader")]
	public void SetShader()
	{
		throw null;
	}
}
