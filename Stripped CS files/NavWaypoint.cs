using FinePrint;
using FinePrint.Utilities;
using ns36;
using ns9;
using UnityEngine;

public class NavWaypoint : MonoBehaviour
{
	public const float ghostOpacity = 0.3f;

	public const string textureKeyword = "_MainTexture";

	public const string colorKeyword = "_TintColor";

	public const string opacityKeyword = "_Opacity";

	public const string copyWaypointName = "ProgradeVector";

	public bool showGhosted;

	public static NavWaypoint _fetch;

	public NavBall _cachedNavBall;

	public Renderer _cachedRenderer;

	public GameObject Visual { get; set; }

	public CelestialBody Body { get; set; }

	public double Latitude { get; set; }

	public double Longitude { get; set; }

	public double Height { get; set; }

	public double Altitude { get; set; }

	public string TextureID { get; set; }

	public Color Color { get; set; }

	public bool IsActive { get; set; }

	public bool IsBlinking { get; set; }

	public static NavWaypoint fetch
	{
		get
		{
			if (_fetch != null)
			{
				return _fetch;
			}
			_fetch = new GameObject("NavWaypoint").AddComponent<NavWaypoint>();
			return _fetch;
		}
	}

	public NavBall cachedNavBall
	{
		get
		{
			if (_cachedNavBall != null)
			{
				return _cachedNavBall;
			}
			_cachedNavBall = Object.FindObjectOfType<NavBall>();
			return _cachedNavBall;
		}
	}

	public Renderer cachedRenderer
	{
		get
		{
			if (Visual == null)
			{
				return null;
			}
			if (_cachedRenderer != null)
			{
				return _cachedRenderer;
			}
			_cachedRenderer = Visual.GetComponent<Renderer>() ?? Visual.AddComponent<Renderer>();
			if (_cachedRenderer.material != null)
			{
				_cachedRenderer.material.EnableKeyword("_MainTexture");
				_cachedRenderer.material.EnableKeyword("_TintColor");
				_cachedRenderer.material.EnableKeyword("_Opacity");
			}
			return _cachedRenderer;
		}
	}

	public Vector3 NavigationVector
	{
		get
		{
			if (!(FlightGlobals.fetch == null) && !(FlightGlobals.fetch.activeVessel == null) && !(Body == null))
			{
				Vector3d worldSurfacePosition = Body.GetWorldSurfacePosition(Latitude, Longitude, Height + Altitude);
				Vector3 vector = new Vector3((float)worldSurfacePosition.x, (float)worldSurfacePosition.y, (float)worldSurfacePosition.z);
				Vector3 vector2 = FlightGlobals.fetch.activeVessel.GetWorldPos3D();
				return (vector - vector2).normalized;
			}
			return Vector3.zero;
		}
	}

	public void Awake()
	{
		Body = Planetarium.fetch.Home;
		Latitude = 0.0;
		Longitude = 0.0;
		Altitude = 0.0;
		TextureID = "default";
		Color = XKCDColors.KSPBadassGreen;
		IsBlinking = false;
		IsActive = false;
		if (cachedNavBall == null)
		{
			return;
		}
		Component[] componentsInChildren = cachedNavBall.transform.parent.GetComponentsInChildren(typeof(Transform), includeInactive: true);
		GameObject gameObject = null;
		int num = componentsInChildren.Length;
		while (num-- > 0)
		{
			Transform transform = componentsInChildren[num] as Transform;
			if (!(transform == null) && !(transform.gameObject.name != "ProgradeVector"))
			{
				gameObject = transform.gameObject;
				break;
			}
		}
		if (!(gameObject == null))
		{
			Visual = Object.Instantiate(gameObject.gameObject);
			Visual.name = base.name + "Visual";
			if (cachedRenderer != null)
			{
				cachedRenderer.material = new Material(cachedRenderer.material);
				cachedRenderer.material.SetTextureOffset("_MainTexture", new Vector2(0f, 0f));
				cachedRenderer.material.SetTextureScale("_MainTexture", new Vector2(1f, 1f));
			}
			Visual.transform.parent = gameObject.transform.parent;
			Visual.transform.localPosition = Vector3.zero;
			Visual.transform.localRotation = Quaternion.Euler(90f, 180f, 0f);
			Visual.transform.localScale = new Vector3(33f, 33f, 33f);
			Clear();
			Deactivate();
			Visual.SetActive(value: false);
			UpdateShowGhosted();
			GameEvents.OnGameSettingsApplied.Add(OnGameSettingsApplied);
		}
	}

	public void OnGameSettingsApplied()
	{
		UpdateShowGhosted();
	}

	public void UpdateShowGhosted()
	{
		showGhosted = ContractDefs.SurveyNavigationGhosting || GameSettings.NAVIGATION_GHOSTING;
		if (HighLogic.CurrentGame != null && (HighLogic.CurrentGame.Mode == Game.Modes.MISSION || HighLogic.CurrentGame.Mode == Game.Modes.MISSION_BUILDER))
		{
			showGhosted = GameSettings.MISSION_NAVIGATION_GHOSTING || showGhosted;
		}
	}

