using System;
using System.Collections.Generic;
using ns2;
using ns9;
using UnityEngine;

namespace CommNet;

public class TooltipController_SignalStrength : TooltipController
{
	public Tooltip_SignalStrength prefab;

	public Tooltip_SignalStrengthItem itemPrefab;

	public Tooltip_SignalStrength tooltip;

	public List<Tooltip_SignalStrengthItem> items = new List<Tooltip_SignalStrengthItem>();

	public static string cacheAutoLOC_121470;

	public static string NoSignal = Localizer.Format("#autoLOC_121427");

	public static string NoSignalPlasma = Localizer.Format("#autoLOC_121428");

	public static string NoSignalControl = Localizer.Format("#autoLOC_121429");

	public static void CacheLocalStrings()
	{
		cacheAutoLOC_121470 = Localizer.Format("#autoLOC_121470");
	}

	public override bool OnTooltipAboutToSpawn()
	{
		if (!(FlightGlobals.ActiveVessel == null) && !(FlightGlobals.ActiveVessel.connection == null))
		{
			return true;
		}
		return false;
	}

	public override void OnTooltipSpawned(Tooltip instance)
	{
		tooltip = (Tooltip_SignalStrength)instance;
		UpdateTooltip();
		if (CommNetNetwork.Instance != null)
		{
			CommNetwork commNet = CommNetNetwork.Instance.CommNet;
			commNet.OnNetworkPostUpdate = (Action)Delegate.Combine(commNet.OnNetworkPostUpdate, new Action(OnNetworkUpdate));
		}
	}

	public override void OnTooltipDespawned(Tooltip instance)
	{
		tooltip = null;
		if (CommNetNetwork.Instance != null)
		{
			CommNetwork commNet = CommNetNetwork.Instance.CommNet;
			commNet.OnNetworkPostUpdate = (Action)Delegate.Remove(commNet.OnNetworkPostUpdate, new Action(OnNetworkUpdate));
		}
		ClearList();
	}

	public override bool OnTooltipUpdate(Tooltip instance)
	{
		if (!OnTooltipAboutToSpawn())
		{
			return false;
		}
		UpdateTooltip();
		return true;
	}

	public virtual void OnNetworkUpdate()
	{
		UpdateTooltip();
	}

	public virtual void UpdateTooltip()
	{
		Vessel activeVessel = FlightGlobals.ActiveVessel;
		if (activeVessel.Connection.IsConnected)
		{
			if (activeVessel.Connection.Signal != 0)
			{
				tooltip.title.text = Localizer.Format("#autoLOC_121438", (Math.Ceiling(activeVessel.Connection.SignalStrength * 100.0) * 0.01).ToString("0%"));
				double signalStrengthModifier = activeVessel.Connection.GetSignalStrengthModifier(activeVessel.Connection.ControlPath.First.b);
				if (signalStrengthModifier < 1.0)
				{
					tooltip.title.text += Localizer.Format("#autoLOC_121442", signalStrengthModifier.ToString("F2"));
				}
			}
			else if (activeVessel.Connection.CanComm)
			{
				if (activeVessel.Connection.InPlasma)
				{
					if (tooltip.title.text != NoSignalPlasma)
					{
						tooltip.title.text = NoSignalPlasma;
					}
				}
				else if (tooltip.title.text != NoSignal)
				{
					tooltip.title.text = NoSignal;
				}
			}
			else if (tooltip.title.text != NoSignalControl)
			{
				tooltip.title.text = NoSignalControl;
			}
			UpdateList();
		}
		else
		{
			tooltip.title.text = cacheAutoLOC_121470;
			ClearList();
		}
	}

	public virtual void UpdateList()
	{
		CommNetVessel connection = FlightGlobals.ActiveVessel.connection;
		_ = connection.Comm;
		CommPath controlPath = connection.ControlPath;
		int count = controlPath.Count;
		if (items.Count > count)
		{
			int i = count;
			for (int count2 = items.Count; i < count2; i++)
			{
				if (items[i] != null)
				{
					UnityEngine.Object.Destroy(items[i].gameObject);
				}
			}
			items.RemoveRange(count, items.Count - count);
		}
		else if (items.Count < count)
		{
			int j = items.Count;
			for (int num = count; j < num; j++)
			{
				items.Add(null);
			}
		}
		for (int k = 0; k < count; k++)
		{
			Tooltip_SignalStrengthItem tooltip_SignalStrengthItem = items[k];
			if (tooltip_SignalStrengthItem == null)
			{
				tooltip_SignalStrengthItem = UnityEngine.Object.Instantiate(itemPrefab);
				tooltip_SignalStrengthItem.transform.SetParent(tooltip.listParent, worldPositionStays: false);
				items[k] = tooltip_SignalStrengthItem;
			}
			tooltip_SignalStrengthItem.Setup(controlPath[k].signal, controlPath[k].end.displayName);
			tooltip_SignalStrengthItem.transform.SetAsLastSibling();
		}
	}

	public virtual void ClearList()
	{
		int count = items.Count;
		while (count-- > 0)
		{
			if (items[count] != null)
			{
				UnityEngine.Object.Destroy(items[count].gameObject);
			}
		}
		items.Clear();
	}
}
