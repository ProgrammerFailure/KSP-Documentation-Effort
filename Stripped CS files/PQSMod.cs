using UnityEngine;

public class PQSMod : MonoBehaviour
{
	[HideInInspector]
	public GClass4 sphere;

	[HideInInspector]
	public GClass4.ModiferRequirements requirements;

	public bool modEnabled = true;

	public int order = 100;

	public bool overrideQuadBuildCheck;

	[SerializeField]
	public bool modExpansionDisabled;

	[ContextMenu("Rebuild")]
	public void RebuildSphere()
	{
		if (sphere != null)
		{
			sphere.RebuildSphere();
		}
	}

	public virtual void OnSetup()
	{
		requirements = GClass4.ModiferRequirements.Default;
		if (modExpansionDisabled)
		{
			modEnabled = false;
		}
	}

	public virtual void OnPostSetup()
	{
	}

	public virtual void OnSphereReset()
	{
	}

	public virtual void OnSphereActive()
	{
	}

	public virtual void OnSphereInactive()
	{
	}

	public virtual bool OnSphereStart()
	{
		return false;
	}

	public virtual void OnSphereStarted()
	{
	}

	public virtual void OnSphereTransformUpdate()
	{
	}

	public virtual void OnPreUpdate()
	{
	}

	public virtual void OnUpdateFinished()
	{
	}

	public virtual void OnVertexBuild(GClass4.VertexBuildData data)
	{
	}

	public virtual void OnVertexBuildHeight(GClass4.VertexBuildData data)
	{
	}

	public virtual double GetVertexMaxHeight()
	{
		return 0.0;
	}

	public virtual double GetVertexMinHeight()
	{
		return 0.0;
	}

	public virtual void OnMeshBuild()
	{
	}

	public virtual void OnQuadCreate(GClass3 quad)
	{
	}

	public virtual void OnQuadDestroy(GClass3 quad)
	{
	}

	public virtual void OnQuadPreBuild(GClass3 quad)
	{
	}

	public virtual void OnQuadBuilt(GClass3 quad)
	{
	}

	public virtual void OnQuadUpdate(GClass3 quad)
	{
	}

	public virtual void OnQuadUpdateNormals(GClass3 quad)
	{
	}
}
