using System.Globalization;
using Contracts;
using FinePrint.Utilities;
using ns9;
using SentinelMission;

namespace FinePrint.Contracts.Parameters;

public class CometDetectionParameter : ContractParameter
{
	public int cometCountRequired;

	public int cometCountDetected;

	public string sentinelPartName;

	public int RemainingDiscoveries;

	public CometDetectionParameter()
	{
		cometCountDetected = 0;
	}

	public CometDetectionParameter(int cometCountRequired, string sentinelPartName)
	{
		this.cometCountRequired = cometCountRequired;
		cometCountDetected = 0;
		this.sentinelPartName = sentinelPartName;
	}

	public override string GetHashString()
	{
		return SystemUtilities.SuperSeed(base.Root).ToString(CultureInfo.InvariantCulture) + base.String_0;
	}

	public override string GetTitle()
	{
		return Localizer.Format("#autoLOC_6011147", cometCountRequired.ToString());
	}

	public override string GetNotes()
	{
		return ((HighLogic.LoadedScene == GameScenes.SPACECENTER) ? "\n" : "") + Localizer.Format("#autoLOC_8003406");
	}

	public override void OnRegister()
	{
		base.DisableOnStateChange = false;
		GameEvents.onSentinelCometDetected.Add(CometDetected);
	}

	public override void OnUnregister()
	{
		GameEvents.onSentinelCometDetected.Remove(CometDetected);
	}

	public void CometDetected(Vessel data)
	{
		cometCountDetected++;
		RemainingDiscoveries = cometCountRequired - cometCountDetected;
		if (RemainingDiscoveries > 0)
		{
			string text = Localizer.Format("#autoLOC_8003397", cometCountDetected, cometCountRequired, base.Root.Agent.Title);
			ScreenMessages.PostScreenMessage(text, SentinelUtilities.CalculateReadDuration(text), ScreenMessageStyle.UPPER_CENTER);
		}
		else
		{
			string text2 = Localizer.Format("#autoLOC_8003398", base.Root.Agent.Title);
			ScreenMessages.PostScreenMessage(text2, SentinelUtilities.CalculateReadDuration(text2), ScreenMessageStyle.UPPER_CENTER);
			SetComplete();
			base.Root.Complete();
		}
	}

	public override void OnReset()
	{
		SetIncomplete();
	}

	public override void OnSave(ConfigNode node)
	{
		node.AddValue("cometCount", cometCountDetected);
		node.AddValue("cometCountRequired", cometCountRequired);
	}

	public override void OnLoad(ConfigNode node)
	{
		SystemUtilities.LoadNode(node, "CometDetectionParameter", "cometCount", ref cometCountDetected, 1);
		SystemUtilities.LoadNode(node, "CometDetectionParameter", "cometCountRequired", ref cometCountRequired, 1);
	}
}
