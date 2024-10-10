using System;
using System.IO;
using ns9;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Expansions.Missions.Editor;

public class CheckpointEntry : MonoBehaviour
{
	public string fullFilePath;

	public Button btnRemove;

	public GameObject warningMarker;

	public Callback<CheckpointEntry> onSelected;

	public Callback<CheckpointEntry> onRemove;

	public bool isValid;

	public bool isCheckpointDirty;

	[SerializeField]
	public Toggle tgtCtrl;

	[SerializeField]
	public TextMeshProUGUI title1;

	[SerializeField]
	public TextMeshProUGUI title2;

	[SerializeField]
	public TextMeshProUGUI fieldMsg;

	public Toggle Toggle => tgtCtrl;

	public Game CheckpointGame { get; set; }

	public Mission CheckpointMission { get; set; }

	public LoadGameDialog.PlayerProfileInfo CheckpointMeta { get; set; }

	public CheckpointEntry Create(string checkpointFile, Callback<CheckpointEntry> onSelected, Callback<CheckpointEntry> onRemove)
	{
		CheckpointEntry component = UnityEngine.Object.Instantiate(this).GetComponent<CheckpointEntry>();
		component.onSelected = onSelected;
		component.onRemove = onRemove;
		component.Init(checkpointFile);
		return component;
	}

	public void Init(string checkpointFile)
	{
		fullFilePath = checkpointFile;
		CheckpointMeta = GetSaveGameMeta(checkpointFile);
		fieldMsg.text = $"UT: {KSPUtil.PrintTimeLong(CheckpointMeta.double_0)}";
		Guid key = new Guid(Path.GetFileNameWithoutExtension(checkpointFile).Split('_')[1]);
		if (MissionEditorLogic.Instance.EditorMission.nodes.ContainsKey(key))
		{
			MENode mENode = MissionEditorLogic.Instance.EditorMission.nodes[key];
			title1.text = Localizer.Format("#autoLOC_8004190", Localizer.Format("#autoLOC_8005052"), mENode.Title);
			if (CheckpointMeta.missionHistoryId != Guid.Empty && CheckpointMeta.missionHistoryId == MissionEditorLogic.Instance.EditorMission.historyId)
			{
				title2.text = Localizer.Format("#autoLOC_8004190", Localizer.Format("#autoLOC_8005053"), mENode.ObjectiveString);
				isCheckpointDirty = false;
				isValid = true;
				tgtCtrl.onValueChanged.AddListener(OnValueChanged);
				btnRemove.onClick.AddListener(OnButtonRemove);
			}
			else
			{
				title2.text = "#autoLOC_8005054";
				isCheckpointDirty = true;
				isValid = false;
			}
		}
		else
		{
			title1.text = Localizer.Format("#autoLOC_8004190", Localizer.Format("#autoLOC_8005052"), Localizer.Format("#autoLOC_8100159"));
			title2.text = "#autoLOC_8005054";
			isCheckpointDirty = true;
			isValid = false;
		}
		warningMarker.SetActive(isCheckpointDirty);
	}

	public void Validate()
	{
		if (!isCheckpointDirty)
		{
			return;
		}
		CheckpointGame = GetSaveGame(fullFilePath);
		ConfigNode missionNode = GetMissionNode(CheckpointGame);
		if (missionNode != null)
		{
			CheckpointMission = Mission.SpawnAndLoad(MissionEditorLogic.Instance.EditorMission.MissionInfo, missionNode);
			title1.text = Localizer.Format("#autoLOC_8004190", Localizer.Format("#autoLOC_8005052"), CheckpointMission.activeNode.Title);
			title2.text = Localizer.Format("#autoLOC_8004190", Localizer.Format("#autoLOC_8005052"), CheckpointMission.activeNode.ObjectiveString);
			isCheckpointDirty = MissionCheckpointValidator.ValidateCheckpoint(CheckpointMission, MissionEditorLogic.Instance.EditorMission);
			if (!isCheckpointDirty)
			{
				CheckpointMeta.missionHistoryId = MissionEditorLogic.Instance.EditorMission.historyId;
				CheckpointMeta.SaveToMetaFile(Path.GetFileNameWithoutExtension(fullFilePath), MissionsUtils.SavesPath + MissionEditorLogic.Instance.EditorMission.MissionInfo.folderName);
				isValid = true;
				tgtCtrl.onValueChanged.AddListener(OnValueChanged);
				btnRemove.onClick.AddListener(OnButtonRemove);
			}
			warningMarker.SetActive(isCheckpointDirty);
		}
		else
		{
			title1.text = fullFilePath;
			title2.text = Localizer.Format("#autoLOC_8005055");
			fieldMsg.text = "";
			isValid = false;
			isCheckpointDirty = true;
		}
	}

