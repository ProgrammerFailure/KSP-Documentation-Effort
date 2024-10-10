using System;

[Serializable]
public class PartResource
{
	public enum FlowMode
	{
		None,
		Out,
		In,
		Both
	}

	public PartResourceDefinition info;

	public string resourceName;

	public Part part;

	public double amount;

	public double maxAmount;

	public bool _flowState;

	public bool isTweakable;

	public bool isVisible = true;

	public bool hideFlow;

	public bool simulationResource;

	public FlowMode _flowMode;

	public bool flowState
	{
		get
		{
			return _flowState;
		}
		set
		{
			bool flag = _flowState;
			_flowState = value;
			if (value != flag)
			{
				GameEvents.onPartResourceFlowStateChange.Fire(new GameEvents.HostedFromToAction<PartResource, bool>(this, flag, value));
			}
		}
	}

	public FlowMode flowMode
	{
		get
		{
			return _flowMode;
		}
		set
		{
			FlowMode flowMode = _flowMode;
			_flowMode = value;
			if (value != flowMode)
			{
				GameEvents.onPartResourceFlowModeChange.Fire(new GameEvents.HostedFromToAction<PartResource, FlowMode>(this, flowMode, value));
			}
		}
	}

	public PartResource(Part p)
	{
		part = p;
	}

	public PartResource(Part p, bool simulate)
	{
		part = p;
		simulationResource = simulate;
	}

	public PartResource(PartResource res)
	{
		part = res.part;
		Copy(res);
	}

	public PartResource(PartResource res, bool simulate)
	{
		part = res.part;
		Copy(res);
		simulationResource = simulate;
	}

	public void Copy(PartResource res)
	{
		info = res.info;
		resourceName = res.resourceName;
		amount = res.amount;
		maxAmount = res.maxAmount;
		_flowState = res._flowState;
		isTweakable = res.isTweakable;
		isVisible = res.isVisible;
		hideFlow = res.hideFlow;
		_flowMode = res._flowMode;
		simulationResource = res.simulationResource;
	}

	public void Load(ConfigNode node)
	{
		if (node.HasValue("amount"))
		{
			amount = double.Parse(node.GetValue("amount"));
		}
		if (node.HasValue("maxAmount"))
		{
			maxAmount = double.Parse(node.GetValue("maxAmount"));
		}
		if (node.HasValue("flowState"))
		{
			flowState = bool.Parse(node.GetValue("flowState"));
		}
		else
		{
			flowState = true;
		}
		if (node.HasValue("isTweakable"))
		{
			isTweakable = bool.Parse(node.GetValue("isTweakable"));
		}
		else
		{
			isTweakable = PartResourceLibrary.Instance.GetDefinition(resourceName.GetHashCode()).isTweakable;
		}
		if (node.HasValue("isVisible"))
		{
			isVisible = bool.Parse(node.GetValue("isVisible"));
		}
		else
		{
			isVisible = PartResourceLibrary.Instance.GetDefinition(resourceName.GetHashCode()).isVisible;
		}
		if (node.HasValue("hideFlow"))
		{
			hideFlow = bool.Parse(node.GetValue("hideFlow"));
		}
		else
		{
			hideFlow = false;
		}
		if (node.HasValue("flowMode"))
		{
			node.TryGetEnum("flowMode", ref _flowMode, FlowMode.Both);
		}
		else
		{
			flowMode = FlowMode.Both;
		}
	}

	public void Save(ConfigNode node)
	{
		node.AddValue("name", info.name);
		node.AddValue("amount", amount);
		node.AddValue("maxAmount", maxAmount);
		node.AddValue("flowState", flowState);
		node.AddValue("isTweakable", isTweakable);
		node.AddValue("hideFlow", hideFlow);
		node.AddValue("isVisible", isVisible);
		node.AddValue("flowMode", flowMode);
	}

	public void SetInfo(PartResourceDefinition info)
	{
		this.info = info;
		resourceName = info.name;
	}

	public string GetInfo()
	{
		return info.displayName + ": " + amount + " / " + maxAmount;
	}

	public bool CanProvide(double demand)
	{
		if (demand > 0.0)
		{
			return amount > 0.0;
		}
		if (demand < 0.0)
		{
			return amount < maxAmount;
		}
		return false;
	}

	public bool CanProvide(bool pulling)
	{
		if (pulling)
		{
			if (amount > 0.0 && _flowState)
			{
				return (_flowMode & FlowMode.Out) > FlowMode.None;
			}
			return false;
		}
		if (amount < maxAmount && _flowState)
		{
			return (_flowMode & FlowMode.In) > FlowMode.None;
		}
		return false;
	}

	public bool Flowing(bool pulling)
	{
		if (_flowState)
		{
			if (pulling)
			{
				if ((_flowMode & FlowMode.Out) > FlowMode.None)
				{
					return true;
				}
			}
			else if ((_flowMode & FlowMode.In) > FlowMode.None)
			{
				return true;
			}
		}
		return false;
	}
}
