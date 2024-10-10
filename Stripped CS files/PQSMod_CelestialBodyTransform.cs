using System;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("PQuadSphere/Mods/Misc/KSP Celestial Body Link")]
public class PQSMod_CelestialBodyTransform : PQSMod
{
	[Serializable]
	public class AltitudeFade
	{
		public string fadeFloatName;

		public int fadeIntID = -1;

		public float fadeStart;

		public float fadeEnd;

		public float valueStart;

		public float valueEnd;

		public float highQualityShaderFadeStart = -1f;

		public float highQualityShaderFadeEnd = -1f;

		public List<GameObject> secondaryRenderers;

		public List<Renderer> renderers;

		public List<GClass4> pqs;

		public float a;

		public void DoFade(double alt)
		{
			float num = fadeStart;
			if (highQualityShaderFadeStart > 0f && GameSettings.TERRAIN_SHADER_QUALITY >= 2)
			{
				num = highQualityShaderFadeStart;
			}
			float num2 = fadeEnd;
			if (highQualityShaderFadeEnd > 0f && GameSettings.TERRAIN_SHADER_QUALITY >= 2)
			{
				num2 = highQualityShaderFadeEnd;
			}
			if (alt <= (double)num)
			{
				a = valueStart;
			}
			else if (alt >= (double)num2)
			{
				FloatingOrigin.ResetTerrainShaderOffset();
				a = valueEnd;
			}
			else
			{
				a = (float)((alt - (double)num) / (double)(num2 - num));
			}
			int count = renderers.Count;
			while (count-- > 0)
			{
				renderers[count].sharedMaterial.SetFloat(fadeIntID, a);
			}
			int count2 = pqs.Count;
			while (count2-- > 0)
			{
				pqs[count2].surfaceMaterial.SetFloat(fadeIntID, a);
				for (int i = 0; i < pqs[count2].materialsForUpdates.Count; i++)
				{
					pqs[count2].materialsForUpdates[i].SetFloat(fadeIntID, a);
				}
			}
		}

		public void DoFade(bool fade)
		{
			if (fade)
			{
				a = valueStart;
			}
			else
			{
				a = valueEnd;
			}
			int count = renderers.Count;
			while (count-- > 0)
			{
				renderers[count].sharedMaterial.SetFloat(fadeIntID, a);
			}
			int count2 = pqs.Count;
			while (count2-- > 0)
			{
				pqs[count2].surfaceMaterial.SetFloat(fadeIntID, a);
				for (int i = 0; i < pqs[count2].materialsForUpdates.Count; i++)
				{
					pqs[count2].materialsForUpdates[i].SetFloat(fadeIntID, a);
				}
			}
		}

		public void Setup()
		{
			fadeIntID = Shader.PropertyToID(fadeFloatName);
			renderers = new List<Renderer>();
			pqs = new List<GClass4>();
			int count = secondaryRenderers.Count;
			while (count-- > 0)
			{
				GClass4 component = secondaryRenderers[count].GetComponent<GClass4>();
				if (component != null)
				{
					if (!pqs.Contains(component))
					{
						pqs.Add(component);
					}
					continue;
				}
				Renderer[] componentsInChildren = secondaryRenderers[count].GetComponentsInChildren<Renderer>();
				foreach (Renderer item in componentsInChildren)
				{
					if (!renderers.Contains(item))
					{
						renderers.Add(item);
					}
				}
			}
		}

		public void Setup(GClass4 pqs)
		{
			Setup();
			if (!this.pqs.Contains(pqs))
			{
				this.pqs.Add(pqs);
			}
		}
	}

	[HideInInspector]
	public CelestialBody body;

	public double deactivateAltitude;

	public double highQualityShaderDeactivateAltitude = -1.0;

	public bool forceRebuildOnTargetChange;

	public AltitudeFade planetFade;

	public AltitudeFade[] secondaryFades;

	public bool forceActivate;

	public bool overrideFade;

	public void Reset()
	{
		deactivateAltitude = 90500.0;
		forceRebuildOnTargetChange = false;
		planetFade = new AltitudeFade();
		planetFade.fadeFloatName = "_FadeAltitude";
		planetFade.fadeStart = 0f;
		planetFade.fadeEnd = 1f;
		planetFade.valueStart = 0f;
		planetFade.valueEnd = 1f;
		secondaryFades = new AltitudeFade[0];
	}

