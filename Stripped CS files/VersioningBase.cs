using UnityEngine;

public abstract class VersioningBase : MonoBehaviour
{
	public static VersioningBase instance;

	public static VersioningBase Instance
	{
		get
		{
			if (instance == null)
			{
				VersioningBase versioningBase = Object.FindObjectOfType<VersioningBase>();
				if (versioningBase != null)
				{
					instance = versioningBase;
					versioningBase.OnAwake();
				}
			}
			return instance;
		}
	}

	public VersioningBase()
	{
	}

	public void Awake()
	{
		if (instance != null)
		{
			if (instance != this)
			{
				Object.Destroy(this);
			}
		}
		else
		{
			instance = this;
			OnAwake();
		}
	}

	public void OnDestroy()
	{
		if (instance != null && instance == this)
		{
			instance = null;
		}
	}

	public abstract void OnAwake();

	public static string GetVersionString()
	{
		if (!(instance != null))
		{
			return "0.0.0";
		}
		return instance.GetVersion();
	}

	public abstract string GetVersion();
}
