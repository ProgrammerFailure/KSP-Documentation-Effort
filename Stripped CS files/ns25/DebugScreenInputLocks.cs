using TMPro;
using UnityEngine;

namespace ns25;

public class DebugScreenInputLocks : MonoBehaviour
{
	public TextMeshProUGUI currentLocks;

	public TextMeshProUGUI bitMask;

	public static DebugScreenInputLocks Instance { get; set; }

	public void Awake()
	{
		Instance = this;
	}

	public void OnDestroy()
	{
		Instance = null;
	}
}
