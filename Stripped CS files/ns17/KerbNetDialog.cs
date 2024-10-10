using System;
using System.Collections;
using System.Collections.Generic;
using CommNet;
using FinePrint;
using FinePrint.Utilities;
using KerbNet;
using ns12;
using ns2;
using ns9;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ns17;

public class KerbNetDialog : MonoBehaviour
{
	public Vessel DisplayVessel;

	public bool display;

	public const string LockID = "KerbNetDialog";

	public bool _inputLocked;

	[SerializeField]
	public Button closeButton;

	[SerializeField]
	public UIStateButton modeButton;

	[SerializeField]
	public CanvasRenderer modeButtonRenderer;

	[SerializeField]
	public TextMeshProUGUI modeButtonText;

	[SerializeField]
	public UIStateButton visibilityButton;

	[SerializeField]
	public UIStateButton autoRefreshButton;

	[SerializeField]
	public Sprite modeErrorButtonSprite;

	[SerializeField]
	public GameObject mapTexture;

	[SerializeField]
	public WaypointTarget waypointTarget;

	[SerializeField]
	public TextMeshProUGUI errorText;

	[SerializeField]
	public GameObject[] disableOnError;

	[SerializeField]
	public GameObject[] enableOnError;

	[SerializeField]
	public CanvasRenderer gridRenderer;

	[SerializeField]
	public CanvasRenderer waypointRenderer;

	[SerializeField]
	public TextMeshProUGUI bodyText;

	[SerializeField]
	public TextMeshProUGUI infoLabel;

	[SerializeField]
	public TextMeshProUGUI infoText;

	[SerializeField]
	public TextMeshProUGUI targetText;

	[SerializeField]
	public TextMeshProUGUI centerText;

	[SerializeField]
	public Slider fovSlider;

	[SerializeField]
	public TextMeshProUGUI fovText;

	[SerializeField]
	public TMP_InputField waypointField;

	[SerializeField]
	public Button customButton;

	[SerializeField]
	public TextMeshProUGUI customButtonText;

	[SerializeField]
	public TooltipController_Text customButtonTooltip;

	[SerializeField]
	public Button refreshButton;

	[SerializeField]
	public Button waypointButton;

	public IAccessKerbNet KerbNetAccessor;

	[HideInInspector]
	public static List<KerbNetMode> resourceDisplayModes;

	[HideInInspector]
	public static Dictionary<string, KerbNetMode> knownDisplayModes;

	[HideInInspector]
	public List<KerbNetMode> activeDisplayModes = new List<KerbNetMode>();

	[HideInInspector]
	public KerbNetMode currentDisplayMode;

	[HideInInspector]
	public double centerLatitude;

	[HideInInspector]
	public double centerLongitude;

	[HideInInspector]
	public double waypointLatitude;

	[HideInInspector]
	public double waypointLongitude;

	public Vector3d localVesselPos;

	public Vector3d cameraX;

	public Vector3d cameraY;

	public Vector3d cameraZ;

	public float fovMin = 5f;

	public float fovMax = 90f;

	public float fovCurrent = 47.5f;

	public double fovScale = 1.0;

	public const int mapSize = 257;

	public const int halfMapSize = 128;

	public float visibilitySpeed = 5f;

	public float gridAlphaTarget = 1f;

	public float waypointAlphaTarget = 1f;

	public float AnomalyChance;

	public bool beenSetup;

	public bool delayedErrorRefresh;

	public Coroutine autoRefreshRoutine;

	public static Texture2D mapTexture2D;

	public static Color32[] mapResetArray;

	public byte[,] questionArray = new byte[13, 13]
	{
		{
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0
		},
		{
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0
		},
		{
			0, 0, 0, 0, 0, 1, 1, 1, 0, 0,
			0, 0, 0
		},
		{
			0, 0, 0, 0, 0, 1, 2, 1, 0, 0,
			0, 0, 0
		},
		{
			0, 0, 0, 0, 0, 1, 1, 1, 0, 0,
			0, 0, 0
		},
		{
			0, 0, 0, 0, 0, 1, 2, 1, 1, 0,
			0, 0, 0
		},
		{
			0, 0, 0, 0, 0, 1, 1, 2, 1, 1,
			0, 0, 0
		},
		{
			0, 0, 0, 1, 1, 1, 1, 1, 2, 1,
			0, 0, 0
		},
		{
			0, 0, 0, 1, 2, 2, 1, 1, 2, 1,
			0, 0, 0
		},
		{
			0, 0, 0, 1, 1, 2, 2, 2, 1, 1,
			0, 0, 0
		},
		{
			0, 0, 0, 0, 1, 1, 1, 1, 1, 0,
			0, 0, 0
		},
		{
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0
		},
		{
			0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
			0, 0, 0
		}
	};

