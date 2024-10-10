using ns26;
using UnityEngine;

namespace ns31;

public class Difficulty : MonoBehaviour
{
	public DebugScreenToggle allowStockVesselsInCareerMode;

	public DebugScreenToggle lostCrewsRespawn;

	public DebugScreenToggle allowRevertToLaunch;

	public DebugScreenToggle allowRevertToEditor;

	public DebugScreenToggle allowQuickSaving;

	public DebugScreenToggle allowQuickLoading;

	public DebugScreenToggle ignoreAgencyMindset;

	public void Start()
	{
		allowStockVesselsInCareerMode.toggle.onValueChanged.AddListener(delegate(bool b)
		{
			HighLogic.CurrentGame.Parameters.Difficulty.AllowStockVessels = b;
		});
		lostCrewsRespawn.toggle.onValueChanged.AddListener(delegate(bool b)
		{
			HighLogic.CurrentGame.Parameters.Difficulty.MissingCrewsRespawn = b;
		});
		allowRevertToLaunch.toggle.onValueChanged.AddListener(delegate(bool b)
		{
			HighLogic.CurrentGame.Parameters.Flight.CanRestart = b;
		});
		allowRevertToEditor.toggle.onValueChanged.AddListener(delegate(bool b)
		{
			HighLogic.CurrentGame.Parameters.Flight.CanLeaveToEditor = b;
		});
		allowQuickSaving.toggle.onValueChanged.AddListener(delegate(bool b)
		{
			HighLogic.CurrentGame.Parameters.Flight.CanQuickSave = b;
		});
		allowQuickLoading.toggle.onValueChanged.AddListener(delegate(bool b)
		{
			HighLogic.CurrentGame.Parameters.Flight.CanQuickLoad = b;
		});
		ignoreAgencyMindset.toggle.onValueChanged.AddListener(delegate(bool b)
		{
			CheatOptions.IgnoreAgencyMindsetOnContracts = b;
		});
	}

	public void Update()
	{
		if (HighLogic.CurrentGame != null)
		{
			allowStockVesselsInCareerMode.Set(HighLogic.CurrentGame.Parameters.Difficulty.AllowStockVessels);
			lostCrewsRespawn.Set(HighLogic.CurrentGame.Parameters.Difficulty.MissingCrewsRespawn);
			allowRevertToLaunch.Set(HighLogic.CurrentGame.Parameters.Flight.CanRestart);
			allowRevertToEditor.Set(HighLogic.CurrentGame.Parameters.Flight.CanLeaveToEditor);
			allowQuickSaving.Set(HighLogic.CurrentGame.Parameters.Flight.CanQuickSave);
			allowQuickLoading.Set(HighLogic.CurrentGame.Parameters.Flight.CanQuickLoad);
			ignoreAgencyMindset.Set(CheatOptions.IgnoreAgencyMindsetOnContracts);
		}
	}
}
