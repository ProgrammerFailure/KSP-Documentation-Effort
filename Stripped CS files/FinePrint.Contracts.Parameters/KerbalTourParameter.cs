using System.Globalization;
using Contracts;
using FinePrint.Utilities;
using ns11;
using ns9;

namespace FinePrint.Contracts.Parameters;

public class KerbalTourParameter : ContractParameter
{
	public string kerbalName;

	public ProtoCrewMember.Gender kerbalGender;

	public bool eventsAdded;

	public bool kerbalInactive;

	public KerbalTourParameter()
	{
		kerbalName = "Jebediah Kerman";
		kerbalGender = ProtoCrewMember.Gender.Male;
	}

	public KerbalTourParameter(string kerbalName, ProtoCrewMember.Gender kerbalGender)
	{
		this.kerbalName = kerbalName;
		this.kerbalGender = kerbalGender;
		allowPartialFailure = true;
	}

	public override string GetHashString()
	{
		return SystemUtilities.SuperSeed(base.Root).ToString(CultureInfo.InvariantCulture) + base.String_0;
	}

	public override string GetTitle()
	{
		return Localizer.Format("#autoLOC_283467", StringUtilities.ShortKerbalName(kerbalName).LocalizeName(kerbalGender));
	}

	public override void OnRegister()
	{
		base.DisableOnStateChange = false;
		if (base.Root.ContractState == Contract.State.Active)
		{
			GameEvents.onCrewKilled.Add(OnCrewKilled);
			GameEvents.onVesselRecoveryProcessing.Add(OnVesselRecovered);
			GameEvents.onKerbalInactiveChange.Add(OnKerbalInactiveChange);
			eventsAdded = true;
		}
	}

	public override void OnUnregister()
	{
		if (eventsAdded)
		{
			GameEvents.onCrewKilled.Remove(OnCrewKilled);
			GameEvents.onVesselRecoveryProcessing.Remove(OnVesselRecovered);
			GameEvents.onKerbalInactiveChange.Remove(OnKerbalInactiveChange);
		}
	}

	public override void OnSave(ConfigNode node)
	{
		node.AddValue("kerbalName", kerbalName);
		node.AddValue("kerbalGender", kerbalGender);
	}

	public override void OnLoad(ConfigNode node)
	{
		SystemUtilities.LoadNode(node, "KerbalTourParameter", "kerbalName", ref kerbalName, "Jebediah Kerman");
		SystemUtilities.LoadNode(node, "KerbalTourParameter", "kerbalGender", ref kerbalGender, ProtoCrewMember.Gender.Male);
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

	public override void OnParameterStateChange(ContractParameter p)
	{
		if (AllChildParametersComplete())
		{
			ProtoCrewMember protoCrewMember = HighLogic.CurrentGame.CrewRoster[kerbalName];
			if (protoCrewMember != null && protoCrewMember.type == ProtoCrewMember.KerbalType.Tourist)
			{
				protoCrewMember.hasToured = true;
			}
		}
	}

	public void OnVesselRecovered(ProtoVessel v, MissionRecoveryDialog mrd, float x)
	{
		if (v.GetVesselCrew().Contains(HighLogic.CurrentGame.CrewRoster[kerbalName]) && AllChildParametersComplete())
		{
			SetComplete();
		}
	}

	public void OnKerbalInactiveChange(ProtoCrewMember pcm, bool oldValue, bool newValue)
	{
		if (ContractDefs.Tour.FailOnInactive && newValue && pcm.name == kerbalName)
		{
			kerbalInactive = true;
			if (pcm != null && pcm.type == ProtoCrewMember.KerbalType.Tourist)
			{
				pcm.hasToured = true;
			}
			SetFailed();
		}
	}

	public override string GetMessageFailed()
	{
		if (kerbalInactive)
		{
			return Localizer.Format("#autoLOC_283554", base.Title, kerbalName);
		}
		return base.GetMessageFailed();
	}
}
