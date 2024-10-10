using UnityEngine;

public class ModuleSurfaceFX : PartModule
{
	public enum SurfaceType
	{
		None,
		Terrain,
		Water,
		Launchpad
	}

	[KSPField]
	public int thrustProviderModuleIndex = -1;

	[KSPField]
	public float fxMax = 1f;

	[KSPField]
	public float maxDistance = 50f;

	[KSPField]
	public float falloff = 2f;

	[KSPField]
	public string thrustTransformName = "";

	public GameObject terrainPrefab;

	public GameObject waterPrefab;

	public IThrustProvider engineModule;

	public Transform trf;

	public RaycastHit hitInfo;

	public SurfaceType hit;

	public float fxScale = 1f;

	public float distance;

	public float scaledDistance;

	public float h0;

	public float dH;

	public bool raycastHit;

	public Vector3 rDir;

	public Vector3 Vsrf;

	public Vector3 point;

	public Vector3 normal;

	public float ScaledFX;

	public SurfaceFX srfFX;

	public SurfaceFX srfFXnext;

	public LaunchPadFX padFX;

	public void Start()
	{
		terrainPrefab = AssetBase.GetPrefab("SurfaceFX");
		waterPrefab = AssetBase.GetPrefab("WaterFX");
		if (!string.IsNullOrEmpty(thrustTransformName))
		{
			trf = base.part.FindModelTransform(thrustTransformName);
		}
		else
		{
			trf = base.part.partTransform;
		}
		if (thrustProviderModuleIndex != -1)
		{
			engineModule = base.part.Modules[thrustProviderModuleIndex] as IThrustProvider;
		}
		if (engineModule == null)
		{
			Debug.LogError("[ModuleSrfFX]: No IThrustProvider module found at index " + thrustProviderModuleIndex + "!", base.gameObject);
		}
	}

	public void Update()
	{
		if (!HighLogic.LoadedSceneIsFlight || !GameSettings.SURFACE_FX)
		{
			return;
		}
		if (engineModule != null)
		{
			fxScale = engineModule.GetCurrentThrust() / engineModule.GetMaxThrust() * fxMax;
		}
		else
		{
			fxScale = 0f;
		}
		if (fxScale > 0f && (!base.part.vessel.mainBody.ocean || FlightGlobals.getAltitudeAtPos((Vector3d)trf.position, base.part.vessel.mainBody) > 0.0))
		{
			raycastHit = Physics.Raycast(trf.position, trf.forward, out hitInfo, maxDistance, 1073774592);
			float num;
			float altitudeAtPos;
			if (raycastHit && FlightGlobals.GetSqrAltitude(hitInfo.point, base.part.vessel.mainBody) >= 0.0)
			{
				if (hitInfo.collider.CompareTag("LaunchpadFX"))
				{
					hit = SurfaceType.Launchpad;
				}
				else if (hitInfo.collider.CompareTag("Wheel_Piston_Collider"))
				{
					hit = SurfaceType.None;
					Vsrf = Vector3.zero;
					ScaledFX = 0f;
				}
				else
				{
					hit = SurfaceType.Terrain;
				}
				point = hitInfo.point;
				normal = hitInfo.normal;
				distance = hitInfo.distance;
			}
			else if (base.part.vessel.mainBody.ocean && (num = Vector3.Dot(trf.forward, -base.vessel.upAxis)) > 0f && (altitudeAtPos = FlightGlobals.getAltitudeAtPos(trf.position, base.vessel.mainBody)) < maxDistance && altitudeAtPos > 0f)
			{
				normal = base.vessel.upAxis;
				distance = altitudeAtPos / num;
				point = trf.position + trf.forward * distance;
				hit = SurfaceType.Water;
			}
			else
			{
				hit = SurfaceType.None;
			}
			scaledDistance = Mathf.Pow(1f - distance / maxDistance, falloff);
			ScaledFX = fxScale * scaledDistance;
			rDir = point - trf.position;
			Vsrf = Vector3.ProjectOnPlane(rDir, normal).normalized * fxScale;
		}
		else
		{
			hit = SurfaceType.None;
			Vsrf = Vector3.zero;
			ScaledFX = 0f;
		}
		UpdateSrfFX(hitInfo);
	}

	public void OnDrawGizmos()
	{
		if (hit != 0)
		{
			Gizmos.color = XKCDColors.Red;
			Gizmos.DrawWireSphere(point, 2f);
			Gizmos.DrawSphere(point, 0.25f);
		}
	}

	public void UpdateSrfFX(RaycastHit hitInfo)
	{
		switch (hit)
		{
		default:
			srfFXnext = null;
			padFX = null;
			break;
		case SurfaceType.Terrain:
			srfFXnext = GetSurfaceFX(srfFX, terrainPrefab, point, normal);
			padFX = null;
			break;
		case SurfaceType.Water:
			srfFXnext = GetSurfaceFX(srfFX, waterPrefab, point, normal);
			padFX = null;
			break;
		case SurfaceType.Launchpad:
			srfFXnext = null;
			if (padFX == null)
			{
				padFX = hitInfo.collider.gameObject.GetComponent<LaunchPadFX>();
			}
			break;
		}
		if (srfFXnext != srfFX)
		{
			if (srfFX != null)
			{
				srfFX.RemoveSource(this);
			}
			if (srfFXnext != null)
			{
				srfFXnext.AddSource(this);
			}
			srfFX = srfFXnext;
		}
		if (padFX != null)
		{
			padFX.AddFX(ScaledFX);
		}
	}

	public SurfaceFX GetSurfaceFX(SurfaceFX sFX, GameObject prefabToSpawn, Vector3 wPos, Vector3 wNormal)
	{
		SurfaceFX surfaceFX = SurfaceFX.FindNearestFX(this, wPos);
		if (surfaceFX != null)
		{
			if (sFX == null)
			{
				return surfaceFX;
			}
			if (sFX.ScaledFX < surfaceFX.ScaledFX)
			{
				return surfaceFX;
			}
			return sFX;
		}
		if (sFX != null && sFX.leadSource == this && sFX.prefab == prefabToSpawn)
		{
			return sFX;
		}
		surfaceFX = Object.Instantiate(prefabToSpawn).GetComponent<SurfaceFX>();
		surfaceFX.prefab = prefabToSpawn;
		return surfaceFX;
	}

	public void OnDestroy()
	{
		if (srfFX != null)
		{
			srfFX.RemoveSource(this);
		}
	}
}
