using UnityEngine;

public class LocalParticleGravity : MonoBehaviour
{
	public float gravScale = 1f;

	public void Start()
	{
		ParticleSystem component = GetComponent<ParticleSystem>();
		if (component != null)
		{
			ParticleSystem.ForceOverLifetimeModule forceOverLifetime = component.forceOverLifetime;
			Vector3 vector = ((!(FlightGlobals.fetch != null)) ? (Vector3.down * (float)PhysicsGlobals.GravitationalAcceleration * gravScale) : ((Vector3)(FlightGlobals.getGeeForceAtPosition(base.transform.position) * gravScale)));
			forceOverLifetime.randomized = false;
			forceOverLifetime.x = new ParticleSystem.MinMaxCurve(0f);
			forceOverLifetime.y = new ParticleSystem.MinMaxCurve(0f - vector.magnitude);
			forceOverLifetime.z = new ParticleSystem.MinMaxCurve(0f);
			forceOverLifetime.enabled = true;
		}
	}
}
