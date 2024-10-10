using System;
using System.Collections.Generic;
using Contracts.Agents;
using Contracts.Parameters;
using FinePrint;
using FinePrint.Utilities;
using ns9;
using UnityEngine;

namespace Contracts.Templates;

[Serializable]
public class PartTest : Contract
{
	[SerializeField]
	public string PartName;

	[SerializeField]
	public bool PartIsExperimental;

	[NonSerialized]
	public AvailablePart tgtPart;

	[SerializeField]
	public CelestialBody destination;

	[SerializeField]
	public Vessel.Situations tgtSituation;

	[SerializeField]
	public bool hauled;

	public bool Hauled => hauled;

	public override bool MeetRequirements()
	{
		if (!ProgressTracking.Instance.NodeComplete("FirstLaunch"))
		{
			return false;
		}
		return true;
	}

	public override List<CelestialBody> GetWeightBodies()
	{
		return new List<CelestialBody> { destination };
	}

	public override string GetDescription()
	{
		return TextGen.GenerateBackStories(GetType().Name, base.Agent.Title, PartName, "Parts" + PartName, base.MissionSeed, allowGenericIntroduction: true, allowGenericProblem: false, allowGenericConclusion: true);
	}

	public override string GetSynopsys()
	{
		if (hauled)
		{
			return Localizer.Format("#autoLOC_6100002", PartName, GetSituationString(tgtSituation, destination, ""));
		}
		return Localizer.Format("#autoLOC_6100003", PartName, GetSituationString(tgtSituation, destination, ""));
	}

	public override string GetTitle()
	{
		if (hauled)
		{
			return Localizer.Format("#autoLOC_6100004", PartName, GetSituationString(tgtSituation, destination, ""));
		}
		return Localizer.Format("#autoLOC_6100005", PartName, GetSituationString(tgtSituation, destination, ""));
	}

	public override string MessageCompleted()
	{
		if (hauled)
		{
			return Localizer.Format("#autoLOC_273203");
		}
		return Localizer.Format("#autoLOC_6003090");
	}

	public string GetSituationString(Vessel.Situations sit, CelestialBody body, string biomeName)
	{
		if (string.IsNullOrEmpty(biomeName))
		{
			switch (sit)
			{
			case Vessel.Situations.FLYING:
				if (hauled)
				{
					return Localizer.Format("#autoLOC_6100008", body.displayName);
				}
				return Localizer.Format("#autoLOC_6100009", body.displayName);
			case Vessel.Situations.LANDED:
				if (hauled)
				{
					if (body.ocean)
					{
						return Localizer.Format("#autoLOC_6100010", body.displayName);
					}
					return Localizer.Format("#autoLOC_6100011", body.displayName);
				}
				return Localizer.Format("#autoLOC_6100012", body.displayName);
			case Vessel.Situations.SPLASHED:
				if (hauled)
				{
					return Localizer.Format("#autoLOC_6100015", body.displayName);
				}
				return Localizer.Format("#autoLOC_6100016", body.displayName);
			case Vessel.Situations.PRELAUNCH:
				if (hauled)
				{
					return Localizer.Format("#autoLOC_6100006");
				}
				return Localizer.Format("#autoLOC_6100007");
			default:
				return "No situation report available";
			case Vessel.Situations.ESCAPING:
				if (hauled)
				{
					return Localizer.Format("#autoLOC_6100019", body.displayName);
				}
				return Localizer.Format("#autoLOC_6100020", body.displayName);
			case Vessel.Situations.ORBITING:
				if (hauled)
				{
					return Localizer.Format("#autoLOC_6100013", body.displayName);
				}
				return Localizer.Format("#autoLOC_6100014", body.displayName);
			case Vessel.Situations.SUB_ORBITAL:
				if (hauled)
				{
					return Localizer.Format("#autoLOC_6100017", body.displayName);
				}
				return Localizer.Format("#autoLOC_6100018", body.displayName);
			}
		}
		string displayName = body.displayName;
		switch (sit)
		{
		case Vessel.Situations.FLYING:
			if (hauled)
			{
				return Localizer.Format("#autoLOC_6100023", displayName, biomeName);
			}
			return Localizer.Format("#autoLOC_6100024", displayName, biomeName);
		case Vessel.Situations.LANDED:
			if (hauled)
			{
				if (body.ocean)
				{
					return Localizer.Format("#autoLOC_6100025", displayName, biomeName);
				}
				return Localizer.Format("#autoLOC_6100026", displayName, biomeName);
			}
			return Localizer.Format("#autoLOC_6100027", displayName, biomeName);
		case Vessel.Situations.SPLASHED:
			if (hauled)
			{
				return Localizer.Format("#autoLOC_6100030", displayName, biomeName);
			}
			return Localizer.Format("#autoLOC_6100031", displayName, biomeName);
		case Vessel.Situations.PRELAUNCH:
			if (hauled)
			{
				return Localizer.Format("#autoLOC_6100021");
			}
			return Localizer.Format("#autoLOC_6100022");
		default:
			return "No situation report available";
		case Vessel.Situations.ESCAPING:
			if (hauled)
			{
				return Localizer.Format("#autoLOC_6100034", displayName, biomeName);
			}
			return Localizer.Format("#autoLOC_6100035", displayName, biomeName);
		case Vessel.Situations.ORBITING:
			if (hauled)
			{
				return Localizer.Format("#autoLOC_6100028", displayName, biomeName);
			}
			return Localizer.Format("#autoLOC_6100029", displayName, biomeName);
		case Vessel.Situations.SUB_ORBITAL:
			if (hauled)
			{
				return Localizer.Format("#autoLOC_6100032", displayName, biomeName);
			}
			return Localizer.Format("#autoLOC_6100033", displayName, biomeName);
		}
	}

