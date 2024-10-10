using UnityEngine;

public class FlagPoleLaunchpad : MonoBehaviour
{
	public Animation flagAnimation;

	public string animationName = "animation";

	public Renderer flagRenderer;

	public Texture2D flagTexture;

	public void Start()
	{
		GameEvents.onFlagSelect.Add(OnFlagSelect);
		GameEvents.onGameSceneLoadRequested.Add(OnSceneChange);
		if (flagAnimation != null)
		{
			flagAnimation.Play(animationName);
		}
		OnSceneChange(GameScenes.LOADING);
	}

	public void OnDestroy()
	{
		GameEvents.onFlagSelect.Remove(OnFlagSelect);
		GameEvents.onGameSceneLoadRequested.Remove(OnSceneChange);
	}

	public void OnSceneChange(GameScenes scene)
	{
		if (HighLogic.CurrentGame != null)
		{
			OnFlagSelect(HighLogic.CurrentGame.flagURL);
		}
	}

	public void OnFlagSelect(string flagName)
	{
		flagTexture = GameDatabase.Instance.GetTexture(flagName, asNormalMap: false);
		flagRenderer.material.mainTexture = flagTexture;
	}
}