	public static string cacheAutoLOC_438370;

	public static string cacheAutoLOC_438382;

	public static string cacheAutoLOC_438390;

	public static string cacheAutoLOC_438413;

	public static string cacheAutoLOC_438415;

	public static string cacheAutoLOC_438467;

	public static string cacheAutoLOC_6001959;

	public static string cacheAutoLOC_258912;

	public static KerbNetDialog Instance { get; set; }

	public static bool isDisplaying
	{
		get
		{
			if (Instance != null)
			{
				return Instance.display;
			}
			return false;
		}
	}

	public bool InputLocked
	{
		get
		{
			return _inputLocked;
		}
		set
		{
			if (_inputLocked != value)
			{
				_inputLocked = value;
				if (_inputLocked)
				{
					InputLockManager.SetControlLock("KerbNetDialog");
				}
				else
				{
					InputLockManager.RemoveControlLock("KerbNetDialog");
				}
			}
		}
	}

	[HideInInspector]
	public bool RecoveryRefreshQueued { get; set; }

	[HideInInspector]
	public bool HasError { get; set; }

	[HideInInspector]
	public string ErrorState { get; set; }

	public int Seed { get; set; }

	public KSPRandom Generator { get; set; }

	public static Texture2D GetTexture()
	{
		if (mapTexture2D == null)
		{
			mapTexture2D = new Texture2D(257, 257, TextureFormat.ARGB32, mipChain: false);
			mapResetArray = mapTexture2D.GetPixels32();
		}
		else if (mapResetArray != null)
		{
			mapTexture2D.SetPixels32(mapResetArray);
		}
		return mapTexture2D;
	}

	public void Awake()
	{
		if (Generator == null)
		{
			Seed = Environment.TickCount ^ Guid.NewGuid().GetHashCode();
			Generator = new KSPRandom(Seed);
		}
	}

	public void OnDestroy()
	{
		if (Instance != null && Instance == this)
		{
			Instance = null;
		}
		DeactivateDisplayMode(currentDisplayMode);
		StopAutoRefresh();
		if (beenSetup)
		{
			GameEvents.onPartDestroyed.Remove(OnPartDestroyed);
			GameEvents.onVesselControlStateChange.Remove(OnVesselStatusChanged);
			GameEvents.CommNet.OnCommHomeStatusChange.Remove(OnVesselStatusChanged);
		}
		InputLocked = false;
	}

	public void Update()
	{
		if (isDisplaying)
		{
			if (!HighLogic.LoadedSceneIsFlight)
			{
				Close();
			}
			if (FlightGlobals.ActiveVessel != DisplayVessel)
			{
				Close();
			}
			if (delayedErrorRefresh)
			{
				RefreshErrorState();
				delayedErrorRefresh = false;
			}
			UpdateVisibility();
		}
		InputLocked = waypointField != null && waypointField.isFocused;
	}

	public static KerbNetDialog Display(IAccessKerbNet accessor)
	{
		if (Instance == null)
		{
			Instance = UnityEngine.Object.Instantiate(AssetBase.GetPrefab("KerbNetDialog")).GetComponent<KerbNetDialog>();
			Instance.name = "KerbNet Dialog Handler";
			(Instance.transform as RectTransform).SetParent(UIMasterController.Instance.appCanvas.transform, worldPositionStays: false);
		}
		Instance.KerbNetAccessor = accessor;
		Instance.DisplayVessel = accessor.GetKerbNetVessel();
		Instance.fovMin = accessor.GetKerbNetMinimumFoV();
		Instance.fovMax = accessor.GetKerbNetMaximumFoV();
		Instance.AnomalyChance = accessor.GetKerbNetAnomalyChance();
		Instance.display = true;
		GenerateKnownModes();
		SetupDisplayModes(accessor.GetKerbNetDisplayModes());
		Instance.SetupGUI();
		if (!HighLogic.LoadedSceneIsEditor)
		{
			Instance.FullRefresh(refreshMap: true);
		}
		return Instance;
	}

	public static void Close()
	{
		if (!(Instance == null))
		{
			Instance.display = false;
			UnityEngine.Object.Destroy(Instance.gameObject);
		}
	}