	public Game GetSaveGame(string checkpointFile)
	{
		ConfigNode configNode = ConfigNode.Load(checkpointFile);
		Game result = null;
		if (configNode != null)
		{
			result = new Game(configNode);
		}
		FlightGlobals.ClearpersistentIdDictionaries();
		return result;
	}

	public LoadGameDialog.PlayerProfileInfo GetSaveGameMeta(string checkpointFile)
	{
		LoadGameDialog.PlayerProfileInfo playerProfileInfo = new LoadGameDialog.PlayerProfileInfo();
		string text = MissionsUtils.SavesPath + MissionEditorLogic.Instance.EditorMission.MissionInfo.folderName;
		string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(checkpointFile);
		long num = -1L;
		try
		{
			num = LoadGameDialog.PlayerProfileInfo.GetLastWriteTime(fileNameWithoutExtension, text);
		}
		catch (Exception ex)
		{
			Debug.LogFormat("[LoadGameDialog]: Unable to get last write time for SFS file-{0}-{1}\n{2}", text, fileNameWithoutExtension, ex.Message);
		}
		if (num != -1L)
		{
			try
			{
				playerProfileInfo.LoadFromMetaFile(fileNameWithoutExtension, text);
				if (playerProfileInfo.lastWriteTime == num)
				{
					return playerProfileInfo;
				}
			}
			catch (Exception ex2)
			{
				Debug.LogWarningFormat("[LoadGameDialog]: Errored when loading .loadmeta file, will load full save-{0}-{1}\n{2}", text, fileNameWithoutExtension, ex2.Message);
			}
		}
		return playerProfileInfo;
	}

	public void LoadMission()
	{
		if (CheckpointGame != null)
		{
			return;
		}
		CheckpointGame = GetSaveGame(fullFilePath);
		if (CheckpointMission == null)
		{
			ConfigNode missionNode = GetMissionNode(CheckpointGame);
			if (missionNode != null)
			{
				CheckpointMission = Mission.SpawnAndLoad(MissionEditorLogic.Instance.EditorMission.MissionInfo, missionNode);
			}
		}
	}

	public ConfigNode GetMissionNode(Game checkpointGame)
	{
		if (checkpointGame.Mode == Game.Modes.MISSION)
		{
			ConfigNode configNode = null;
			int i = 0;
			for (int count = checkpointGame.scenarios.Count; i < count; i++)
			{
				if (checkpointGame.scenarios[i].moduleName == "MissionSystem")
				{
					configNode = checkpointGame.scenarios[i].GetData();
					break;
				}
			}
			if (configNode != null)
			{
				ConfigNode node = null;
				ConfigNode node2 = null;
				if (configNode.TryGetNode("MISSIONS", ref node) && node.TryGetNode("MISSION", ref node2))
				{
					return node2;
				}
			}
		}
		return null;
	}

	public void OnValueChanged(bool st)
	{
		if (st)
		{
			onSelected(this);
		}
	}

	public void OnButtonRemove()
	{
		onRemove(this);
	}

	public void Terminate()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}
}
