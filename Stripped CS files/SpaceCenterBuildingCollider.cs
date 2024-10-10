using UnityEngine;

public class SpaceCenterBuildingCollider : MonoBehaviour
{
	public SpaceCenterBuilding building;

	public bool destroyGameObject;

	public void OnDestroy()
	{
		GameEvents.onGameSceneLoadRequested.Remove(OnGameSceneChange);
	}

	public void Setup(SpaceCenterBuilding bld, bool ownGameObject)
	{
		building = bld;
		destroyGameObject = ownGameObject;
		GameEvents.onGameSceneLoadRequested.Add(OnGameSceneChange);
	}

	public void OnGameSceneChange(GameScenes scene)
	{
		GameEvents.onGameSceneLoadRequested.Remove(OnGameSceneChange);
		if (destroyGameObject && base.gameObject != null)
		{
			Object.Destroy(base.gameObject);
		}
		else
		{
			Object.Destroy(this);
		}
	}

	public void OnMouseOver()
	{
		building.ColliderHover(hover: true);
	}

	public void OnMouseExit()
	{
		building.ColliderHover(hover: false);
	}
}
