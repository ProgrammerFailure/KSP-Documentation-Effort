using UnityEngine;

namespace ns2;

[RequireComponent(typeof(CanvasGroup))]
public class CanvasGroupEventLocker : MonoBehaviour
{
	public CanvasGroup canvasGroup;

	[SerializeField]
	public bool lockWhileKSPediaOpen;

	public void Awake()
	{
		canvasGroup = GetComponent<CanvasGroup>();
		if (lockWhileKSPediaOpen)
		{
			GameEvents.onGUIKSPediaSpawn.Add(Lock);
			GameEvents.onGUIKSPediaDespawn.Add(Unlock);
		}
	}

	public void OnDestroy()
	{
		if (lockWhileKSPediaOpen)
		{
			GameEvents.onGUIKSPediaSpawn.Remove(Lock);
			GameEvents.onGUIKSPediaDespawn.Remove(Unlock);
		}
	}

	public void Lock()
	{
		canvasGroup.blocksRaycasts = false;
	}

	public void Unlock()
	{
		canvasGroup.blocksRaycasts = true;
	}
}
