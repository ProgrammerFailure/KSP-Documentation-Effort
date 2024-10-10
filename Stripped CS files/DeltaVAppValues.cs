using System;
using System.Collections.Generic;

public class DeltaVAppValues
{
	public class InfoLine
	{
		public string name;

		public string displayAppName;

		public string displayStageName;

		public Func<DeltaVStageInfo, DeltaVSituationOptions, string> infoValue;

		public string valueSuffix = "";

		public bool enabled;

		public bool Enabled
		{
			get
			{
				return enabled;
			}
			set
			{
				enabled = value;
				DeltaVGlobals.DeltaVAppValues.UpdateEnabledInfoItems();
			}
		}

		public InfoLine(string name, string appName, string stageName, Func<DeltaVStageInfo, DeltaVSituationOptions, string> value)
			: this(name, appName, stageName, value, "", enabled: true)
		{
		}

		public InfoLine(string name, string appName, string stageName, Func<DeltaVStageInfo, DeltaVSituationOptions, string> value, string valueSuffix)
			: this(name, appName, stageName, value, valueSuffix, enabled: true)
		{
		}

		public InfoLine(string name, string appName, string stageName, Func<DeltaVStageInfo, DeltaVSituationOptions, string> value, bool enabled)
			: this(name, appName, stageName, value, "", enabled: true)
		{
		}

		public InfoLine(string name, string appName, string stageName, Func<DeltaVStageInfo, DeltaVSituationOptions, string> value, string valueSuffix, bool enabled)
		{
			this.name = name;
			displayAppName = appName;
			displayStageName = stageName;
			infoValue = value;
			this.valueSuffix = valueSuffix;
			this.enabled = enabled;
		}

		public void InitEnabledValue(bool enabled)
		{
			this.enabled = enabled;
		}

		public string GetValue(DeltaVStageInfo stage, DeltaVSituationOptions situation)
		{
			if (infoValue != null)
			{
				return infoValue(stage, situation);
			}
			return "";
		}
	}

	public CelestialBody body;

	public DeltaVSituationOptions situation;

	public double altitude;

	public List<InfoLine> infoLines;

	public List<string> infoLinesEnabled;

	public float atmPressure => (float)(body.GetPressure(altitude) * 0.009869232667160128);

	public double atmDensity => body.GetDensity(body.GetPressure(altitude), body.GetTemperature(altitude));

	public DeltaVAppValues()
	{
		body = FlightGlobals.GetHomeBody();
		situation = DeltaVSituationOptions.Vaccum;
		altitude = 0.0;
		infoLines = new List<InfoLine>();
		infoLinesEnabled = new List<string>();
	}

	public void LoadEnabledInfoLines(string settingString)
	{
		infoLinesEnabled.Clear();
		string[] array = settingString.Split(',');
		for (int i = 0; i < array.Length; i++)
		{
			infoLinesEnabled.Add(array[i]);
		}
		for (int j = 0; j < infoLines.Count; j++)
		{
			infoLines[j].InitEnabledValue(infoLinesEnabled.Contains(infoLines[j].name));
		}
	}

	public void UpdateEnabledInfoItems()
	{
		for (int i = 0; i < infoLines.Count; i++)
		{
			if (infoLines[i].Enabled)
			{
				infoLinesEnabled.AddUnique(infoLines[i].name);
			}
			else
			{
				infoLinesEnabled.Remove(infoLines[i].name);
			}
		}
		GameSettings.STAGE_GROUP_INFO_ITEMS = string.Join(",", infoLinesEnabled.ToArray());
		GameSettings.SaveGameSettingsOnly();
		GameEvents.onDeltaVAppInfoItemsChanged.Fire();
	}
}
