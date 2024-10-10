using System;
using UnityEngine;

public class PQSMod_LandClassScatterQuad : MonoBehaviour
{
	public GameObject obj;

	public Mesh mesh;

	public MeshFilter mf;

	public MeshRenderer mr;

	public bool isVisible;

	public bool isBuilt;

	public int seed;

	public GClass3 quad;

	public int count;

	public PQSLandControl.LandClassScatter scatter;

	public void Setup(GClass3 quad, int seed, PQSLandControl.LandClassScatter scatter, int count)
	{
		this.quad = quad;
		this.seed = seed;
		this.count = count;
		this.scatter = scatter;
		base.gameObject.name = "_" + quad.gameObject.name;
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
		if (quad.sphereRoot.surfaceRelativeQuads)
		{
			base.transform.localPosition = quad.positionPlanet;
			base.transform.localRotation = Quaternion.identity;
		}
		else
		{
			base.transform.localPosition = Vector3.zero;
			base.transform.localRotation = Quaternion.identity;
		}
	}

	public void Destroy()
	{
		if (isVisible)
		{
			isVisible = false;
			if (isBuilt)
			{
				obj.SetActive(value: false);
				isBuilt = false;
			}
		}
		if (quad != null)
		{
			GClass3 gClass = quad;
			gClass.onVisible = (GClass3.QuadDelegate)Delegate.Remove(gClass.onVisible, new GClass3.QuadDelegate(OnQuadVisible));
			GClass3 gClass2 = quad;
			gClass2.onInvisible = (GClass3.QuadDelegate)Delegate.Remove(gClass2.onInvisible, new GClass3.QuadDelegate(OnQuadInvisible));
			GClass3 gClass3 = quad;
			gClass3.onDestroy = (GClass3.QuadDelegate)Delegate.Remove(gClass3.onDestroy, new GClass3.QuadDelegate(OnQuadDestroy));
			GClass3 gClass4 = quad;
			gClass4.onUpdate = (GClass3.QuadDelegate)Delegate.Remove(gClass4.onUpdate, new GClass3.QuadDelegate(OnQuadUpdate));
			quad = null;
		}
	}

	public void OnQuadVisible(GClass3 quad)
	{
		if (!isVisible)
		{
			isVisible = true;
			if (isBuilt)
			{
				obj.SetActive(value: true);
			}
		}
	}

	public void OnQuadInvisible(GClass3 quad)
	{
		if (isVisible)
		{
			isVisible = false;
			if (isBuilt)
			{
				obj.SetActive(value: false);
			}
		}
	}

	public void OnQuadDestroy(GClass3 quad)
	{
		scatter.DestroyQuad(this);
	}

	public void OnQuadUpdate(GClass3 quad)
	{
		if (!isBuilt && isVisible && quad.sphereRoot.quadAllowBuild && quad.sphereRoot.targetSpeed < scatter.maxSpeed)
		{
			scatter.CreateScatterMesh(this);
			isBuilt = true;
			obj.SetActive(value: true);
			quad.onUpdate = (GClass3.QuadDelegate)Delegate.Remove(quad.onUpdate, new GClass3.QuadDelegate(OnQuadUpdate));
		}
	}

	public void OnDestroy()
	{
		UnityEngine.Object.DestroyImmediate(mesh);
	}
}