	public override void OnLoad(ConfigNode node)
	{
		if (node.HasValue("part"))
		{
			tgtPart = PartLoader.getPartInfoByName(node.GetValue("part"));
			PartName = ((tgtPart != null) ? tgtPart.title : "Part");
		}
		if (node.HasValue("haul"))
		{
			hauled = bool.Parse(node.GetValue("haul"));
		}
		if (node.HasValue("dest"))
		{
			destination = FlightGlobals.Bodies[int.Parse(node.GetValue("dest"))];
		}
		if (node.HasValue("sit"))
		{
			tgtSituation = (Vessel.Situations)Enum.Parse(typeof(Vessel.Situations), node.GetValue("sit"));
		}
		if (node.HasValue("exp"))
		{
			PartIsExperimental = bool.Parse(node.GetValue("exp"));
		}
	}

	public override void OnSave(ConfigNode node)
	{
		node.AddValue("part", (tgtPart != null) ? tgtPart.name : "Part");
		node.AddValue("haul", hauled);
		node.AddValue("dest", FlightGlobals.Bodies.IndexOf(destination));
		node.AddValue("sit", tgtSituation.ToString());
		node.AddValue("exp", PartIsExperimental);
	}

	public override bool Generate()
	{
		PartTest[] currentContracts = ContractSystem.Instance.GetCurrentContracts<PartTest>();
		if (currentContracts.Length >= ContractDefs.Test.MaximumExistent)
		{
			return false;
		}
		List<ModuleTestSubject> list = new List<ModuleTestSubject>();
		List<string> list2 = new List<string>();
		int num = currentContracts.Length;
		for (int i = 0; i < num; i++)
		{
			list2.Add(currentContracts[i].PartName);
		}
		List<ProtoTechNode> nextUnavailableNodes = AssetBase.RnDTechTree.GetNextUnavailableNodes();
		num = PartLoader.LoadedPartsList.Count;
		for (int j = 0; j < num; j++)
		{
			AvailablePart availablePart = PartLoader.LoadedPartsList[j];
			if (availablePart.TechHidden || list2.Contains(availablePart.title))
			{
				continue;
			}
			int count = nextUnavailableNodes.Count;
			while (count-- > 0)
			{
				if (!(nextUnavailableNodes[count].techID != availablePart.TechRequired))
				{
					ModuleTestSubject moduleTestSubject = availablePart.partPrefab.FindModuleImplementing<ModuleTestSubject>();
					if (moduleTestSubject != null)
					{
						list.Add(moduleTestSubject);
					}
					break;
				}
			}
		}
		if (list.Count < 5 || base.Prestige == ContractPrestige.Trivial)
		{
			num = PartLoader.LoadedPartsList.Count;
			for (int k = 0; k < num; k++)
			{
				AvailablePart availablePart2 = PartLoader.LoadedPartsList[k];
				if (!list2.Contains(availablePart2.title) && ResearchAndDevelopment.GetTechnologyState(availablePart2.TechRequired) == RDTech.State.Available)
				{
					ModuleTestSubject moduleTestSubject2 = availablePart2.partPrefab.FindModuleImplementing<ModuleTestSubject>();
					if (moduleTestSubject2 != null && !list.Contains(moduleTestSubject2))
					{
						list.Add(moduleTestSubject2);
					}
				}
			}
		}
		if (list.Count <= 0)
		{
			return false;
		}
		ModuleTestSubject moduleTestSubject3 = list[UnityEngine.Random.Range(0, list.Count)];
		tgtPart = moduleTestSubject3.GetComponent<Part>().partInfo;
		PartName = ((tgtPart != null) ? tgtPart.title : "Part");
		if (ResearchAndDevelopment.GetTechnologyState((tgtPart != null) ? tgtPart.TechRequired : "Technology") != RDTech.State.Available)
		{
			PartIsExperimental = true;
		}
		List<Vessel.Situations> list3 = new List<Vessel.Situations>();
		if (moduleTestSubject3.SituationAvailable(Vessel.Situations.PRELAUNCH))
		{
			list3.Add(Vessel.Situations.PRELAUNCH);
		}
		if (moduleTestSubject3.SituationAvailable(Vessel.Situations.LANDED))
		{
			list3.Add(Vessel.Situations.LANDED);
		}
		if (moduleTestSubject3.SituationAvailable(Vessel.Situations.SPLASHED))
		{
			list3.Add(Vessel.Situations.SPLASHED);
		}
		if (moduleTestSubject3.SituationAvailable(Vessel.Situations.FLYING))
		{
			list3.Add(Vessel.Situations.FLYING);
		}
		if (moduleTestSubject3.SituationAvailable(Vessel.Situations.SUB_ORBITAL))
		{
			list3.Add(Vessel.Situations.SUB_ORBITAL);
		}
		if (moduleTestSubject3.SituationAvailable(Vessel.Situations.ORBITING))
		{
			list3.Add(Vessel.Situations.ORBITING);
		}
		if (moduleTestSubject3.SituationAvailable(Vessel.Situations.ESCAPING))
		{
			list3.Add(Vessel.Situations.ESCAPING);
		}
		if (moduleTestSubject3.SituationAvailable(Vessel.Situations.DOCKED))
		{
			list3.Add(Vessel.Situations.DOCKED);
		}
		List<PartTestConstraint> constraints = new List<PartTestConstraint>();
		num = moduleTestSubject3.constraints.Count;
		for (int l = 0; l < num; l++)
		{
			PartTestConstraint partTestConstraint = moduleTestSubject3.constraints[l];
			if (partTestConstraint.PrestigeAvailable(base.Prestige))
			{
				constraints.Add(partTestConstraint);
			}
		}
		List<CelestialBody> list4 = null;
		if (moduleTestSubject3.useProgressForBodies)
		{
			switch (prestige)
			{
			default:
				list4 = new List<CelestialBody> { Planetarium.fetch.Home };
				break;
			case ContractPrestige.Exceptional:
				list4 = ProgressUtilities.GetBodiesProgress(bodyReached: true, (CelestialBody cb) => cb != Planetarium.fetch.Home);
				list4.AddRange(ProgressUtilities.GetNextUnreached(1, (CelestialBody cb) => cb != Planetarium.fetch.Home));
				break;
			case ContractPrestige.Significant:
				list4 = ProgressUtilities.GetBodiesProgress(bodyReached: true, (CelestialBody cb) => cb != Planetarium.fetch.Sun);
				break;
			}
		}
		else
		{
			list4 = new List<CelestialBody>(FlightGlobals.Bodies);
		}
		if (list4 != null && list4.Count >= 1)
		{
			List<CelestialBody> list5 = new List<CelestialBody>();
			num = list4.Count;
			for (int m = 0; m < num; m++)
			{
				CelestialBody celestialBody = list4[m];
				int count2 = moduleTestSubject3.constraints.Count;
				for (int n = 0; n < count2; n++)
				{
					PartTestConstraint partTestConstraint2 = moduleTestSubject3.constraints[n];
					if (!partTestConstraint2.BodyAvailable(celestialBody))
					{
						continue;
					}
					switch (partTestConstraint2.type)
					{
					case PartTestConstraint.ConstraintType.ATMOSPHERE:
						if (!partTestConstraint2.Test(celestialBody.atmosphere))
						{
							list5.AddUnique(celestialBody);
						}
						break;
					case PartTestConstraint.ConstraintType.OXYGEN:
						if (!partTestConstraint2.Test(celestialBody.atmosphereContainsOxygen) || !partTestConstraint2.Test(celestialBody.atmosphere))
						{
							list5.AddUnique(celestialBody);
						}
						break;
					}
				}
			}
			num = list5.Count;
			for (int num2 = 0; num2 < num; num2++)
			{
				list4.Remove(list5[num2]);
			}
			if (list4.Count == 0)
			{
				return false;
			}
			destination = WeightedBodyChoice(list4);
			constraints.RemoveAll((PartTestConstraint c) => !c.BodyAvailable(destination));
			list3.RemoveAll((Vessel.Situations s) => !PartTestConstraint.SituationAvailable(constraints, s));
			if (!destination.atmosphere)
			{
				list3.Remove(Vessel.Situations.FLYING);
			}
			if (!destination.ocean)
			{
				list3.Remove(Vessel.Situations.SPLASHED);
			}
			if (!destination.hasSolidSurface)
			{
				list3.Remove(Vessel.Situations.LANDED);
				list3.Remove(Vessel.Situations.SPLASHED);
			}
			if (!destination.isHomeWorld)
			{
				list3.Remove(Vessel.Situations.PRELAUNCH);
			}
			if (list3.Count < 1)
			{
				return false;
			}
			if (moduleTestSubject3.usePrestigeForSit)
			{
				if (base.Prestige == ContractPrestige.Trivial)
				{
					List<Vessel.Situations> list6 = new List<Vessel.Situations>(3);
					if (destination.isHomeWorld)
					{
						list6.Add(Vessel.Situations.PRELAUNCH);
						list6.Add(Vessel.Situations.LANDED);
						if (destination.atmosphere)
						{
							list6.Add(Vessel.Situations.FLYING);
						}
						if (destination.ocean)
						{
							list6.Add(Vessel.Situations.SPLASHED);
						}
					}
					else
					{
						list6.Add(Vessel.Situations.ORBITING);
						list6.Add(Vessel.Situations.ESCAPING);
						if (destination.atmosphere)
						{
							list6.Add(Vessel.Situations.FLYING);
						}
					}
					CombineSituations(list6, list3);
					if (list6.Count <= 0)
					{
						return false;
					}
					tgtSituation = list6[UnityEngine.Random.Range(0, list6.Count)];
					if (!ProgressTracking.Instance.reachSpace.IsComplete)
					{
						if (!destination.isHomeWorld)
						{
							return false;
						}
						if (tgtSituation == Vessel.Situations.SUB_ORBITAL)
						{
							if (!list3.Contains(Vessel.Situations.FLYING))
							{
								return false;
							}
							tgtSituation = Vessel.Situations.FLYING;
						}
					}
				}
				else if (base.Prestige == ContractPrestige.Significant)
				{
					List<Vessel.Situations> list7 = new List<Vessel.Situations>
					{
						Vessel.Situations.FLYING,
						Vessel.Situations.SUB_ORBITAL,
						Vessel.Situations.ORBITING,
						Vessel.Situations.ESCAPING
					};
					if (destination == Planetarium.fetch.Sun)
					{
						RemoveSituations(list7, Vessel.Situations.LANDED, Vessel.Situations.SPLASHED, Vessel.Situations.FLYING, Vessel.Situations.SUB_ORBITAL, Vessel.Situations.ESCAPING);
					}
					CombineSituations(list7, list3);
					if (list7.Count <= 0)
					{
						return false;
					}
					tgtSituation = list7[UnityEngine.Random.Range(0, list7.Count)];
				}
				else
				{
					List<Vessel.Situations> list8 = new List<Vessel.Situations>
					{
						Vessel.Situations.LANDED,
						Vessel.Situations.SPLASHED,
						Vessel.Situations.FLYING,
						Vessel.Situations.SUB_ORBITAL,
						Vessel.Situations.ORBITING,
						Vessel.Situations.ESCAPING
					};
					if (destination == Planetarium.fetch.Sun)
					{
						RemoveSituations(list8, Vessel.Situations.LANDED, Vessel.Situations.SPLASHED, Vessel.Situations.FLYING, Vessel.Situations.SUB_ORBITAL, Vessel.Situations.ESCAPING);
					}
					if (destination == Planetarium.fetch.Home || !destination.hasSolidSurface)
					{
						RemoveSituations(list8, Vessel.Situations.LANDED, Vessel.Situations.SPLASHED);
					}
					if (!destination.atmosphere)
					{
						RemoveSituations(list8, Vessel.Situations.FLYING);
					}
					if (!destination.ocean)
					{
						RemoveSituations(list8, Vessel.Situations.SPLASHED);
					}
					CombineSituations(list8, list3);
					if (list8.Count <= 0)
					{
						return false;
					}
					tgtSituation = list8[UnityEngine.Random.Range(0, list8.Count)];
				}
			}
			else
			{
				tgtSituation = list3[UnityEngine.Random.Range(0, list3.Count)];
			}
			constraints.RemoveAll((PartTestConstraint c) => !c.SituationAvailable(tgtSituation));
			double num3 = 0.0;
			double num4 = double.MaxValue;
			bool flag = false;
			double num5 = 0.0;
			double num6 = double.MaxValue;
			bool flag2 = false;
			double num7 = 0.0;
			double num8 = double.MaxValue;
			double num9 = 0.0;
			double num10 = double.MaxValue;
			double num11 = double.MaxValue;
			double num12 = 0.0;
			bool flag3 = false;
			double num13 = 0.0;
			double num14 = 0.0;
			PartTestConstraint.TestRepeatability testRepeatability = PartTestConstraint.TestRepeatability.ONCEPERPART;
			if (destination.atmosphere && tgtSituation == Vessel.Situations.FLYING)
			{
				num4 = destination.atmosphereDepth;
			}
			else if (tgtSituation == Vessel.Situations.SUB_ORBITAL || tgtSituation == Vessel.Situations.ORBITING || tgtSituation == Vessel.Situations.ESCAPING)
			{
				num3 = destination.minOrbitalDistance - destination.Radius;
			}
			num = constraints.Count;
			for (int num15 = 0; num15 < num; num15++)
			{
				PartTestConstraint partTestConstraint3 = constraints[num15];
				switch (partTestConstraint3.type)
				{
				case PartTestConstraint.ConstraintType.SPEED:
					if (partTestConstraint3.test == PartTestConstraint.ConstraintTest.const_0)
					{
						num5 = Math.Max(num5, partTestConstraint3.valueD);
					}
					else if (partTestConstraint3.test == PartTestConstraint.ConstraintTest.const_1)
					{
						num6 = Math.Min(num6, partTestConstraint3.valueD);
					}
					flag2 = true;
					break;
				case PartTestConstraint.ConstraintType.SPEEDENV:
					if (partTestConstraint3.test == PartTestConstraint.ConstraintTest.const_0)
					{
						num9 = Math.Max(num9, partTestConstraint3.valueD);
					}
					else if (partTestConstraint3.test == PartTestConstraint.ConstraintTest.const_1)
					{
						num10 = Math.Min(num10, partTestConstraint3.valueD);
					}
					break;
				case PartTestConstraint.ConstraintType.ALTITUDE:
					if (partTestConstraint3.test == PartTestConstraint.ConstraintTest.const_0)
					{
						num3 = Math.Max(num3, partTestConstraint3.valueD);
					}
					else if (partTestConstraint3.test == PartTestConstraint.ConstraintTest.const_1)
					{
						num4 = Math.Min(num4, partTestConstraint3.valueD);
					}
					flag = true;
					break;
				case PartTestConstraint.ConstraintType.ALTITUDEENV:
					if (partTestConstraint3.test == PartTestConstraint.ConstraintTest.const_0)
					{
						num7 = Math.Max(num7, partTestConstraint3.valueD);
					}
					else if (partTestConstraint3.test == PartTestConstraint.ConstraintTest.const_1)
					{
						num8 = Math.Min(num8, partTestConstraint3.valueD);
					}
					break;
				case PartTestConstraint.ConstraintType.DENSITY:
				{
					double altitudeForDensity = CelestialUtilities.GetAltitudeForDensity(destination, partTestConstraint3.valueD);
					if (partTestConstraint3.test == PartTestConstraint.ConstraintTest.const_0)
					{
						num4 = Math.Min(num4, altitudeForDensity);
					}
					else if (partTestConstraint3.test == PartTestConstraint.ConstraintTest.const_1)
					{
						num3 = Math.Max(num3, altitudeForDensity);
					}
					flag = true;
					break;
				}
				case PartTestConstraint.ConstraintType.DYNAMICPRESSURE:
					if (partTestConstraint3.test == PartTestConstraint.ConstraintTest.const_0)
					{
						num12 = Math.Max(num12, partTestConstraint3.valueD);
					}
					else if (partTestConstraint3.test == PartTestConstraint.ConstraintTest.const_1)
					{
						num11 = Math.Min(num11, partTestConstraint3.valueD);
					}
					flag3 = true;
					break;
				case PartTestConstraint.ConstraintType.REPEATABILITY:
					testRepeatability = partTestConstraint3.valueR;
					break;
				}
			}
			if (!(num3 > num4) && num5 <= num6)
			{
				if (list.Count < ContractDefs.Test.SubjectsToRepeat && testRepeatability < PartTestConstraint.TestRepeatability.ONCEPERPART)
				{
					testRepeatability = PartTestConstraint.TestRepeatability.ONCEPERPART;
				}
				if (list.Count < ContractDefs.Test.SubjectsToRepeat * 2 && testRepeatability < PartTestConstraint.TestRepeatability.BODYANDSITUATION)
				{
					testRepeatability = PartTestConstraint.TestRepeatability.BODYANDSITUATION;
				}
				if (num4 - num3 < num8)
				{
					num8 = num4 - num3;
				}
				if (num8 < num7)
				{
					num7 = num8;
				}
				if (tgtSituation != Vessel.Situations.LANDED && tgtSituation != Vessel.Situations.SPLASHED && tgtSituation != Vessel.Situations.PRELAUNCH)
				{
					if (tgtSituation == Vessel.Situations.FLYING)
					{
						double density = destination.GetDensity(destination.GetPressure(num4), destination.GetTemperature(num4));
						double density2 = destination.GetDensity(destination.GetPressure(num3), destination.GetTemperature(num3));
						double num16 = 0.005 * density2 * num6 * num6;
						if (num6 == double.MaxValue)
						{
							num16 = double.MaxValue;
						}
						double num17 = 0.005 * density * num5 * num5;
						if (num16 < num12)
						{
							return false;
						}
						if (num17 > num11)
						{
							return false;
						}
						if (flag)
						{
							num13 = Math.Ceiling((double)UnityEngine.Random.Range((float)num7, (float)num8) * 0.001) * 1000.0;
							num3 = Math.Round((double)UnityEngine.Random.Range((float)num3, (float)(num4 - num13)) * 0.001) * 1000.0;
							num4 = num3 + num13;
						}
						if (flag3)
						{
							density = destination.GetDensity(destination.GetPressure(num4), destination.GetTemperature(num4));
							density2 = destination.GetDensity(destination.GetPressure(num3), destination.GetTemperature(num3));
							num16 = 0.005 * density2 * num6 * num6;
							num17 = 0.005 * density * num5 * num5;
							if (num6 == double.MaxValue)
							{
								num16 = double.MaxValue;
							}
							if (num16 < num12)
							{
								num6 = Math.Sqrt(num12 / (0.005 * density2));
							}
							if (num17 > num11)
							{
								num5 = Math.Sqrt(num11 / (0.005 * density));
							}
							if (num6 < num5)
							{
								return false;
							}
						}
						if (flag2)
						{
							if (num6 - num5 < num10)
							{
								num10 = num6 - num5;
							}
							if (num10 < num9)
							{
								num9 = num10;
							}
							num14 = Math.Ceiling((double)UnityEngine.Random.Range((float)num9, (float)num10) * 0.1) * 10.0;
							num5 = Math.Round((double)UnityEngine.Random.Range((float)num5, (float)(num6 - num14)) * 0.1) * 10.0;
							num6 = num5 + num14;
						}
					}
					else
					{
						if (flag)
						{
							double sphereOfInfluence = destination.sphereOfInfluence;
							if (num4 > sphereOfInfluence)
							{
								num4 = sphereOfInfluence;
							}
							num13 = Math.Ceiling((double)UnityEngine.Random.Range((float)num7, (float)num8) * 0.0001) * 10000.0;
							num3 = Math.Round((double)UnityEngine.Random.Range((float)num3, (float)(num4 - num13)) * 0.0001) * 10000.0;
							num4 = num3 + num13;
						}
						if (flag2)
						{
							if (num6 - num5 < num10)
							{
								num10 = num6 - num5;
							}
							if (num10 < num9)
							{
								num9 = num10;
							}
							num14 = Math.Round((double)UnityEngine.Random.Range((float)num9, (float)num10) * 0.01) * 100.0;
							num5 = Math.Round((double)UnityEngine.Random.Range((float)num5, (float)(num6 - num14)) * 0.01) * 100.0;
							num6 = num5 + num14;
						}
					}
				}
				KSPRandom kSPRandom = new KSPRandom(base.MissionSeed);
				hauled = false;
				if (ContractDefs.Test.AllowHauls && (!destination.isHomeWorld || (tgtSituation != Vessel.Situations.PRELAUNCH && tgtSituation != Vessel.Situations.LANDED && tgtSituation != Vessel.Situations.SPLASHED)))
				{
					int num18 = 0;
					num18 = prestige switch
					{
						ContractPrestige.Significant => ContractDefs.Test.SignificantHaulChance, 
						ContractPrestige.Trivial => ContractDefs.Test.TrivialHaulChance, 
						_ => ContractDefs.Test.ExceptionalHaulChance, 
					};
					if (kSPRandom.Next(0, 100) < num18)
					{
						hauled = true;
					}
				}
				agent = AgentList.Instance.GetPartManufacturer(moduleTestSubject3.part.partInfo);
				ContractParameter contractParameter = AddParameter(new Contracts.Parameters.PartTest(moduleTestSubject3.part.partInfo, hauled ? null : moduleTestSubject3.GetTestNotes(), testRepeatability, destination, tgtSituation, kSPRandom.NextDouble().ToString("G17"), hauled));
				IContractParameterHost contractParameterHost2;
				if (!hauled)
				{
					IContractParameterHost contractParameterHost = contractParameter;
					contractParameterHost2 = contractParameterHost;
				}
				else
				{
					IContractParameterHost contractParameterHost = this;
					contractParameterHost2 = contractParameterHost;
				}
				IContractParameterHost contractParameterHost3 = contractParameterHost2;
				contractParameterHost3.AddParameter(new ReachDestination(destination, "")).DisableOnStateChange = false;
				contractParameterHost3.AddParameter(new ReachSituation(tgtSituation, "")).DisableOnStateChange = false;
				if (num13 != 0.0)
				{
					contractParameterHost3.AddParameter(new ReachAltitudeEnvelope((float)num3, (float)num4)).DisableOnStateChange = false;
				}
				if (num14 != 0.0)
				{
					contractParameterHost3.AddParameter(new ReachSpeedEnvelope((float)num5, (float)num6)).DisableOnStateChange = false;
				}
				float num19 = GameVariables.Instance.ScoreSituation(tgtSituation, destination) * (hauled ? 0.75f : 1f);
				SetExpiry(ContractDefs.Test.Expire.MinimumExpireDays, ContractDefs.Test.Expire.MaximumExpireDays);
				SetDeadlineDays(ContractDefs.Test.Expire.DeadlineDays, destination);
				SetFunds(Mathf.Round(ContractDefs.Test.Funds.BaseAdvance * num19), Mathf.Round(ContractDefs.Test.Funds.BaseReward * num19), Mathf.Round(ContractDefs.Test.Funds.BaseFailure * num19), destination);
				SetScience(Mathf.Round(ContractDefs.Test.Science.BaseReward * num19));
				SetReputation(Mathf.Round(ContractDefs.Test.Reputation.BaseReward * num19), Mathf.Round(ContractDefs.Test.Reputation.BaseFailure * num19));
				float num20 = ((tgtPart != null) ? tgtPart.cost : 0f);
				int num21 = ((tgtPart != null) ? tgtPart.entryCost : 0);
				if (FundsAdvance < (double)num20)
				{
					FundsAdvance = Mathf.Round(num20 * GameVariables.Instance.GetContractFundsAdvanceFactor(base.Prestige));
				}
				if (FundsFailure < (double)num20)
				{
					FundsFailure = Mathf.Round(num20 * GameVariables.Instance.GetContractFundsFailureFactor(base.Prestige));
				}
				if (FundsCompletion < (double)num21)
				{
					FundsCompletion = Mathf.Round((float)num21 * GameVariables.Instance.GetContractFundsCompletionFactor(base.Prestige));
				}
				return true;
			}
			return false;
		}
		return false;
	}

