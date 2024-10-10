using System;
using System.Collections.Generic;
using System.Text;
using FinePrint.Utilities;
using ns17;
using ns9;
using UnityEngine;

public class ModuleKerbNetAccess : PartModule, IAccessKerbNet, IModuleInfo
{
	[Serializable]
	public class Modes
	{
		public string Mode;

		public string displayMode;

		public Modes(string mode, string displaymode)
		{
			Mode = mode;
			displayMode = displaymode;
		}
	}

	[KSPField]
	public float MinimumFoV = 5f;

	[KSPField]
	public float MaximumFoV = 90f;

	[KSPField]
	public uint EnhancedSituationMask;

	[KSPField]
	public float EnhancedMinimumFoV = 5f;

	[KSPField]
	public float EnhancedMaximumFoV = 90f;

	[KSPField]
	public float AnomalyDetection = 0.25f;

	[KSPField]
	public bool RequiresAnimation;

	public const string eventName = "OpenKerbNet";

	public List<Modes> modes;

	public List<string> effects;

	public bool isCheckingEffects;

	public bool partHasEffects;

	public bool partIsAnimating;

	public bool isEnhanced;

	public static string cacheAutoLOC_230499;

	public static string cacheAutoLOC_230500;

	public static string cacheAutoLOC_230501;

	public static string cacheAutoLOC_230505;

	public static string cacheAutoLOC_230517;

	public static string cacheAutoLOC_230521;

	public static string cacheAutoLOC_230533;

	public static string cacheAutoLOC_230585;

	public static string cacheAutoLOC_230588;

	public override void OnAwake()
	{
		if (modes == null)
		{
			modes = new List<Modes>();
		}
		if (effects == null)
		{
			effects = new List<string>();
		}
	}

	public override void OnLoad(ConfigNode node)
	{
		LoadDisplayModes(node);
		LoadRequiredEffects(node);
	}

	public void LoadDisplayModes(ConfigNode node)
	{
		ConfigNode node2 = node.GetNode("DISPLAY_MODES");
		if (node2 == null)
		{
			return;
		}
		modes.Clear();
		string[] values = node2.GetValues("Mode");
		int i = 0;
		for (int num = values.Length; i < num; i++)
		{
			if (!string.IsNullOrEmpty(values[i]))
			{
				string[] array = values[i].Split(',');
				string mode = array[0].Trim();
				string displaymode = mode;
				if (array.Length == 2)
				{
					displaymode = Localizer.Format(array[1]);
				}
				if (modes.Find((Modes p) => p.Mode == mode) == null)
				{
					Modes item = new Modes(mode, displaymode);
					modes.Add(item);
				}
			}
		}
	}

	public void LoadRequiredEffects(ConfigNode node)
	{
		ConfigNode node2 = node.GetNode("REQUIRED_EFFECTS");
		if (node2 != null)
		{
			effects.Clear();
			string[] values = node2.GetValues("Effect");
			int i = 0;
			for (int num = values.Length; i < num; i++)
			{
				string text = values[i].Trim();
				if (!string.IsNullOrEmpty(text) && !effects.Contains(text))
				{
					effects.Add(text);
				}
			}
		}
		isCheckingEffects = effects.Count > 0;
	}

	public override void OnStart(StartState state)
	{
		base.Events["OpenKerbNet"].guiActive = HighLogic.LoadedSceneIsFlight;
		GameEvents.onVesselSituationChange.Add(OnVesselSituationChanged);
		Vessel.Situations situation = ((base.vessel != null) ? base.vessel.situation : Vessel.Situations.PRELAUNCH);
		CheckVesselSituationFoV(situation, firstRun: true);
		if (isCheckingEffects)
		{
			GameEvents.onVesselCrewWasModified.Add(InvokeCrewEffectCheck);
			CheckRequiredEffects();
		}
		if (RequiresAnimation)
		{
			GameEvents.OnAnimationGroupStateChanged.Add(OnAnimationGroupStateChanged);
		}
	}

	public void OnDestroy()
	{
		GameEvents.onVesselSituationChange.Remove(OnVesselSituationChanged);
		if (isCheckingEffects)
		{
			GameEvents.onVesselCrewWasModified.Remove(InvokeCrewEffectCheck);
			if (IsInvoking())
			{
				CancelInvoke();
			}
		}
		if (RequiresAnimation)
		{
			GameEvents.OnAnimationGroupStateChanged.Remove(OnAnimationGroupStateChanged);
		}
	}

