using System.Collections.Generic;
using UnityEngine;

[EffectDefinition("MODEL_MULTI_PARTICLE")]
public class ModelMultiParticleFX : EffectBehaviour
{
	[Persistent]
	public string modelName = "";

	[Persistent]
	public string transformName = "";

	[Persistent]
	public Vector3 localRotation = Vector3.zero;

	[Persistent]
	public Vector3 localPosition = Vector3.zero;

	[Persistent]
	public Vector3 localScale = Vector3.one;

	public FXCurve emission = new FXCurve("emission", 1f);

	public FXCurve energy = new FXCurve("energy", 1f);

	public FXCurve speed = new FXCurve("speed", 1f);

	public List<Transform> modelParents;

	public new List<KSPParticleEmitter> emitters;

	public KSPParticleEmitter emitter;

	public float minEmission;

	public float maxEmission;

	public float minEnergy;

	public float maxEnergy;

	public Vector3 localVelocity;

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
			Debug.LogError("ParticleModelFX: Cannot find transform of name '" + transformName + "'");
			return;
		}
		GameObject model = GameDatabase.Instance.GetModel(modelName);
		if (model == null)
		{
			Debug.LogError("ParticleModelFX: Cannot find model of name '" + modelName + "'");
			return;
		}
		model.SetActive(value: true);
		emitter = model.GetComponentInChildren<KSPParticleEmitter>();
		if (emitter == null)
		{
			Debug.LogError("ParticleModelFX: Cannot find particle emitter on model of name '" + modelName + "'");
			Object.Destroy(model);
			return;
		}
		minEmission = emitter.minEmission;
		maxEmission = emitter.maxEmission;
		minEnergy = emitter.minEnergy;
		maxEnergy = emitter.maxEnergy;
		localVelocity = emitter.localVelocity;
		if (emitters == null)
		{
			emitters = new List<KSPParticleEmitter>();
		}
		int i = 0;
		for (int count = modelParents.Count; i < count; i++)
		{
			GameObject gameObject;
			if (i > 0)
			{
				gameObject = Object.Instantiate(model);
				emitter = gameObject.GetComponentInChildren<KSPParticleEmitter>();
			}
			else
			{
				gameObject = model;
			}
			emitters.Add(emitter);
			gameObject.transform.NestToParent(modelParents[i]);
			gameObject.transform.localPosition = localPosition;
			gameObject.transform.localRotation = Quaternion.Euler(localRotation);
			gameObject.transform.localScale = Vector3.Scale(gameObject.transform.localScale, localScale);
		}
		int j = 0;
		for (int count2 = emitters.Count; j < count2; j++)
		{
			EffectBehaviour.AddParticleEmitter(emitters[j]);
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
			emitters[transformIdx].Emit();
			return;
		}
		int i = 0;
		for (int count = emitters.Count; i < count; i++)
		{
			emitters[i].Emit();
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
				emitters[transformIdx].emit = false;
				return;
			}
			int i = 0;
			for (int count = emitters.Count; i < count; i++)
			{
				emitters[i].emit = false;
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
		Vector3 localVelocityVal = localVelocity * speedPower;
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
		KSPParticleEmitter kSPParticleEmitter = emitters[transformIdx];
		kSPParticleEmitter.emit = true;
		kSPParticleEmitter.minEmission = minEmissionVal;
		kSPParticleEmitter.maxEmission = maxEmissionVal;
		kSPParticleEmitter.minEnergy = minEnergyVal;
		kSPParticleEmitter.maxEnergy = maxEnergyVal;
		kSPParticleEmitter.localVelocity = localVelocityVal;
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
