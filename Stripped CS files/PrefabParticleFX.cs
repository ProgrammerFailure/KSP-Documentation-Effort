using UnityEngine;

[EffectDefinition("PREFAB_PARTICLE")]
public class PrefabParticleFX : EffectBehaviour
{
	[Persistent]
	public string prefabName = "";

	[Persistent]
	public string transformName = "";

	public FXCurve emission = new FXCurve("emission", 1f);

	public FXCurve energy = new FXCurve("energy", 1f);

	public FXCurve speed = new FXCurve("speed", 1f);

	[Persistent]
	public Vector3 localOffset = Vector3.zero;

	[Persistent]
	public Vector4 localRotation = Vector4.zero;

	[Persistent]
	public Vector3 localScale = Vector3.one;

	[Persistent]
	public bool oneShot;

	public Transform modelParent;

	public ParticleSystem emitter;

	public float minEmission;

	public float maxEmission;

	public float minEnergy;

	public float maxEnergy;

	public Vector3 velocity;

	public float emissionPower;

	public float energyPower;

	public float speedPower;

	public override void OnLoad(ConfigNode node)
	{
		ConfigNode.LoadObjectFromConfig(this, node);
		emission.Load("emission", node);
		energy.Load("energy", node);
		speed.Load("speed", node);
	}

	public override void OnSave(ConfigNode node)
	{
		ConfigNode.CreateConfigFromObject(this, node);
		emission.Save(node);
		energy.Save(node);
		speed.Save(node);
	}

	public override void OnInitialize()
	{
		modelParent = hostPart.FindModelTransform(transformName);
		if (modelParent == null)
		{
			Debug.LogError("PrefabParticleFX: Cannot find transform of name '" + transformName + "'");
			return;
		}
		Object @object = Resources.Load("Effects/" + prefabName);
		if (@object == null)
		{
			Debug.LogError("PrefabParticleFX: Cannot find prefab of name '" + prefabName + "'");
			return;
		}
		GameObject gameObject = (GameObject)Object.Instantiate(@object);
		if (gameObject == null)
		{
			Debug.LogError("PrefabParticleFX: Cannot find prefab of name '" + prefabName + "'");
			return;
		}
		gameObject.SetActive(value: true);
		emitter = gameObject.GetComponentInChildren<ParticleSystem>();
		if (emitter == null)
		{
			Debug.LogError("PrefabParticleFX: Cannot find particle emitter on model of name '" + prefabName + "'");
			Object.Destroy(gameObject);
			return;
		}
		emitter.transform.NestToParent(modelParent);
		emitter.transform.localPosition = localOffset;
		emitter.transform.localRotation = Quaternion.AngleAxis(localRotation.w, localRotation);
		emitter.transform.localScale = Vector3.Scale(emitter.transform.localScale, localScale);
		emitter.Stop();
		ParticleSystem.EmissionModule emissionModule = emitter.emission;
		if (emissionModule.burstCount > 0)
		{
			ParticleSystem.Burst[] array = new ParticleSystem.Burst[emissionModule.burstCount];
			minEmission = array[0].minCount;
			maxEmission = array[0].maxCount;
		}
		else
		{
			minEmission = emissionModule.rateOverTime.constant;
			maxEmission = emissionModule.rateOverTime.constant;
		}
		ParticleSystem.MinMaxCurve startLifetime = emitter.main.startLifetime;
		minEnergy = startLifetime.constantMin;
		maxEnergy = startLifetime.constantMax;
		ParticleSystem.VelocityOverLifetimeModule velocityOverLifetime = emitter.velocityOverLifetime;
		velocityOverLifetime.enabled = true;
		velocity = new Vector3(velocityOverLifetime.x.constant, velocityOverLifetime.y.constant, velocityOverLifetime.z.constant);
		EffectBehaviour.AddParticleEmitter(emitter);
	}

	public override void OnEvent()
	{
		if (!(emitter == null))
		{
			emitter.Play();
		}
	}

	public override void OnEvent(float power)
	{
		if (emitter == null)
		{
			return;
		}
		if (power <= 0f)
		{
			emitter.Stop();
			return;
		}
		ParticleSystem.EmissionModule emissionModule = emitter.emission;
		emissionPower = emission.Value(power);
		if (emissionModule.burstCount > 0)
		{
			emissionModule.SetBursts(new ParticleSystem.Burst[1]
			{
				new ParticleSystem.Burst(0f, (short)Mathf.FloorToInt(minEmission * emissionPower), (short)Mathf.FloorToInt(maxEmission * emissionPower))
			});
		}
		else
		{
			emissionModule.rateOverTime = Mathf.FloorToInt(minEmission * emissionPower);
		}
		energyPower = energy.Value(power);
		ParticleSystem.MainModule main = emitter.main;
		ParticleSystem.MinMaxCurve startLifetime = main.startLifetime;
		startLifetime.constantMin = minEnergy * energyPower;
		startLifetime.constantMax = maxEnergy * energyPower;
		startLifetime.mode = ParticleSystemCurveMode.TwoConstants;
		main.startLifetime = startLifetime;
		speedPower = speed.Value(power);
		ParticleSystem.VelocityOverLifetimeModule velocityOverLifetime = emitter.velocityOverLifetime;
		ParticleSystem.MinMaxCurve x = velocityOverLifetime.x;
		ParticleSystem.MinMaxCurve y = velocityOverLifetime.y;
		ParticleSystem.MinMaxCurve z = velocityOverLifetime.z;
		x.constant = velocity.x * speedPower;
		y.constant = velocity.y * speedPower;
		z.constant = velocity.z * speedPower;
		emitter.Play();
	}

	public void OnDestroy()
	{
		EffectBehaviour.RemoveParticleEmitter(emitter);
	}
}