	[KSPAction("#autoLOC_230490")]
	public void OpenKerbNetAction(KSPActionParam param)
	{
		if (base.Events["OpenKerbNet"].guiActive)
		{
			OpenKerbNet();
		}
	}

	[KSPEvent(unfocusedRange = 3f, active = true, guiActiveUnfocused = false, externalToEVAOnly = false, guiActive = true, guiActiveEditor = false, guiName = "#autoLOC_230490")]
	public void OpenKerbNet()
	{
		KerbNetDialog.Display(this);
		KerbNetDialog.ResetMapPosition();
	}

	public string GetModuleTitle()
	{
		return "KerbNet Access";
	}

	public override string GetModuleDisplayName()
	{
		return Localizer.Format("#autoLOC_230490");
	}

	public override string GetInfo()
	{
		List<Vessel.Situations> enhancedSituations = GetEnhancedSituations();
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(cacheAutoLOC_230499 + " ").Append(Math.Round(MinimumFoV)).Append("°\n");
		stringBuilder.Append(cacheAutoLOC_230500 + " ").Append(Math.Round(MaximumFoV)).Append("°\n");
		stringBuilder.Append(cacheAutoLOC_230501 + " ").Append(Math.Round(AnomalyDetection * 100f)).Append("%\n");
		if (modes != null && modes.Count > 0)
		{
			stringBuilder.Append(cacheAutoLOC_230505);
			int i = 0;
			for (int count = modes.Count; i < count; i++)
			{
				stringBuilder.Append("\n - ").Append(modes[i].displayMode);
			}
			stringBuilder.Append("\n");
		}
		if (effects != null && effects.Count > 0 && GameDatabase.Instance != null && GameDatabase.Instance.ExperienceConfigs != null)
		{
			stringBuilder.Append(cacheAutoLOC_230517);
			int j = 0;
			for (int count2 = effects.Count; j < count2; j++)
			{
				string value = StringUtilities.ThisThisAndThat(GameDatabase.Instance.ExperienceConfigs.GetTraitsWithEffect(effects[j]), cacheAutoLOC_230521);
				if (!string.IsNullOrEmpty(value))
				{
					stringBuilder.Append("\n - ").Append(value);
				}
			}
			if (enhancedSituations != null && enhancedSituations.Count > 0)
			{
				stringBuilder.Append("\n");
			}
		}
		if (enhancedSituations != null && enhancedSituations.Count > 0)
		{
			stringBuilder.Append(cacheAutoLOC_230533);
			int k = 0;
			for (int count3 = enhancedSituations.Count; k < count3; k++)
			{
				stringBuilder.Append("\n - ").Append(Vessel.GetSituationString(enhancedSituations[k]));
			}
		}
		return stringBuilder.ToString();
	}

	public Callback<Rect> GetDrawModulePanelCallback()
	{
		return null;
	}

	public string GetPrimaryField()
	{
		return null;
	}

	public Vessel GetKerbNetVessel()
	{
		return base.vessel;
	}

	public Part GetKerbNetPart()
	{
		return base.part;
	}

	public List<string> GetKerbNetDisplayModes()
	{
		List<string> list = new List<string>();
		for (int i = 0; i < modes.Count; i++)
		{
			list.Add(modes[i].Mode);
		}
		return list;
	}

	public float GetKerbNetMinimumFoV()
	{
		if (!isEnhanced)
		{
			return MinimumFoV;
		}
		return EnhancedMinimumFoV;
	}

	public float GetKerbNetMaximumFoV()
	{
		if (!isEnhanced)
		{
			return MaximumFoV;
		}
		return EnhancedMaximumFoV;
	}

	public float GetKerbNetAnomalyChance()
	{
		return AnomalyDetection;
	}

	public string GetKerbNetErrorState()
	{
		if (isCheckingEffects && !partHasEffects)
		{
			return cacheAutoLOC_230585;
		}
		if (RequiresAnimation && !partIsAnimating)
		{
			return cacheAutoLOC_230588;
		}
		return null;
	}

	public bool SituationIsEnhanced(Vessel.Situations situation)
	{
		return ((uint)situation & EnhancedSituationMask) != 0;
	}

