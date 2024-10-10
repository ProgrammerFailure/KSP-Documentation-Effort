using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class FXPrefab : MonoBehaviour
{
	public ParticleSystem particleSystem;

	public static Vector3 currentKrakensbaneFixedDelta;

	public void Awake()
	{
		particleSystem = GetComponent<ParticleSystem>();
	}

	public void OnEnable()
	{
		FloatingOrigin.RegisterParticleSystem(particleSystem);
	}

	public void OnDisable()
	{
		FloatingOrigin.UnregisterParticleSystem(particleSystem);
	}
}
