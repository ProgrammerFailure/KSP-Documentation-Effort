using System;
using UnityEngine;

namespace Expansions.Missions.Editor;

public class NodeListTooltipMasterController : MonoBehaviour
{
	[NonSerialized]
	public bool displayExtendedInfo;

	[NonSerialized]
	public NodeListTooltip currentTooltip;

	public static NodeListTooltipMasterController Instance { get; set; }

	public void Awake()
	{
		if (Instance != null)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
		else
		{
			Instance = this;
		}
	}

	public void OnDestroy()
	{
		if (Instance != null && Instance == this)
		{
			Instance = null;
		}
	}
}
