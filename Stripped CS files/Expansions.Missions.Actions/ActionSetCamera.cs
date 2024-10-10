using System.Collections;
using Expansions.Missions.Editor;
using ns9;

namespace Expansions.Missions.Actions;

public class ActionSetCamera : ActionModule
{
	[MEGUI_Dropdown(resetValue = "NoChange", guiName = "#autoLOC_8004182")]
	public MissionCameraModeOptions cameraLockMode;

	[MEGUI_Dropdown(resetValue = "Unlock", guiName = "#autoLOC_8004186")]
	public MissionCameraLockOptions cameraLockOptions;

	public override void Awake()
	{
		base.Awake();
		title = Localizer.Format("#autoLOC_8004179");
	}

	public override IEnumerator Fire()
	{
		node.mission.SetLockedCamera(cameraLockMode, cameraLockOptions);
		yield break;
	}

	public override string GetInfo()
	{
		return Localizer.Format("#autoLOC_8004180");
	}

	public override void Save(ConfigNode node)
	{
		base.Save(node);
		node.AddValue("cameraTypeOptions", cameraLockMode);
		node.AddValue("cameraLockOptions", cameraLockOptions);
	}

	public override void Load(ConfigNode node)
	{
		base.Load(node);
		node.TryGetEnum("cameraTypeOptions", ref cameraLockMode, MissionCameraModeOptions.NoChange);
		node.TryGetEnum("cameraLockOptions", ref cameraLockOptions, MissionCameraLockOptions.Unlock);
	}
}
