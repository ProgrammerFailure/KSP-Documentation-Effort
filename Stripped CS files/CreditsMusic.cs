using UnityEngine;

public class CreditsMusic : MonoBehaviour
{
	public void Awake()
	{
		GetComponent<AudioSource>().volume = GameSettings.MUSIC_VOLUME;
	}
}