	public void SetupGUI()
	{
		SetupModeButton(activeDisplayModes);
		UpdateFoVSlider(!beenSetup);
		if (!beenSetup)
		{
			gridAlphaTarget = 1f;
			gridRenderer.SetAlpha(Instance.gridAlphaTarget);
			waypointAlphaTarget = 1f;
			waypointRenderer.SetAlpha(Instance.waypointAlphaTarget);
			visibilityButton.SetState("noVisibility", invokeChange: false);
			autoRefreshButton.SetState("refreshNone", invokeChange: false);
			RandomizeWaypointField();
			modeButton.onValueChanged.AddListener(OnModeChanged);
			visibilityButton.onValueChanged.AddListener(OnVisibilityChanged);
			autoRefreshButton.onValueChanged.AddListener(OnAutoRefreshChanged);
			closeButton.onClick.AddListener(Close);
			fovSlider.onValueChanged.AddListener(OnFoVSet);
			waypointField.onEndEdit.AddListener(OnWaypointFieldEndEdit);
			refreshButton.onClick.AddListener(OnRefreshClicked);
			waypointButton.onClick.AddListener(OnWaypointClicked);
			GameEvents.onPartDestroyed.Add(OnPartDestroyed);
			GameEvents.onVesselControlStateChange.Add(OnVesselStatusChanged);
			GameEvents.CommNet.OnCommHomeStatusChange.Add(OnVesselStatusChanged);
			beenSetup = true;
		}
	}

	public void FullRefresh(bool refreshMap, bool refreshErrors = true)
	{
		if (refreshErrors)
		{
			RefreshErrorState();
		}
		if (HasError)
		{
			RecoveryRefreshQueued = true;
			return;
		}
		RecoveryRefreshQueued = false;
		centerLatitude = ResourceUtilities.clampLat(DisplayVessel.latitude);
		centerLongitude = ResourceUtilities.clampLon(DisplayVessel.longitude);
		if (refreshMap)
		{
			RefreshMap();
		}
		RefreshDataLabels(waypointTarget.transform.localPosition);
	}

	public void RefreshDataLabels(Vector2 pos)
	{
		if (currentDisplayMode != null)
		{
			int x = Mathf.RoundToInt(pos.x) + 128;
			int y = Mathf.RoundToInt(pos.y) + 128;
			bool flag = !GetScanLatitudeAndLongitude(x, y, out waypointLatitude, out waypointLongitude);
			CelestialBody celestialBody = ((!(DisplayVessel != null) || !(DisplayVessel.mainBody != null)) ? FlightGlobals.GetHomeBody() : DisplayVessel.mainBody);
			infoLabel.text = currentDisplayMode.localCoordinateInfoLabel + ":";
			string text = currentDisplayMode.LocalCoordinateInfo(DisplayVessel, centerLatitude, centerLongitude, waypointLatitude, waypointLongitude, flag);
			bodyText.text = "<color=#FFFFFF>" + Localizer.Format("#autoLOC_7001301", celestialBody.displayName) + "</color>";
			infoText.text = "<color=#FFFFFF>" + text + "</color>";
			centerText.text = "<color=#FFFFFF>" + KSPUtil.PrintCoordinates(centerLatitude, centerLongitude, singleLine: true, includeMinutes: true, includeSeconds: false) + "</color>";
			string text2 = (flag ? cacheAutoLOC_258912 : KSPUtil.PrintCoordinates(waypointLatitude, waypointLongitude, singleLine: true, includeMinutes: true, includeSeconds: false));
			targetText.text = "<color=#FFFFFF>" + text2 + "</color>";
		}
	}

	public void UpdateVisibility()
	{
		float alpha = gridRenderer.GetAlpha();
		float alpha2 = waypointRenderer.GetAlpha();
		if (alpha < gridAlphaTarget)
		{
			gridRenderer.SetAlpha(Mathf.Min(alpha + Time.deltaTime * visibilitySpeed, gridAlphaTarget));
		}
		else if (alpha > gridAlphaTarget)
		{
			gridRenderer.SetAlpha(Mathf.Max(alpha - Time.deltaTime * visibilitySpeed, gridAlphaTarget));
		}
		if (alpha2 < waypointAlphaTarget)
		{
			waypointRenderer.SetAlpha(Mathf.Min(alpha2 + Time.deltaTime * visibilitySpeed, waypointAlphaTarget));
		}
		else if (alpha2 > waypointAlphaTarget)
		{
			waypointRenderer.SetAlpha(Mathf.Max(alpha2 - Time.deltaTime * visibilitySpeed, waypointAlphaTarget));
		}
	}

