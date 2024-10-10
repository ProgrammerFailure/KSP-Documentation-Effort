using System;
using System.Collections.Generic;
using CommNet;
using Experience;
using Experience.Effects;
using ns9;
using UnityEngine;

public class ScienceUtil
{
	public static ExperimentSituations GetExperimentSituation(Vessel v)
	{
		switch (v.situation)
		{
		case Vessel.Situations.FLYING:
			if (!(v.altitude < (double)v.mainBody.scienceValues.flyingAltitudeThreshold))
			{
				return ExperimentSituations.FlyingHigh;
			}
			return ExperimentSituations.FlyingLow;
		case Vessel.Situations.SPLASHED:
			return ExperimentSituations.SrfSplashed;
		case Vessel.Situations.LANDED:
		case Vessel.Situations.PRELAUNCH:
			return ExperimentSituations.SrfLanded;
		default:
			if (!(v.altitude < (double)v.mainBody.scienceValues.spaceAltitudeThreshold))
			{
				return ExperimentSituations.InSpaceHigh;
			}
			return ExperimentSituations.InSpaceLow;
		}
	}

	public static string GetExperimentBiome(CelestialBody body, double lat, double lon)
	{
		if (body.BiomeMap != null)
		{
			return body.BiomeMap.GetAtt(lat * 0.01745329238474369, lon * 0.01745329238474369).name;
		}
		return string.Empty;
	}

	public static string GetExperimentBiomeLocalized(CelestialBody body, double lat, double lon)
	{
		if (body.BiomeMap != null)
		{
			return body.BiomeMap.GetAtt(lat * 0.01745329238474369, lon * 0.01745329238474369).displayname;
		}
		return string.Empty;
	}

	public static string GetExperimentBodyName(string subjectID)
	{
		string result = "";
		try
		{
			string[] array = subjectID.Split('@');
			if (array.Length != 2)
			{
				return result;
			}
			string[] names = Enum.GetNames(typeof(ExperimentSituations));
			result = array[1].Split(names, StringSplitOptions.None)[0];
		}
		catch
		{
		}
		return result;
	}

	public static void GetExperimentFieldsFromScienceID(string subjectID, out string BodyName, out ExperimentSituations Situation, out string Biome)
	{
		string Situation2 = "";
		Situation = ExperimentSituations.SrfLanded;
		GetExperimentFieldsFromScienceID(subjectID, out BodyName, out Situation2, out Biome);
		ExperimentSituations experimentSituations = ExperimentSituations.SrfLanded;
		try
		{
			experimentSituations = (ExperimentSituations)Enum.Parse(typeof(ExperimentSituations), Situation2);
		}
		catch (Exception ex)
		{
			Debug.LogError("[ScienceSubject] - Error in converting Situation " + Situation2 + ex);
			return;
		}
		Situation = experimentSituations;
	}

	public static void GetExperimentFieldsFromScienceID(string subjectID, out string BodyName, out string Situation, out string Biome)
	{
		BodyName = "";
		Situation = "";
		Biome = "";
		string[] array = subjectID.Split('@');
		if (array.Length != 2 || ResearchAndDevelopment.GetExperiment(array[0]) == null)
		{
			return;
		}
		if (array[1].Contains("'_"))
		{
			int num = array[1].IndexOf('_');
			array[1] = array[1].Substring(0, num + 1);
		}
		string[] names = Enum.GetNames(typeof(ExperimentSituations));
		string[] array2 = array[1].Split(names, StringSplitOptions.None);
		string text = "";
		text = ((array2.Length != 1) ? array[1].Substring(array2[0].Length, array[1].Length - array2[0].Length - array2[1].Length) : array[1].Substring(array2[0].Length, array[1].Length - array2[0].Length));
		Situation = text;
		for (int i = 0; i < FlightGlobals.Bodies.Count; i++)
		{
			if (array2[0] == FlightGlobals.Bodies[i].bodyName)
			{
				BodyName = FlightGlobals.Bodies[i].bodyName;
				break;
			}
		}
		if (array2.Length == 2)
		{
			Biome = array2[1];
		}
	}

	public static string GenerateLocalizedTitle(string id)
	{
		return GenerateLocalizedTitle(id, skipAsteroidsComets: false);
	}

