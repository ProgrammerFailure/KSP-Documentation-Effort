using System.Runtime.CompilerServices;
using UnityEngine;

public class KerbalPreview : MonoBehaviour
{
	[SerializeField]
	private SkinnedMeshRenderer bodyMesh;

	[SerializeField]
	private SkinnedMeshRenderer helmetMesh;

	[SerializeField]
	private SkinnedMeshRenderer neckringMesh;

	[SerializeField]
	private GameObject helmet;

	public Material bodyMaterial;

	public Material helmetMaterial;

	public Material neckringMaterial;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public KerbalPreview()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void PreviewHelmetSelection(ProtoCrewMember crew)
	{
		throw null;
	}
}