	public void SetupModeButton(List<KerbNetMode> modes)
	{
		if (modes != null && modes.Count > 0)
		{
			modeButton.gameObject.SetActive(value: true);
			List<ButtonState> list = new List<ButtonState>(modes.Count);
			int i = 0;
			for (int count = modes.Count; i < count; i++)
			{
				KerbNetMode kerbNetMode = modes[i];
				if (kerbNetMode.buttonSprite == null)
				{
					Debug.Log("KerbNetDialog - Unable to add mode " + kerbNetMode.name + " (" + kerbNetMode.GetType().Name + ") with a null sprite");
				}
				else
				{
					ButtonState buttonState = new ButtonState();
					buttonState.name = kerbNetMode.displayName;
					buttonState.normal = kerbNetMode.buttonSprite;
					list.Add(buttonState);
				}
			}
			modeButton.states = list.ToArray();
			modeButton.SetState(modes[0].displayName, invokeChange: false);
		}
		else
		{
			modeButton.gameObject.SetActive(value: false);
			modeButton.states = new ButtonState[1]
			{
				new ButtonState
				{
					name = "error",
					normal = modeErrorButtonSprite
				}
			};
			modeButton.SetState(0, invokeChange: false);
		}
	}

	public void RefreshCustomButton()
	{
		customButton.onClick.RemoveAllListeners();
		if (currentDisplayMode == null)
		{
			customButton.gameObject.SetActive(value: false);
			return;
		}
		customButton.gameObject.SetActive(currentDisplayMode.customButtonCallback != null);
		if (customButton.IsActive())
		{
			customButton.onClick.AddListener(currentDisplayMode.customButtonCallback);
			customButtonText.text = currentDisplayMode.customButtonCaption;
			customButtonTooltip.textString = currentDisplayMode.customButtonTooltip;
		}
	}

	public void SetFoVBounds(float min, float max)
	{
		fovMin = min;
		fovMax = max;
		UpdateFoVSlider(center: false);
	}

	public void UpdateFoVSlider(bool center)
	{
		fovSlider.minValue = fovMin;
		fovSlider.maxValue = fovMax;
		if (center)
		{
			fovCurrent = (fovMin + fovMax) / 2f;
			fovSlider.value = fovCurrent;
		}
		else
		{
			fovCurrent = fovSlider.value;
		}
		SetFoVText();
	}

	public void SetFoVText()
	{
		fovText.text = Localizer.Format("#autoLOC_437761", fovCurrent.ToString("N2"));
	}

	public void OnPartDestroyed(Part part)
	{
		if (isDisplaying && DisplayVessel != null && part != null && part.vessel == DisplayVessel)
		{
			delayedErrorRefresh = true;
		}
	}

	public void OnVesselStatusChanged(Vessel vessel, bool data)
	{
		if (isDisplaying && DisplayVessel != null && vessel == DisplayVessel)
		{
			delayedErrorRefresh = true;
		}
	}

	public void OnModeChanged(UIStateButton button)
	{
		string currentState = button.currentState;
		KerbNetMode modeByName = GetModeByName(currentState);
		if (currentDisplayMode != modeByName)
		{
			ActivateDisplayMode(modeByName);
			FullRefresh(refreshMap: true);
		}
	}

	public void OnVisibilityChanged(UIStateButton button)
	{
		string currentState = button.currentState;
		if (!(currentState == "fullVisibility"))
		{
			if (!(currentState == "halfVisibility"))
			{
				waypointAlphaTarget = 1f;
				gridAlphaTarget = 1f;
			}
			else
			{
				waypointAlphaTarget = 1f;
				gridAlphaTarget = 0f;
			}
		}
		else
		{
			waypointAlphaTarget = 0f;
			gridAlphaTarget = 0f;
		}
	}

	public void OnFoVSet(float value)
	{
		fovCurrent = value;
		SetFoVText();
	}

	public void OnWaypointFieldEndEdit(string name)
	{
		InputLocked = false;
	}

	public void OnRefreshClicked()
	{
		FullRefresh(refreshMap: true);
	}

	public void OnWaypointClicked()
	{
		InputLocked = false;
		if (!(ScenarioCustomWaypoints.Instance == null))
		{
			CelestialBody mainBody = DisplayVessel.mainBody;
			string value = waypointField.text.Trim();
			if (string.IsNullOrEmpty(value))
			{
				RandomizeWaypointField();
				value = waypointField.text.Trim();
			}
			Waypoint waypoint = new Waypoint
			{
				name = value,
				celestialName = mainBody.name,
				latitude = waypointLatitude,
				longitude = waypointLongitude
			};
			ScenarioCustomWaypoints.AddWaypoint(waypoint);
			ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_437851", waypoint.FullName, mainBody.displayName), 5f, ScreenMessageStyle.UPPER_CENTER);
			waypointTarget.Reset();
			RandomizeWaypointField();
		}
	}