	public static string GenerateLocalizedTitle(string id, bool skipAsteroidsComets)
	{
		string result = "";
		string[] array = id.Split('@');
		if (array.Length != 2)
		{
			return result;
		}
		if (array[0] == "recovery")
		{
			return generateRecoveryLocalizedTitle(id);
		}
		ScienceExperiment scienceExperiment = null;
		scienceExperiment = ResearchAndDevelopment.GetExperiment(array[0]);
		if (scienceExperiment == null)
		{
			return result;
		}
		if (array[1].Contains("'_"))
		{
			int num = array[1].IndexOf('_');
			array[1] = array[1].Substring(0, num + 1);
		}
		string[] names = Enum.GetNames(typeof(ExperimentSituations));
		string[] array2 = array[1].Split(names, StringSplitOptions.None);
		if (array2[1].IndexOf('_') == 0)
		{
			if (skipAsteroidsComets)
			{
				return "";
			}
			array2[1] = array2[1].Substring(1, array2[1].Length - 1);
		}
		string text = "";
		text = ((array2.Length != 1) ? array[1].Substring(array2[0].Length, array[1].Length - array2[0].Length - array2[1].Length) : array[1].Substring(array2[0].Length, array[1].Length - array2[0].Length));
		ExperimentSituations experimentSituations = ExperimentSituations.SrfLanded;
		try
		{
			experimentSituations = (ExperimentSituations)Enum.Parse(typeof(ExperimentSituations), text);
		}
		catch (Exception ex)
		{
			Debug.LogError("[ScienceSubject] - Error in converting Situation " + text + ex);
			return result;
		}
		CelestialBody celestialBody = null;
		for (int i = 0; i < FlightGlobals.Bodies.Count; i++)
		{
			if (array2[0] == FlightGlobals.Bodies[i].bodyName)
			{
				celestialBody = FlightGlobals.Bodies[i];
				break;
			}
		}
		if (celestialBody != null)
		{
			result = GenerateScienceSubjectTitle(scienceExperiment, experimentSituations, celestialBody, (array2.Length == 2) ? array2[1] : "");
		}
		return result;
	}

	public static string generateRecoveryLocalizedTitle(string id)
	{
		string result = "";
		string[] array = id.Split('@');
		ScienceExperiment scienceExperiment = null;
		scienceExperiment = ResearchAndDevelopment.GetExperiment(array[0]);
		if (scienceExperiment == null)
		{
			return result;
		}
		string[] names = Enum.GetNames(typeof(RecoverySituations));
		string[] array2 = array[1].Split(names, StringSplitOptions.None);
		string text = "";
		text = ((array2.Length != 1) ? array[1].Substring(array2[0].Length, array[1].Length - array2[0].Length - array2[1].Length) : array[1].Substring(array2[0].Length, array[1].Length - array2[0].Length));
		RecoverySituations recoverySituations = RecoverySituations.Flew;
		try
		{
			recoverySituations = (RecoverySituations)Enum.Parse(typeof(RecoverySituations), text);
		}
		catch (Exception ex)
		{
			Debug.LogError("[ScienceSubject] - Error in converting Situation " + text + ex);
			return result;
		}
		CelestialBody celestialBody = null;
		for (int i = 0; i < FlightGlobals.Bodies.Count; i++)
		{
			if (array2[0] == FlightGlobals.Bodies[i].bodyName)
			{
				celestialBody = FlightGlobals.Bodies[i];
				break;
			}
		}
		if (celestialBody != null)
		{
			result = GenerateScienceSubjectRecoveryTitle(scienceExperiment, recoverySituations, celestialBody);
		}
		return result;
	}

