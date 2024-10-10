using System.Globalization;
using Contracts;
using FinePrint.Utilities;
using ns9;

namespace FinePrint.Contracts.Parameters;

public class KerbalGeeAdventureParameter : ContractParameter
{
	public string kerbalName;

	public bool eventsAdded;

	public KerbalGeeAdventureParameter()
	{
		kerbalName = "Jebediah Kerman";
	}

	public KerbalGeeAdventureParameter(string kerbalName)
	{
		this.kerbalName = kerbalName;
		allowPartialFailure = true;
	}

	public override string GetHashString()
	{
		return SystemUtilities.SuperSeed(base.Root).ToString(CultureInfo.InvariantCulture) + base.String_0;
	}

	public override string GetTitle()
	{
		return Localizer.Format("#autoLOC_283371", StringUtilities.ShortKerbalName(kerbalName));
	}

	public override void OnRegister()
	{
		base.DisableOnStateChange = false;
		if (base.Root.ContractState == Contract.State.Active)
		{
			GameEvents.onCrewKilled.Add(OnCrewKilled);
			GameEvents.onKerbalPassedOutFromGeeForce.Add(OnKerbalPassedOut);
			eventsAdded = true;
		}
	}

	public override void OnUnregister()
	{
		if (eventsAdded)
		{
			GameEvents.onCrewKilled.Remove(OnCrewKilled);
			GameEvents.onKerbalPassedOutFromGeeForce.Remove(OnKerbalPassedOut);
		}
	}

	public override void OnSave(ConfigNode node)
	{
		node.AddValue("kerbalName", kerbalName);
	}

	public override void OnLoad(ConfigNode node)
	{
		SystemUtilities.LoadNode(node, "KerbalGeeAdventureParameter", "kerbalName", ref kerbalName, "Jebediah Kerman");
	}

	public void OnCrewKilled(EventReport evt)
	{
		if (evt.sender == kerbalName)
		{
			if (state != ParameterState.Failed)
			{
				allowPartialFailure = false;
				SetFailed();
			}
			else
			{
				base.Root.Fail();
			}
		}
	}

	public void OnKerbalPassedOut(ProtoCrewMember pcm)
	{
		if (pcm.name == kerbalName && AllChildParametersComplete())
		{
			if (pcm != null && pcm.type == ProtoCrewMember.KerbalType.Tourist)
			{
				pcm.hasToured = true;
			}
			SetComplete();
		}
	}
}