	public override void OnSetup()
	{
		body = sphere.gameObject.GetComponentUpwards<CelestialBody>();
		if (body == null)
		{
			Debug.LogWarning("CelestialBodyTransform: Cannot find CelestialBody.");
			forceActivate = true;
		}
		planetFade.Setup(sphere);
		int num = secondaryFades.Length;
		while (num-- > 0)
		{
			secondaryFades[num].Setup();
		}
		if (forceActivate)
		{
			planetFade.DoFade(fade: true);
			int num2 = secondaryFades.Length;
			while (num2-- > 0)
			{
				secondaryFades[num2].DoFade(fade: true);
			}
		}
		else
		{
			planetFade.DoFade(fade: false);
			int num3 = secondaryFades.Length;
			while (num3-- > 0)
			{
				secondaryFades[num3].DoFade(fade: false);
			}
		}
	}

	public override bool OnSphereStart()
	{
		if (forceActivate)
		{
			if (sphere.target == null)
			{
				if (!(sphere.secondaryTarget != null))
				{
					Debug.LogError("[PQS Error]: Sphere target is null!", sphere.gameObject);
					return true;
				}
				sphere.SetTarget(sphere.secondaryTarget);
			}
			sphere.isAlive = true;
			return true;
		}
		if (FlightGlobals.fetch != null)
		{
			if (FlightGlobals.ActiveVessel != null)
			{
				sphere.SetTarget(FlightGlobals.ActiveVessel.transform);
			}
			if (FlightGlobals.currentMainBody == body)
			{
				sphere.isAlive = true;
			}
		}
		return true;
	}

	public override void OnPreUpdate()
	{
		if (forceActivate)
		{
			if (!sphere.isActive)
			{
				sphere.ActivateSphere();
				sphere.isAlive = true;
			}
		}
		else if (FlightGlobals.fetch == null)
		{
			if (!sphere.isActive)
			{
				sphere.ActivateSphere();
			}
		}
		else
		{
			if (HighLogic.LoadedScene == GameScenes.PSYSTEM || HighLogic.LoadedScene == GameScenes.MAINMENU)
			{
				return;
			}
			if (HighLogic.LoadedScene == GameScenes.MISSIONBUILDER)
			{
				if (!sphere.isActive)
				{
					sphere.ActivateSphere();
				}
				return;
			}
			if (FlightGlobals.fetch.activeVessel != sphere.target)
			{
				if (FlightGlobals.fetch.activeVessel == null)
				{
					sphere.SetTarget(null);
					return;
				}
				sphere.SetTarget(FlightGlobals.fetch.activeVessel.transform);
			}
			if (FlightGlobals.currentMainBody != body)
			{
				if (sphere.isActive)
				{
					Debug.Log(sphere.gameObject.name + ": Resetting");
					sphere.DeactivateSphere();
					sphere.ResetAndWait();
				}
			}
			else if (sphere.isActive)
			{
				if (MapView.MapIsEnabled || (sphere.visibleAltitude > deactivateAltitude && (GameSettings.TERRAIN_SHADER_QUALITY < 2 || highQualityShaderDeactivateAltitude < 0.0)) || (highQualityShaderDeactivateAltitude > 0.0 && sphere.visibleAltitude > highQualityShaderDeactivateAltitude && GameSettings.TERRAIN_SHADER_QUALITY >= 2))
				{
					sphere.DeactivateSphere();
				}
			}
			else if (!MapView.MapIsEnabled && ((sphere.visibleAltitude < deactivateAltitude && (GameSettings.TERRAIN_SHADER_QUALITY < 2 || highQualityShaderDeactivateAltitude < 0.0)) || (highQualityShaderDeactivateAltitude > 0.0 && sphere.visibleAltitude < highQualityShaderDeactivateAltitude && GameSettings.TERRAIN_SHADER_QUALITY >= 2)))
			{
				sphere.ActivateSphere();
			}
		}
	}

	public override void OnSphereTransformUpdate()
	{
		if (body != null)
		{
			sphere.transformPosition = body.position;
			sphere.transformRotation = body.rotation;
		}
		else
		{
			sphere.transformPosition = sphere.transform.position;
			sphere.transformRotation = sphere.transform.rotation;
		}
	}

	public override void OnUpdateFinished()
	{
		if (!overrideFade)
		{
			if (sphere.isActive)
			{
				planetFade.DoFade(sphere.visibleAltitude);
				int num = secondaryFades.Length;
				while (num-- > 0)
				{
					secondaryFades[num].DoFade(sphere.visibleAltitude);
				}
			}
		}
		else
		{
			planetFade.DoFade(0.0);
			int num2 = secondaryFades.Length;
			while (num2-- > 0)
			{
				secondaryFades[num2].DoFade(0.0);
			}
		}
	}

	public override void OnSphereReset()
	{
		FadeOutSphere();
	}

	public override void OnSphereInactive()
	{
		FadeOutSphere();
	}

	public void FadeOutSphere()
	{
		planetFade.DoFade(fade: false);
		int num = secondaryFades.Length;
		while (num-- > 0)
		{
			secondaryFades[num].DoFade(fade: false);
		}
	}
}