	public static string GenerateScienceSubjectTitle(ScienceExperiment exp, ExperimentSituations sit, CelestialBody body, string biome = "", string displaybiome = "")
	{
		string result = "";
		try
		{
			string displayName = body.displayName;
			bool flag = BiomeIsUnlisted(body, biome);
			string empty = string.Empty;
			empty = ((!(displaybiome != string.Empty)) ? GetBiomedisplayName(body, biome) : displaybiome);
			result = sit switch
			{
				ExperimentSituations.FlyingHigh => (!(biome != string.Empty)) ? Localizer.Format("#autoLOC_301735", exp.experimentTitle, displayName) : Localizer.Format("#autoLOC_301731", exp.experimentTitle, displayName, empty), 
				ExperimentSituations.SrfLanded => (!(biome != string.Empty)) ? Localizer.Format("#autoLOC_301698", exp.experimentTitle, displayName) : ((!flag) ? Localizer.Format("#autoLOC_301693", exp.experimentTitle, displayName, empty) : Localizer.Format("#autoLOC_301689", exp.experimentTitle, empty)), 
				ExperimentSituations.SrfSplashed => (!(biome != string.Empty)) ? Localizer.Format("#autoLOC_301711", exp.experimentTitle, displayName) : Localizer.Format("#autoLOC_301707", exp.experimentTitle, displayName, empty), 
				ExperimentSituations.FlyingLow => (!(biome != string.Empty)) ? Localizer.Format("#autoLOC_301723", exp.experimentTitle, displayName) : Localizer.Format("#autoLOC_301719", exp.experimentTitle, displayName, empty), 
				ExperimentSituations.InSpaceHigh => (!(biome != string.Empty)) ? Localizer.Format("#autoLOC_301762", exp.experimentTitle, displayName) : Localizer.Format("#autoLOC_301758", exp.experimentTitle, displayName, empty), 
				ExperimentSituations.InSpaceLow => (!(biome != string.Empty)) ? Localizer.Format("#autoLOC_301748", exp.experimentTitle, displayName) : Localizer.Format("#autoLOC_301744", exp.experimentTitle, displayName, empty), 
				_ => (!(biome != string.Empty)) ? Localizer.Format("#autoLOC_301775", exp.experimentTitle, displayName) : Localizer.Format("#autoLOC_301771", exp.experimentTitle, displayName, empty), 
			};
		}
		catch (Exception ex)
		{
			Debug.LogError("[Science] - Unable to generate science subject " + exp.id + " " + sit.ToString() + " " + biome + ex);
		}
		return result;
	}

	public static string GenerateScienceSubjectRecoveryTitle(ScienceExperiment exp, RecoverySituations sit, CelestialBody body)
	{
		string result = "";
		try
		{
			string text = Localizer.Format("#autoLOC_7001301", body.displayName);
			result = Localizer.Format(sit.displayDescription(), text);
		}
		catch (Exception ex)
		{
			Debug.LogError("[Science] - Unable to generate science subject " + exp.id + " " + sit.ToString() + ex);
		}
		return result;
	}

	public static string GenerateScienceSubjectTitle(ScienceExperiment exp, ExperimentSituations sit, string sourceUid, string sourceTitle, CelestialBody body, string biome = "", string displaybiome = "")
	{
		string result = "";
		try
		{
			string text = Localizer.Format("#autoLOC_7001301", body.displayName);
			bool flag = BiomeIsUnlisted(body, biome);
			string empty = string.Empty;
			empty = ((!(displaybiome != string.Empty)) ? GetBiomedisplayName(body, biome) : displaybiome);
			result = sit switch
			{
				ExperimentSituations.FlyingHigh => (!(biome != string.Empty)) ? Localizer.Format("#autoLOC_301844", exp.experimentTitle, sourceTitle, text) : Localizer.Format("#autoLOC_301840", exp.experimentTitle, sourceTitle, text, empty), 
				ExperimentSituations.SrfLanded => (!(biome != string.Empty)) ? Localizer.Format("#autoLOC_301811", exp.experimentTitle, sourceTitle, text) : ((!flag) ? Localizer.Format("#autoLOC_301806", exp.experimentTitle, sourceTitle, text, empty) : Localizer.Format("#autoLOC_301802", exp.experimentTitle, sourceTitle, empty)), 
				ExperimentSituations.SrfSplashed => (!(biome != string.Empty)) ? Localizer.Format("#autoLOC_301822", exp.experimentTitle, sourceTitle, text) : Localizer.Format("#autoLOC_301818", exp.experimentTitle, sourceTitle, text, empty), 
				ExperimentSituations.FlyingLow => (!(biome != string.Empty)) ? Localizer.Format("#autoLOC_301833", exp.experimentTitle, sourceTitle, text) : Localizer.Format("#autoLOC_301829", exp.experimentTitle, sourceTitle, text, empty), 
				ExperimentSituations.InSpaceHigh => (!(biome != string.Empty)) ? Localizer.Format("#autoLOC_301869", exp.experimentTitle, sourceTitle, text) : Localizer.Format("#autoLOC_301865", exp.experimentTitle, sourceTitle, text, empty), 
				ExperimentSituations.InSpaceLow => (!(biome != string.Empty)) ? Localizer.Format("#autoLOC_301856", exp.experimentTitle, sourceTitle, text) : Localizer.Format("#autoLOC_301852", exp.experimentTitle, sourceTitle, text, empty), 
				_ => (!(biome != string.Empty)) ? Localizer.Format("#autoLOC_301881", exp.experimentTitle, sourceTitle, text) : Localizer.Format("#autoLOC_301877", exp.experimentTitle, sourceTitle, text, empty), 
			};
		}
		catch (Exception ex)
		{
			Debug.LogError("[Science] - Unable to generate science subject " + exp.id + " " + sit.ToString() + " " + biome + ex);
		}
		return result;
	}

