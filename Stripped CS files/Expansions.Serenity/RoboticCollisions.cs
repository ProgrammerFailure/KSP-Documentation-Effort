using UnityEngine;

namespace Expansions.Serenity;

public class RoboticCollisions : MonoBehaviour
{
	public Part part;

	public void Start()
	{
		if (!ExpansionsLoader.IsExpansionInstalled("Serenity") && HighLogic.LoadedSceneIsGame)
		{
			base.enabled = false;
			Object.Destroy(this);
		}
	}

	public void OnCollisionEnter(Collision other)
	{
		if (part != null)
		{
			part.OnCollisionEnter(other);
		}
	}

	public void OnCollisionExit(Collision other)
	{
		if (part != null)
		{
			part.OnCollisionExit(other);
		}
	}

	public void OnTriggerEnter(Collider other)
	{
		if (part != null)
		{
			part.OnTriggerEnter(other);
		}
	}

	public void OnTriggerExit(Collider other)
	{
		if (part != null)
		{
			part.OnTriggerExit(other);
		}
	}
}
