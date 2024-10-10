using System;
using ns9;
using UnityEngine;

namespace Contracts;

[Serializable]
public class ContractPredicate
{
	[HideInInspector]
	public IContractParameterHost Parent { get; set; }

	[HideInInspector]
	public Contract Root { get; set; }

	public string Description => GetDescription();

	public bool AllowMultiple => GetAllowMultiple();

	public ContractPredicate(IContractParameterHost parent)
	{
		Parent = parent;
		Root = parent.Root;
	}

	public void Load(ConfigNode node)
	{
		OnLoad(node);
	}

	public void Save(ConfigNode node)
	{
		OnSave(node);
	}

	public virtual bool Test(Vessel vessel)
	{
		return false;
	}

	public virtual bool Test(ProtoVessel vessel)
	{
		return false;
	}

	public virtual string GetDescription()
	{
		return Localizer.Format("#autoLOC_268399");
	}

	public virtual bool GetAllowMultiple()
	{
		return false;
	}

	public virtual void OnLoad(ConfigNode node)
	{
	}

	public virtual void OnSave(ConfigNode node)
	{
	}

	public void Update()
	{
		OnUpdate();
	}

	public virtual void OnUpdate()
	{
	}
}