	public void OnDestroy()
	{
		if (Visual != null)
		{
			Object.Destroy(Visual);
		}
		if (_fetch != null && _fetch == this)
		{
			_fetch = null;
		}
		GameEvents.OnGameSettingsApplied.Remove(OnGameSettingsApplied);
	}

	public void LateUpdate()
	{
		if (SystemUtilities.FlightIsReady(checkVessel: true) && !(cachedNavBall == null) && !(Visual == null))
		{
			UpdateVisualPosition();
			UpdateVisualAppearance();
		}
		else if (Visual != null)
		{
			Visual.SetActive(value: false);
		}
	}

	public void UpdateVisualPosition()
	{
		Visual.transform.localPosition = cachedNavBall.attitudeGymbal * (NavigationVector * cachedNavBall.VectorUnitScale);
		Visual.transform.rotation = Quaternion.Euler(90f, 180f, 0f);
	}

	public void UpdateVisualAppearance()
	{
		if (!(cachedRenderer == null))
		{
			Visual.SetActive(IsVisible(showGhosted || Visual.transform.localPosition.z >= cachedNavBall.VectorUnitCutoff));
			if (Visual.activeSelf)
			{
				float value = Mathf.Clamp(Vector3.Dot(Visual.transform.localPosition.normalized, Vector3.forward), showGhosted ? 0.3f : 0f, 1f);
				cachedRenderer.material.SetFloat("_Opacity", value);
			}
		}
	}

	public bool IsVisible(bool condition = true)
	{
		if (IsActive && condition && SystemUtilities.FlightIsReady(checkVessel: true, Body))
		{
			float num = 1f;
			if (IsBlinking)
			{
				num = Mathf.PingPong(Time.time * 2f, 1f) / 1f;
			}
			return num > 0.5f;
		}
		return false;
	}

	public void SetMaterialTexture(Texture2D texture)
	{
		if (cachedRenderer != null)
		{
			cachedRenderer.material.SetTexture("_MainTexture", texture);
		}
	}

	public void SetMaterialTexture(string textureID)
	{
		if (ContractDefs.sprites != null)
		{
			Texture2D texture = ContractDefs.sprites[textureID].texture;
			if (texture != null)
			{
				SetMaterialTexture(texture);
			}
		}
	}

	public void SetMaterialColor(Color color)
	{
		if (cachedRenderer != null)
		{
			cachedRenderer.material.SetColor("_TintColor", color);
		}
	}

	public void Activate()
	{
		IsActive = true;
	}

	public void Deactivate()
	{
		IsActive = false;
	}

	public static void DeactivateIfWaypoint(Waypoint waypoint)
	{
		if (!(fetch == null) && fetch.IsActive && fetch.IsUsing(waypoint))
		{
			ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_282620"), 2.5f, ScreenMessageStyle.UPPER_CENTER);
			fetch.Deactivate();
		}
	}

	public void Setup(CelestialBody body, double latitude, double longitude, double altitude, string texture, Color color)
	{
		if (HighLogic.LoadedSceneIsFlight)
		{
			TextureID = texture;
			Color = color;
			SetMaterialTexture(TextureID);
			SetMaterialColor(Color);
			Latitude = latitude;
			Longitude = longitude;
			Altitude = altitude;
			if (!(body == null))
			{
				Body = body;
				Height = CelestialUtilities.TerrainAltitude(body, latitude, longitude);
			}
		}
	}

	public void Setup(Waypoint waypoint)
	{
		if (waypoint.seed == -1)
		{
			Setup(waypoint.celestialBody, waypoint.latitude, waypoint.longitude, waypoint.altitude, waypoint.id, XKCDColors.KSPYellowishGreen);
		}
		else
		{
			Setup(waypoint.celestialBody, waypoint.latitude, waypoint.longitude, waypoint.altitude, waypoint.id, SystemUtilities.RandomColor(waypoint.seed, 1f, 1f, 1f));
		}
		Vessel activeVessel = FlightGlobals.fetch.activeVessel;
		if (activeVessel != null)
		{
			activeVessel.navigationWaypoint = waypoint;
		}
	}

	public void Clear()
	{
		Setup(Planetarium.fetch.Sun, 500.0, 500.0, 0.0, "default", new Color(0f, 0f, 0f));
		Vessel activeVessel = FlightGlobals.fetch.activeVessel;
		if (activeVessel != null)
		{
			activeVessel.navigationWaypoint = null;
		}
	}

	public bool IsUsing(double latitude, double longitude, double altitude)
	{
		if (Latitude == latitude && Longitude == longitude)
		{
			return Altitude == altitude;
		}
		return false;
	}

	public bool IsUsing(Waypoint waypoint)
	{
		return IsUsing(waypoint.latitude, waypoint.longitude, waypoint.altitude);
	}
}
