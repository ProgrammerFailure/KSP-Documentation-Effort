using System;
using System.Collections;
using System.Collections.Generic;
using Contracts;
using ns11;
using ns2;
using UniLinq;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.SceneManagement;

public class DebugToolbar : MonoBehaviour
{
	public enum WindowToolbar
	{
		Cheats,
		Physics,
		Debug,
		Database,
		Contracts,
		Missions,
		Kerbals,
		Gameplay,
		Resources,
		RnD,
		Performance
	}

	public enum PhysicsToolbar
	{
		Aero,
		Drag,
		DragProfile,
		Thermal,
		Database
	}

	public enum DragProfileType
	{
		Tail,
		Surface,
		Tip,
		Multiplier
	}

	public enum DatabaseToolbar
	{
		Overview,
		Configs,
		Assemblies,
		Models,
		Textures,
		Audio
	}

	public enum ContractsToolbar
	{
		Active,
		Offered,
		Archive,
		Add,
		Tools
	}

	public enum MissionsToolbar
	{
		Active = 0,
		Completed = 1,
		Tools = 4
	}

	public enum KerbalToolbar
	{
		Roster,
		Details,
		Career,
		Flight
	}

	public static bool toolbarShown;

	public bool inputlockStack;

	public bool setShipOrbit;

	public bool drawFlightStats;

	public bool drawWhackAKerbal;

	public float realDt;

	public float lastRealTime;

	public float ActualTime;

	public Dictionary<CelestialBody, double> originalGeeForces;

	public bool gravityHack;

	public bool over;

	public bool closed;

	public string[] windowToolbarStrings;

	public WindowToolbar windowToolbarIndex;

	public Rect windowRect;

	public Rect setOrbitRect = new Rect(60f, 40f, 240f, 40f);

	public string ecc = "0";

	public string inc = "0";

	public string sma = "700000";

	public string lPe = "0";

	public string MnA = "0";

	public string string_0 = "0";

	public string ObT = "0";

	public int selBodyIndex;

	public Game.Modes currentMode;

	public bool drawExtraCheats;

	public const float cheatDisplayTime = 1.5f;

	public bool drawAeroOptions;

	public bool drawCameraFX;

	public string[] windowPhysicsStrings;

	public PhysicsToolbar windowPhysicsIndex;

	public bool updateDragData;

	public static int dragCurvePointCount = 50;

	public float dragMachDisplay;

	public Vector2[] dragMachPoints;

	public float[] dragCurveGrads = new float[3] { 0.25f, 0.5f, 0.75f };

	public bool dragUpdateCurves = true;

	public float[] dragProfileGrads = new float[4] { 1f, 5f, 10f, 25f };

	public DragProfileType dragProfileSelected;

	public string[] dragProfileTypeString = new string[4] { "Tail", "Surface", "Tip", "Multiplier" };

	public Vector2[] dragProfilePoints;

	public Vector2 debugScrollView = Vector2.zero;

	public string debugInput = string.Empty;

	public string[] databaseToolbarStrings;

	public DatabaseToolbar databaseToolbarIndex;

	public Vector2 databaseOverviewScroll;

	public Vector2 databaseConfigScroll;

	public Vector2 databaseConfigPreviewScroll;

	public string[] databaseConfigPreview;

	public UrlDir.UrlConfig databaseConfigPreviewURL;

	public bool databaseConfigFilterPART = true;

	public bool databaseConfigFilterPROP = true;

	public bool databaseConfigFilterINTERNAL = true;

	public bool databaseConfigFilterRESOURCE = true;

	public Vector2 databaseAssemblyScroll;

	public Vector2 databaseModelsScroll;

	public Vector2 databaseTexturesScroll;

	public Vector2 databaseAudioScroll;

	public bool isRecompiling;

	public string[] contractsToolbarStrings;

	public ContractsToolbar contractsToolbarIndex;

	public Vector2 contractsOfferedScroll;

	public Vector2 contractsActiveScroll;

	public Vector2 contractsArchiveScroll;

	public string[] missionsToolbarStrings;

	public MissionsToolbar missionsToolbarIndex;

	public GameParameters gPars;

	public Vector2 gameplayTweaksScrollPos;

	public Vector2 resourceTweaksScrollPos;

	public string[] kerbalToolbarStrings;

	public KerbalToolbar kerbalToolbarIndex;

	public int kerbalIndex;

	public Vector2 kerbalRosterScroll;

	public Vector2 kerbalRosterFlightScroll;

	public Vector2 kerbalRosterCareerScroll;

	public RnDDebugUtil RDdebugUtil;

	public int RDwindowID;

	public void Start()
	{
		windowToolbarStrings = Enum.GetNames(typeof(WindowToolbar));
		databaseToolbarStrings = Enum.GetNames(typeof(DatabaseToolbar));
		contractsToolbarStrings = Enum.GetNames(typeof(ContractsToolbar));
		missionsToolbarStrings = Enum.GetNames(typeof(MissionsToolbar));
		kerbalToolbarStrings = Enum.GetNames(typeof(KerbalToolbar));
		windowPhysicsStrings = Enum.GetNames(typeof(PhysicsToolbar));
		windowRect = new Rect(10f, 200f, 560f, 700f);
		setShipOrbit = Application.isEditor;
		originalGeeForces = new Dictionary<CelestialBody, double>();
		RDTechTree.OnTechTreeSpawn.Add(OnRnDSpawn);
		RDTechTree.OnTechTreeDespawn.Add(OnRnDDespawn);
		SceneManager.sceneLoaded += OnSceneLoaded;
	}

	public void OnDestroy()
	{
		RDTechTree.OnTechTreeSpawn.Remove(OnRnDSpawn);
		RDTechTree.OnTechTreeDespawn.Remove(OnRnDDespawn);
		SceneManager.sceneLoaded -= OnSceneLoaded;
	}

