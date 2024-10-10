using UnityEngine;

public class DetonatorDriver : MonoBehaviour
{
	public void Awake()
	{
		DetonatorSound.GetSoundVolume = GetDetonatorVolume;
	}

	public float GetDetonatorVolume()
	{
		return GameSettings.SHIP_VOLUME;
	}
}
