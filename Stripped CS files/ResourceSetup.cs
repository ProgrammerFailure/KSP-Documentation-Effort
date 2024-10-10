using UnityEngine;

public class ResourceSetup : MonoBehaviour
{
	public class ResourceConfig
	{
		public bool HeatEnabled { get; set; }

		public int ECMinScale { get; set; }

		public int OverlayStyle { get; set; }

		public bool ShowDebugOptions { get; set; }
	}

	public static ResourceSetup instance;

	public static ResourceConfig _resConfig;

	public static ResourceSetup Instance => instance ?? (instance = new GameObject("ResourceSetup").AddComponent<ResourceSetup>());

	public ResourceConfig ResConfig => _resConfig ?? (_resConfig = LoadResourceConfig());

	public void OnDestroy()
	{
		if (instance != null && instance == this)
		{
			instance = null;
		}
	}

	public ResourceConfig LoadResourceConfig()
	{
		if (GameDatabase.Instance.GetConfigNodes("RESOURCE_CONFIGURATION").Length == 0)
		{
			return null;
		}
		return ResourceUtilities.LoadNodeProperties<ResourceConfig>(GameDatabase.Instance.GetConfigNodes("RESOURCE_CONFIGURATION")[0]);
	}
}
