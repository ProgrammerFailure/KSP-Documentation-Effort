using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Expansions.Missions;
using ns2;
using ns9;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadGameDialog : MonoBehaviour
{
	public delegate void FinishedCallback(string path);

	public class PlayerProfileInfo : IConfigNode
	{
		public string name = "";

		public int vesselCount;

		public double double_0 = -1.0;

		public Game.Modes gameMode;

		public bool gameNull;

		public bool gameCompatible;

		public double funds;

		public int science;

		public int reputationPercent;

		public int ongoingContracts;

		public string kspVersion = string.Empty;

		public float missionCurrentScore;

		public bool missionIsStarted;

		public bool missionIsEnded;

		public Guid missionHistoryId;

		public string missionExpansionVersion = string.Empty;

		public List<string> testModules = new List<string>();

		public List<string> actionModules = new List<string>();

		public bool errorAccess;

		public string errorDetails = "";

		public string saveMD5 = "";

		public long lastWriteTime;

		public PlayerProfileInfo()
		{
		}

		public PlayerProfileInfo(string filename, string saveFolder, Game game)
		{
			LoadDetailsFromGame(game);
			lastWriteTime = GetLastWriteTime(filename, saveFolder);
		}

		public PlayerProfileInfo LoadDetailsFromGame(Game game)
		{
			gameNull = game == null;
			if (!gameNull)
			{
				kspVersion = game.file_version_major + "." + game.file_version_minor + "." + game.file_version_revision;
				gameCompatible = game.compatible;
				gameMode = game.Mode;
			}
			if (game.flightState != null)
			{
				int count = game.flightState.protoVessels.Count;
				while (count-- > 0)
				{
					ProtoVessel protoVessel = game.flightState.protoVessels[count];
					if (protoVessel.vesselType > VesselType.Unknown && protoVessel.vesselType != VesselType.Flag && protoVessel.vesselType != VesselType.DeployedSciencePart)
					{
						vesselCount++;
					}
				}
				double_0 = game.flightState.universalTime;
				int count2 = game.scenarios.Count;
				while (count2-- > 0)
				{
					ProtoScenarioModule protoScenarioModule = game.scenarios[count2];
					switch (protoScenarioModule.moduleName)
					{
					case "ResearchAndDevelopment":
						if (protoScenarioModule.GetData().HasValue("sci"))
						{
							science = (int)float.Parse(protoScenarioModule.GetData().GetValue("sci"));
						}
						break;
					case "Reputation":
						if (protoScenarioModule.GetData().HasValue("rep"))
						{
							reputationPercent = (int)(float.Parse(protoScenarioModule.GetData().GetValue("rep")) / 10f);
						}
						break;
					case "Funding":
						if (protoScenarioModule.GetData().HasValue("funds"))
						{
							funds = double.Parse(protoScenarioModule.GetData().GetValue("funds"));
						}
						break;
					case "ContractSystem":
						if (protoScenarioModule.GetData().HasNode("CONTRACTS"))
						{
							int num = 0;
							ConfigNode node = protoScenarioModule.GetData().GetNode("CONTRACTS");
							int count3 = node.nodes.Count;
							while (count3-- > 0)
							{
								ConfigNode configNode = node.nodes[count3];
								if (!(configNode.name != "CONTRACT") && configNode.GetValue("state").ToLower() == "active" && (!configNode.HasValue("autoAccept") || !bool.Parse(configNode.GetValue("autoAccept"))))
								{
									num++;
								}
							}
							ongoingContracts = num;
						}
						else
						{
							ongoingContracts = 0;
						}
						break;
					}
				}
			}
			if (game.scenarios != null)
			{
				int count4 = game.scenarios.Count;
				while (count4-- > 0)
				{
					ProtoScenarioModule protoScenarioModule = game.scenarios[count4];
					string moduleName = protoScenarioModule.moduleName;
					if (!(moduleName == "MissionSystem") || !protoScenarioModule.GetData().HasNode("MISSIONS"))
					{
						continue;
					}
					ConfigNode node2 = protoScenarioModule.GetData().GetNode("MISSIONS");
					if (!node2.HasNode("MISSION"))
					{
						continue;
					}
					ConfigNode node3 = node2.GetNode("MISSION");
					node3.TryGetValue("currentScore", ref missionCurrentScore);
					node3.TryGetValue("isStarted", ref missionIsStarted);
					node3.TryGetValue("isEnded", ref missionIsEnded);
					node3.TryGetValue("historyId", ref missionHistoryId);
					node3.TryGetValue("expansionVersion", ref missionExpansionVersion);
					if (string.IsNullOrEmpty(missionExpansionVersion))
					{
						missionExpansionVersion = "1.2.0";
					}
					if (!node3.HasNode("NODES"))
					{
						continue;
					}
					ConfigNode[] nodes = node3.GetNode("NODES").GetNodes("NODE");
					ConfigNode configNode2 = null;
					ConfigNode[] array = null;
					ConfigNode[] array2 = null;
					ConfigNode[] array3 = null;
					for (int i = 0; i < nodes.Length; i++)
					{
						configNode2 = nodes[i];
						if (configNode2.HasNode("ACTIONS"))
						{
							array = configNode2.GetNode("ACTIONS").GetNodes("ACTIONMODULE");
							for (int j = 0; j < array.Length; j++)
							{
								actionModules.AddUnique(array[j].GetValue("name"));
							}
						}
						if (!configNode2.HasNode("TESTGROUPS"))
						{
							continue;
						}
						array2 = configNode2.GetNode("TESTGROUPS").GetNodes("TESTGROUP");
						for (int k = 0; k < array.Length; k++)
						{
							array3 = array2[k].GetNodes("TESTMODULE");
							for (int l = 0; l < array3.Length; l++)
							{
								testModules.AddUnique(array3[l].GetValue("name"));
							}
						}
					}
				}
			}
			return this;
		}

		public void Load(ConfigNode node)
		{
			if (node.HasValue("vesselCount"))
			{
				vesselCount = Convert.ToInt32(node.GetValue("vesselCount"));
			}
			if (node.HasValue("UT"))
			{
				double_0 = Convert.ToDouble(node.GetValue("UT"));
			}
			if (node.HasValue("gameMode"))
			{
				gameMode = (Game.Modes)Enum.Parse(typeof(Game.Modes), node.GetValue("gameMode"));
			}
			if (node.HasValue("gameNull"))
			{
				gameNull = Convert.ToBoolean(node.GetValue("gameNull"));
			}
			if (node.HasValue("kspVersion"))
			{
				kspVersion = node.GetValue("kspVersion");
			}
			gameCompatible = IsCompatible();
			if (node.HasValue("funds"))
			{
				funds = Convert.ToDouble(node.GetValue("funds"));
			}
			if (node.HasValue("science"))
			{
				science = Convert.ToInt32(node.GetValue("science"));
			}
			if (node.HasValue("reputationPercent"))
			{
				reputationPercent = Convert.ToInt32(node.GetValue("reputationPercent"));
			}
			if (node.HasValue("ongoingContracts"))
			{
				ongoingContracts = Convert.ToInt32(node.GetValue("ongoingContracts"));
			}
			node.TryGetValue("missionCurrentScore", ref missionCurrentScore);
			node.TryGetValue("missionIsStarted", ref missionIsStarted);
			node.TryGetValue("missionIsEnded", ref missionIsEnded);
			node.TryGetValue("missionHistoryId", ref missionHistoryId);
			node.TryGetValue("missionExpansionVersion", ref missionExpansionVersion);
			if (string.IsNullOrEmpty(missionExpansionVersion))
			{
				missionExpansionVersion = "1.2.0";
			}
			if (node.HasValue("testModules"))
			{
				testModules = node.GetValuesList("testModules");
			}
			if (node.HasValue("actionModules"))
			{
				actionModules = node.GetValuesList("actionModules");
			}
			if (node.HasValue("saveMD5"))
			{
				saveMD5 = node.GetValue("saveMD5");
			}
			node.TryGetValue("lastWriteTime", ref lastWriteTime);
		}

		public void Save(ConfigNode node)
		{
			node.AddValue("vesselCount", vesselCount);
			node.AddValue("UT", double_0);
			node.AddValue("gameMode", gameMode);
			node.AddValue("gameNull", gameNull);
			node.AddValue("kspVersion", kspVersion);
			node.AddValue("funds", funds);
			node.AddValue("science", science);
			node.AddValue("reputationPercent", reputationPercent);
			node.AddValue("ongoingContracts", ongoingContracts);
			node.AddValue("missionCurrentScore", missionCurrentScore);
			node.AddValue("missionIsStarted", missionIsStarted);
			node.AddValue("missionIsEnded", missionIsEnded);
			node.AddValue("missionHistoryId", missionHistoryId);
			node.AddValue("missionExpansionVersion", missionExpansionVersion);
			for (int i = 0; i < testModules.Count; i++)
			{
				node.AddValue("testModules", testModules[i]);
			}
			for (int j = 0; j < actionModules.Count; j++)
			{
				node.AddValue("actionModules", actionModules[j]);
			}
			node.AddValue("saveMD5", saveMD5);
			node.AddValue("lastWriteTime", lastWriteTime);
		}

		public void LoadFromMetaFile(string filename, string saveFolder)
		{
			string text = KSPUtil.ApplicationRootPath + "saves/" + saveFolder + "/" + filename + ".loadmeta";
			if (!File.Exists(text))
			{
				Debug.Log("No meta file found for path: " + text);
				return;
			}
			ConfigNode configNode = ConfigNode.Load(text, bypassLocalization: true);
			if (!configNode.HasValue("kspVersion"))
			{
				Debug.Log("Meta file has no version information, forcing rebuild: " + text);
			}
			else if (!configNode.HasValue("missionExpansionVersion"))
			{
				Debug.Log("Meta file has no mission version information, forcing rebuild: " + text);
			}
			else if (!configNode.HasValue("lastWriteTime"))
			{
				Debug.Log("Meta file has no last write time information, forcing rebuild: " + text);
			}
			else
			{
				Load(ConfigNode.Load(text));
			}
		}

		public void SaveToMetaFile(string filename, string saveFolder)
		{
			ConfigNode configNode = new ConfigNode();
			Save(configNode);
			configNode.Save(KSPUtil.ApplicationRootPath + "saves/" + saveFolder + "/" + filename + ".loadmeta");
		}

		public static string GetSFSMD5(string filename, string saveFolder)
		{
			if (File.Exists(KSPUtil.ApplicationRootPath + "saves/" + saveFolder + "/" + filename + ".sfs"))
			{
				return Versioning.FileMD5String(KSPUtil.ApplicationRootPath + "saves/" + saveFolder + "/" + filename + ".sfs");
			}
			return "";
		}

		public static long GetLastWriteTime(string filename, string saveFolder)
		{
			string path = KSPUtil.ApplicationRootPath + "saves/" + saveFolder + "/" + filename + ".sfs";
			if (File.Exists(path))
			{
				return File.GetLastWriteTimeUtc(path).Ticks;
			}
			return -1L;
		}

		public bool AreVesselsInFlightCompatible(string filename, string saveFolder, ref string errorString)
		{
			ConfigNode configNode = ConfigNode.Load(KSPUtil.ApplicationRootPath + "saves/" + saveFolder + "/" + filename + ".sfs");
			bool result = true;
			ConfigNode node = configNode.GetNode("GAME");
			if (node != null)
			{
				ConfigNode node2 = node.GetNode("FLIGHTSTATE");
				if (node2 != null)
				{
					ConfigNode[] nodes = node2.GetNodes("VESSEL");
					for (int i = 0; i < nodes.Length; i++)
					{
						ConfigNode[] nodes2 = nodes[i].GetNodes("PART");
						string empty = string.Empty;
						for (int j = 0; j < nodes2.Length; j++)
						{
							empty = nodes2[j].GetValue("name");
							if (empty != null && !PartLoader.DoesPartExist(empty) && !PartLoader.DoesPartHaveReplacement(empty) && !PartLoader.DoesPartExist(PartLoader.GetPartReplacementName(empty)))
							{
								string value = nodes[i].GetValue("name");
								errorString += Localizer.Format("#autoLOC_8004243", value, empty);
								result = false;
							}
						}
					}
				}
			}
			return result;
		}

		public bool IsCompatible()
		{
			if (string.IsNullOrEmpty(kspVersion))
			{
				return false;
			}
			return KSPUtil.CheckVersion(kspVersion, 0, 21, 0) == VersionCompareResult.COMPATIBLE;
		}
	}

	public Sprite careerIcon;

	public Sprite scienceSandboxIcon;

	public Sprite sandboxIcon;

	public Sprite scenarioIcon;

	public Sprite tutorialIcon;

	[SerializeField]
	public Button btnLoad;

	[SerializeField]
	public Button btnCancel;

	[SerializeField]
	public Button btnDelete;

	[SerializeField]
	public RectTransform scrollListContent;

	[SerializeField]
	public ToggleGroup listGroup;

	[SerializeField]
	public TextMeshProUGUI header;

	[SerializeField]
	public TMP_InputField searchInput;

	public List<DialogGUIToggleButton> items = new List<DialogGUIToggleButton>();

	public FinishedCallback OnFinishedCallback = delegate
	{
	};

	public UISkinDef skin;

	public string directory;

	public bool persistent;

	public string selectedGame = string.Empty;

	public PlayerProfileInfo selectedSave;

	public PopupDialog popupdialog;

	public MenuNavigation menuNav;

	public List<PlayerProfileInfo> saves;

	public bool confirmGameDelete;

	public void Start()
	{
		btnDelete.interactable = false;
		btnLoad.interactable = false;
		btnDelete.onClick.AddListener(OnButtonDelete);
		btnCancel.onClick.AddListener(OnBtnCancel);
		btnLoad.onClick.AddListener(OnButtonLoad);
	}

	public void Update()
	{
		if ((!Input.GetKeyUp(KeyCode.Escape) || !(searchInput != null) || string.IsNullOrEmpty(searchInput.text)) && Input.GetKeyUp(KeyCode.Escape) && popupdialog == null && !UIMasterController.Instance.CameraMode)
		{
			CancelLoadGame();
		}
	}

	public void OnButtonDelete()
	{
		confirmGameDelete = true;
		SetLocked(locked: true);
		popupdialog = PopupDialog.SpawnPopupDialog(new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new MultiOptionDialog("DeleteGame", Localizer.Format("#autoLOC_464287"), Localizer.Format("#autoLOC_464288"), skin, new DialogGUIButton(Localizer.Format("#autoLOC_464290"), ConfirmDeleteGame, dismissOnSelect: true), new DialogGUIButton(Localizer.Format("#autoLOC_464291"), DismissDeleteGame, dismissOnSelect: true)), persistAcrossScenes: false, skin);
		popupdialog.OnDismiss = DismissDeleteGame;
		MenuNavigation.SpawnMenuNavigation(popupdialog.gameObject, Navigation.Mode.Vertical, limitCheck: true);
	}

	public void OnBtnCancel()
	{
		CancelLoadGame();
	}

	public void OnButtonLoad()
	{
		if (!selectedSave.gameNull && !selectedSave.errorAccess && selectedSave.gameCompatible)
		{
			string errorString = string.Empty;
			string empty = string.Empty;
			string empty2 = string.Empty;
			if (persistent)
			{
				empty = selectedGame;
				empty2 = "persistent";
			}
			else
			{
				empty = directory;
				empty2 = selectedGame;
			}
			if (selectedSave.AreVesselsInFlightCompatible(empty2, empty, ref errorString))
			{
				ConfirmLoadGame();
				return;
			}
			SetLocked(locked: true);
			DialogGUILabel dialogGUILabel = new DialogGUILabel(errorString, skin.customStyles[0]);
			dialogGUILabel.bypassTextStyleColor = true;
			DialogGUIVerticalLayout dialogGUIVerticalLayout = new DialogGUIVerticalLayout(true, false, 0f, new RectOffset(4, 11, 4, 4), TextAnchor.UpperLeft, dialogGUILabel);
			dialogGUIVerticalLayout.AddChild(new DialogGUIContentSizer(ContentSizeFitter.FitMode.Unconstrained, ContentSizeFitter.FitMode.PreferredSize));
			popupdialog = PopupDialog.SpawnPopupDialog(new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new MultiOptionDialog("Confirmation Needed", Localizer.Format("#autoLOC_8004242"), Localizer.Format("#autoLOC_464288"), skin, 350f, new DialogGUISpace(6f), new DialogGUIScrollList(new Vector2(-1f, 80f), hScroll: false, vScroll: true, dialogGUIVerticalLayout), new DialogGUISpace(6f), new DialogGUIHorizontalLayout(TextAnchor.UpperLeft, new DialogGUIFlexibleSpace(), new DialogGUIButton(Localizer.Format("#autoLOC_417274"), ConfirmLoadGame, 100f, -1f, true), new DialogGUIButton(Localizer.Format("#autoLOC_226976"), delegate
			{
				SetLocked(locked: false);
			}, 150f, -1f, true))), persistAcrossScenes: false, skin);
			popupdialog.OnDismiss = delegate
			{
				SetLocked(locked: false);
			};
		}
		else
		{
			SetLocked(locked: true);
			popupdialog = PopupDialog.SpawnPopupDialog(new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new MultiOptionDialog("Confirmation Needed", Localizer.Format("#autoLOC_8004240"), Localizer.Format("#autoLOC_464288"), skin, 350f, new DialogGUIButton(Localizer.Format("#autoLOC_417274"), ConfirmLoadGame, dismissOnSelect: true), new DialogGUIButton(Localizer.Format("#autoLOC_226976"), delegate
			{
				SetLocked(locked: false);
			}, dismissOnSelect: true)), persistAcrossScenes: false, skin);
			popupdialog.OnDismiss = delegate
			{
				SetLocked(locked: false);
			};
		}
	}

	public void OnSelectionChanged(bool haveSelection)
	{
		btnDelete.interactable = haveSelection && !confirmGameDelete && (persistent || (!persistent && selectedGame != "persistent"));
		btnLoad.interactable = haveSelection && selectedSave != null && !selectedSave.errorAccess;
	}

	public void ConfirmLoadGame()
	{
		SetLocked(locked: false);
		OnFinishedCallback(selectedGame);
		UnityEngine.Object.Destroy(base.gameObject);
	}

	public void CancelLoadGame()
	{
		OnFinishedCallback(string.Empty);
		UnityEngine.Object.Destroy(base.gameObject);
	}

	public void ConfirmDeleteGame()
	{
		string text = ((!persistent) ? (KSPUtil.ApplicationRootPath + "saves/" + directory + "/" + selectedGame + ".sfs") : (KSPUtil.ApplicationRootPath + "saves/" + selectedGame));
		try
		{
			if (persistent)
			{
				Directory.Delete(text, recursive: true);
			}
			else
			{
				File.Delete(text);
				if (File.Exists(KSPUtil.ApplicationRootPath + "saves/" + directory + "/" + selectedGame + ".loadmeta"))
				{
					File.Delete(KSPUtil.ApplicationRootPath + "saves/" + directory + "/" + selectedGame + ".loadmeta");
				}
			}
		}
		catch (Exception ex)
		{
			Debug.LogError("Unable to delete save: " + text + "\n" + ex.Message);
			ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_485809", text), 5f, ScreenMessageStyle.UPPER_LEFT);
		}
		DismissDeleteGame();
		if (persistent)
		{
			PersistentLoadGame();
		}
		else
		{
			LoadGame();
		}
	}

	public void DismissDeleteGame()
	{
		listGroup.SetAllTogglesOff();
		confirmGameDelete = false;
		selectedGame = string.Empty;
		selectedSave = null;
		OnSelectionChanged(haveSelection: false);
		SetLocked(locked: false);
	}

	public static LoadGameDialog Create(FinishedCallback onDismiss, string directory, bool persistent, UISkinDef skin)
	{
		GameObject obj = UnityEngine.Object.Instantiate(AssetBase.GetPrefab("LoadGamePrefab"));
		obj.name = "LoadSavedGame";
		obj.transform.SetParent(DialogCanvasUtil.DialogCanvasRect, worldPositionStays: false);
		LoadGameDialog component = obj.GetComponent<LoadGameDialog>();
		component.OnFinishedCallback = onDismiss;
		component.skin = skin;
		component.directory = directory;
		component.persistent = persistent;
		if (persistent)
		{
			component.PersistentLoadGame();
		}
		else
		{
			component.LoadGame();
		}
		return component;
	}

	public void SetHidden(bool hide)
	{
		if (hide)
		{
			base.gameObject.SetActive(value: false);
		}
		else
		{
			base.gameObject.SetActive(value: true);
		}
	}

	public void SetLocked(bool locked)
	{
		int count = items.Count;
		while (count-- > 0)
		{
			items[count].toggle.interactable = !locked;
		}
		btnCancel.interactable = !locked;
		btnDelete.interactable = !locked && !string.IsNullOrEmpty(selectedGame);
		btnLoad.interactable = !locked && !string.IsNullOrEmpty(selectedGame);
	}

	public static PlayerProfileInfo GetSaveData(PlayerProfileInfo save, string fname, string folder)
	{
		bool flag = false;
		string text = save.name;
		long num = -1L;
		try
		{
			num = PlayerProfileInfo.GetLastWriteTime(fname, folder);
		}
		catch (Exception ex)
		{
			Debug.LogFormat("[LoadGameDialog]: Unable to get last write time for SFS file-{0}-{1}\n{2}", folder, fname, ex.Message);
		}
		if (num != -1L)
		{
			try
			{
				PlayerProfileInfo playerProfileInfo = new PlayerProfileInfo();
				playerProfileInfo.LoadFromMetaFile(fname, folder);
				if (playerProfileInfo.lastWriteTime == num)
				{
					save = playerProfileInfo;
					flag = true;
				}
			}
			catch (Exception ex2)
			{
				Debug.LogWarningFormat("[LoadGameDialog]: Errored when loading .loadmeta file, will load full save-{0}-{1}\n{2}", folder, fname, ex2.Message);
			}
		}
		if (!flag)
		{
			Game game = GamePersistence.LoadGame(fname, folder, nullIfIncompatible: false, suppressIncompatibleMessage: false);
			try
			{
				save = save.LoadDetailsFromGame(game);
				save.lastWriteTime = num;
				save.SaveToMetaFile(fname, folder);
			}
			catch (Exception ex3)
			{
				Debug.LogFormat("[LoadGameDialog]: Failed to save .loadmeta data for save-{0}-{1}\n{2}", folder, fname, ex3.Message);
			}
			finally
			{
				game = null;
			}
		}
		save.name = text;
		return save;
	}

	public void LoadGame()
	{
		saves = new List<PlayerProfileInfo>();
		string[] files = Directory.GetFiles(KSPUtil.ApplicationRootPath + "saves/" + directory, "*.sfs");
		int num = files.Length;
		for (int i = 0; i < num; i++)
		{
			string path = files[i];
			PlayerProfileInfo playerProfileInfo = new PlayerProfileInfo();
			playerProfileInfo.name = Path.GetFileNameWithoutExtension(path);
			if (playerProfileInfo.name.StartsWith("checkpoint_") && directory.StartsWith(MissionsUtils.testsavesPath))
			{
				continue;
			}
			try
			{
				playerProfileInfo = GetSaveData(playerProfileInfo, playerProfileInfo.name, directory);
			}
			catch (Exception ex)
			{
				playerProfileInfo.errorAccess = true;
				playerProfileInfo.errorDetails = ex.Message;
				Debug.LogErrorFormat("[LoadGameDialogs] Error loading save {0} - {1}", playerProfileInfo.name, ex.Message);
			}
			finally
			{
				saves.Add(playerProfileInfo);
			}
		}
		selectedGame = string.Empty;
		selectedSave = null;
		CreateLoadList();
	}

	public void PersistentLoadGame()
	{
		saves = new List<PlayerProfileInfo>();
		DirectoryInfo[] directories = new DirectoryInfo(KSPUtil.ApplicationRootPath + directory).GetDirectories();
		int num = directories.Length;
		for (int i = 0; i < num; i++)
		{
			DirectoryInfo directoryInfo = directories[i];
			if (!(directoryInfo.Name != "training") || !(directoryInfo.Name != "scenarios") || !(directoryInfo.Name != "missions") || !(directoryInfo.Name != "test_missions") || !(directoryInfo.Name != ".svn"))
			{
				continue;
			}
			PlayerProfileInfo playerProfileInfo = new PlayerProfileInfo();
			playerProfileInfo.name = directoryInfo.Name;
			try
			{
				if (File.Exists(directoryInfo.FullName + "/persistent.sfs"))
				{
					if (new FileInfo(directoryInfo.FullName + "/persistent.sfs").Length == 0L)
					{
						playerProfileInfo.errorAccess = true;
						playerProfileInfo.errorDetails = "File length is 0";
					}
					else
					{
						playerProfileInfo = GetSaveData(playerProfileInfo, "persistent", directoryInfo.Name);
					}
				}
			}
			catch (Exception ex)
			{
				playerProfileInfo.errorAccess = true;
				playerProfileInfo.errorDetails = ex.Message;
				Debug.LogErrorFormat("[LoadGameDialogs] Error loading save {0} - {1}", directoryInfo.Name, ex.Message);
			}
			finally
			{
				saves.Add(playerProfileInfo);
			}
		}
		selectedGame = string.Empty;
		selectedSave = null;
		SetHidden(hide: false);
		CreateLoadList();
	}

	public void CreateLoadList()
	{
		DialogGUILabel.TextLabelOptions textLabelOptions = new DialogGUILabel.TextLabelOptions
		{
			enableWordWrapping = false,
			OverflowMode = TextOverflowModes.Overflow,
			resizeBestFit = true,
			resizeMinFontSize = 11,
			resizeMaxFontSize = 12
		};
		SetHidden(hide: false);
		ClearListItems();
		saves = saves.OrderByAlphaNumeric((PlayerProfileInfo s) => s.name);
		int count = saves.Count;
		for (int i = 0; i < count; i++)
		{
			PlayerProfileInfo save = saves[i];
			Sprite sprite = null;
			if (save == null || save.gameNull)
			{
				continue;
			}
			sprite = save.gameMode switch
			{
				Game.Modes.CAREER => careerIcon, 
				Game.Modes.SCIENCE_SANDBOX => scienceSandboxIcon, 
				_ => sandboxIcon, 
			};
			DialogGUIToggleButton dialogGUIToggleButton = new DialogGUIToggleButton(set: false, string.Empty, delegate(bool isActive)
			{
				selectedGame = save.name;
				selectedSave = save;
				if ((Mouse.Left.GetDoubleClick(isDelegate: true) || (menuNav != null && menuNav.SumbmitOnSelectedToggle())) && !confirmGameDelete && isActive)
				{
					ConfirmLoadGame();
				}
				else
				{
					OnSelectionChanged(haveSelection: true);
				}
			});
			dialogGUIToggleButton.guiStyle = skin.customStyles[11];
			dialogGUIToggleButton.OptionInteractableCondition = () => save.gameNull && save.gameCompatible;
			DialogGUILabel dialogGUILabel = new DialogGUILabel(save.name, skin.customStyles[0], expandW: true);
			dialogGUILabel.textLabelOptions = new DialogGUILabel.TextLabelOptions
			{
				resizeBestFit = true,
				resizeMinFontSize = 11,
				resizeMaxFontSize = 12,
				enableWordWrapping = false,
				OverflowMode = TextOverflowModes.Ellipsis
			};
			DialogGUIVerticalLayout dialogGUIVerticalLayout = new DialogGUIVerticalLayout(true, false, 0f, new RectOffset(), TextAnchor.UpperLeft, dialogGUILabel);
			if (!save.gameNull)
			{
				if (save.errorAccess)
				{
					dialogGUIVerticalLayout.AddChild(new DialogGUILabel(Localizer.Format("#autoLOC_485819"), skin.customStyles[1], expandW: true)
					{
						textLabelOptions = textLabelOptions
					});
					string message = save.errorDetails.Split('\n')[0];
					dialogGUIVerticalLayout.AddChild(new DialogGUILabel(message, skin.customStyles[2], expandW: true)
					{
						textLabelOptions = textLabelOptions
					});
				}
				else if (save.gameCompatible)
				{
					if (save.double_0 != -1.0)
					{
						string text = KSPUtil.PrintDate(save.double_0, includeTime: true);
						switch (save.gameMode)
						{
						case Game.Modes.CAREER:
							dialogGUIVerticalLayout.AddChild(new DialogGUILabel(string.Format("<color=" + XKCDColors.HexFormat.ElectricLime + ">{0, -21}</color><color=" + XKCDColors.HexFormat.BrightCyan + ">{1, -17}</color><color=" + XKCDColors.HexFormat.BrightYellow + ">{2, 18}</color>", Localizer.Format("#autoLOC_464659", save.funds.ToString("N0")), Localizer.Format("#autoLOC_464660", save.science.ToString("N0")), Localizer.Format("#autoLOC_464661", save.reputationPercent.ToString("N0"))), skin.customStyles[1], expandW: true)
							{
								textLabelOptions = textLabelOptions
							});
							dialogGUIVerticalLayout.AddChild(new DialogGUILabel(Localizer.Format("#autoLOC_5700006", text, save.vesselCount, save.ongoingContracts), skin.customStyles[2], expandW: true)
							{
								textLabelOptions = textLabelOptions
							});
							break;
						default:
							dialogGUIVerticalLayout.AddChild(new DialogGUILabel(Localizer.Format("#autoLOC_5050039", save.vesselCount), skin.customStyles[1], expandW: true)
							{
								textLabelOptions = textLabelOptions
							});
							dialogGUIVerticalLayout.AddChild(new DialogGUILabel(text, skin.customStyles[2], expandW: true)
							{
								textLabelOptions = textLabelOptions
							});
							break;
						case Game.Modes.SCIENCE_SANDBOX:
							dialogGUIVerticalLayout.AddChild(new DialogGUILabel(Localizer.Format("#autoLOC_5050039", save.vesselCount) + "<color=" + XKCDColors.HexFormat.BrightCyan + ">\t\t" + Localizer.Format("#autoLOC_419420", save.science.ToString("N0")) + "</color>", skin.customStyles[1], expandW: true)
							{
								textLabelOptions = textLabelOptions
							});
							dialogGUIVerticalLayout.AddChild(new DialogGUILabel(text, skin.customStyles[2], expandW: true)
							{
								textLabelOptions = textLabelOptions
							});
							break;
						}
					}
					else
					{
						dialogGUIVerticalLayout.AddChild(new DialogGUILabel(Localizer.Format("#autoLOC_464686"), skin.customStyles[1], expandW: true));
					}
				}
				else
				{
					dialogGUIVerticalLayout.AddChild(new DialogGUILabel(Localizer.Format("#autoLOC_8004247"), skin.customStyles[1], expandW: true));
				}
			}
			else
			{
				dialogGUIVerticalLayout.AddChild(new DialogGUILabel(Localizer.Format("#autoLOC_464696"), skin.customStyles[1], expandW: true));
			}
			dialogGUIToggleButton.AddChild(new DialogGUIHorizontalLayout(false, false, 4f, new RectOffset(0, 8, 6, 7), TextAnchor.MiddleLeft, new DialogGUISprite(new Vector2(58f, 58f), Vector2.zero, Color.white, sprite), dialogGUIVerticalLayout));
			items.Add(dialogGUIToggleButton);
		}
		count = items.Count;
		for (int j = 0; j < count; j++)
		{
			DialogGUIToggleButton dialogGUIToggleButton2 = items[j];
			Stack<Transform> layouts = new Stack<Transform>();
			layouts.Push(base.transform);
			dialogGUIToggleButton2.Create(ref layouts, skin).transform.SetParent(scrollListContent, worldPositionStays: false);
			dialogGUIToggleButton2.toggle.group = listGroup;
		}
		MenuNavigation.SpawnMenuNavigation(base.gameObject, Navigation.Mode.Automatic, SliderFocusType.Scrollbar);
		StartCoroutine(LoadMenuNav());
	}

	public IEnumerator LoadMenuNav()
	{
		yield return null;
		menuNav = GetComponent<MenuNavigation>();
	}

	public void ClearListItems()
	{
		menuNav = GetComponent<MenuNavigation>();
		if (menuNav != null)
		{
			menuNav.ResetAll();
			UnityEngine.Object.Destroy(menuNav);
		}
		int count = items.Count;
		while (count-- > 0)
		{
			items[count].uiItem.gameObject.transform.SetParent(null);
			UnityEngine.Object.Destroy(items[count].uiItem);
		}
		items.Clear();
		selectedGame = string.Empty;
		selectedSave = null;
		OnSelectionChanged(haveSelection: false);
	}
}