	public static float GetTransmitterScore(IScienceDataTransmitter t)
	{
		return 1f / t.DataRate * (1f + (float)t.DataResourceCost) * (t.IsBusy() ? 100f : 1f) * (t.CanTransmit() ? 1f : 100000f);
	}

	[Obsolete("Use GetBestTransmitter(Vessel v) instead")]
	public static IScienceDataTransmitter GetBestTransmitter(List<IScienceDataTransmitter> vesselTransmitters)
	{
		int index = 0;
		float num = float.MaxValue;
		int count = vesselTransmitters.Count;
		while (count-- > 0)
		{
			IScienceDataTransmitter scienceDataTransmitter = vesselTransmitters[count];
			if (scienceDataTransmitter.CanTransmit())
			{
				float transmitterScore = GetTransmitterScore(scienceDataTransmitter);
				if (transmitterScore < num)
				{
					num = transmitterScore;
					index = count;
				}
			}
		}
		return vesselTransmitters[index];
	}

	public static IScienceDataTransmitter GetBestTransmitter(Vessel v)
	{
		if (CommNetScenario.CommNetEnabled && v.connection != null)
		{
			return v.connection.GetBestTransmitter();
		}
		IScienceDataTransmitter result = null;
		float num = float.MaxValue;
		int count = v.Parts.Count;
		while (count-- > 0)
		{
			Part part = v.Parts[count];
			int count2 = part.Modules.Count;
			while (count2-- > 0)
			{
				if (part.Modules[count2] is IScienceDataTransmitter scienceDataTransmitter && scienceDataTransmitter.CanTransmit())
				{
					float transmitterScore = GetTransmitterScore(scienceDataTransmitter);
					if (transmitterScore < num)
					{
						num = transmitterScore;
						result = scienceDataTransmitter;
					}
				}
			}
		}
		return result;
	}

	public static float GetLabScore(ModuleScienceLab lab)
	{
		return 1f;
	}

	public static bool RequiredUsageInternalAvailable(Vessel v, Part p, ExperimentUsageReqs req, ScienceExperiment exp, ref string message)
	{
		if (req == ExperimentUsageReqs.Never)
		{
			message = string.Empty;
			return false;
		}
		if ((req & ExperimentUsageReqs.VesselControl) != 0 && !v.IsControllable)
		{
			message = Localizer.Format("#autoLOC_302066", exp.experimentTitle);
			return false;
		}
		if ((req & ExperimentUsageReqs.CrewInVessel) != 0 && !CrewListHasType(v.GetVesselCrew(), ProtoCrewMember.KerbalType.Crew))
		{
			message = Localizer.Format("#autoLOC_302075", exp.experimentTitle);
			return false;
		}
		if ((req & ExperimentUsageReqs.CrewInPart) != 0 && !CrewListHasType(p.protoModuleCrew, ProtoCrewMember.KerbalType.Crew))
		{
			message = Localizer.Format("#autoLOC_302084", exp.experimentTitle, p.partInfo.title);
			return false;
		}
		if ((req & ExperimentUsageReqs.ScientistCrew) != 0)
		{
			if ((req & ExperimentUsageReqs.CrewInPart) != 0)
			{
				if (!CrewListHasEffect<SpecialExperimentSkill>(p.protoModuleCrew))
				{
					message = Localizer.Format("#autoLOC_302095", exp.experimentTitle, p.partInfo.title);
					return false;
				}
			}
			else
			{
				if ((req & ExperimentUsageReqs.CrewInVessel) == 0)
				{
					message = Localizer.Format("#autoLOC_302109", exp.experimentTitle);
					return false;
				}
				if (!CrewListHasEffect<SpecialExperimentSkill>(v.GetVesselCrew()))
				{
					message = Localizer.Format("#autoLOC_302103", exp.experimentTitle);
					return false;
				}
			}
		}
		return true;
	}

