using System.Collections.Generic;
using UnityEngine;

[EffectDefinition("PREFAB_MULTI_PARTICLE")]
public class PrefabMultiParticleFX : EffectBehaviour
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

	public List<Transform> modelParents;

	public new List<ParticleSystem> emitters;

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
		modelParents = new List<Transform>(hostPart.FindModelTransforms(transformName));
		if (modelParents.Count == 0)
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
		if (emitters == null)
		{
			emitters = new List<ParticleSystem>();
		}
		int i = 0;
		for (int count = modelParents.Count; i < count; i++)
		{
			GameObject gameObject2;
			if (i > 0)
			{
				gameObject2 = Object.Instantiate(gameObject);
				emitter = gameObject2.GetComponentInChildren<ParticleSystem>();
			}
			else
			{
				gameObject2 = gameObject;
			}
			emitters.Add(emitter);
			gameObject2.transform.NestToParent(modelParents[i]);
			gameObject2.transform.localPosition = localOffset;
			gameObject2.transform.localRotation = Quaternion.AngleAxis(localRotation.w, localRotation);
			gameObject2.transform.localScale = Vector3.Scale(gameObject2.transform.localScale, localScale);
			emitter.Stop();
			EffectBehaviour.AddParticleEmitter(emitter);
		}
	}

	public override void OnEvent(int transformIdx)
	{
		if (emitters == null)
		{
			return;
		}
		if (transformIdx > -1 && transformIdx < emitters.Count)
		{
			emitters[transformIdx].Play();
			return;
		}
		int i = 0;
		for (int count = emitters.Count; i < count; i++)
		{
			emitters[i].Play();
		}
	}

	public override void OnEvent(float power, int transformIdx)
	{
		if (emitters == null)
		{
			return;
		}
		if (power <= 0f)
		{
			if (transformIdx > -1 && transformIdx < emitters.Count)
			{
				emitters[transformIdx].Stop();
				return;
			}
			int i = 0;
			for (int count = emitters.Count; i < count; i++)
			{
				emitters[i].Stop();
			}
			return;
		}
		emissionPower = emission.Value(power);
		energyPower = energy.Value(power);
		speedPower = speed.Value(power);
		int minEmissionVal = Mathf.FloorToInt(minEmission * emissionPower);
		int maxEmissionVal = Mathf.FloorToInt(maxEmission * emissionPower);
		float minEnergyVal = minEnergy * energyPower;
		float maxEnergyVal = maxEnergy * energyPower;
		Vector3 localVelocityVal = velocity * speedPower;
		if (transformIdx > -1 && transformIdx < emitters.Count)
		{
			SetEmitter(transformIdx, minEmissionVal, maxEmissionVal, minEnergyVal, maxEnergyVal, localVelocityVal);
			return;
		}
		int j = 0;
		for (int count2 = emitters.Count; j < count2; j++)
		{
			SetEmitter(j, minEmissionVal, maxEmissionVal, minEnergyVal, maxEnergyVal, localVelocityVal);
		}
	}

	public void SetEmitter(int transformIdx, int minEmissionVal, int maxEmissionVal, float minEnergyVal, float maxEnergyVal, Vector3 localVelocityVal)
	{
		ParticleSystem particleSystem = emitters[transformIdx];
		ParticleSystem.MainModule main = particleSystem.main;
		main.duration = 1f;
		ParticleSystem.EmissionModule emissionModule = particleSystem.emission;
		if (emissionModule.burstCount > 0)
		{
			emissionModule.SetBursts(new ParticleSystem.Burst[1]
			{
				new ParticleSystem.Burst(0f, (short)minEmissionVal, (short)maxEmissionVal)
			});
		}
		else
		{
			emissionModule.rateOverTime = minEmissionVal;
		}
		ParticleSystem.MinMaxCurve startLifetime = main.startLifetime;
		startLifetime.constantMin = minEnergyVal;
		startLifetime.constantMax = maxEnergyVal;
		startLifetime.mode = ParticleSystemCurveMode.TwoConstants;
		main.startLifetime = startLifetime;
		ParticleSystem.VelocityOverLifetimeModule velocityOverLifetime = particleSystem.velocityOverLifetime;
		velocityOverLifetime.enabled = true;
		velocityOverLifetime.x = localVelocityVal.x;
		velocityOverLifetime.y = localVelocityVal.y;
		velocityOverLifetime.z = localVelocityVal.z;
		particleSystem.Play();
	}

	public void OnDestroy()
	{
		if (emitters != null)
		{
			int i = 0;
			for (int count = emitters.Count; i < count; i++)
			{
				EffectBehaviour.RemoveParticleEmitter(emitter);
			}
		}
	}
}
