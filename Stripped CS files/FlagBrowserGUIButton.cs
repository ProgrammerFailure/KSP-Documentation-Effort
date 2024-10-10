using UnityEngine;
using UnityEngine.UI;

public class FlagBrowserGUIButton : DialogGUIButton
{
	public GameObject FlagBrowserPrefab;

	public GUISkin skin;

	public Rect buttonRect;

	public Texture contentTexture;

	public FlagBrowser browser;

	public bool locked;

	public Callback flagBrowserOpened;

	public FlagBrowser.FlagSelectedCallback flagSelected;

	public Callback flagCancelled;

	public string _flagURL = "";

	public string flagURL
	{
		get
		{
			return _flagURL;
		}
		set
		{
			if (_flagURL != value)
			{
				_flagURL = value;
				contentTexture = GameDatabase.Instance.GetTexture(_flagURL, asNormalMap: false);
			}
		}
	}

	public FlagBrowserGUIButton(Texture texture, Callback onBrowserOpen, FlagBrowser.FlagSelectedCallback onFlagSelected, Callback onFlagCancelled)
		: base("", delegate
		{
		}, 64f, 40f, false, new DialogGUIHorizontalLayout(true, true, 0f, new RectOffset(4, 4, 4, 4), TextAnchor.MiddleCenter, new DialogGUIImage(new Vector2(56f, 32f), Vector2.zero, Color.white, texture)))
	{
		contentTexture = texture;
		onOptionSelected = SpawnBrowser;
		flagBrowserOpened = onBrowserOpen;
		flagSelected = onFlagSelected;
		flagCancelled = onFlagCancelled;
		FlagBrowserPrefab = AssetBase.GetPrefab("FlagBrowser");
	}

	public void Draw(Rect rect)
	{
		GUI.enabled = !locked;
		if (GUI.Button(rect, contentTexture))
		{
			SpawnBrowser();
		}
		GUI.enabled = true;
	}

	public void Draw(float width, float height)
	{
		GUI.enabled = !locked;
		if (GUILayout.Button(contentTexture, GUILayout.Width(width), GUILayout.Height(height)))
		{
			SpawnBrowser();
		}
		GUI.enabled = true;
	}

	public void SpawnBrowser()
	{
		browser = Object.Instantiate(FlagBrowserPrefab).GetComponent<FlagBrowser>();
		browser.OnDismiss = OnFlagCancel;
		browser.OnFlagSelected = OnFlagSelect;
		flagBrowserOpened();
		locked = true;
	}

	public void OnFlagSelect(FlagBrowser.FlagEntry selected)
	{
		contentTexture = selected.textureInfo.texture;
		flagSelected(selected);
		uiItem.GetComponentInChildren<RawImage>().texture = contentTexture;
		locked = false;
	}

	public void OnFlagCancel()
	{
		flagCancelled();
		locked = false;
	}
}
