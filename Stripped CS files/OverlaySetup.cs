using UnityEngine;

public class OverlaySetup : MonoBehaviour
{
	public class MapConfig
	{
		public int MapResolution { get; set; }

		public int InterpolationLevel { get; set; }

		public float LoOpacity { get; set; }

		public float HiOpacity { get; set; }

		public float GridWeight { get; set; }

		public float LoColor { get; set; }

		public float HiColor { get; set; }

		public int CapPixels { get; set; }

		public int FilterMode { get; set; }
	}

	public static OverlaySetup instance;

	public static MapConfig _mapConfig;

	public static OverlaySetup Instance => instance ?? (instance = new GameObject("OverlaySetup").AddComponent<OverlaySetup>());

	public MapConfig OverlayConfig => _mapConfig ?? (_mapConfig = LoadOverlayConfig());

	public void OnDestroy()
	{
		if (instance != null && instance == this)
		{
			instance = null;
		}
	}

	public void ResetMapConfig()
	{
		_mapConfig = LoadOverlayConfig();
	}

	public MapConfig LoadOverlayConfig()
	{
		int overlayStyle = OverlayGenerator.Instance.OverlayStyle;
		string text = "";
		switch (overlayStyle)
		{
		default:
			OverlayGenerator.Instance.OverlayStyle = 1;
			text = "_LINES";
			break;
		case 1:
			text = "_LINES";
			break;
		case 2:
			text = "_DOTS";
			break;
		case 3:
			text = "_SOLID";
			break;
		}
		ConfigNode node = null;
		ConfigNode[] configNodes = GameDatabase.Instance.GetConfigNodes("RESOURCE_OVERLAY_CONFIGURATION" + text);
		if (configNodes.Length != 0)
		{
			node = configNodes[0];
		}
		return ResourceUtilities.LoadNodeProperties<MapConfig>(node);
	}
}
