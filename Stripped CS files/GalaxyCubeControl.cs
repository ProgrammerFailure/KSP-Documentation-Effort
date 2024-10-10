using System;
using UnityEngine;

public class GalaxyCubeControl : MonoBehaviour
{
	public Color minGalaxyColor = Color.black;

	public Color maxGalaxyColor = Color.white;

	public float atmosFadeLimit = 0.6f;

	public float glareFadeLimit = 0.6f;

	public float daytimeFadeLimit = 0.9f;

	public float airPressureFade = 1.1f;

	public float glareFadeLerpRate = 0.05f;

	public Sun sunRef;

	public Camera tgt;

	public double sunSrfAngle;

	public double sunCamAngle;

	public QuaternionD initRot;

	public Renderer[] cubeRenderers;

	public bool lineOfSightToSun;

	public float atmosFade;

	public float dayTimeFade;

	public float glareFade;

	public float totalFade;

	public int layerMask;

	public MaterialPropertyBlock mpb;

	public static GalaxyCubeControl Instance;

	public RaycastHit lineOfSightToSunHit;

	public void Awake()
	{
		Instance = this;
	}

	public void SetEnabled(bool state)
	{
		base.enabled = state;
		base.gameObject.SetActive(state);
	}

	public void Start()
	{
		initRot = base.transform.rotation;
		cubeRenderers = base.gameObject.GetComponentsInChildren<Renderer>();
		layerMask = 1 << LayerMask.NameToLayer("Scaled Scenery");
		mpb = new MaterialPropertyBlock();
	}

	public void OnDestroy()
	{
		if (Instance != null && Instance == this)
		{
			Instance = null;
		}
	}

	public void Update()
	{
		lineOfSightToSun = LineOfSightToSun();
		if (lineOfSightToSun)
		{
			sunCamAngle = Math.Acos(Vector3d.Dot(-sunRef.sunDirection, tgt.transform.forward)) * (180.0 / Math.PI);
			if (double.IsNaN(sunCamAngle))
			{
				sunCamAngle = 0.0;
			}
			double num = (double)tgt.fieldOfView * 0.5;
			float t = ((!(sunCamAngle < num)) ? Mathf.Clamp01((float)((sunCamAngle + 10.0 - num) * 0.05)) : 0f);
			glareFade = Mathf.Lerp(glareFade, Mathf.Lerp(glareFadeLimit, 0f, t), glareFadeLerpRate);
		}
		else
		{
			sunCamAngle = 0.0;
			glareFade = Mathf.Lerp(glareFade, 0f, glareFadeLerpRate);
		}
		if ((HighLogic.LoadedScene == GameScenes.TRACKSTATION || HighLogic.LoadedScene == GameScenes.FLIGHT) && MapView.MapIsEnabled)
		{
			atmosFade = 0f;
			dayTimeFade = 0f;
		}
		else
		{
			Vector3d vector3d = ScaledSpace.ScaledToLocalSpace(tgt.transform.position);
			CelestialBody mainBody = FlightGlobals.getMainBody(vector3d);
			double staticPressure = FlightGlobals.getStaticPressure(mainBody.GetAltitude(vector3d), mainBody);
			staticPressure *= 0.009869232667160128;
			if (staticPressure > 0.0)
			{
				sunSrfAngle = Math.Acos(Vector3d.Dot(-sunRef.sunDirection, FlightGlobals.getUpAxis(vector3d))) * (180.0 / Math.PI);
				sunSrfAngle = UtilMath.Clamp01((sunSrfAngle - 75.0) * 0.025);
				if (double.IsNaN(sunSrfAngle))
				{
					sunSrfAngle = 0.0;
				}
				dayTimeFade = (float)staticPressure * Mathf.Lerp(daytimeFadeLimit, 0f, (float)sunSrfAngle);
				atmosFade = Mathf.Lerp(0f, atmosFadeLimit, (float)staticPressure * airPressureFade);
			}
			else
			{
				dayTimeFade = 0f;
				atmosFade = 0f;
			}
		}
		totalFade = atmosFade + dayTimeFade + glareFade;
		mpb.SetColor(PropertyIDs._Color, Color.Lerp(maxGalaxyColor, minGalaxyColor, totalFade));
		for (int i = 0; i < cubeRenderers.Length; i++)
		{
			cubeRenderers[i].SetPropertyBlock(mpb);
		}
	}

	public void LateUpdate()
	{
		base.transform.rotation = Planetarium.Rotation * initRot;
	}

	public bool LineOfSightToSun()
	{
		Vector3 direction = ScaledSun.Instance.transform.position - ScaledCamera.Instance.transform.position;
		if (Physics.Raycast(new Ray(ScaledCamera.Instance.transform.position, direction), out lineOfSightToSunHit, direction.magnitude, layerMask))
		{
			if (lineOfSightToSunHit.collider.gameObject == ScaledSun.Instance.gameObject)
			{
				return true;
			}
			return false;
		}
		return true;
	}
}