	public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		OnLevelLoaded(HighLogic.GetLoadedGameSceneFromBuildIndex(scene.buildIndex));
	}

	public void OnLevelLoaded(GameScenes lvl)
	{
		InputLockManager.RemoveControlLock("DebugToolbar");
		drawExtraCheats = false;
		drawCameraFX = false;
		if (HighLogic.LoadedSceneIsGame)
		{
			StartCoroutine(waitForGameLoad(5));
		}
	}

	public void Update()
	{
		currentMode = ((HighLogic.CurrentGame != null) ? HighLogic.CurrentGame.Mode : Game.Modes.SANDBOX);
		if (Input.GetKeyDown(KeyCode.F12) && GameSettings.MODIFIER_KEY.GetKey())
		{
			if (toolbarShown)
			{
				toolbarShown = false;
				drawExtraCheats = false;
			}
			else
			{
				toolbarShown = true;
			}
		}
		if ((currentMode == Game.Modes.CAREER || currentMode == Game.Modes.SCIENCE_SANDBOX) && (HighLogic.LoadedScene == GameScenes.SPACECENTER || HighLogic.LoadedScene == GameScenes.EDITOR))
		{
			drawExtraCheats = true;
		}
		realDt = Time.realtimeSinceStartup - lastRealTime;
		lastRealTime = Time.realtimeSinceStartup;
		ActualTime = Time.deltaTime / realDt;
	}

	public void OnGUI()
	{
		if (!toolbarShown)
		{
			if (!closed)
			{
				InputLockManager.RemoveControlLock("DebugToolbar");
				closed = true;
				over = false;
			}
			return;
		}
		if (HighLogic.LoadedSceneIsFlight && drawFlightStats)
		{
			DrawFlightStats();
		}
		windowRect = GUILayout.Window(9999, windowRect, Window, "Debug Toolbar", GUILayout.ExpandHeight(expand: true));
		if (toolbarShown)
		{
			closed = false;
			if (windowRect.Contains(new Vector3(Input.mousePosition.x, (float)Screen.height - Input.mousePosition.y, Input.mousePosition.z)))
			{
				if (!over)
				{
					over = true;
					InputLockManager.SetControlLock(ControlTypes.ALLBUTCAMERAS, "DebugToolbar");
				}
			}
			else if (over)
			{
				over = false;
				InputLockManager.RemoveControlLock("DebugToolbar");
			}
		}
		if (windowRect.Contains(Event.current.mousePosition))
		{
			GUI.BringWindowToFront(9999);
		}
		if (FlightGlobals.fetch != null && setShipOrbit)
		{
			setOrbitRect = GUILayout.Window(42, setOrbitRect, drawMoveShipWindow, "Set Ship Orbit");
		}
	}

	public void DrawFlightStats()
	{
		if (FlightGlobals.ready && FlightGlobals.currentMainBody != null)
		{
			string text = "";
			text = ((!FlightGlobals.currentMainBody.rotates || !FlightGlobals.currentMainBody.inverseRotation) ? "Frame of Reference: INERTIAL" : "Frame of Reference: ROTATING");
			GUI.Label(new Rect(Screen.width - 210, 10f, 200f, 200f), "UT: " + Planetarium.GetUniversalTime() + "\nPhysics Time Ratio: " + ActualTime.ToString("0.000") + "x\nLat: " + FlightGlobals.ship_latitude + "\nLon: " + FlightGlobals.ship_longitude + "\nRef Body: " + FlightGlobals.currentMainBody.bodyName + ((!FlightGlobals.currentMainBody.orbitDriver) ? "(Inertial)" : (FlightGlobals.currentMainBody.orbitDriver.reverse ? "(Reverse)" : "(Normal)")) + "\n" + text + "\nVfrm: " + Krakensbane.GetFrameVelocityV3f().ToString() + "\nExt Temp: " + FlightGlobals.ActiveVessel.externalTemperature.ToString("0.00") + "°");
		}
	}

	public void Window(int id)
	{
		if (isRecompiling)
		{
			GUILayout.Label("RECOMPILING NECESSARY ASSETS");
			GUILayout.Space(10f);
			WindowDebug();
		}
		else
		{
			windowToolbarIndex = (WindowToolbar)GUILayout.Toolbar((int)windowToolbarIndex, windowToolbarStrings);
			switch (windowToolbarIndex)
			{
			case WindowToolbar.Cheats:
				WindowCheats();
				break;
			case WindowToolbar.Physics:
				WindowPhysics();
				break;
			case WindowToolbar.Debug:
				WindowDebug();
				break;
			case WindowToolbar.Database:
				WindowDatabase();
				break;
			case WindowToolbar.Contracts:
				WindowContracts();
				break;
			case WindowToolbar.Missions:
				WindowMissions();
				break;
			case WindowToolbar.Kerbals:
				WindowKerbals();
				break;
			case WindowToolbar.Gameplay:
				WindowGameplayTweaks();
				break;
			case WindowToolbar.Resources:
				WindowResourceTweaks();
				break;
			case WindowToolbar.RnD:
				DrawWindowRnD();
				break;
			case WindowToolbar.Performance:
				WindowPerformance();
				break;
			}
		}
		GUI.DragWindow();
	}

	public void WindowCheats()
	{
		if (GUILayout.Button((inputlockStack ? "Hide " : "Show ") + "Input Lock Stack"))
		{
			inputlockStack = !inputlockStack;
		}
		if (inputlockStack)
		{
			GUILayout.Label(InputLockManager.PrintLockStack());
			if (GUILayout.Button("Clear Input Lock Stack"))
			{
				InputLockManager.ClearControlLocks();
			}
		}
		if (HighLogic.LoadedSceneIsFlight)
		{
			if (Versioning.Experimental != 0 && GUILayout.Button((setShipOrbit ? "Hide " : "Show ") + "Set Orbit Dialog"))
			{
				setShipOrbit = !setShipOrbit;
			}
			if (GUILayout.Button((drawFlightStats ? "Hide " : "Show ") + "Flight Debug Stats"))
			{
				drawFlightStats = !drawFlightStats;
			}
			if (GUILayout.Button((drawCameraFX ? "Hide " : "Show ") + "Camera FX Stack"))
			{
				drawCameraFX = !drawCameraFX;
			}
			if (!gravityHack)
			{
				if (GUILayout.Button("Hack Gravity"))
				{
					originalGeeForces.Clear();
					for (int i = 0; i < FlightGlobals.Bodies.Count; i++)
					{
						CelestialBody celestialBody = FlightGlobals.Bodies[i];
						originalGeeForces.Add(celestialBody, celestialBody.GeeASL);
					}
					FlightGlobals.currentMainBody.GeeASL = 0.01;
					gravityHack = true;
				}
			}
			else if (GUILayout.Button("Unhack Gravity"))
			{
				for (int j = 0; j < FlightGlobals.Bodies.Count; j++)
				{
					CelestialBody celestialBody2 = FlightGlobals.Bodies[j];
					celestialBody2.GeeASL = originalGeeForces[celestialBody2];
				}
				gravityHack = false;
			}
		}
		if (GUILayout.Button((drawWhackAKerbal ? "Hide " : "Show ") + "Whack A Kerbal"))
		{
			drawWhackAKerbal = !drawWhackAKerbal;
			ShowWhackAKerbalDialog(drawWhackAKerbal);
		}
		GUILayout.Space(5f);
		CheatOptions.PauseOnVesselUnpack = GUILayout.Toggle(CheatOptions.PauseOnVesselUnpack, "Pause on vessel unpack");
		GUILayout.Space(5f);
		CheatOptions.UnbreakableJoints = GUILayout.Toggle(CheatOptions.UnbreakableJoints, "Unbreakable Joints");
		CheatOptions.NoCrashDamage = GUILayout.Toggle(CheatOptions.NoCrashDamage, "No Crash Damage");
		CheatOptions.IgnoreMaxTemperature = GUILayout.Toggle(CheatOptions.IgnoreMaxTemperature, "Ignore Max Temperature");
		GUILayout.Space(5f);
		CheatOptions.InfinitePropellant = GUILayout.Toggle(CheatOptions.InfinitePropellant, "Infinite Propellant");
		CheatOptions.InfiniteElectricity = GUILayout.Toggle(CheatOptions.InfiniteElectricity, "Infinite Electricity");
		GUILayout.Space(5f);
		if (GUILayout.Toggle(CheatOptions.BiomesVisible, "Biomes visible in map") != CheatOptions.BiomesVisible)
		{
			SetBiomesVisible(!CheatOptions.BiomesVisible);
		}
		GUILayout.Space(5f);
		CheatOptions.AllowPartClipping = GUILayout.Toggle(CheatOptions.AllowPartClipping, "Allow Part Clipping in Editors [Bug Hazard!]");
		CheatOptions.NonStrictAttachmentOrientation = GUILayout.Toggle(CheatOptions.NonStrictAttachmentOrientation, "Non-Strict Part Attachment Orientation Checks");
		GUILayout.Space(5f);
		if (!drawExtraCheats)
		{
			return;
		}
		GUILayout.Space(15f);
		GUILayout.BeginHorizontal();
		if (GUILayout.Button("+5 Science", GUILayout.Width(120f)))
		{
			CheatInScience(5f);
		}
		if (GUILayout.Button("+50 Science", GUILayout.Width(120f)))
		{
			CheatInScience(50f);
		}
		if (GUILayout.Button("+500 Science", GUILayout.Width(120f)))
		{
			CheatInScience(500f);
		}
		GUILayout.Space(5f);
		if (GUILayout.Button("Zero Science", GUILayout.Width(120f)) && ResearchAndDevelopment.Instance != null)
		{
			ResearchAndDevelopment.Instance.AddScience(0f - ResearchAndDevelopment.Instance.Science, TransactionReasons.Cheating);
		}
		GUILayout.EndHorizontal();
		if (currentMode == Game.Modes.CAREER)
		{
			GUILayout.BeginHorizontal();
			if (GUILayout.Button("+10k Funds", GUILayout.Width(100f)))
			{
				CheatInFunds(10000.0);
			}
			if (GUILayout.Button("+50k Funds", GUILayout.Width(100f)))
			{
				CheatInFunds(50000.0);
			}
			if (GUILayout.Button("+250k Funds", GUILayout.Width(100f)))
			{
				CheatInFunds(250000.0);
			}
			if (GUILayout.Button("+1M Funds", GUILayout.Width(100f)))
			{
				CheatInFunds(1000000.0);
			}
			GUILayout.Space(5f);
			if (GUILayout.Button("Zero Funds", GUILayout.Width(100f)) && Funding.Instance != null)
			{
				Funding.Instance.AddFunds(0.0 - Funding.Instance.Funds, TransactionReasons.Cheating);
			}
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal();
			if (GUILayout.Button("-50 Rep", GUILayout.Width(70f)))
			{
				CheatInReputation(-50f);
			}
			if (GUILayout.Button("-25 Rep", GUILayout.Width(70f)))
			{
				CheatInReputation(-25f);
			}
			if (GUILayout.Button("-5 Rep", GUILayout.Width(70f)))
			{
				CheatInReputation(-5f);
			}
			GUILayout.Space(5f);
			if (GUILayout.Button("+5 Rep", GUILayout.Width(70f)))
			{
				CheatInReputation(5f);
			}
			if (GUILayout.Button("+25 Rep", GUILayout.Width(70f)))
			{
				CheatInReputation(25f);
			}
			if (GUILayout.Button("+50 Rep", GUILayout.Width(70f)))
			{
				CheatInReputation(50f);
			}
			GUILayout.EndHorizontal();
		}
		GUILayout.Space(15f);
		GUILayout.BeginHorizontal();
		if (GUILayout.Button("Stock Vessels", GUILayout.Width(120f)) && HighLogic.CurrentGame != null)
		{
			if (HighLogic.CurrentGame.Parameters.Difficulty.AllowStockVessels)
			{
				HighLogic.CurrentGame.Parameters.Difficulty.AllowStockVessels = false;
				ScreenMessages.PostScreenMessage("Stock vessel availability has been disabled.", 5f, ScreenMessageStyle.UPPER_CENTER);
			}
			else
			{
				HighLogic.CurrentGame.Parameters.Difficulty.AllowStockVessels = true;
				ScreenMessages.PostScreenMessage("Stock vessel availability has been enabled.", 5f, ScreenMessageStyle.UPPER_CENTER);
			}
		}
		if (GUILayout.Button("Max Research", GUILayout.Width(120f)))
		{
			if (ResearchAndDevelopment.Instance != null)
			{
				ScreenMessages.PostScreenMessage("All technology in Research and Development has been researched.", 5f, ScreenMessageStyle.UPPER_CENTER);
				ResearchAndDevelopment.Instance.CheatTechnology();
			}
			else
			{
				ScreenMessages.PostScreenMessage("Research and Development is not available at this time.", 5f, ScreenMessageStyle.UPPER_CENTER);
			}
		}
		if (currentMode == Game.Modes.CAREER)
		{
			if (GUILayout.Button("Max Experience", GUILayout.Width(120f)))
			{
				ScreenMessages.PostScreenMessage(CheatInExperience() ? "All crew members are now fully experienced." : "Crew experience can not be modified at this time.", 5f, ScreenMessageStyle.UPPER_CENTER);
			}
			if (GUILayout.Button("Max Facility", GUILayout.Width(120f)))
			{
				if (ScenarioUpgradeableFacilities.Instance != null)
				{
					ScreenMessages.PostScreenMessage("All facilities in the space center have been fully upgraded.", 5f, ScreenMessageStyle.UPPER_CENTER);
					ScenarioUpgradeableFacilities.Instance.CheatFacilities();
				}
				else
				{
					ScreenMessages.PostScreenMessage("Facility upgrades are not available at this time.", 5f, ScreenMessageStyle.UPPER_CENTER);
				}
			}
		}
		if (GUILayout.Button("Max Progress", GUILayout.Width(120f)))
		{
			if (ProgressTracking.Instance != null)
			{
				if (Event.current.button == 0)
				{
					ScreenMessages.PostScreenMessage("All game progression has been fully completed.", 5f, ScreenMessageStyle.UPPER_CENTER);
					ProgressTracking.Instance.CheatProgression();
				}
				else if (Event.current.button == 1)
				{
					ScreenMessages.PostScreenMessage("Early game progression on " + FlightGlobals.GetHomeBodyDisplayName() + " has been completed.", 5f, ScreenMessageStyle.UPPER_CENTER);
					ProgressTracking.Instance.CheatEarlyProgression();
				}
			}
			else
			{
				ScreenMessages.PostScreenMessage("Progress modifications are not available at this time.", 5f, ScreenMessageStyle.UPPER_CENTER);
			}
		}
		GUILayout.EndHorizontal();
	}

	public void SetBiomesVisible(bool isTrue)
	{
		CheatOptions.BiomesVisible = isTrue;
		for (int i = 0; i < FlightGlobals.fetch.bodies.Count; i++)
		{
			CelestialBody celestialBody = FlightGlobals.fetch.bodies[i];
			GameObject scaledBody = celestialBody.scaledBody;
			if (celestialBody == null || scaledBody == null)
			{
				continue;
			}
			MeshRenderer component = scaledBody.GetComponent<MeshRenderer>();
			if (component.material.HasProperty("_ResourceMap"))
			{
				Texture2D texture2D = (Texture2D)component.material.GetTexture("_ResourceMap");
				if (texture2D != null)
				{
					UnityEngine.Object.Destroy(texture2D);
					texture2D = null;
				}
				if (isTrue && celestialBody.BiomeMap != null)
				{
					texture2D = celestialBody.BiomeMap.CompileToTexture();
				}
				component.material.SetTexture("_ResourceMap", texture2D);
			}
		}
	}

	public void CheatInScience(float science)
	{
		if (ResearchAndDevelopment.Instance != null)
		{
			ResearchAndDevelopment.Instance.AddScience(science, TransactionReasons.Cheating);
		}
	}

	public void CheatInFunds(double funds)
	{
		if (Funding.Instance != null)
		{
			Funding.Instance.AddFunds(funds, TransactionReasons.Cheating);
		}
	}

	public void CheatInReputation(float rep)
	{
		if (Reputation.Instance != null)
		{
			Reputation.Instance.AddReputation(rep, TransactionReasons.Cheating);
		}
	}

	public bool CheatInExperience()
	{
		if (HighLogic.CurrentGame != null && HighLogic.CurrentGame.Mode == Game.Modes.CAREER && HighLogic.CurrentGame.CrewRoster != null && FlightGlobals.Bodies != null)
		{
			foreach (ProtoCrewMember item in HighLogic.CurrentGame.CrewRoster.Crew)
			{
				int count = FlightGlobals.Bodies.Count;
				while (count-- > 0)
				{
					item.careerLog.AddEntryUnique((!FlightGlobals.Bodies[count].hasSolidSurface) ? FlightLog.EntryType.Orbit : FlightLog.EntryType.Land, FlightGlobals.Bodies[count].name);
				}
				item.experience = item.CalculateExperiencePoints(HighLogic.CurrentGame);
				item.experienceLevel = KerbalRoster.CalculateExperienceLevel(item.experience);
			}
			CrewListItem[] array = UnityEngine.Object.FindObjectsOfType<CrewListItem>();
			int num = array.Length;
			while (num-- > 0)
			{
				CrewListItem crewListItem = array[num];
				if (!(crewListItem == null))
				{
					ProtoCrewMember crewRef = crewListItem.GetCrewRef();
					if (crewRef != null && crewRef.type == ProtoCrewMember.KerbalType.Crew)
					{
						crewListItem.SetXP(crewRef);
					}
				}
			}
			return true;
		}
		return false;
	}

	public void ShowWhackAKerbalDialog(bool show)
	{
		if (!(Camera.main == null))
		{
			WhackAKerbal component = Camera.main.GetComponent<WhackAKerbal>();
			if (show && component == null)
			{
				Camera.main.gameObject.AddComponent<WhackAKerbal>();
			}
			if (!show && component != null)
			{
				UnityEngine.Object.Destroy(component);
			}
		}
	}

	public void drawMoveShipWindow(int id)
	{
		GUILayout.BeginHorizontal();
		if (GUILayout.Button("<"))
		{
			selBodyIndex = Mathf.Max(selBodyIndex - 1, 0);
		}
		GUILayout.Label(FlightGlobals.Bodies[selBodyIndex].name);
		if (GUILayout.Button(">"))
		{
			selBodyIndex = Mathf.Min(selBodyIndex + 1, FlightGlobals.Bodies.Count - 1);
		}
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		GUILayout.Label("Ecc: ");
		ecc = GUILayout.TextField(ecc);
		GUILayout.Label("MnA: ");
		MnA = GUILayout.TextField(MnA);
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		GUILayout.Label("Inc: ");
		inc = GUILayout.TextField(inc);
		GUILayout.Label("LAN: ");
		string_0 = GUILayout.TextField(string_0);
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		GUILayout.Label("SMA: ");
		sma = GUILayout.TextField(sma);
		GUILayout.Label("lPe: ");
		lPe = GUILayout.TextField(lPe);
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		GUILayout.Label("ObT: ");
		ObT = GUILayout.TextField(ObT);
		GUILayout.EndHorizontal();
		if (GUILayout.Button("Go!") && double.Parse(sma) > FlightGlobals.Bodies[selBodyIndex].Radius && double.Parse(ecc) >= 0.0 && double.Parse(inc) <= 180.0 && double.Parse(inc) >= -180.0)
		{
			FlightGlobals.fetch.SetShipOrbit(selBodyIndex, double.Parse(ecc), double.Parse(sma), double.Parse(inc), double.Parse(string_0), double.Parse(MnA), double.Parse(lPe), double.Parse(ObT));
		}
		GUI.DragWindow();
	}

	public void WindowPhysics()
	{
		windowPhysicsIndex = (PhysicsToolbar)GUILayout.Toolbar((int)windowPhysicsIndex, windowPhysicsStrings);
		switch (windowPhysicsIndex)
		{
		case PhysicsToolbar.Aero:
			WindowAero();
			break;
		case PhysicsToolbar.Drag:
			WindowDrag();
			break;
		case PhysicsToolbar.DragProfile:
			WindowDragProfile();
			break;
		case PhysicsToolbar.Thermal:
			WindowThermal();
			break;
		case PhysicsToolbar.Database:
			WindowPhysicsDatabase();
			break;
		}
	}

	public void WindowPhysicsDatabase()
	{
		GUILayout.Space(10f);
		GUILayout.BeginHorizontal();
		GUILayout.Label("Physics Filename: ", GUILayout.Width(100f));
		PhysicsGlobals.PhysicsDatabaseFilename = GUILayout.TextField(PhysicsGlobals.PhysicsDatabaseFilename, GUILayout.Width(182f));
		GUILayout.EndHorizontal();
		GUILayout.Space(10f);
		GUILayout.BeginHorizontal();
		if (GUILayout.Button("Save DB", GUILayout.Width(80f)))
		{
			PhysicsGlobals.Instance.SaveDatabase();
		}
		if (GUILayout.Button("Load DB", GUILayout.Width(80f)))
		{
			PhysicsGlobals.Instance.LoadDatabase();
			updateDragData = true;
		}
		GUILayout.EndHorizontal();
	}

	public void WindowAero()
	{
		GUILayout.Space(10f);
		PhysicsGlobals.AeroDataDisplay = GUILayout.Toggle(PhysicsGlobals.AeroDataDisplay, "Display Aero Data in Action Menus");
		PhysicsGlobals.AeroGUIDisplay = GUILayout.Toggle(PhysicsGlobals.AeroGUIDisplay, "Display Aero Data GUI");
		PhysicsGlobals.AeroForceDisplay = GUILayout.Toggle(PhysicsGlobals.AeroForceDisplay, "Display Aero Forces In Flight");
		if (PhysicsGlobals.AeroForceDisplay)
		{
			GUILayout.BeginHorizontal();
			GUILayout.Label("Aero Forces Display Scale: " + PhysicsGlobals.AeroForceDisplayScale.ToString("F2"), GUILayout.Width(182f));
			PhysicsGlobals.AeroForceDisplayScale = GUILayout.HorizontalSlider(PhysicsGlobals.AeroForceDisplayScale, 0.01f, 1f, GUILayout.ExpandWidth(expand: true));
			GUILayout.EndHorizontal();
		}
		GUILayout.BeginHorizontal();
		GUILayout.Label("Global Lift Multiplier: " + PhysicsGlobals.LiftMultiplier.ToString("F3"), GUILayout.Width(182f));
		PhysicsGlobals.LiftMultiplier = GUILayout.HorizontalSlider(PhysicsGlobals.LiftMultiplier, 0.0001f, 0.1f, GUILayout.ExpandWidth(expand: true));
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		GUILayout.Label("Lift/Drag Multiplier: " + PhysicsGlobals.LiftDragMultiplier.ToString("F3"), GUILayout.Width(182f));
		PhysicsGlobals.LiftDragMultiplier = GUILayout.HorizontalSlider(PhysicsGlobals.LiftDragMultiplier, 0.001f, 0.25f, GUILayout.ExpandWidth(expand: true));
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		GUILayout.Label("Body Lift Multiplier: " + PhysicsGlobals.BodyLiftMultiplier.ToString("F3"), GUILayout.Width(182f));
		PhysicsGlobals.BodyLiftMultiplier = GUILayout.HorizontalSlider(PhysicsGlobals.BodyLiftMultiplier, 0f, 100f, GUILayout.ExpandWidth(expand: true));
		GUILayout.EndHorizontal();
	}

	public void WindowDrag()
	{
		GUILayout.Space(10f);
		PhysicsGlobals.ApplyDrag = GUILayout.Toggle(PhysicsGlobals.ApplyDrag, "Apply Drag");
		GUILayout.Space(5f);
		if (PhysicsGlobals.ApplyDrag)
		{
			PhysicsGlobals.DragUsesAcceleration = GUILayout.Toggle(PhysicsGlobals.DragUsesAcceleration, "Apply Drag As Acceleration Instead of Force");
			GUILayout.Space(5f);
			PhysicsGlobals.ApplyDragToNonPhysicsParts = GUILayout.Toggle(PhysicsGlobals.ApplyDragToNonPhysicsParts, "Apply Drag To Non-Physical Parts");
			if (PhysicsGlobals.ApplyDragToNonPhysicsParts)
			{
				PhysicsGlobals.ApplyDragToNonPhysicsPartsAtParentCoM = GUILayout.Toggle(PhysicsGlobals.ApplyDragToNonPhysicsPartsAtParentCoM, "Apply Non-Physical Part Drag At Parent CoM");
			}
			GUILayout.Space(10f);
			GUILayout.BeginHorizontal();
			GUILayout.Label("Global Drag Multiplier: " + PhysicsGlobals.DragMultiplier.ToString("F3"), GUILayout.Width(182f));
			PhysicsGlobals.DragMultiplier = GUILayout.HorizontalSlider(PhysicsGlobals.DragMultiplier, 0.1f, 10f, GUILayout.ExpandWidth(expand: true));
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal();
			GUILayout.Label("Drag Cube Multiplier: " + PhysicsGlobals.DragCubeMultiplier.ToString("F3"), GUILayout.Width(182f));
			PhysicsGlobals.DragCubeMultiplier = GUILayout.HorizontalSlider(PhysicsGlobals.DragCubeMultiplier, 0.001f, 0.1f, GUILayout.ExpandWidth(expand: true));
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal();
			GUILayout.Label("Angular Drag Multiplier: " + PhysicsGlobals.AngularDragMultiplier.ToString("F3"), GUILayout.Width(182f));
			PhysicsGlobals.AngularDragMultiplier = GUILayout.HorizontalSlider(PhysicsGlobals.AngularDragMultiplier, 0.01f, 10f, GUILayout.ExpandWidth(expand: true));
			GUILayout.EndHorizontal();
			GUILayout.Space(5f);
			PhysicsGlobals.DragCubesUseSpherical = GUILayout.Toggle(PhysicsGlobals.DragCubesUseSpherical, "Replace Drag Cubes With Spherical Model");
		}
	}

	public void WindowThermal()
	{
		GUILayout.Space(10f);
		PhysicsGlobals.ThermalDataDisplay = GUILayout.Toggle(PhysicsGlobals.ThermalDataDisplay, "Display Thermal Data in Action Menus");
		PhysicsGlobals.ThermoGUIDisplay = GUILayout.Toggle(PhysicsGlobals.ThermoGUIDisplay, "Display Thermal Data GUI");
		GUILayout.Space(5f);
		PhysicsGlobals.ThermalColorsDebug = GUILayout.Toggle(PhysicsGlobals.ThermalColorsDebug, "Thermal Debug Colors");
		GUILayout.Space(10f);
		PhysicsGlobals.ThermalRadiationEnabled = GUILayout.Toggle(PhysicsGlobals.ThermalRadiationEnabled, "Radiation Enabled");
		GUILayout.BeginHorizontal();
		GUILayout.Label("Radiation Factor: " + PhysicsGlobals.RadiationFactor.ToString("F3"), GUILayout.Width(182f));
		PhysicsGlobals.RadiationFactor = GUILayout.HorizontalSlider((float)PhysicsGlobals.RadiationFactor, 0.1f, 100f, GUILayout.ExpandWidth(expand: true));
		GUILayout.EndHorizontal();
		GUILayout.Space(5f);
		PhysicsGlobals.ThermalConductionEnabled = GUILayout.Toggle(PhysicsGlobals.ThermalConductionEnabled, "Conduction Enabled");
		GUILayout.BeginHorizontal();
		GUILayout.Label("Conduction Factor: " + PhysicsGlobals.ConductionFactor.ToString("F3"), GUILayout.Width(182f));
		PhysicsGlobals.ConductionFactor = GUILayout.HorizontalSlider((float)PhysicsGlobals.ConductionFactor, 0.1f, 200f, GUILayout.ExpandWidth(expand: true));
		GUILayout.EndHorizontal();
		GUILayout.Space(5f);
		PhysicsGlobals.ThermalConvectionEnabled = GUILayout.Toggle(PhysicsGlobals.ThermalConvectionEnabled, "Convection Enabled");
		GUILayout.BeginHorizontal();
		GUILayout.Label("Convection Factor: " + PhysicsGlobals.MachConvectionFactor.ToString("F3"), GUILayout.Width(182f));
		PhysicsGlobals.MachConvectionFactor = GUILayout.HorizontalSlider((float)PhysicsGlobals.MachConvectionFactor, 0.1f, 100f, GUILayout.ExpandWidth(expand: true));
		GUILayout.EndHorizontal();
		GUILayout.Space(10f);
		GUILayout.BeginHorizontal();
		GUILayout.Label("Generation Factor: " + PhysicsGlobals.InternalHeatProductionFactor.ToString("F3"), GUILayout.Width(182f));
		PhysicsGlobals.InternalHeatProductionFactor = GUILayout.HorizontalSlider((float)PhysicsGlobals.InternalHeatProductionFactor, 0f, 2f, GUILayout.ExpandWidth(expand: true));
		GUILayout.EndHorizontal();
		GUILayout.Space(5f);
		GUILayout.BeginHorizontal();
		GUILayout.Label("Newtonian Temperature Factor: " + PhysicsGlobals.NewtonianTemperatureFactor.ToString("F3"), GUILayout.Width(182f));
		PhysicsGlobals.NewtonianTemperatureFactor = GUILayout.HorizontalSlider((float)PhysicsGlobals.NewtonianTemperatureFactor, 0.01f, 5f, GUILayout.ExpandWidth(expand: true));
		GUILayout.EndHorizontal();
		GUILayout.Space(10f);
		GUILayout.BeginHorizontal();
		GUILayout.Label("Solar Luminosity At Home: " + PhysicsGlobals.SolarLuminosityAtHome, GUILayout.Width(182f));
		PhysicsGlobals.SolarLuminosityAtHome = GUILayout.HorizontalSlider((float)PhysicsGlobals.SolarLuminosityAtHome, 1f, 10000f, GUILayout.ExpandWidth(expand: true));
		GUILayout.EndHorizontal();
		GUILayout.Space(5f);
		GUILayout.BeginHorizontal();
		GUILayout.Label("Solar Insolation At Home: " + PhysicsGlobals.SolarInsolationAtHome, GUILayout.Width(182f));
		PhysicsGlobals.SolarInsolationAtHome = GUILayout.HorizontalSlider((float)PhysicsGlobals.SolarInsolationAtHome, 0.01f, 1f, GUILayout.ExpandWidth(expand: true));
		GUILayout.EndHorizontal();
		GUILayout.Space(5f);
		if (GUI.changed)
		{
			PhysicsGlobals.Instance.CalculateValues();
		}
	}

	public void WindowDragProfile()
	{
		GUILayout.Space(10f);
		if (dragUpdateCurves || updateDragData)
		{
			dragMachPoints = GetSurfaceDragCurvePoints(dragMachDisplay);
			dragUpdateCurves = false;
		}
		GUILayout.BeginHorizontal();
		GUILayout.Label("Drag curve at mach " + dragMachDisplay.ToString("F1"), GUILayout.Width(160f));
		float num = GUILayout.HorizontalSlider(dragMachDisplay, 0f, 25f, GUILayout.ExpandWidth(expand: true));
		GUILayout.EndHorizontal();
		if (num != dragMachDisplay)
		{
			dragMachDisplay = num;
			dragMachPoints = GetSurfaceDragCurvePoints(num);
		}
		Drawing.DrawGraph(dragMachPoints, dragCurveGrads, Color.white, Color.green, 120f);
		GUILayout.Space(20f);
		WindowDragDisplay_Profiles();
		updateDragData = false;
	}

	public void WindowDragDisplay_Profiles()
	{
		bool flag = false;
		DragProfileType dragProfileType = (DragProfileType)GUILayout.Toolbar((int)dragProfileSelected, dragProfileTypeString);
		if (dragProfileType != dragProfileSelected || updateDragData)
		{
			dragProfileSelected = dragProfileType;
			dragProfilePoints = null;
		}
		switch (dragProfileSelected)
		{
		case DragProfileType.Tail:
			flag = FloatCurveEditor(PhysicsGlobals.DragCurveTail);
			if (dragProfilePoints == null || flag)
			{
				dragProfilePoints = GetAnimationCurvePoints(PhysicsGlobals.DragCurveTail.Curve);
			}
			break;
		case DragProfileType.Surface:
			flag = FloatCurveEditor(PhysicsGlobals.DragCurveSurface);
			if (dragProfilePoints == null || flag)
			{
				dragProfilePoints = GetAnimationCurvePoints(PhysicsGlobals.DragCurveSurface.Curve);
			}
			break;
		case DragProfileType.Tip:
			flag = FloatCurveEditor(PhysicsGlobals.DragCurveTip);
			if (dragProfilePoints == null || flag)
			{
				dragProfilePoints = GetAnimationCurvePoints(PhysicsGlobals.DragCurveTip.Curve);
			}
			break;
		case DragProfileType.Multiplier:
			flag = FloatCurveEditor(PhysicsGlobals.DragCurveMultiplier);
			if (dragProfilePoints == null || flag)
			{
				dragProfilePoints = GetAnimationCurvePoints(PhysicsGlobals.DragCurveMultiplier.Curve);
			}
			break;
		}
		if (dragProfilePoints != null)
		{
			Drawing.DrawGraph(dragProfilePoints, dragProfileGrads, Color.white, Color.green, 100f);
		}
		if (flag)
		{
			dragUpdateCurves = true;
		}
	}

	public bool FloatCurveEditor(FloatCurve curve)
	{
		bool flag = false;
		Keyframe[] keys = curve.Curve.keys;
		GUILayout.Label("Keys: " + curve.Curve.keys.Length);
		GUILayout.BeginHorizontal();
		GUILayout.Label("Time", GUILayout.Width(100f));
		GUILayout.Label("Value", GUILayout.Width(100f));
		GUILayout.Label("TanIn", GUILayout.Width(100f));
		GUILayout.Label("TanOut", GUILayout.Width(100f));
		GUILayout.EndHorizontal();
		for (int i = 0; i < curve.Curve.keys.Length; i++)
		{
			GUILayout.BeginHorizontal();
			if (float.TryParse(GUILayout.TextField(keys[i].time.ToString(), GUILayout.Width(100f)), out var result) && result != keys[i].time)
			{
				keys[i].time = result;
				flag = true;
			}
			if (float.TryParse(GUILayout.TextField(keys[i].value.ToString(), GUILayout.Width(100f)), out result) && result != keys[i].value)
			{
				keys[i].value = result;
				flag = true;
			}
			if (float.TryParse(GUILayout.TextField(keys[i].inTangent.ToString(), GUILayout.Width(100f)), out result) && result != keys[i].inTangent)
			{
				keys[i].inTangent = result;
				flag = true;
			}
			if (float.TryParse(GUILayout.TextField(keys[i].outTangent.ToString(), GUILayout.Width(100f)), out result) && result != keys[i].outTangent)
			{
				keys[i].outTangent = result;
				flag = true;
			}
			if (flag)
			{
				curve.Curve.keys = keys;
			}
			GUILayout.FlexibleSpace();
			if (curve.Curve.keys.Length > 1 && GUILayout.Button("-", GUILayout.Width(16f)))
			{
				List<Keyframe> list = new List<Keyframe>(curve.Curve.keys);
				list.RemoveAt(i);
				curve.Curve.keys = list.ToArray();
				flag = true;
			}
			if (GUILayout.Button("+", GUILayout.Width(16f)))
			{
				List<Keyframe> list2 = new List<Keyframe>(curve.Curve.keys);
				if (i == curve.Curve.keys.Length - 1)
				{
					list2.Add(default(Keyframe));
				}
				else
				{
					list2.Insert(i + 1, default(Keyframe));
				}
				curve.Curve.keys = list2.ToArray();
				flag = true;
			}
			GUILayout.EndHorizontal();
		}
		return flag;
	}

	public Vector2[] GetSurfaceDragCurvePoints(float mach)
	{
		Vector2[] array = new Vector2[dragCurvePointCount];
		float num = 1f / ((float)dragCurvePointCount - 1f);
		for (int i = 0; i < dragCurvePointCount; i++)
		{
			float num2 = num * (float)i;
			array[i] = new Vector2(num2, PhysicsGlobals.DragCurveValue(PhysicsGlobals.SurfaceCurves, num2, mach));
		}
		return array;
	}

	public Vector2[] GetAnimationCurvePoints(AnimationCurve curve)
	{
		Vector2[] array = new Vector2[dragCurvePointCount];
		float num = curve.keys[curve.length - 1].time / (float)(dragCurvePointCount - 1);
		for (int i = 0; i < dragCurvePointCount; i++)
		{
			float num2 = num * (float)i;
			array[i] = new Vector2(num2, curve.Evaluate(num2));
		}
		return array;
	}

	public void WindowDebug()
	{
		debugScrollView = GUILayout.BeginScrollView(debugScrollView, false, true);
		GUILayout.EndScrollView();
		GUILayout.BeginHorizontal();
		debugInput = GUILayout.TextField(debugInput, GUILayout.Width(windowRect.width - 100f));
		if (GUILayout.Button("Comment"))
		{
			bool flag;
			if (!(flag = Event.current.button != 0) && debugInput == string.Empty)
			{
				return;
			}
			Debug.LogWarning("+++++++++++++++++++++ RUNTIME COMMENT +++++++++++++++++++++");
			Debug.LogWarning("Time: " + DateTime.UtcNow.ToString("T"));
			if (flag)
			{
				if (AssemblyLoader.loadedAssemblies != null)
				{
					Debug.LogWarning("Assemblies: " + string.Join(", ", AssemblyLoader.loadedAssemblies.Select((AssemblyLoader.LoadedAssembly o) => o.name).ToArray()));
				}
				if (HighLogic.CurrentGame != null)
				{
					Debug.LogWarning("Save: " + HighLogic.CurrentGame.Title);
					Debug.LogWarning("Mode: " + HighLogic.CurrentGame.Mode);
				}
				Debug.LogWarning("Scene: " + HighLogic.LoadedScene);
				if (HighLogic.LoadedSceneIsFlight && FlightGlobals.ActiveVessel != null)
				{
					Vessel activeVessel = FlightGlobals.ActiveVessel;
					Debug.LogWarning("Vessel: " + activeVessel.GetDisplayName());
					Debug.LogWarning("Body: " + activeVessel.mainBody.name);
					Debug.LogWarning("Situation: " + activeVessel.situation);
					Debug.LogWarning("Latitude: " + activeVessel.latitude);
					Debug.LogWarning("Longitude: " + activeVessel.longitude);
					Debug.LogWarning("Altitude: " + activeVessel.altitude);
				}
			}
			if (debugInput != string.Empty)
			{
				Debug.LogWarning("Comment: " + debugInput);
			}
			Debug.LogWarning("+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++");
			debugInput = string.Empty;
		}
		GUILayout.EndHorizontal();
		GUI.skin.textArea.normal.textColor = Color.white;
	}

	public void WindowDatabase()
	{
		databaseToolbarIndex = (DatabaseToolbar)GUILayout.Toolbar((int)databaseToolbarIndex, databaseToolbarStrings);
		switch (databaseToolbarIndex)
		{
		case DatabaseToolbar.Overview:
			DatabaseOverview();
			break;
		case DatabaseToolbar.Configs:
			DatabaseConfigs();
			break;
		case DatabaseToolbar.Assemblies:
			DatabaseAssemblies();
			break;
		case DatabaseToolbar.Models:
			DatabaseModels();
			break;
		case DatabaseToolbar.Textures:
			DatabaseTextures();
			break;
		case DatabaseToolbar.Audio:
			DatabaseAudio();
			break;
		}
	}

	public void DatabaseOverview()
	{
		GUILayout.Space(10f);
		GUILayout.BeginHorizontal();
		if (HighLogic.LoadedScene != GameScenes.FLIGHT && HighLogic.LoadedScene != GameScenes.EDITOR)
		{
			if (GUILayout.Button("Reload all"))
			{
				GameDatabase.Instance.Recompile = true;
				StartCoroutine(RecompileAssets());
			}
		}
		else
		{
			GUILayout.Label("Cannot reload in this scene");
		}
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		GUILayout.Space(20f);
		GUILayout.Label("Database Totals");
		GUILayout.Space(5f);
		databaseConfigScroll = GUILayout.BeginScrollView(databaseOverviewScroll, false, true);
		GUILayout.Label("Configs");
		DatabaseOverviewConfig("PART", ref databaseConfigFilterPART);
		DatabaseOverviewConfig("PROP", ref databaseConfigFilterPROP);
		DatabaseOverviewConfig("INTERNAL", ref databaseConfigFilterINTERNAL);
		DatabaseOverviewConfig("RESOURCE", ref databaseConfigFilterRESOURCE);
		GUILayout.Space(20f);
		GUILayout.BeginHorizontal();
		GUILayout.Label("Assemblies", GUILayout.Width(120f));
		GUILayout.Label(AssemblyLoader.loadedAssemblies.Count.ToString());
		GUILayout.FlexibleSpace();
		if (GUILayout.Button("View"))
		{
			databaseToolbarIndex = DatabaseToolbar.Assemblies;
		}
		GUILayout.EndHorizontal();
		GUILayout.Space(20f);
		GUILayout.BeginHorizontal();
		GUILayout.Label("Models", GUILayout.Width(120f));
		GUILayout.Label(GameDatabase.Instance.databaseModel.Count.ToString());
		GUILayout.FlexibleSpace();
		if (GUILayout.Button("View"))
		{
			databaseToolbarIndex = DatabaseToolbar.Assemblies;
		}
		GUILayout.EndHorizontal();
		GUILayout.Space(20f);
		GUILayout.BeginHorizontal();
		GUILayout.Label("Textures", GUILayout.Width(120f));
		GUILayout.Label(GameDatabase.Instance.databaseTexture.Count.ToString());
		GUILayout.FlexibleSpace();
		if (GUILayout.Button("View"))
		{
			databaseToolbarIndex = DatabaseToolbar.Textures;
		}
		GUILayout.EndHorizontal();
		GUILayout.Space(20f);
		GUILayout.BeginHorizontal();
		GUILayout.Label("Audio", GUILayout.Width(120f));
		GUILayout.Label(GameDatabase.Instance.databaseAudio.Count.ToString());
		GUILayout.FlexibleSpace();
		if (GUILayout.Button("View"))
		{
			databaseToolbarIndex = DatabaseToolbar.Audio;
		}
		GUILayout.EndHorizontal();
		GUILayout.EndScrollView();
	}

	public void DatabaseOverviewConfig(string type, ref bool filter)
	{
		GUILayout.BeginHorizontal();
		GUILayout.Label(type, GUILayout.Width(120f));
		GUILayout.Label(GameDatabase.Instance.GetConfigs(type).Length.ToString());
		GUILayout.FlexibleSpace();
		if (GUILayout.Button("View"))
		{
			databaseToolbarIndex = DatabaseToolbar.Configs;
			filter = true;
		}
		GUILayout.EndHorizontal();
	}

	public void DatabaseConfigs()
	{
		GUILayout.BeginHorizontal();
		databaseConfigFilterPART = GUILayout.Toggle(databaseConfigFilterPART, "PART", GUILayout.Width(120f));
		databaseConfigFilterRESOURCE = GUILayout.Toggle(databaseConfigFilterRESOURCE, "RESOURCE", GUILayout.Width(120f));
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		databaseConfigFilterPROP = GUILayout.Toggle(databaseConfigFilterPROP, "PROP", GUILayout.Width(120f));
		databaseConfigFilterINTERNAL = GUILayout.Toggle(databaseConfigFilterINTERNAL, "INTERNAL", GUILayout.Width(120f));
		GUILayout.EndHorizontal();
		databaseConfigScroll = GUILayout.BeginScrollView(databaseConfigScroll, false, true);
		if (databaseConfigFilterPART)
		{
			DatabaseConfigsSpam("PART");
		}
		if (databaseConfigFilterPROP)
		{
			DatabaseConfigsSpam("PROP");
		}
		if (databaseConfigFilterINTERNAL)
		{
			DatabaseConfigsSpam("INTERNAL");
		}
		if (databaseConfigFilterRESOURCE)
		{
			DatabaseConfigsSpam("RESOURCE");
		}
		GUILayout.EndScrollView();
		if (databaseConfigPreviewURL == null)
		{
			return;
		}
		GUILayout.Space(20f);
		GUILayout.BeginHorizontal();
		GUILayout.Label(databaseConfigPreviewURL.type + ": " + databaseConfigPreviewURL.url);
		GUILayout.FlexibleSpace();
		if (GUILayout.Button("Close"))
		{
			databaseConfigPreviewURL = null;
			databaseConfigPreview = null;
			return;
		}
		GUILayout.EndHorizontal();
		databaseConfigPreviewScroll = GUILayout.BeginScrollView(databaseConfigPreviewScroll, GUILayout.Height(200f));
		string[] array = databaseConfigPreview;
		for (int i = 0; i < array.Length; i++)
		{
			GUILayout.Label(array[i]);
		}
		GUILayout.EndScrollView();
	}

	public void DatabaseConfigsSpam(string typeName)
	{
		GUILayout.Label(typeName);
		GUILayout.Space(10f);
		UrlDir.UrlConfig[] configs = GameDatabase.Instance.GetConfigs(typeName);
		foreach (UrlDir.UrlConfig urlConfig in configs)
		{
			GUILayout.BeginHorizontal();
			GUILayout.Label(urlConfig.url);
			GUILayout.FlexibleSpace();
			if (GUILayout.Button("Debug"))
			{
				DatabaseConfigPreview(urlConfig);
			}
			GUILayout.EndHorizontal();
		}
		GUILayout.Space(20f);
	}

	public void DatabaseConfigPreview(UrlDir.UrlConfig cfg)
	{
		databaseConfigPreviewURL = cfg;
		databaseConfigPreview = cfg.config.ToString().Split(new char[2] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
	}

	public void DatabaseAssemblies()
	{
		databaseAssemblyScroll = GUILayout.BeginScrollView(databaseAssemblyScroll, false, true);
		foreach (AssemblyLoader.LoadedAssembly loadedAssembly in AssemblyLoader.loadedAssemblies)
		{
			GUILayout.BeginHorizontal();
			GUILayout.Label(loadedAssembly.assembly.FullName);
			GUILayout.EndHorizontal();
		}
		GUILayout.EndScrollView();
	}

	public void DatabaseModels()
	{
		databaseModelsScroll = GUILayout.BeginScrollView(databaseModelsScroll, false, true);
		foreach (GameObject item in GameDatabase.Instance.databaseModel)
		{
			GUILayout.BeginHorizontal();
			GUILayout.Label(item.name);
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
		}
		GUILayout.EndScrollView();
	}

	public void DatabaseTextures()
	{
		databaseTexturesScroll = GUILayout.BeginScrollView(databaseTexturesScroll, false, true);
		foreach (GameDatabase.TextureInfo item in GameDatabase.Instance.databaseTexture)
		{
			GUILayout.BeginHorizontal();
			if (item == null)
			{
				GUILayout.Label("TEXTURE IS NULL");
			}
			else
			{
				GUILayout.Label(item.name);
				GUILayout.FlexibleSpace();
			}
			GUILayout.EndHorizontal();
		}
		GUILayout.EndScrollView();
	}

	public void DatabaseAudio()
	{
		databaseAudioScroll = GUILayout.BeginScrollView(databaseAudioScroll, false, true);
		foreach (AudioClip item in GameDatabase.Instance.databaseAudio)
		{
			GUILayout.BeginHorizontal();
			GUILayout.Label(item.name);
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
		}
		GUILayout.EndScrollView();
	}

	public IEnumerator RecompileAssets()
	{
		isRecompiling = true;
		GameDatabase.Instance.Recompile = true;
		GameDatabase.Instance.StartLoad();
		while (!GameDatabase.Instance.IsReady())
		{
			yield return null;
		}
		PartLoader.Instance.StartLoad();
		while (!PartLoader.Instance.IsReady())
		{
			yield return null;
		}
		isRecompiling = false;
	}

	public void WindowContracts()
	{
		contractsToolbarIndex = (ContractsToolbar)GUILayout.Toolbar((int)contractsToolbarIndex, contractsToolbarStrings);
		switch (contractsToolbarIndex)
		{
		case ContractsToolbar.Active:
			ContractsActive();
			break;
		case ContractsToolbar.Offered:
			ContractsOffered();
			break;
		case ContractsToolbar.Archive:
			ContractsArchive();
			break;
		case ContractsToolbar.Add:
			ContractsAdd();
			break;
		case ContractsToolbar.Tools:
			ContractsTools();
			break;
		}
	}

	public void ContractsOffered()
	{
		if (ContractSystem.Instance == null)
		{
			GUILayout.Space(10f);
			GUILayout.BeginHorizontal();
			GUILayout.Label("Contract system is not loaded");
			GUILayout.EndHorizontal();
			return;
		}
		GUILayout.Space(20f);
		contractsOfferedScroll = GUILayout.BeginScrollView(contractsOfferedScroll, false, true);
		for (int i = 0; i < ContractSystem.Instance.Contracts.Count; i++)
		{
			Contract contract = ContractSystem.Instance.Contracts[i];
			if (contract.ContractState == Contract.State.Offered)
			{
				GUILayout.BeginHorizontal();
				GUILayout.Label(contract.Prestige.displayDescription(), GUILayout.Width(80f));
				GUILayout.Label(contract.Title, GUILayout.ExpandWidth(expand: true));
				if (contract.CanBeDeclined() && GUILayout.Button("Dec", GUILayout.Width(42f)))
				{
					contract.Decline();
				}
				if (GUILayout.Button("Acc", GUILayout.Width(42f)))
				{
					contract.Accept();
				}
				GUILayout.EndHorizontal();
			}
		}
		GUILayout.EndScrollView();
	}

	public void ContractsActive()
	{
		if (ContractSystem.Instance == null)
		{
			GUILayout.Space(10f);
			GUILayout.BeginHorizontal();
			GUILayout.Label("Contract system is not loaded");
			GUILayout.EndHorizontal();
			return;
		}
		GUILayout.Space(20f);
		contractsActiveScroll = GUILayout.BeginScrollView(contractsActiveScroll, false, true);
		for (int i = 0; i < ContractSystem.Instance.Contracts.Count; i++)
		{
			Contract contract = ContractSystem.Instance.Contracts[i];
			if (contract.ContractState == Contract.State.Active)
			{
				GUILayout.BeginHorizontal();
				GUILayout.Label(contract.Prestige.displayDescription(), GUILayout.Width(80f));
				GUILayout.Label(contract.Title, GUILayout.ExpandWidth(expand: true));
				if (contract.CanBeCancelled() && GUILayout.Button("Can", GUILayout.Width(42f)))
				{
					contract.Cancel();
				}
				if (GUILayout.Button("Com", GUILayout.Width(42f)))
				{
					contract.Complete();
				}
				GUILayout.EndHorizontal();
			}
		}
		GUILayout.EndScrollView();
	}

	public void ContractsArchive()
	{
		if (ContractSystem.Instance == null)
		{
			GUILayout.Space(10f);
			GUILayout.BeginHorizontal();
			GUILayout.Label("Contract system is not loaded");
			GUILayout.EndHorizontal();
			return;
		}
		GUILayout.Space(20f);
		contractsArchiveScroll = GUILayout.BeginScrollView(contractsArchiveScroll, false, true);
		int num = 0;
		while (true)
		{
			if (num < ContractSystem.Instance.ContractsFinished.Count)
			{
				Contract contract = ContractSystem.Instance.ContractsFinished[num];
				GUILayout.BeginHorizontal();
				GUILayout.Label(contract.Prestige.displayDescription(), GUILayout.Width(80f));
				GUILayout.Label(contract.Title, GUILayout.ExpandWidth(expand: true));
				GUILayout.Label(contract.ContractState.ToString(), GUILayout.Width(100f));
				if (GUILayout.Button("Rem", GUILayout.Width(42f)))
				{
					break;
				}
				GUILayout.EndHorizontal();
				num++;
				continue;
			}
			GUILayout.EndScrollView();
			return;
		}
		ContractSystem.Instance.ContractsFinished.RemoveAt(num);
	}

	public void ContractsAdd()
	{
		if (ContractSystem.Instance == null)
		{
			GUILayout.Space(10f);
			GUILayout.BeginHorizontal();
			GUILayout.Label("Contract system is not loaded");
			GUILayout.EndHorizontal();
			return;
		}
		GUILayout.Space(20f);
		contractsArchiveScroll = GUILayout.BeginScrollView(contractsArchiveScroll, false, true);
		for (int i = 0; i < ContractSystem.ContractTypes.Count; i++)
		{
			Type type = ContractSystem.ContractTypes[i];
			GUILayout.BeginHorizontal();
			if (GUILayout.Button("E", GUILayout.Width(32f)))
			{
				ContractGenerate(type, Contract.ContractPrestige.Trivial);
			}
			if (GUILayout.Button("M", GUILayout.Width(32f)))
			{
				ContractGenerate(type, Contract.ContractPrestige.Significant);
			}
			if (GUILayout.Button("H", GUILayout.Width(32f)))
			{
				ContractGenerate(type, Contract.ContractPrestige.Exceptional);
			}
			GUILayout.Label(type.Name, GUILayout.ExpandWidth(expand: true));
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal();
			GUILayout.Label(type.Assembly.FullName);
			GUILayout.EndHorizontal();
			GUILayout.Space(10f);
		}
		GUILayout.EndScrollView();
	}

	public bool ContractGenerate(Type cType, Contract.ContractPrestige difficulty)
	{
		Contract contract = ContractSystem.Instance.GenerateContract(UnityEngine.Random.Range(int.MinValue, int.MaxValue), difficulty, cType);
		if (contract != null)
		{
			ContractSystem.Instance.Contracts.Add(contract);
			contract.Offer();
			return true;
		}
		Debug.LogError("Contract of type '" + cType.Name + "' was not be generated.");
		return false;
	}

	public void ContractsTools()
	{
		if (ContractSystem.Instance == null)
		{
			GUILayout.Space(10f);
			GUILayout.BeginHorizontal();
			GUILayout.Label("Contract system is not loaded");
			GUILayout.EndHorizontal();
			return;
		}
		GUILayout.Space(20f);
		if (GUILayout.Button("Reset Contract Weights"))
		{
			ContractSystem.ResetWeights();
		}
		if (GUILayout.Button("Regenerate Current Contracts"))
		{
			ContractSystem.Instance.RebuildContracts();
		}
		if (GUILayout.Button("Clear Current Contracts"))
		{
			ContractSystem.Instance.ClearContractsCurrent();
		}
		if (GUILayout.Button("Clear Finished Contracts"))
		{
			ContractSystem.Instance.ClearContractsFinished();
		}
	}

	public void WindowMissions()
	{
		missionsToolbarIndex = (MissionsToolbar)GUILayout.Toolbar((int)missionsToolbarIndex, missionsToolbarStrings);
		switch (missionsToolbarIndex)
		{
		case MissionsToolbar.Active:
			MissionsActive();
			break;
		case MissionsToolbar.Completed:
			MissionsCompleted();
			break;
		case MissionsToolbar.Tools:
			MissionsTools();
			break;
		case (MissionsToolbar)2:
		case (MissionsToolbar)3:
			break;
		}
	}

	public void MissionsActive()
	{
	}

	public void MissionsCompleted()
	{
	}

	public void MissionsTools()
	{
	}

	public IEnumerator waitForGameLoad(int waitdelay)
	{
		for (int i = 0; i < waitdelay; i++)
		{
			yield return null;
		}
		gPars = HighLogic.CurrentGame.Parameters;
		StartCoroutine(CallbackUtil.DelayedCallback(15, delegate
		{
			ShowWhackAKerbalDialog(drawWhackAKerbal);
		}));
	}

	public void WindowGameplayTweaks()
	{
		if (HighLogic.LoadedSceneIsGame)
		{
			gameplayTweaksScrollPos = GUILayout.BeginScrollView(gameplayTweaksScrollPos, false, false, GUILayout.ExpandWidth(expand: true), GUILayout.ExpandHeight(expand: true));
			GUILayout.Label("Difficulty Options:");
			gPars.Difficulty.AllowStockVessels = GUILayout.Toggle(gPars.Difficulty.AllowStockVessels, "Allow Stock Vessels", GUILayout.ExpandWidth(expand: true));
			gPars.Difficulty.MissingCrewsRespawn = GUILayout.Toggle(gPars.Difficulty.MissingCrewsRespawn, "Lost Crews Respawn", GUILayout.ExpandWidth(expand: true));
			gPars.Flight.CanRestart = GUILayout.Toggle(gPars.Flight.CanRestart, "Allow Revert To Launch");
			gPars.Flight.CanLeaveToEditor = GUILayout.Toggle(gPars.Flight.CanLeaveToEditor, "Allow Revert to Editor");
			gPars.Flight.CanQuickLoad = GUILayout.Toggle(gPars.Flight.CanQuickLoad, "Allow Quickloading");
			gPars.Flight.CanQuickSave = GUILayout.Toggle(gPars.Flight.CanQuickSave, "Allow Quicksaving");
			GUILayout.Space(5f);
			GUILayout.Label("Misc:");
			CheatOptions.IgnoreAgencyMindsetOnContracts = GUILayout.Toggle(CheatOptions.IgnoreAgencyMindsetOnContracts, "Ignore Agency Mindset on Contracts", GUILayout.ExpandWidth(expand: true));
			GUILayout.EndScrollView();
		}
		else
		{
			GUILayout.Label("Load a game before editing gameplay settings");
		}
	}

	public void WindowResourceTweaks()
	{
		if (HighLogic.LoadedSceneIsGame)
		{
			resourceTweaksScrollPos = GUILayout.BeginScrollView(resourceTweaksScrollPos, false, false, GUILayout.ExpandWidth(expand: true), GUILayout.ExpandHeight(expand: true));
			ResourceSetup.Instance.ResConfig.HeatEnabled = GUILayout.Toggle(ResourceSetup.Instance.ResConfig.HeatEnabled, "Enable heat generation for resource parts", GUILayout.ExpandWidth(expand: true));
			ResourceSetup.Instance.ResConfig.ShowDebugOptions = GUILayout.Toggle(ResourceSetup.Instance.ResConfig.ShowDebugOptions, "Show debug info on resource parts", GUILayout.ExpandWidth(expand: true));
			GUILayout.EndScrollView();
		}
		else
		{
			GUILayout.Label("Load a game before editing resource settings");
		}
	}

	public void WindowKerbals()
	{
		kerbalToolbarIndex = (KerbalToolbar)GUILayout.Toolbar((int)kerbalToolbarIndex, kerbalToolbarStrings);
		switch (kerbalToolbarIndex)
		{
		case KerbalToolbar.Roster:
			KerbalRosterAll();
			break;
		case KerbalToolbar.Details:
			KerbalRosterDetails();
			break;
		case KerbalToolbar.Career:
			KerbalRosterDetails();
			break;
		case KerbalToolbar.Flight:
			KerbalRosterDetails();
			break;
		}
	}

	public void KerbalRosterAll()
	{
		if (HighLogic.CurrentGame == null || HighLogic.CurrentGame.CrewRoster == null)
		{
			return;
		}
		GUILayout.Space(20f);
		kerbalRosterScroll = GUILayout.BeginScrollView(kerbalRosterScroll, false, true);
		for (int i = 0; i < HighLogic.CurrentGame.CrewRoster.Count; i++)
		{
			ProtoCrewMember protoCrewMember = HighLogic.CurrentGame.CrewRoster[i];
			GUILayout.BeginHorizontal();
			if (GUILayout.Button(">", GUILayout.Width(16f)))
			{
				kerbalIndex = i;
				kerbalRosterFlightScroll = Vector2.zero;
				kerbalRosterCareerScroll = Vector2.zero;
			}
			GUILayout.Label(protoCrewMember.name, GUILayout.ExpandWidth(expand: true));
			GUILayout.Label(protoCrewMember.experience.ToString(), GUILayout.Width(30f));
			GUILayout.Label(protoCrewMember.experienceLevel.ToString(), GUILayout.Width(30f));
			GUILayout.Label(protoCrewMember.type.ToString(), GUILayout.Width(80f));
			GUILayout.Label(protoCrewMember.rosterStatus.ToString(), GUILayout.Width(80f));
			GUILayout.EndHorizontal();
		}
		GUILayout.EndScrollView();
	}

	public void KerbalRosterDetails()
	{
		if (HighLogic.CurrentGame != null && HighLogic.CurrentGame.CrewRoster != null)
		{
			ProtoCrewMember protoCrewMember = HighLogic.CurrentGame.CrewRoster[kerbalIndex];
			if (protoCrewMember == null)
			{
				kerbalToolbarIndex = KerbalToolbar.Roster;
			}
			GUILayout.Space(20f);
			KerbalDatabaseSelector();
			GUILayout.BeginHorizontal();
			GUILayout.Label(protoCrewMember.name, GUILayout.ExpandWidth(expand: true));
			GUILayout.Label(protoCrewMember.experience.ToString(), GUILayout.Width(30f));
			GUILayout.Label(protoCrewMember.experienceLevel.ToString(), GUILayout.Width(30f));
			GUILayout.Label(protoCrewMember.type.ToString(), GUILayout.Width(80f));
			GUILayout.Label(protoCrewMember.rosterStatus.ToString(), GUILayout.Width(80f));
			GUILayout.EndHorizontal();
		}
	}

	public void KerbalRosterCareer()
	{
		if (HighLogic.CurrentGame == null || HighLogic.CurrentGame.CrewRoster == null)
		{
			return;
		}
		ProtoCrewMember protoCrewMember = HighLogic.CurrentGame.CrewRoster[kerbalIndex];
		if (protoCrewMember == null)
		{
			kerbalToolbarIndex = KerbalToolbar.Roster;
		}
		GUILayout.Space(20f);
		KerbalDatabaseSelector();
		GUILayout.BeginHorizontal();
		GUILayout.Label(protoCrewMember.name, GUILayout.ExpandWidth(expand: true));
		GUILayout.Label(protoCrewMember.experience.ToString(), GUILayout.Width(30f));
		GUILayout.Label(protoCrewMember.experienceLevel.ToString(), GUILayout.Width(30f));
		GUILayout.Label(protoCrewMember.type.ToString(), GUILayout.Width(80f));
		GUILayout.Label(protoCrewMember.rosterStatus.ToString(), GUILayout.Width(80f));
		GUILayout.EndHorizontal();
		kerbalRosterFlightScroll = GUILayout.BeginScrollView(kerbalRosterFlightScroll, false, true);
		for (int i = 0; i < protoCrewMember.careerLog.Count; i++)
		{
			FlightLog.Entry entry = protoCrewMember.careerLog[i];
			GUILayout.BeginHorizontal();
			GUILayout.Label(entry.flight.ToString(), GUILayout.Width(40f));
			GUILayout.Label(entry.type, GUILayout.Width(40f));
			if (!string.IsNullOrEmpty(entry.target))
			{
				GUILayout.Label(entry.target.ToString());
			}
			GUILayout.EndHorizontal();
		}
		GUILayout.EndScrollView();
	}

	public void KerbalRosterFlight()
	{
		if (HighLogic.CurrentGame == null || HighLogic.CurrentGame.CrewRoster == null)
		{
			return;
		}
		ProtoCrewMember protoCrewMember = HighLogic.CurrentGame.CrewRoster[kerbalIndex];
		if (protoCrewMember == null)
		{
			kerbalToolbarIndex = KerbalToolbar.Roster;
		}
		GUILayout.Space(20f);
		KerbalDatabaseSelector();
		GUILayout.BeginHorizontal();
		GUILayout.Label(protoCrewMember.name, GUILayout.ExpandWidth(expand: true));
		GUILayout.Label(protoCrewMember.experience.ToString(), GUILayout.Width(30f));
		GUILayout.Label(protoCrewMember.experienceLevel.ToString(), GUILayout.Width(30f));
		GUILayout.Label(protoCrewMember.type.ToString(), GUILayout.Width(40f));
		GUILayout.Label(protoCrewMember.rosterStatus.ToString(), GUILayout.Width(40f));
		GUILayout.EndHorizontal();
		kerbalRosterCareerScroll = GUILayout.BeginScrollView(kerbalRosterCareerScroll, false, true);
		for (int i = 0; i < protoCrewMember.flightLog.Count; i++)
		{
			FlightLog.Entry entry = protoCrewMember.flightLog[i];
			GUILayout.BeginHorizontal();
			GUILayout.Label(entry.flight.ToString(), GUILayout.Width(40f));
			GUILayout.Label(entry.type, GUILayout.Width(40f));
			if (!string.IsNullOrEmpty(entry.target))
			{
				GUILayout.Label(entry.target.ToString());
			}
			GUILayout.EndHorizontal();
		}
		GUILayout.EndScrollView();
	}

	public void KerbalDatabaseSelector()
	{
		GUILayout.BeginHorizontal();
		if (GUILayout.Button("<<", GUILayout.Width(24f)))
		{
			kerbalIndex = 0;
			kerbalRosterFlightScroll = Vector2.zero;
			kerbalRosterCareerScroll = Vector2.zero;
		}
		if (GUILayout.Button("<", GUILayout.Width(24f)))
		{
			kerbalIndex--;
			kerbalIndex = Mathf.Max(kerbalIndex, 0);
			kerbalRosterFlightScroll = Vector2.zero;
			kerbalRosterCareerScroll = Vector2.zero;
		}
		if (GUILayout.Button(">", GUILayout.Width(24f)))
		{
			kerbalIndex++;
			kerbalIndex = Mathf.Min(kerbalIndex, HighLogic.CurrentGame.CrewRoster.Count - 1);
			kerbalRosterFlightScroll = Vector2.zero;
			kerbalRosterCareerScroll = Vector2.zero;
		}
		if (GUILayout.Button(">>", GUILayout.Width(24f)))
		{
			kerbalIndex = HighLogic.CurrentGame.CrewRoster.Count - 1;
			kerbalRosterFlightScroll = Vector2.zero;
			kerbalRosterCareerScroll = Vector2.zero;
		}
		GUILayout.EndHorizontal();
		GUILayout.Space(5f);
	}

	public void WindowPerformance()
	{
		GUILayout.Space(10f);
		Vector2[] framePerSecondPoints = Performance.Instance.GetFramePerSecondPoints();
		GUILayout.Label("Frames Per Second");
		Drawing.DrawGraph(framePerSecondPoints, null, Color.white, Color.green, 120f);
		GUILayout.Space(15f);
		GUILayout.BeginHorizontal();
		GUILayout.Label("FPS", GUILayout.Width(120f));
		GUILayout.Label(Performance.Instance.FramesPerSecond.ToString("F1"));
		GUILayout.EndHorizontal();
		GUILayout.Space(5f);
		GUILayout.BeginHorizontal();
		GUILayout.Label("FPS Max", GUILayout.Width(120f));
		GUILayout.Label((1f / Performance.Instance.FrameTimeMin).ToString("F1"));
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		GUILayout.Label("FPS Min", GUILayout.Width(120f));
		GUILayout.Label((1f / Performance.Instance.FrameTimeMax).ToString("F1"));
		GUILayout.EndHorizontal();
		GUILayout.Space(15f);
		GUILayout.Label("Memory");
		GUILayout.Space(5f);
		GUILayout.BeginHorizontal();
		GUILayout.Label("Used Heap Size", GUILayout.Width(120f));
		GUILayout.Label(((float)Profiler.usedHeapSizeLong / 1048576f).ToString("F2") + "mb");
		GUILayout.EndHorizontal();
		GUILayout.Space(5f);
		GUILayout.BeginHorizontal();
		GUILayout.Label("Allocated", GUILayout.Width(120f));
		GUILayout.Label(((float)Profiler.GetTotalAllocatedMemoryLong() / 1048576f).ToString("F2") + "mb");
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		GUILayout.Label("Reserved", GUILayout.Width(120f));
		GUILayout.Label(((float)Profiler.GetTotalReservedMemoryLong() / 1048576f).ToString("F2") + "mb");
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		GUILayout.Label("Unused Reserved", GUILayout.Width(120f));
		GUILayout.Label(((float)Profiler.GetTotalUnusedReservedMemoryLong() / 1048576f).ToString("F2") + "mb");
		GUILayout.EndHorizontal();
		GUILayout.Space(5f);
		GUILayout.BeginHorizontal();
		GUILayout.Label("Mono Heap", GUILayout.Width(120f));
		GUILayout.Label(((float)Profiler.GetMonoHeapSizeLong() / 1048576f).ToString("F2") + "mb");
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		GUILayout.Label("Mono Heap Used", GUILayout.Width(120f));
		GUILayout.Label(((float)Profiler.GetMonoUsedSizeLong() / 1048576f).ToString("F2") + "mb");
		GUILayout.EndHorizontal();
	}

	public void OnRnDSpawn(RDTechTree tecTree)
	{
		RDdebugUtil = new RnDDebugUtil();
		RDdebugUtil.Init(tecTree, windowRect.width);
		RDwindowID = GetInstanceID();
	}

	public void OnRnDDespawn(RDTechTree tecTree)
	{
		RDdebugUtil.Terminate();
	}

	public void DrawWindowRnD()
	{
		if (RDdebugUtil != null)
		{
			RDdebugUtil.DrawWindow(RDwindowID);
		}
		else
		{
			GUILayout.Label("Please open the Tech Tree Screen at the R&D Facility to use these tools");
		}
	}

	public void FloatField(ref float var, params GUILayoutOption[] options)
	{
		if (float.TryParse(GUILayout.TextField(var.ToString(), options), out var result))
		{
			var = result;
		}
	}
}