	public static void SetupDisplayModes(List<string> askedDisplayModesNames)
	{
		if (Instance == null)
		{
			return;
		}
		Instance.activeDisplayModes.Clear();
		int i = 0;
		for (int count = askedDisplayModesNames.Count; i < count; i++)
		{
			string text = askedDisplayModesNames[i];
			KerbNetMode value;
			if (text == "Resources")
			{
				SetupAllResourceDisplayModes();
			}
			else if (knownDisplayModes.TryGetValue(text, out value) && !Instance.activeDisplayModes.Contains(value) && value.isModeActive(Instance.DisplayVessel))
			{
				Instance.activeDisplayModes.Add(value);
			}
		}
		Instance.ActivateDisplayMode((Instance.activeDisplayModes.Count > 0) ? Instance.activeDisplayModes[0] : null);
	}

	public static void SetupAllResourceDisplayModes()
	{
		int i = 0;
		for (int count = resourceDisplayModes.Count; i < count; i++)
		{
			KerbNetMode kerbNetMode = resourceDisplayModes[i];
			if (!Instance.activeDisplayModes.Contains(kerbNetMode) && kerbNetMode.isModeActive(Instance.DisplayVessel))
			{
				Instance.activeDisplayModes.Add(kerbNetMode);
			}
		}
	}

	public KerbNetMode GetModeByName(string modeName)
	{
		int num = 0;
		int count = activeDisplayModes.Count;
		KerbNetMode kerbNetMode;
		while (true)
		{
			if (num < count)
			{
				kerbNetMode = activeDisplayModes[num];
				if (!(kerbNetMode.name == modeName))
				{
					if (kerbNetMode.displayName == modeName)
					{
						break;
					}
					num++;
					continue;
				}
				return kerbNetMode;
			}
			return null;
		}
		return kerbNetMode;
	}

	public static void GenerateAssemblyModes()
	{
		AssemblyLoader.loadedAssemblies.TypeOperation(delegate(Type t)
		{
			if (t.IsSubclassOf(typeof(KerbNetMode)))
			{
				KerbNetMode kerbNetMode = (KerbNetMode)Activator.CreateInstance(t);
				if (kerbNetMode.AutoGenerateMode())
				{
					AddKnownMode(kerbNetMode);
				}
			}
		});
	}

	public static void GenerateResourceModes()
	{
		if (!(ResourceMap.Instance == null))
		{
			List<string> list = ResourceMap.Instance.FetchAllResourceNames(HarvestTypes.Planetary);
			int i = 0;
			for (int count = list.Count; i < count; i++)
			{
				KerbNetModeResource kerbNetModeResource = new KerbNetModeResource(list[i]);
				resourceDisplayModes.Add(kerbNetModeResource);
				AddKnownMode(kerbNetModeResource);
			}
		}
	}

	public static void GenerateKnownModes()
	{
		if (knownDisplayModes == null)
		{
			resourceDisplayModes = new List<KerbNetMode>();
			knownDisplayModes = new Dictionary<string, KerbNetMode>();
			GenerateResourceModes();
			GenerateAssemblyModes();
		}
	}

	public static void AddKnownMode(KerbNetMode mode)
	{
		mode.Init();
		if (knownDisplayModes.TryGetValue(mode.name, out var value))
		{
			Debug.Log("[KerbNet] Unable to add mode " + mode.name + " (" + mode.GetType().Name + ") with the same name as an other added mode " + value.GetType().Name + " from " + value.GetType().Assembly.GetName().Name);
			return;
		}
		if (mode.buttonSprite == null)
		{
			mode.buttonSprite = Instance.modeErrorButtonSprite;
		}
		Debug.Log("[KerbNet] Adding mode " + mode.name + " (" + mode.GetType().Name + ") from " + mode.GetType().Assembly.GetName().Name);
		knownDisplayModes.Add(mode.name, mode);
	}

	public void ActivateDisplayMode(KerbNetMode mode)
	{
		DeactivateDisplayMode(currentDisplayMode);
		currentDisplayMode = mode;
		if (mode != null)
		{
			modeButtonRenderer.SetColor(currentDisplayMode.GetModeColorTint());
			modeButton.Button.colors = currentDisplayMode.GetModeColorTintBlock();
			modeButtonText.text = currentDisplayMode.GetModeCaption();
			RefreshCustomButton();
			currentDisplayMode.OnActivated();
		}
	}

	public void DeactivateDisplayMode(KerbNetMode mode)
	{
		if (currentDisplayMode != null && mode == currentDisplayMode)
		{
			currentDisplayMode.OnDeactivated();
			currentDisplayMode = null;
		}
	}

