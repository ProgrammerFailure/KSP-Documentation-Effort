using System.Collections.Generic;
using ns9;

namespace PreFlightTests;

public class ExperimentalPartsAvailable : IPreFlightTest
{
	public Dictionary<AvailablePart, int> expPartsOnShip;

	public ExperimentalPartsAvailable(ShipConstruct ship)
	{
		if (ResearchAndDevelopment.Instance == null)
		{
			return;
		}
		expPartsOnShip = new Dictionary<AvailablePart, int>();
		int i = ship.parts.Count;
		while (i-- > 0)
		{
			ProtoTechNode techState = ResearchAndDevelopment.Instance.GetTechState(ship.parts[i].partInfo.TechRequired);
			if (techState == null || techState.state != RDTech.State.Available || !RUIutils.Any(techState.partsPurchased, (AvailablePart a) => a.name == ship.parts[i].partInfo.name))
			{
				AvailablePart partInfoByName = PartLoader.getPartInfoByName(ship.parts[i].partInfo.name);
				if (!expPartsOnShip.ContainsKey(partInfoByName))
				{
					expPartsOnShip.Add(partInfoByName, 1);
				}
				else
				{
					expPartsOnShip[partInfoByName] += 1;
				}
			}
		}
	}

	public ExperimentalPartsAvailable(VesselCrewManifest manifest)
	{
		if (ResearchAndDevelopment.Instance == null)
		{
			return;
		}
		expPartsOnShip = new Dictionary<AvailablePart, int>();
		int count = manifest.PartManifests.Count;
		PartCrewManifest pcm;
		while (count-- > 0)
		{
			pcm = manifest.PartManifests[count];
			ProtoTechNode techState = ResearchAndDevelopment.Instance.GetTechState(pcm.PartInfo.TechRequired);
			if (techState == null || techState.state != RDTech.State.Available || !RUIutils.Any(techState.partsPurchased, (AvailablePart a) => a.name == pcm.PartInfo.name))
			{
				AvailablePart partInfoByName = PartLoader.getPartInfoByName(pcm.PartInfo.name);
				if (!expPartsOnShip.ContainsKey(partInfoByName))
				{
					expPartsOnShip.Add(partInfoByName, 1);
				}
				else
				{
					expPartsOnShip[partInfoByName] += 1;
				}
			}
		}
	}

	public bool Test()
	{
		if (ResearchAndDevelopment.Instance != null)
		{
			Dictionary<AvailablePart, int>.KeyCollection.Enumerator enumerator = expPartsOnShip.Keys.GetEnumerator();
			do
			{
				if (!enumerator.MoveNext())
				{
					return true;
				}
			}
			while (ResearchAndDevelopment.IsExperimentalPart(enumerator.Current));
			return false;
		}
		return true;
	}

	public string GetWarningTitle()
	{
		return Localizer.Format("#autoLOC_253217");
	}

	public string GetWarningDescription()
	{
		string text = Localizer.Format("#autoLOC_253222");
		Dictionary<AvailablePart, int>.KeyCollection.Enumerator enumerator = expPartsOnShip.Keys.GetEnumerator();
		while (enumerator.MoveNext())
		{
			if (!ResearchAndDevelopment.IsExperimentalPart(enumerator.Current))
			{
				text = text + "<color=orange><b>" + expPartsOnShip[enumerator.Current] + "x " + enumerator.Current.title + "</b></color>\n";
			}
		}
		return text;
	}

	public string GetProceedOption()
	{
		return null;
	}

	public string GetAbortOption()
	{
		return Localizer.Format("#autoLOC_253243");
	}
}
