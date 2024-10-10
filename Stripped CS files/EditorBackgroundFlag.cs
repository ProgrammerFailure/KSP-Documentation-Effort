using UnityEngine;

public class EditorBackgroundFlag : MonoBehaviour
{
	public MeshRenderer flagMeshRenderer;

	public SkinnedMeshRenderer skinnedMeshRender;

	public Texture2D texture;

	public void Awake()
	{
		GameEvents.onMissionFlagSelect.Add(updateFlag);
	}

	public void OnDestroy()
	{
		GameEvents.onMissionFlagSelect.Remove(updateFlag);
	}

	public void Start()
	{
		if (HighLogic.LoadedSceneIsGame)
		{
			updateFlag((EditorLogic.FlagURL == string.Empty) ? HighLogic.CurrentGame.flagURL : EditorLogic.FlagURL);
		}
	}

	public void updateFlag(string flagURL)
	{
		texture = GameDatabase.Instance.GetTexture(flagURL, asNormalMap: false);
		if (flagMeshRenderer != null && texture != null)
		{
			flagMeshRenderer.material.mainTexture = texture;
		}
		if (skinnedMeshRender != null && texture != null)
		{
			skinnedMeshRender.material.mainTexture = texture;
		}
	}
}