	public static bool CrewListHasType(List<ProtoCrewMember> crew, ProtoCrewMember.KerbalType type)
	{
		int count = crew.Count;
		do
		{
			if (count-- <= 0)
			{
				return false;
			}
		}
		while (crew[count].type != type);
		return true;
	}

	public static bool CrewListHasEffect<T>(List<ProtoCrewMember> crew) where T : ExperienceEffect
	{
		int count = crew.Count;
		do
		{
			if (count-- <= 0)
			{
				return false;
			}
		}
		while (!crew[count].HasEffect<T>());
		return true;
	}

	public static bool RequiredUsageExternalAvailable(Vessel v, Vessel vExt, ExperimentUsageReqs req, ScienceExperiment exp, ref string message)
	{
		if (req == ExperimentUsageReqs.Never)
		{
			message = string.Empty;
			return false;
		}
		if (!vExt.isEVA)
		{
			if ((req & ExperimentUsageReqs.ScientistCrew) != 0)
			{
				message = Localizer.Format("#autoLOC_302151", exp.experimentTitle);
			}
			else
			{
				message = Localizer.Format("#autoLOC_302155", exp.experimentTitle);
			}
			return false;
		}
		if ((req & ExperimentUsageReqs.VesselControl) != 0 && v.CurrentControlLevel == Vessel.ControlLevel.NONE)
		{
			message = Localizer.Format("#autoLOC_302164", exp.experimentTitle);
			return false;
		}
		List<ProtoCrewMember> vesselCrew = vExt.GetVesselCrew();
		ProtoCrewMember protoCrewMember = null;
		if (vesselCrew.Count > 0)
		{
			protoCrewMember = vesselCrew[0];
		}
		if (protoCrewMember == null)
		{
			message = Localizer.Format("#autoLOC_302177", exp.experimentTitle);
			return false;
		}
		if ((req & ExperimentUsageReqs.ScientistCrew) != 0)
		{
			if (protoCrewMember.type == ProtoCrewMember.KerbalType.Crew && !protoCrewMember.HasEffect<ExternalExperimentSkill>())
			{
				message = Localizer.Format("#autoLOC_302187", exp.experimentTitle);
				return false;
			}
		}
		else if (protoCrewMember.type != 0)
		{
			message = Localizer.Format("#autoLOC_302196", exp.experimentTitle);
			return false;
		}
		return true;
	}

	public static bool BiomeIsUnlisted(CelestialBody body, string biome)
	{
		if (!(body == null) && !(body.BiomeMap == null))
		{
			CBAttributeMapSO.MapAttribute[] attributes = body.BiomeMap.Attributes;
			int num = attributes.Length;
			do
			{
				if (num-- > 0)
				{
					if (attributes[num].name == biome)
					{
						return false;
					}
					continue;
				}
				return true;
			}
			while (!attributes[num].name.Contains(" ") || !(attributes[num].name.Replace(" ", string.Empty) == biome));
			return false;
		}
		return false;
	}

	public static string GetBiomedisplayName(CelestialBody body, string biome)
	{
		return GetBiomedisplayName(body, biome, formatted: true);
	}

	public static string GetBiomedisplayName(CelestialBody body, string biome, bool formatted)
	{
		if (body != null && body.BiomeMap != null)
		{
			CBAttributeMapSO.MapAttribute[] attributes = body.BiomeMap.Attributes;
			int num = attributes.Length;
			while (num-- > 0)
			{
				if (!(attributes[num].name == biome))
				{
					if (attributes[num].name.Contains(" ") && attributes[num].name.Replace(" ", string.Empty) == biome)
					{
						if (!formatted)
						{
							return attributes[num].localizationTag;
						}
						return attributes[num].displayname;
					}
					continue;
				}
				if (!formatted)
				{
					return attributes[num].localizationTag;
				}
				return attributes[num].displayname;
			}
		}
		return ResearchAndDevelopment.GetMiniBiomedisplayNameByScienceID(biome, formatted);
	}
}
