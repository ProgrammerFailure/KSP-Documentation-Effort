using UnityEngine;

public class PAsteroid : PSpaceObject
{
	public override void SetupPartParameters()
	{
		partComponent = base.gameObject.transform.parent.GetComponent<Part>();
		if (partComponent != null)
		{
			partComponent.ResetModelMeshRenderersCache();
			partComponent.ResetModelRenderersCache();
			partComponent.ResetModelSkinnedMeshRenderersCache();
		}
		switch (DiscoveryInfo.GetObjectClass(partComponent.vessel.DiscoveryInfo.size.Value))
		{
		default:
			partComponent.explosionPotential = 0.75f;
			partComponent.maxTemp = 4000.0;
			break;
		case UntrackedObjectClass.const_0:
			partComponent.explosionPotential = 0.15f;
			partComponent.maxTemp = 2000.0;
			break;
		case UntrackedObjectClass.const_1:
			partComponent.explosionPotential = 0.3f;
			partComponent.maxTemp = 2500.0;
			break;
		case UntrackedObjectClass.const_2:
			partComponent.explosionPotential = 0.45f;
			partComponent.maxTemp = 2750.0;
			break;
		case UntrackedObjectClass.const_3:
			partComponent.explosionPotential = 0.6f;
			partComponent.maxTemp = 3000.0;
			break;
		case UntrackedObjectClass.const_4:
			partComponent.explosionPotential = 0.75f;
			partComponent.maxTemp = 4000.0;
			break;
		case UntrackedObjectClass.const_5:
			partComponent.explosionPotential = 0.8f;
			partComponent.maxTemp = 4500.0;
			break;
		case UntrackedObjectClass.const_6:
			partComponent.explosionPotential = 0.85f;
			partComponent.maxTemp = 4750.0;
			break;
		case UntrackedObjectClass.const_7:
			partComponent.explosionPotential = 0.9f;
			partComponent.maxTemp = 5000.0;
			break;
		case UntrackedObjectClass.const_8:
			partComponent.explosionPotential = 0.95f;
			partComponent.maxTemp = 5500.0;
			break;
		}
		partComponent.asteroidCollider = soc as AsteroidCollider;
		partComponent.collider.gameObject.GetComponent<MeshRenderer>().enabled = false;
	}
}
