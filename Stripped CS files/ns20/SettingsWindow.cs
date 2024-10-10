using UnityEngine;

namespace ns20;

public class SettingsWindow : MonoBehaviour
{
	public SettingsTemplate templatePrefab;

	public SettingsTemplate Template { get; set; }

	public virtual bool IsValid()
	{
		return true;
	}
}
