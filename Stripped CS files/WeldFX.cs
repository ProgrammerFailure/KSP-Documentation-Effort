using System.Collections;
using UnityEngine;

public class WeldFX : MonoBehaviour
{
	public MeshRenderer mesh;

	public LineRenderer LaserLine;

	public Light WeldLight;

	public ParticleSystem Flare;

	public ParticleSystem Sparks;

	public float MaxIntensity = 10f;

	public float intensityDeltaThreshold = 0.01f;

	public float LightFadeInDamper = 0.5f;

	public float LightFadeOutDamper = 1f;

	public float SpaceSparksSpeed = 0.3f;

	public float LaserOffset = 0.035f;

	public RaycastHit laserHitInfo;

	public Transform WeldingTransform;

	public Transform WeldingTransformFl;

	public Transform RestTransform;

	public float StartDelay = 1.1f;

	public float LaserDuration = 1.6f;

	public bool IsFloating;

	public bool aimingEnabled;

	public Transform AimPivot;

	[HideInInspector]
	public KerbalEVA evaController;

	[HideInInspector]
	public bool LaserFXActive;

	[HideInInspector]
	public bool LightFXActive;

	[HideInInspector]
	public bool Active;

	public float raycastRange = float.MaxValue;

	public Vector3 attachPoint;

	public void Awake()
	{
		Reset();
	}

	public void Start()
	{
		raycastRange = GameSettings.EVA_CONSTRUCTION_RANGE * 1.2f;
	}

	[ContextMenu("Play")]
	public void Play()
	{
		Reset();
		StartCoroutine(PlaySequence());
	}

	public IEnumerator PlaySequence()
	{
		GameEvents.OnEVAConstructionWeldStart.Fire(evaController);
		GetAttachPoint();
		SetSparksGravity();
		yield return new WaitForSeconds(StartDelay);
		LaserFXActive = true;
		LightFXActive = true;
		WeldLight.enabled = true;
		FadeInLight();
		AdjustWeldingTransform(isWelding: true);
		Flare.Play();
		Sparks.Play();
		aimingEnabled = true;
		yield return new WaitForSeconds(0.15f);
		LaserLine.enabled = true;
		yield return new WaitForSeconds(LaserDuration);
		aimingEnabled = false;
		Stop();
		AdjustWeldingTransform(isWelding: false);
		GameEvents.OnEVAConstructionWeldFinish.Fire(evaController);
	}

	public void GetAttachPoint()
	{
		if (evaController.constructionTarget.attachMode == AttachModes.SRF_ATTACH)
		{
			attachPoint = evaController.constructionTarget.transform.TransformPoint(evaController.constructionTarget.srfAttachNode.position);
			return;
		}
		for (int i = 0; i < evaController.constructionTarget.attachNodes.Count; i++)
		{
			if (evaController.constructionTarget.attachNodes[i].attachedPart == evaController.constructionTarget.parent)
			{
				attachPoint = evaController.constructionTarget.transform.TransformPoint(evaController.constructionTarget.attachNodes[i].position);
			}
		}
	}

	public void Update()
	{
		if (!Active)
		{
			return;
		}
		if (LaserFXActive)
		{
			if (Physics.Raycast(LaserLine.transform.position, attachPoint - LaserLine.transform.position, out laserHitInfo, raycastRange, LayerUtil.DefaultEquivalent, QueryTriggerInteraction.Ignore))
			{
				WeldLight.transform.position = laserHitInfo.point - LaserLine.transform.forward * LaserOffset;
				Flare.transform.position = WeldLight.transform.position;
				Sparks.transform.position = WeldLight.transform.position;
				LaserLine.SetPosition(1, LaserLine.transform.InverseTransformPoint(laserHitInfo.point));
			}
			if (aimingEnabled)
			{
				UpdateGunAiming();
			}
			FadeInLight();
		}
		else if (LightFXActive)
		{
			FadeOutLight();
		}
	}

	[ContextMenu("Stop")]
	public void Stop()
	{
		StopAllCoroutines();
		LaserFXActive = false;
		LaserLine.SetPosition(1, Vector3.zero);
		LaserLine.enabled = false;
		WeldLight.transform.SetParent(null, worldPositionStays: true);
		Flare.transform.SetParent(null, worldPositionStays: true);
		Sparks.transform.SetParent(null, worldPositionStays: true);
		Flare.Stop();
		Sparks.Stop();
	}

	public void Reset()
	{
		LaserFXActive = false;
		LaserLine.SetPosition(1, Vector3.zero);
		WeldLight.transform.SetParent(LaserLine.transform.parent, worldPositionStays: true);
		WeldLight.transform.position = Vector3.zero;
		Flare.transform.SetParent(mesh.transform.parent, worldPositionStays: true);
		Flare.transform.position = Vector3.zero;
		Sparks.transform.SetParent(mesh.transform.parent, worldPositionStays: true);
		Sparks.transform.position = Vector3.zero;
		LaserLine.enabled = false;
		WeldLight.intensity = 0f;
		if (evaController != null)
		{
			evaController.VisorRenderer.material.mainTextureOffset.Set(0f, 0f);
		}
	}

	public void FadeInLight()
	{
		WeldLight.enabled = true;
		WeldLight.intensity = Mathf.Lerp(WeldLight.intensity, MaxIntensity, Time.deltaTime * LightFadeInDamper);
	}

	public void FadeOutLight()
	{
		if (WeldLight.intensity < intensityDeltaThreshold && WeldLight.intensity != 0f)
		{
			WeldLight.intensity = 0f;
			WeldLight.enabled = false;
			LightFXActive = false;
		}
		else
		{
			WeldLight.intensity = Mathf.Lerp(WeldLight.intensity, 0f, Time.deltaTime * LightFadeOutDamper);
		}
	}

	public void AdjustWeldingTransform(bool isWelding)
	{
		if (isWelding)
		{
			AimPivot.SetParent(IsFloating ? WeldingTransformFl : WeldingTransform, worldPositionStays: false);
		}
		else
		{
			AimPivot.SetParent(RestTransform, worldPositionStays: false);
		}
		AimPivot.localPosition = Vector3.zero;
		AimPivot.localRotation = Quaternion.identity;
	}

	public void UpdateGunAiming()
	{
		Vector3 vector = evaController.constructionTarget.transform.position - evaController.constructionTarget.transform.forward * evaController.constructionTargetPivotOffset - AimPivot.position;
		AimPivot.rotation = Quaternion.LookRotation(vector.normalized, evaController.transform.up);
	}

	public void EnableMesh(float delay = 0f)
	{
		StartCoroutine(ToggleMesh(meshEnabled: true, delay));
	}

	public void DisableMesh(float delay = 0f)
	{
		StartCoroutine(ToggleMesh(meshEnabled: false, delay));
	}

	public IEnumerator ToggleMesh(bool meshEnabled, float delay)
	{
		yield return new WaitForSeconds(delay);
		mesh.enabled = meshEnabled;
	}

	public void SetSparksGravity()
	{
		ParticleSystem.MainModule main = Sparks.main;
		ParticleSystem.ForceOverLifetimeModule forceOverLifetime = Sparks.forceOverLifetime;
		if (!IsFloating)
		{
			forceOverLifetime.enabled = true;
			Vector3 vector = evaController.vessel.graviticAcceleration;
			forceOverLifetime.x = vector.x;
			forceOverLifetime.y = vector.y;
			forceOverLifetime.z = vector.z;
			main.simulationSpeed = 1f;
		}
		else
		{
			forceOverLifetime.enabled = false;
			main.simulationSpeed = SpaceSparksSpeed;
		}
	}
}
