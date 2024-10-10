using System;
using System.Collections.Generic;
using ns11;
using UnityEngine;

public class InternalIndicatorPanel : InternalModule
{
	public enum IndicatorValue
	{
		const_0,
		const_1,
		Gear,
		KillRot,
		Stage,
		Warn,
		Fuel,
		Oxygen,
		MECO,
		Airlock,
		Heat,
		HighG
	}

	[Serializable]
	public class Indicator
	{
		public string name = "xxx";

		public bool enabled = true;

		public Color colorOff = Color.black;

		public Color colorOn = Color.green;

		public IndicatorValue value;

		public Renderer renderer;

		public static Indicator Create(ConfigNode node)
		{
			Indicator indicator = new Indicator();
			if (node.HasValue("name"))
			{
				indicator.name = node.GetValue("name");
				if (node.HasValue("enabled"))
				{
					indicator.enabled = bool.Parse(node.GetValue("enabled"));
				}
				if (node.HasValue("colorOff"))
				{
					indicator.colorOff = ConfigNode.ParseColor(node.GetValue("colorOff"));
				}
				if (node.HasValue("colorOn"))
				{
					indicator.colorOn = ConfigNode.ParseColor(node.GetValue("colorOn"));
				}
				if (node.HasValue("value"))
				{
					indicator.value = (IndicatorValue)Enum.Parse(typeof(IndicatorValue), node.GetValue("value"));
				}
				return indicator;
			}
			return null;
		}

		public void Set(bool on)
		{
			if (renderer != null)
			{
				if (on)
				{
					renderer.material.SetColor(PropertyIDs._EmissiveColor, colorOn);
				}
				else
				{
					renderer.material.SetColor(PropertyIDs._EmissiveColor, colorOff);
				}
			}
		}

		public void SetOn()
		{
			if (renderer != null)
			{
				renderer.material.SetColor(PropertyIDs._EmissiveColor, colorOn);
			}
		}

		public void SetOff()
		{
			if (renderer != null)
			{
				renderer.material.SetColor(PropertyIDs._EmissiveColor, colorOff);
			}
		}
	}

	[Serializable]
	public class IndicatorList : IConfigNode
	{
		public List<Indicator> list;

		public void Load(ConfigNode node)
		{
			ConfigNode[] nodes = node.GetNodes("indicator");
			list = new List<Indicator>();
			int i = 0;
			for (int num = nodes.Length; i < num; i++)
			{
				Indicator indicator = Indicator.Create(nodes[i]);
				if (indicator != null)
				{
					list.Add(indicator);
				}
			}
		}

		public void Save(ConfigNode node)
		{
		}
	}

	[KSPField]
	public IndicatorList indicators = new IndicatorList();

	public override void OnAwake()
	{
		int i = 0;
		for (int count = indicators.list.Count; i < count; i++)
		{
			Indicator indicator = indicators.list[i];
			indicator.renderer = internalProp.FindModelComponent<Renderer>(indicator.name);
			indicator.SetOff();
		}
	}

	public override void OnUpdate()
	{
		int i = 0;
		for (int count = indicators.list.Count; i < count; i++)
		{
			Indicator indicator = indicators.list[i];
			if (indicator.renderer != null)
			{
				switch (indicator.value)
				{
				case IndicatorValue.HighG:
					indicator.Set(FlightGlobals.ship_geeForce > 9.0);
					break;
				case IndicatorValue.const_1:
					indicator.Set(base.vessel.ActionGroups[KSPActionGroup.flag_5]);
					break;
				case IndicatorValue.Gear:
					indicator.Set(base.vessel.ActionGroups[KSPActionGroup.Gear]);
					break;
				case IndicatorValue.const_0:
				case IndicatorValue.KillRot:
					indicator.Set(base.vessel.ActionGroups[KSPActionGroup.flag_6]);
					break;
				case IndicatorValue.Stage:
					indicator.Set(StageManager.CanSeparate);
					break;
				}
			}
		}
	}
}
