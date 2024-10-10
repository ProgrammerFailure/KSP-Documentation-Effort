using System;
using UnityEngine;

public class PQSMod_MeshScatter_QuadControl : MonoBehaviour
{
	public MeshFilter mf;

	public bool isVisible;

	public bool isBuilt;

	public int seed;

	public GClass3 quad;

	public int count;

	public PQSMod_MeshScatter scatter;

	public static double maxTargetSpeedForBuild = 10.0;

	public void Setup(GClass3 quad, int seed, PQSMod_MeshScatter scatter, int count)
	{
		this.quad = quad;
		this.seed = seed;
		this.scatter = scatter;
		isVisible = true;
		isBuilt = false;
		if (quad.onVisible != null)
		{
			quad.onVisible = (GClass3.QuadDelegate)Delegate.Combine(quad.onVisible, new GClass3.QuadDelegate(OnQuadVisible));
		}
		else
		{
			quad.onVisible = OnQuadVisible;
		}
		if (quad.onUpdate != null)
		{
			quad.onUpdate = (GClass3.QuadDelegate)Delegate.Combine(quad.onUpdate, new GClass3.QuadDelegate(OnQuadUpdate));
		}
		else
		{
			quad.onUpdate = OnQuadUpdate;
		}
		if (quad.onInvisible != null)
		{
			quad.onInvisible = (GClass3.QuadDelegate)Delegate.Combine(quad.onInvisible, new GClass3.QuadDelegate(OnQuadInvisible));
		}
		else
		{
			quad.onInvisible = OnQuadInvisible;
		}
		if (quad.onDestroy != null)
		{
			quad.onDestroy = (GClass3.QuadDelegate)Delegate.Combine(quad.onDestroy, new GClass3.QuadDelegate(OnQuadDestroy));
		}
		else
		{
			quad.onDestroy = OnQuadDestroy;
		}
	}

	public void OnQuadVisible(GClass3 quad)
	{
		if (!isVisible)
		{
			isVisible = true;
			if (isBuilt && mf != null)
			{
				mf.GetComponent<Renderer>().enabled = true;
			}
		}
	}

	public void OnQuadInvisible(GClass3 quad)
	{
		if (isVisible)
		{
			isVisible = false;
			if (isBuilt && mf != null)
			{
				mf.GetComponent<Renderer>().enabled = true;
			}
		}
	}

	public void OnQuadDestroy(GClass3 quad)
	{
		if (isBuilt && mf != null)
		{
			scatter.UnassignScatterMesh(mf);
		}
		quad.onVisible = (GClass3.QuadDelegate)Delegate.Remove(quad.onVisible, new GClass3.QuadDelegate(OnQuadVisible));
		quad.onInvisible = (GClass3.QuadDelegate)Delegate.Remove(quad.onInvisible, new GClass3.QuadDelegate(OnQuadInvisible));
		quad.onDestroy = (GClass3.QuadDelegate)Delegate.Remove(quad.onDestroy, new GClass3.QuadDelegate(OnQuadDestroy));
		quad.onUpdate = (GClass3.QuadDelegate)Delegate.Remove(quad.onUpdate, new GClass3.QuadDelegate(OnQuadUpdate));
		UnityEngine.Object.Destroy(this);
	}

	public void OnQuadUpdate(GClass3 quad)
	{
		if (isVisible && !isBuilt && quad.sphereRoot.quadAllowBuild && quad.sphereRoot.targetSpeed < maxTargetSpeedForBuild)
		{
			mf = scatter.AssignScatterMesh(quad, seed, count);
			if (mf != null)
			{
				isBuilt = true;
				quad.onUpdate = (GClass3.QuadDelegate)Delegate.Remove(quad.onUpdate, new GClass3.QuadDelegate(OnQuadUpdate));
			}
		}
	}
}
