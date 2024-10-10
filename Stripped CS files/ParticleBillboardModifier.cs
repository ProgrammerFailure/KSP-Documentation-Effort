using UnityEngine;

[ExecuteInEditMode]
public class ParticleBillboardModifier : MonoBehaviour
{
	public Mesh quadMesh;

	public string BaseProperty = "Base";

	[ContextMenu("Test")]
	public void Test()
	{
	}

	[ContextMenu("Replace Hor. Billboards With Quad")]
	public void ReplaceHorizontalBillboardsWithMesh()
	{
	}

	[ContextMenu("Explore System Properties")]
	public void ExploreSystemProperties()
	{
	}
}
