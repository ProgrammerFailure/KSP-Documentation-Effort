using System;
using UnityEngine;

public class PQSMod_ROCScatterQuad : MonoBehaviour
{
	public GameObject rocObj;

	public bool isVisible;

	public bool isBuilt;

	public int quadSeed;

	public GClass3 quad;

	public int count;

	public LandClassROC roc;

	public bool isSetup;

	public void Setup(GClass3 quad, int quadSeed, LandClassROC roc, int count, int rocIDCounter)
	{
		this.quad = quad;
		this.quadSeed = quadSeed;
		this.count = count;
		this.roc = roc;
		base.gameObject.name = "_" + roc.rocName;
		GClass0 component = base.gameObject.GetComponent<GClass0>();
		if (component != null)
		{
			component.rocID = quadSeed + roc.rocName.GetHashCode_Net35() + rocIDCounter;
		}
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
		isSetup = true;
	}

	public void Destroy()
	{
		if (rocObj != null)
		{
			int childCount = rocObj.transform.childCount;
			while (childCount-- > 0)
			{
				Transform child = rocObj.transform.GetChild(childCount);
				if (child != null)
				{
					child.gameObject.DestroyGameObjectImmediate();
				}
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
		isBuilt = false;
		isVisible = false;
		isSetup = false;
	}

	public void OnQuadVisible(GClass3 quad)
	{
		if (!isVisible)
		{
			isVisible = true;
			if (isBuilt)
			{
				rocObj.SetActive(value: true);
			}
			if (!isSetup)
			{
				Debug.LogWarningFormat("[PQSMod_ROCScatterQuad]: {0} - Quad {1} Call to OnQuadVisible but not setup!", base.name, quad.name);
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
				rocObj.SetActive(value: false);
			}
			if (!isSetup)
			{
				Debug.LogWarningFormat("[PQSMod_ROCScatterQuad]: {0} - Quad {1} Call to OnQuadInvisible but not setup!", base.name, quad.name);
			}
		}
	}

	public void OnQuadDestroy(GClass3 quad)
	{
		roc.DestroyQuad(this);
	}

	public void OnQuadUpdate(GClass3 quad)
	{
		if (isBuilt || !isVisible || !quad.sphereRoot.quadAllowBuild || !(quad.sphereRoot.target != null) || !(quad.sphereRoot.targetSpeed < (double)GameSettings.SERENITY_ROCS_VISUAL_SPEED) || quad.subdivision < roc.minLevel)
		{
			return;
		}
		if (!isSetup)
		{
			Debug.LogWarningFormat("[PQSMod_ROCScatterQuad]: {0} - Quad {1} Call to OnQuadUpdate but not setup!", base.name, quad.name);
			return;
		}
		if (rocObj.transform.childCount <= 0)
		{
			roc.CreateScatterMesh(this);
		}
		isBuilt = true;
		rocObj.SetActive(value: true);
		quad.onUpdate = (GClass3.QuadDelegate)Delegate.Remove(quad.onUpdate, new GClass3.QuadDelegate(OnQuadUpdate));
	}
}