	public void OnAutoRefreshChanged(UIStateButton button)
	{
		StopAutoRefresh();
		float num = -1f;
		string currentState = button.currentState;
		if (!(currentState == "refreshSlow"))
		{
			if (currentState == "refreshFast")
			{
				num = GameSettings.KERBNET_REFRESH_FAST_INTERVAL;
			}
		}
		else
		{
			num = GameSettings.KERBNET_REFRESH_SLOW_INTERVAL;
		}
		if (num > 0f)
		{
			ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_438022", num), 2.5f, ScreenMessageStyle.UPPER_CENTER);
			StartAutoRefresh(num);
		}
		else
		{
			ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_438026"), 2.5f, ScreenMessageStyle.UPPER_CENTER);
		}
	}

	public void StartAutoRefresh(float duration)
	{
		if (duration > 0f)
		{
			autoRefreshRoutine = StartCoroutine(AutoRefreshRoutine(duration));
		}
	}

	public void StopAutoRefresh(string notification = null)
	{
		if (autoRefreshRoutine != null)
		{
			if (notification != null)
			{
				ScreenMessages.PostScreenMessage(notification, 2.5f, ScreenMessageStyle.UPPER_CENTER);
			}
			StopCoroutine(autoRefreshRoutine);
			autoRefreshRoutine = null;
		}
	}

	public IEnumerator AutoRefreshRoutine(float duration)
	{
		while ((bool)this && !(duration <= 0f))
		{
			yield return new WaitForSecondsRealtime(duration);
			FullRefresh(refreshMap: true);
		}
	}

	public void UpdateCameraVectors()
	{
		CelestialBody mainBody = DisplayVessel.mainBody;
		Vector3d worldPosition = DisplayVessel.vesselTransform.position;
		localVesselPos = mainBody.GetRelSurfacePosition(worldPosition);
		cameraZ = -localVesselPos.normalized;
		if (GameSettings.KERBNET_ALIGNS_WITH_ORBIT)
		{
			Vector3d xzy = DisplayVessel.orbit.GetOrbitNormal().xzy;
			cameraX = -mainBody.GetRelSurfaceDirection(xzy);
			cameraY = Vector3d.Cross(cameraZ, cameraX).normalized;
		}
		else
		{
			cameraX = Vector3d.Cross(Vector3d.up, cameraZ).normalized;
			if (cameraX.sqrMagnitude == 0.0)
			{
				cameraX = Vector3d.right;
			}
			cameraY = Vector3d.Cross(cameraZ, cameraX);
		}
		fovScale = Math.Tan(Math.Max(1.0, Mathf.Min(fovCurrent, 179.999f)) * (Math.PI / 180.0) / 2.0);
	}

	public void RefreshMap()
	{
		UpdateCameraVectors();
		RawImage component = mapTexture.GetComponent<RawImage>();
		Texture2D texture2D = CreateMapTexture();
		if (texture2D != null)
		{
			component.texture = texture2D;
		}
	}

	public bool GetScanLatitudeAndLongitude(int x, int y, out double latitude, out double longitude)
	{
		if (!(DisplayVessel == null) && !(DisplayVessel.mainBody == null))
		{
			double num = ((double)x - 128.0) / 128.0 * fovScale;
			double num2 = ((double)y - 128.0) / 128.0 * fovScale;
			Vector3d velocity = cameraZ + num * cameraX + num2 * cameraY;
			return DisplayVessel.mainBody.GetImpactLatitudeAndLongitude(localVesselPos, velocity, out latitude, out longitude);
		}
		latitude = 0.0;
		longitude = 0.0;
		return false;
	}

	public bool ScanSpaceLocation(Vector3d pos, out Vector2 cam)
	{
		Vector3d normalized = pos.normalized;
		Vector3d lhs = pos - localVesselPos;
		if (Vector3d.Dot(lhs, normalized) <= 0.0)
		{
			double num = Vector3d.Dot(lhs, cameraX);
			double num2 = Vector3d.Dot(lhs, cameraY);
			double num3 = Vector3d.Dot(lhs, cameraZ);
			if (num3 >= 1.0)
			{
				double num4 = 1.0 / (num3 * fovScale);
				cam = new Vector2((float)(num * num4), (float)(num2 * num4));
				if (cam.x * cam.x <= 1f)
				{
					return cam.y * cam.y <= 1f;
				}
				return false;
			}
		}
		cam = new Vector2(0f, 0f);
		return false;
	}

