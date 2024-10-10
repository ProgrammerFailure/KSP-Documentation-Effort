using UnityEngine;

namespace Strategies;

public class StrategyEffect
{
	public Strategy parent;

	public Strategy Parent => parent;

	public string Description => GetDescription();

	public StrategyEffect(Strategy parent)
	{
		this.parent = parent;
	}

	public void Register()
	{
		OnRegister();
	}

	public void Unregister()
	{
		OnUnregister();
	}

	public void Load(ConfigNode node)
	{
		OnLoad(node);
	}

	public void Save(ConfigNode node)
	{
		node.AddValue("name", GetType().Name);
		OnSave(node);
	}

	public void LoadFromConfig(ConfigNode node)
	{
		OnLoadFromConfig(node);
	}

	public void Update()
	{
		OnUpdate();
	}

	public virtual string GetDescription()
	{
		return "";
	}

	public virtual void OnRegister()
	{
	}

	public virtual void OnUnregister()
	{
	}

	public virtual void OnLoad(ConfigNode node)
	{
	}

	public virtual void OnSave(ConfigNode node)
	{
	}

	public virtual void OnLoadFromConfig(ConfigNode node)
	{
	}

	public virtual void OnUpdate()
	{
	}

	public virtual bool CanActivate(ref string reason)
	{
		return true;
	}

	public string ToPercentage(float percentage, string format = "F0")
	{
		float num = percentage * 100f - 100f;
		return ((num > 0f) ? "+" : "") + num.ToString(format) + "%";
	}

	public float ParentLerp(float minValue, float maxValue)
	{
		return Mathf.Lerp(minValue, maxValue, Parent.Factor);
	}
}