	public List<Vessel.Situations> GetEnhancedSituations()
	{
		List<Vessel.Situations> list = new List<Vessel.Situations>();
		Vessel.Situations[] array = (Vessel.Situations[])Enum.GetValues(typeof(Vessel.Situations));
		int i = 0;
		for (int num = array.Length; i < num; i++)
		{
			Vessel.Situations situations = array[i];
			if (SituationIsEnhanced(situations))
			{
				list.Add(situations);
			}
		}
		return list;
	}

	public void OnVesselSituationChanged(GameEvents.HostedFromToAction<Vessel, Vessel.Situations> vs)
	{
		if (vs.host == base.vessel)
		{
			CheckVesselSituationFoV(base.vessel.situation, firstRun: false);
		}
	}

	public bool HasThisDialogOpen()
	{
		if (KerbNetDialog.isDisplaying)
		{
			return KerbNetDialog.Instance.KerbNetAccessor == this;
		}
		return false;
	}

	public void CheckVesselSituationFoV(Vessel.Situations situation, bool firstRun)
	{
		bool flag = HasThisDialogOpen();
		bool flag2 = SituationIsEnhanced(situation);
		bool flag3 = false;
		float min = 0f;
		float max = 0f;
		if ((!isEnhanced || firstRun) && flag2)
		{
			isEnhanced = true;
			flag3 = true;
			min = EnhancedMinimumFoV;
			max = EnhancedMaximumFoV;
			if (flag)
			{
				ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_230641", base.part.partInfo.title), 2.5f, ScreenMessageStyle.UPPER_CENTER);
			}
		}
		else if ((isEnhanced || firstRun) && !flag2)
		{
			isEnhanced = false;
			flag3 = true;
			min = MinimumFoV;
			max = MaximumFoV;
			if (flag)
			{
				ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_230651", base.part.partInfo.title), 2.5f, ScreenMessageStyle.UPPER_CENTER);
			}
		}
		if (flag3 && flag)
		{
			KerbNetDialog.Instance.SetFoVBounds(min, max);
		}
	}

	public void InvokeCrewEffectCheck(Vessel v)
	{
		if (!(v != base.vessel))
		{
			if (IsInvoking())
			{
				CancelInvoke();
			}
			Invoke("CheckRequiredEffects", 0.2f);
		}
	}

	public void CheckRequiredEffects()
	{
		partHasEffects = PartHasRequiredEffects();
		if (KerbNetDialog.Instance != null)
		{
			KerbNetDialog.Instance.RefreshErrorState(this);
		}
	}

	public bool PartHasRequiredEffects()
	{
		if (effects != null && effects.Count > 0)
		{
			int num = effects.Count - 1;
			while (true)
			{
				if (num >= 0)
				{
					string effect = effects[num];
					bool flag = false;
					int count = base.part.protoModuleCrew.Count;
					while (count-- > 0)
					{
						if (base.part.protoModuleCrew[count].HasEffect(effect))
						{
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						break;
					}
					num--;
					continue;
				}
				return true;
			}
			return false;
		}
		return true;
	}

	public void OnAnimationGroupStateChanged(ModuleAnimationGroup group, bool state)
	{
		if (RequiresAnimation && !(group.part != base.part))
		{
			partIsAnimating = state;
			if (KerbNetDialog.Instance != null)
			{
				KerbNetDialog.Instance.RefreshErrorState(this);
			}
		}
	}

	public static void CacheLocalStrings()
	{
		cacheAutoLOC_230499 = Localizer.Format("#autoLOC_230499");
		cacheAutoLOC_230500 = Localizer.Format("#autoLOC_230500");
		cacheAutoLOC_230501 = Localizer.Format("#autoLOC_230501");
		cacheAutoLOC_230505 = Localizer.Format("#autoLOC_230505");
		cacheAutoLOC_230517 = Localizer.Format("#autoLOC_230517");
		cacheAutoLOC_230521 = Localizer.Format("#autoLOC_230521");
		cacheAutoLOC_230533 = Localizer.Format("#autoLOC_230533");
		cacheAutoLOC_230585 = Localizer.Format("#autoLOC_230585");
		cacheAutoLOC_230588 = Localizer.Format("#autoLOC_230588");
	}
}