	public Texture2D CreateMapTexture()
	{
		CelestialBody mainBody = DisplayVessel.mainBody;
		if (mainBody == null)
		{
			return null;
		}
		bool flag = mainBody == Planetarium.fetch.Sun;
		Texture2D texture = GetTexture();
		texture.filterMode = FilterMode.Trilinear;
		currentDisplayMode.Precache(DisplayVessel);
		for (int i = 0; i < 257; i++)
		{
			for (int j = 0; j < 257; j++)
			{
				if (j % 4 == 0 && i % 4 == 0)
				{
					double latitude;
					double longitude;
					Color color = ((!GetScanLatitudeAndLongitude(j, i, out latitude, out longitude)) ? currentDisplayMode.GetBackgroundColor(j, i) : ((!currentDisplayMode.doCoordinatePass) ? (flag ? Color.white : Color.black) : currentDisplayMode.GetCoordinateColor(DisplayVessel, latitude, longitude)));
					texture.SetPixel(j, i, color);
				}
			}
		}
		currentDisplayMode.InterpolateMainTexture(texture);
		if (currentDisplayMode.doTerrainContourPass)
		{
			DrawContourLines(texture);
		}
		if (currentDisplayMode.doAnomaliesPass)
		{
			DrawAnomaliesOnMap(texture);
		}
		if (currentDisplayMode.doCustomPass)
		{
			currentDisplayMode.CustomPass(texture);
		}
		texture.Apply(updateMipmaps: false, makeNoLongerReadable: false);
		return texture;
	}

	public void DrawContourLines(Texture2D tex)
	{
		double num = 1000000.0;
		double num2 = 0.0;
		double num3 = 1000000.0;
		double num4 = 0.0;
		for (int i = 0; i < 257; i++)
		{
			for (int j = 0; j < 257; j++)
			{
				if (j % 10 == 0 && i % 10 == 0 && GetScanLatitudeAndLongitude(j, i, out var latitude, out var longitude))
				{
					double num5 = CelestialUtilities.TerrainAltitude(DisplayVessel.mainBody, latitude, longitude, underwater: true);
					if (num5 > num2)
					{
						num2 = num5;
					}
					if (num5 < num)
					{
						num = num5;
					}
				}
			}
		}
		num2 = Math.Max(num2 * (double)currentDisplayMode.terrainContourThreshold, num);
		double num6 = num2 - num;
		currentDisplayMode.GetTerrainContourColors(DisplayVessel, out var lowColor, out var highColor);
		for (int k = 0; k < 257; k++)
		{
			for (int l = 0; l < 257; l++)
			{
				if (l % 4 == 0 && k % 4 == 0 && GetScanLatitudeAndLongitude(l, k, out var latitude2, out var longitude2))
				{
					double num7 = CelestialUtilities.TerrainAltitude(DisplayVessel.mainBody, latitude2, longitude2);
					float num8 = ((!DisplayVessel.mainBody.ocean || !(num7 <= 0.0)) ? ((float)((num7 - num) * (1.0 / num6))) : 0f);
					tex.SetPixel(l, k, Color.Lerp(lowColor, highColor, num8));
					if ((double)num8 < num3)
					{
						num3 = num8;
					}
					if ((double)num8 > num4)
					{
						num4 = num8;
					}
				}
			}
		}
		currentDisplayMode.InterpolateContourTexture(tex);
	}

	public void DrawAnomaliesOnMap(Texture2D tex)
	{
		if (DisplayVessel == null || !(AnomalyChance > 0f))
		{
			return;
		}
		CelestialBody mainBody = DisplayVessel.mainBody;
		if (mainBody == null || mainBody.pqsSurfaceObjects == null)
		{
			return;
		}
		int i = 0;
		for (int num = mainBody.pqsSurfaceObjects.Length; i < num; i++)
		{
			PQSSurfaceObject pQSSurfaceObject = mainBody.pqsSurfaceObjects[i];
			if (pQSSurfaceObject == null || !ShouldShowAnomaly(pQSSurfaceObject, AnomalyChance))
			{
				continue;
			}
			Vector3d relSurfacePosition = mainBody.GetRelSurfacePosition(pQSSurfaceObject.transform.position);
			if (!ScanSpaceLocation(relSurfacePosition, out var cam))
			{
				continue;
			}
			int num2 = (int)(cam.x * 128f) + 128;
			int num3 = (int)(cam.y * 128f) + 128;
			for (int j = 0; j < 13; j++)
			{
				for (int k = 0; k < 13; k++)
				{
					byte b = questionArray[k, j];
					int x = num2 + j - 6;
					int y = num3 + k - 6;
					switch (b)
					{
					case 1:
						tex.SetPixel(x, y, Color.black);
						break;
					case 2:
						tex.SetPixel(x, y, Color.white);
						break;
					}
				}
			}
		}
	}

	public bool HasActiveConnection()
	{
		if (!CommNetScenario.CommNetEnabled)
		{
			return true;
		}
		if (!(DisplayVessel == null) && !(DisplayVessel.connection == null))
		{
			return DisplayVessel.connection.IsConnectedHome;
		}
		return false;
	}

	public bool HasVesselControl()
	{
		if (DisplayVessel != null)
		{
			return DisplayVessel.IsControllable;
		}
		return false;
	}

