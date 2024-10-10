using System.Collections;
using ns9;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ns34;

public class ScreenDatabaseOverview : MonoBehaviour
{
	public Button reloadButton;

	public Button reloadTextsButton;

	public TextMeshProUGUI partCount;

	public TextMeshProUGUI propCount;

	public TextMeshProUGUI internalCount;

	public TextMeshProUGUI resourceCount;

	public TextMeshProUGUI localizedStringCount;

	public TextMeshProUGUI assemblyCount;

	public TextMeshProUGUI modelCount;

	public TextMeshProUGUI textureCount;

	public TextMeshProUGUI audioCount;

	public bool isRecompiling;

	public void Start()
	{
		ResetCounts();
		CheckInteractable(HighLogic.LoadedScene);
		reloadButton.onClick.AddListener(OnReloadClicked);
		reloadTextsButton.onClick.AddListener(OnReloadTextsClicked);
		GameEvents.onLevelWasLoaded.Add(CheckInteractable);
	}

	public void OnDestroy()
	{
		GameEvents.onLevelWasLoaded.Remove(CheckInteractable);
	}

	public void OnReloadClicked()
	{
		GameDatabase.Instance.StartCoroutine(RecompileAssets());
	}

	public void OnReloadTextsClicked()
	{
		bool num = Localizer.SwitchToLanguage(Localizer.GetLanguageIdFromFile());
		string message = (num ? "Language dictionary reloaded successfully." : "Unable to reload the language dictionary. Check the log for errors.");
		if (num)
		{
			Debug.Log(message);
		}
		else
		{
			Debug.LogError(message);
		}
		ScreenMessages.PostScreenMessage(message, 5f, ScreenMessageStyle.UPPER_CENTER);
	}

	public void CheckInteractable(GameScenes scene)
	{
		reloadButton.interactable = scene != GameScenes.FLIGHT && scene != GameScenes.EDITOR && !isRecompiling;
	}

	public void ResetCounts()
	{
		if (GameDatabase.Instance == null)
		{
			FlushCounts("N/A");
			return;
		}
		assemblyCount.text = AssemblyLoader.loadedAssemblies.Count.ToString();
		modelCount.text = GameDatabase.Instance.databaseModel.Count.ToString();
		textureCount.text = GameDatabase.Instance.databaseTexture.Count.ToString();
		audioCount.text = GameDatabase.Instance.databaseAudio.Count.ToString();
		partCount.text = GameDatabase.Instance.GetConfigs("PART").Length.ToString();
		propCount.text = GameDatabase.Instance.GetConfigs("PROP").Length.ToString();
		internalCount.text = GameDatabase.Instance.GetConfigs("INTERNAL").Length.ToString();
		resourceCount.text = GameDatabase.Instance.GetConfigs("RESOURCE").Length.ToString();
		localizedStringCount.text = Localizer.TagsLength.ToString();
	}

	public void FlushCounts(string s)
	{
		partCount.text = s;
		propCount.text = s;
		internalCount.text = s;
		resourceCount.text = s;
		localizedStringCount.text = s;
		assemblyCount.text = s;
		modelCount.text = s;
		textureCount.text = s;
		audioCount.text = s;
	}

	public IEnumerator RecompileAssets()
	{
		if (isRecompiling)
		{
			Debug.LogError("Database is already recompiling!");
			yield break;
		}
		FlushCounts(Localizer.Format("#autoLOC_7001100"));
		reloadButton.interactable = false;
		isRecompiling = true;
		if ((bool)GameDatabase.Instance)
		{
			GameDatabase.Instance.Recompile = true;
			GameDatabase.Instance.StartLoad();
			while (!GameDatabase.Instance.IsReady())
			{
				yield return null;
			}
		}
		else
		{
			Debug.LogError("GameDatabase is not available to reload right now.");
		}
		if ((bool)PartLoader.Instance)
		{
			PartLoader.Instance.StartLoad();
			while (!PartLoader.Instance.IsReady())
			{
				yield return null;
			}
		}
		else
		{
			Debug.LogError("PartLoader is not available to reload right now.");
		}
		if (GameDatabase.Instance != null)
		{
			GameDatabase.Instance.Recompile = false;
		}
		isRecompiling = false;
		CheckInteractable(HighLogic.LoadedScene);
		ResetCounts();
	}
}
