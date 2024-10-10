using ns2;
using UnityEngine;

namespace ns36;

public class MapViewPanelTransition : MonoBehaviour
{
	public bool inDuringMap = true;

	public UIPanelTransition panel;

	public void Reset()
	{
		panel = GetComponent<UIPanelTransition>();
	}

	public void Awake()
	{
		GameEvents.OnMapEntered.Add(SetModeMapOn);
		GameEvents.OnMapExited.Add(SetModeMapOff);
		if (panel == null)
		{
			panel = GetComponent<UIPanelTransition>();
		}
	}

	public void OnDestroy()
	{
		GameEvents.OnMapEntered.Remove(SetModeMapOn);
		GameEvents.OnMapExited.Remove(SetModeMapOff);
	}

	public void SetModeMapOn()
	{
		panel.Transition(inDuringMap ? "In" : "Out");
	}

	public void SetModeMapOff()
	{
		panel.Transition((!inDuringMap) ? "In" : "Out");
	}
}
