using FinePrint.Utilities;
using ns11;
using ns9;

namespace KSPAchievements;

public class PointOfInterest : ProgressNode
{
	public VesselRef firstVessel;

	public CrewRef firstCrew;

	public string body { get; set; }

	public string name { get; set; }

	public string text { get; set; }

	public bool uplifting { get; set; }

	public bool launchSite { get; set; }

	public PointOfInterest(string body, string name, string text, bool uplifting)
		: base("POI" + body + name, startReached: false)
	{
		this.body = body;
		this.name = name;
		this.text = text;
		this.uplifting = uplifting;
		launchSite = false;
		OnDeploy = delegate
		{
			GameEvents.OnPOIRangeEntered.Add(OnAnomalyLoaded);
		};
		OnStow = delegate
		{
			GameEvents.OnPOIRangeEntered.Remove(OnAnomalyLoaded);
		};
		firstVessel = new VesselRef();
		firstCrew = new CrewRef();
	}

	public PointOfInterest(string body, string name, string text, bool uplifting, bool launchSite)
		: this(body, name, text, uplifting)
	{
		this.name = name;
		this.launchSite = launchSite;
	}

	public void OnAnomalyLoaded(CelestialBody anomalyBody, string anomalyName)
	{
		if (anomalyBody.name != body || anomalyName != name || !FlightGlobals.ready)
		{
			return;
		}
		Vessel activeVessel = FlightGlobals.ActiveVessel;
		if (activeVessel == null || !activeVessel.isCommandable || activeVessel.DiscoveryInfo.Level != DiscoveryLevels.Owned)
		{
			return;
		}
		if (!base.IsComplete)
		{
			firstVessel = VesselRef.FromVessel(activeVessel);
			firstCrew = CrewRef.FromVessel(activeVessel);
			Complete();
			CelestialBody celestialBody = null;
			int count = FlightGlobals.Bodies.Count;
			while (count-- > 0)
			{
				if (FlightGlobals.Bodies[count].name == body)
				{
					celestialBody = FlightGlobals.Bodies[count];
					break;
				}
			}
			if (this.launchSite)
			{
				bool flag = false;
				string text = string.Empty;
				string text2 = string.Empty;
				LaunchSite launchSite = null;
				if (PSystemSetup.Instance != null && PSystemSetup.Instance.LaunchSites != null)
				{
					for (int i = 0; i < PSystemSetup.Instance.LaunchSites.Count; i++)
					{
						if (PSystemSetup.Instance.LaunchSites[i].requiresPOIVisit && string.Equals(PSystemSetup.Instance.LaunchSites[i].name, name))
						{
							text = Localizer.Format(PSystemSetup.Instance.LaunchSites[i].launchSiteName);
							flag = true;
							text2 = Localizer.Format(PSystemSetup.Instance.LaunchSites[i].editorFacility.displayDescription());
							launchSite = PSystemSetup.Instance.LaunchSites[i];
							break;
						}
					}
				}
				if (flag)
				{
					ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_6006121", text, text2), 10f, persist: false);
					if (MessageSystem.Instance != null)
					{
						MessageSystem.Instance.AddMessage(new MessageSystem.Message(Localizer.Format("#autoLOC_6006122"), Localizer.Format("#autoLOC_6006121", text, text2), MessageSystemButton.MessageButtonColor.BLUE, MessageSystemButton.ButtonIcons.MESSAGE));
					}
					if (launchSite != null)
					{
						GameEvents.LaunchSiteFound.Fire(launchSite);
					}
				}
				else if (MessageSystem.Instance != null)
				{
					MessageSystem.Instance.AddMessage(new MessageSystem.Message(Localizer.Format("#autoLOC_8003616"), Localizer.Format("#autoLOC_296488", this.text, celestialBody.displayName), MessageSystemButton.MessageButtonColor.BLUE, MessageSystemButton.ButtonIcons.MESSAGE));
				}
			}
			else
			{
				if (uplifting)
				{
					int pQSCitySeed = PQSCity.GetPQSCitySeed(anomalyName, anomalyBody.bodyName);
					string text3 = Localizer.Format("#autoLOC_296488", this.text, celestialBody.displayName);
					text3 += GetUpliftText(pQSCitySeed);
					AwardProgressRandomTech(text3, pQSCitySeed);
				}
				else
				{
					AwardProgressStandard(Localizer.Format("#autoLOC_296494", this.text, celestialBody.displayName), ProgressType.POINTOFINTEREST, celestialBody);
				}
				if (HighLogic.CurrentGame.Mode != Game.Modes.CAREER && MessageSystem.Instance != null)
				{
					MessageSystem.Instance.AddMessage(new MessageSystem.Message(Localizer.Format("#autoLOC_8003616"), Localizer.Format("#autoLOC_296488", this.text, celestialBody.displayName), MessageSystemButton.MessageButtonColor.BLUE, MessageSystemButton.ButtonIcons.MESSAGE));
				}
			}
			AnalyticsUtil.LogPointOfInterestReached(this, celestialBody);
		}
		Achieve();
	}

	public override void OnLoad(ConfigNode node)
	{
		if (node.HasNode("vessel"))
		{
			firstVessel.Load(node.GetNode("vessel"));
		}
		if (node.HasNode("crew"))
		{
			firstCrew.Load(node.GetNode("crew"));
		}
	}

	public override void OnSave(ConfigNode node)
	{
		firstVessel.Save(node.AddNode("vessel"));
		if (firstCrew.HasAny)
		{
			firstCrew.Save(node.AddNode("crew"));
		}
	}

	public string GetUpliftText(int seed)
	{
		return new KSPRandom(seed).Next(10) switch
		{
			0 => Localizer.Format("#autoLOC_296522"), 
			1 => Localizer.Format("#autoLOC_296524"), 
			2 => Localizer.Format("#autoLOC_296526"), 
			3 => Localizer.Format("#autoLOC_296528"), 
			4 => Localizer.Format("#autoLOC_296530"), 
			5 => Localizer.Format("#autoLOC_296532"), 
			6 => Localizer.Format("#autoLOC_296534"), 
			7 => Localizer.Format("#autoLOC_296536"), 
			8 => Localizer.Format("#autoLOC_296538"), 
			_ => Localizer.Format("#autoLOC_296540"), 
		};
	}
}
