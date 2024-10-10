using System;
using System.Collections.Generic;
using System.Reflection;
using SaveUpgradePipeline;
using UnityEngine;
using UnityEngine.UI;

public static class KSPUpgradePipeline
{
	public enum UpgradeFailOption
	{
		Cancel,
		LoadAnyway
	}

	public static bool Enabled = true;

	public static global::SaveUpgradePipeline.SaveUpgradePipeline _pipeline;

	public static int FailDialogIndex = 0;

	public static global::SaveUpgradePipeline.SaveUpgradePipeline Pipeline
	{
		get
		{
			if (_pipeline == null)
			{
				_pipeline = new global::SaveUpgradePipeline.SaveUpgradePipeline();
				List<Assembly> list = new List<Assembly>();
				int count = AssemblyLoader.loadedAssemblies.Count;
				for (int i = 0; i < count; i++)
				{
					list.Add(AssemblyLoader.loadedAssemblies[i].assembly);
				}
				_pipeline.Init(list);
			}
			return _pipeline;
		}
	}

	public static bool Init()
	{
		return Pipeline.initialized;
	}

	public static void Process(ConfigNode n, string saveName, LoadContext loadContext, Callback<ConfigNode> onSucceed, Callback<UpgradeFailOption, ConfigNode> onFail)
	{
		if (!Enabled)
		{
			onSucceed(n);
			return;
		}
		if (n == null)
		{
			onFail(UpgradeFailOption.Cancel, n);
			return;
		}
		Version cfgVersion = NodeUtil.GetCfgVersion(n, loadContext);
		Version version = new Version(Versioning.version_major, Versioning.version_minor, Versioning.Revision);
		bool runSuccess;
		string runInfo;
		ConfigNode arg = Pipeline.Run(n, loadContext, version, out runSuccess, out runInfo);
		if (runSuccess)
		{
			if (cfgVersion != version)
			{
				Debug.Log(string.Concat("[KSPUpgradePipeline]: ", saveName, " updated from ", cfgVersion, " to ", version, "."));
			}
			else
			{
				Debug.Log(string.Concat("[KSPUpgradePipeline]: ", saveName, " (", cfgVersion, ") is up to date."));
			}
			onSucceed(arg);
		}
		else
		{
			InputLockManager.SetControlLock("SaveUpgradeFailDialog");
			DialogGUILabel dialogGUILabel = new DialogGUILabel(runInfo);
			dialogGUILabel.bypassTextStyleColor = true;
			DialogGUIVerticalLayout dialogGUIVerticalLayout = new DialogGUIVerticalLayout(true, false, 0f, new RectOffset(4, 4, 4, 4), TextAnchor.UpperLeft, dialogGUILabel);
			dialogGUIVerticalLayout.AddChild(new DialogGUIContentSizer(ContentSizeFitter.FitMode.Unconstrained, ContentSizeFitter.FitMode.PreferredSize));
			PopupDialog.SpawnPopupDialog(new MultiOptionDialog("SaveUpgradeFail", string.Concat("\nThe game was unable to upgrade the file to version ", version, " format. Check the game log (Alt+F2) for more information.\n\nUpgrade Log:"), "Could not upgrade " + saveName, UISkinManager.GetSkin("KSP window 7"), 450f, new DialogGUIScrollList(new Vector2(-1f, 64f), hScroll: false, vScroll: true, dialogGUIVerticalLayout), new DialogGUILabel("\nWhat would you like to do?"), new DialogGUIButton("Cancel", delegate
			{
				OnFinish(UpgradeFailOption.Cancel, null, onFail);
			}), new DialogGUIButton("Attempt to Load Anyway", delegate
			{
				OnFinish(UpgradeFailOption.LoadAnyway, n, onFail);
			})), persistAcrossScenes: true, HighLogic.UISkin, isModal: true, FailDialogIndex.ToString());
		}
	}

	public static void OnFinish(UpgradeFailOption opt, ConfigNode n, Callback<UpgradeFailOption, ConfigNode> onFail)
	{
		InputLockManager.RemoveControlLock("SaveUpgradeFailDialog");
		onFail(opt, n);
	}
}