	public bool HasValidDisplayMode(out string error)
	{
		if (currentDisplayMode == null)
		{
			error = cacheAutoLOC_438370;
			return false;
		}
		error = currentDisplayMode.GetErrorState();
		return error == null;
	}

	public bool HasValidAccessor(out string error)
	{
		if (KerbNetAccessor == null)
		{
			error = cacheAutoLOC_438382;
			return false;
		}
		Part kerbNetPart = KerbNetAccessor.GetKerbNetPart();
		if (!(kerbNetPart == null) && kerbNetPart.State != PartStates.DEAD)
		{
			error = KerbNetAccessor.GetKerbNetErrorState();
			return error == null;
		}
		error = cacheAutoLOC_438390;
		return false;
	}

	public void RefreshErrorState(IAccessKerbNet accessor = null)
	{
		if (accessor != null && accessor != KerbNetAccessor)
		{
			return;
		}
		bool hasError = HasError;
		ErrorState = null;
		string error = null;
		if (!HasActiveConnection())
		{
			ErrorState = cacheAutoLOC_438413;
		}
		else if (!HasVesselControl())
		{
			ErrorState = cacheAutoLOC_438415;
		}
		else if (!HasValidDisplayMode(out error))
		{
			ErrorState = error;
		}
		else if (!HasValidAccessor(out error))
		{
			ErrorState = error;
		}
		HasError = !string.IsNullOrEmpty(ErrorState);
		if (HasError && errorText != null)
		{
			errorText.text = ErrorState;
		}
		if (HasError != hasError)
		{
			SetActivesForError(HasError);
			if (HasError)
			{
				StopAutoRefresh(cacheAutoLOC_6001959);
				autoRefreshButton.SetState("refreshNone", invokeChange: false);
			}
			else if (RecoveryRefreshQueued)
			{
				FullRefresh(refreshMap: true, refreshErrors: false);
			}
		}
	}

	public void SetActivesForError(bool error)
	{
		for (int num = disableOnError.Length - 1; num >= 0; num--)
		{
			disableOnError[num].SetActive(!error);
		}
		for (int num2 = enableOnError.Length - 1; num2 >= 0; num2--)
		{
			enableOnError[num2].SetActive(error);
		}
	}

	public static void ChangeMapPosition(Vector2 pos)
	{
		Instance.RefreshDataLabels(pos);
	}

	public static void ResetMapPosition(bool showDragTip = true)
	{
		Instance.RefreshDataLabels(new Vector2(0f, 0f));
		if (showDragTip)
		{
			Instance.targetText.text = cacheAutoLOC_438467;
		}
	}

	public void RandomizeWaypointField()
	{
		string text = StringUtilities.GenerateSiteName(Generator.Next(), DisplayVessel.mainBody, landLocked: false);
		waypointField.text = Localizer.Format("#autoLOC_7001301", text);
	}

	public bool ShouldShowAnomaly(PQSSurfaceObject anomaly, float chance)
	{
		int seed = HighLogic.CurrentGame.Seed;
		int hashCode_Net = anomaly.name.GetHashCode_Net35();
		int num = (seed + hashCode_Net) * (seed + hashCode_Net + 1) / 2 + hashCode_Net;
		KSPRandom kSPRandom = new KSPRandom(num);
		int day = KSPUtil.dateTimeFormatter.Day;
		int num2 = kSPRandom.Next(day);
		int num3 = (int)Math.Floor((Planetarium.GetUniversalTime() + (double)num2) / (double)day);
		num = (num + num3) * (num + num3 + 1) / 2 + num3;
		return Convert.ToSingle(new KSPRandom(num).NextDouble()) < chance;
	}

	public static float NormalizedDistanceFromCenter(int x, int y)
	{
		float value = (128 - x) * (128 - x) + (128 - y) * (128 - y);
		float b = 32768f;
		return Mathf.InverseLerp(0f, b, value);
	}

	public static void CacheLocalStrings()
	{
		cacheAutoLOC_438370 = Localizer.Format("#autoLOC_438370");
		cacheAutoLOC_438382 = Localizer.Format("#autoLOC_438382");
		cacheAutoLOC_438390 = Localizer.Format("#autoLOC_438390");
		cacheAutoLOC_438413 = Localizer.Format("#autoLOC_438413");
		cacheAutoLOC_438415 = Localizer.Format("#autoLOC_438415");
		cacheAutoLOC_438467 = Localizer.Format("#autoLOC_438467");
		cacheAutoLOC_6001959 = Localizer.Format("#autoLOC_6001959");
		cacheAutoLOC_258912 = Localizer.Format("#autoLOC_258912").ToUpper();
	}
}
