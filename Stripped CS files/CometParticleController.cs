using UnityEngine;

public class CometParticleController : MonoBehaviour
{
	[SerializeField]
	public ParticleSystem pSystem;

	[SerializeField]
	public ParticleSystem.MainModule pMain;

	[SerializeField]
	public ParticleSystem.MinMaxCurve pStartSize;

	[SerializeField]
	public ParticleSystem.MinMaxCurve pStartLife;

	[SerializeField]
	public ParticleSystem.MinMaxCurve pStartSpeed;

	[SerializeField]
	public ParticleSystem.EmissionModule pEmission;

	[SerializeField]
	public ParticleSystem.MinMaxCurve pEmissionRate;

	[SerializeField]
	public ParticleSystem.ShapeModule pShape;

	[SerializeField]
	public ParticleSystem.ForceOverLifetimeModule pForce;

	[SerializeField]
	public ParticleSystem.MinMaxCurve pForceX;

	[SerializeField]
	public ParticleSystem.MinMaxCurve pForceY;

	[SerializeField]
	public ParticleSystem.SizeOverLifetimeModule pSize;

	public CometVessel comet;

	public float tailWidthComaRatio = 1f;

	public float tailWidthToEmitterScale = 0.6f;

	public float maxEmitterLength = 4000000f;

	public FloatCurve logLengthToSpeed;

	public FloatCurve logLengthToRate;

	public float tailWidthToParticleSizeMin = 2.5f;

	public float tailWidthToParticleSizeMax = 2.85f;

	public FloatCurve speedToSpreadForce;

	public float retrogradeSpreadRatio = 0.4f;

	public float normalSpreadRatio = 0.2f;

	[SerializeField]
	public float minLifeRatio = 0.5f;

	[Tooltip("Color that will be set on the emitter to drive the particles")]
	public Color emitterColor = new Color(1f, 1f, 1f, 0.2f);

	public float calcedEmitterRadius;

	public float maxSimSpeed = 100f;

	public float minSimSpeed = 0.01f;

	[SerializeField]
	public float currentSimSpeed = 1f;

	[SerializeField]
	public float maxWarpSpeed;

	[SerializeField]
	public bool vesselDead;

	public float debugEmitterMultiplier = 1f;

	public float debugSizeMultiplier = 1f;

	public float initialSizeMax = 14f;

	public bool triggerPreWarm;

	public void Start()
	{
		pMain = pSystem.main;
		pEmission = pSystem.emission;
		pEmissionRate = pEmission.rateOverTime;
		pShape = pSystem.shape;
		pStartSize = pMain.startSize;
		pStartLife = pMain.startLifetime;
		pStartSpeed = pMain.startSpeed;
		pForce = pSystem.forceOverLifetime;
		pForceX = pForce.x;
		pForceY = pForce.y;
		pSize = pSystem.sizeOverLifetime;
		initialSizeMax = pSize.sizeMultiplier;
		currentSimSpeed = 1f;
		maxWarpSpeed = TimeWarp.MaxRate;
		triggerPreWarm = true;
		GameEvents.onPartWillDie.Add(OnPartWillDie);
	}

	public void OnDestroy()
	{
		GameEvents.onPartWillDie.Remove(OnPartWillDie);
	}

	public void OnPartWillDie(Part p)
	{
		if (comet != null && p.persistentId == comet.cometPartId)
		{
			vesselDead = true;
		}
	}

	public void Update()
	{
		currentSimSpeed = Mathf.Lerp(minSimSpeed, maxSimSpeed, Mathf.Clamp01((TimeWarp.CurrentRate - 1f) / maxWarpSpeed));
		pMain.simulationSpeed = currentSimSpeed;
		if (comet != null)
		{
			calcedEmitterRadius = comet.ComaRadiusScaledSpace * tailWidthComaRatio * tailWidthToEmitterScale;
			pShape.radius = calcedEmitterRadius;
			float time = Mathf.Log10(Mathf.Min(comet.TailLengthScaledSpace, maxEmitterLength));
			pStartSize.constantMin = comet.ComaRadiusScaledSpace * tailWidthComaRatio * tailWidthToParticleSizeMin;
			pStartSize.constantMax = comet.ComaRadiusScaledSpace * tailWidthComaRatio * tailWidthToParticleSizeMax;
			pMain.startSize = pStartSize;
			pEmissionRate.constantMin = logLengthToRate.Evaluate(time) / minSimSpeed * debugEmitterMultiplier;
			pEmissionRate.constantMax = pEmissionRate.constantMin * 1.5f;
			pEmission.rateOverTime = pEmissionRate;
			float num = logLengthToSpeed.Evaluate(time);
			pStartSpeed.constant = num / minSimSpeed;
			pMain.startSpeed = pStartSpeed;
			pStartLife.constantMax = comet.TailLengthScaledSpace / pMain.startSpeed.constant / base.transform.localScale.z;
			pStartLife.constantMin = pStartLife.constantMax * minLifeRatio;
			pMain.startLifetime = pStartLife;
			pSize.sizeMultiplier = initialSizeMax * debugSizeMultiplier;
			if (pForce.enabled)
			{
				float num2 = (0f - speedToSpreadForce.Evaluate(num)) / minSimSpeed / minSimSpeed;
				pForceX.constantMax = num2;
				pForceX.constantMin = num2 - num2 * retrogradeSpreadRatio;
				pForce.x = pForceX;
				pForceY.constantMin = num2 * normalSpreadRatio / 2f;
				pForceY.constantMax = num2 * (0f - normalSpreadRatio) / 2f;
				pForce.y = pForceY;
			}
			Color color = emitterColor;
			color.a *= comet.atmosphereVFXRatio;
			pMain.startColor = color;
		}
		if (triggerPreWarm && !vesselDead)
		{
			currentSimSpeed = Mathf.Lerp(minSimSpeed, maxSimSpeed, Mathf.Clamp01((TimeWarp.CurrentRate - 1f) / maxWarpSpeed));
			pMain.simulationSpeed = currentSimSpeed;
			pSystem.Stop();
			pSystem.Play();
			triggerPreWarm = false;
		}
	}
}