	public override void OnRegister()
	{
		GameEvents.OnPartPurchased.Add(OnPartResearched);
	}

	public override void OnUnregister()
	{
		GameEvents.OnPartPurchased.Remove(OnPartResearched);
	}

	public override void OnAccepted()
	{
		if (tgtPart != null)
		{
			PartIsExperimental = !ResearchAndDevelopment.PartModelPurchased(tgtPart) || ResearchAndDevelopment.IsExperimentalPart(tgtPart);
			if (PartIsExperimental)
			{
				ResearchAndDevelopment.AddExperimentalPart(tgtPart);
			}
		}
	}

	public void OnPartResearched(AvailablePart ap)
	{
		if (tgtPart != null && !(ap.title != PartName) && PartIsExperimental)
		{
			Debug.Log("[PartTest]: Contract for " + ap.title + " is no longer an experimental test. Part was researched.");
			ResearchAndDevelopment.RemoveExperimentalPart(tgtPart);
			PartIsExperimental = false;
		}
	}

	public override void OnFinished()
	{
		if (tgtPart != null && PartIsExperimental)
		{
			ResearchAndDevelopment.RemoveExperimentalPart(tgtPart);
		}
	}

	public void RemoveSituations(List<Vessel.Situations> list, params Vessel.Situations[] remove)
	{
		int num = remove.Length;
		while (num-- > 0)
		{
			list.Remove(remove[num]);
		}
	}

	public void CombineSituations(List<Vessel.Situations> list, List<Vessel.Situations> check)
	{
		int count = list.Count;
		while (count-- > 0)
		{
			if (!check.Contains(list[count]))
			{
				list.RemoveAt(count);
			}
		}
	}
}
