using UnityEngine;

[EffectDefinition("PREFAB_SPAWN")]
public class PrefabSpawnFX : EffectBehaviour
{
	[Persistent]
	public string prefabName = "";

	[Persistent]
	public string transformName = "";

	[Persistent]
	public bool setParent = true;

	public Transform modelParent;

	public GameObject prefabObject;

	public override void OnLoad(ConfigNode node)
	{
		ConfigNode.LoadObjectFromConfig(this, node);
	}

	public override void OnSave(ConfigNode node)
	{
		ConfigNode.CreateConfigFromObject(this, node);
	}

	public override void OnInitialize()
	{
		modelParent = hostPart.FindModelTransform(transformName);
		if (modelParent == null)
		{
			Debug.LogError("ParticleModelFX: Cannot find transform of name '" + transformName + "'");
			return;
		}
		prefabObject = (GameObject)Resources.Load("Effects/" + prefabName);
		if (prefabObject == null)
		{
			Debug.LogError("PrefabSpawnFX: Cannot find prefab of name '" + prefabName + "'");
		}
	}

	public override void OnEvent()
	{
		if (modelParent != null && prefabObject != null)
		{
			GameObject gameObject = Object.Instantiate(prefabObject);
			gameObject.transform.NestToParent(modelParent);
			if (!setParent)
			{
				gameObject.transform.parent = null;
			}
		}
	}

	public override void OnEvent(float power)
	{
		if (modelParent != null && prefabObject != null)
		{
			GameObject gameObject = Object.Instantiate(prefabObject);
			gameObject.transform.NestToParent(modelParent);
			if (!setParent)
			{
				gameObject.transform.parent = null;
			}
		}
	}
}
