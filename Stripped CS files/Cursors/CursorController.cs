using UnityEngine;
using UnityEngine.SceneManagement;

namespace Cursors;

public class CursorController : MonoBehaviour
{
	public static CursorController Instance;

	public TextureCursor DefaultCursor;

	public TextureCursor DefaultLeftClick;

	public TextureCursor DefaultRightClick;

	public CursorItem basicDefault;

	public CursorItem currentCursor;

	public DictionaryValueList<string, CursorItem> cursorDictionary;

	public CursorItem this[int index] => cursorDictionary.At(index);

	public CursorItem this[string id] => cursorDictionary[id];

	public int Count => cursorDictionary.Count;

	public void Awake()
	{
		if ((bool)Instance)
		{
			Object.Destroy(this);
			return;
		}
		Instance = this;
		cursorDictionary = new DictionaryValueList<string, CursorItem>();
	}

	public void Start()
	{
		basicDefault = new CursorItem(DefaultCursor, DefaultLeftClick, DefaultRightClick);
		currentCursor = basicDefault;
		SetCursor(basicDefault.defaultCursor);
		SceneManager.sceneLoaded += OnSceneLoaded;
	}

	public void OnDestroy()
	{
		SceneManager.sceneLoaded -= OnSceneLoaded;
		if (Instance != null && Instance == this)
		{
			Instance = null;
		}
	}

	public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		OnLevelLoaded(HighLogic.GetLoadedGameSceneFromBuildIndex(scene.buildIndex));
	}

	public void OnLevelLoaded(GameScenes level)
	{
		if (Mouse.Left.GetButton())
		{
			SetCursor(basicDefault.leftClickCursor);
		}
		else if (Mouse.Right.GetButton())
		{
			SetCursor(basicDefault.rightClickCursor);
		}
		else
		{
			SetCursor(basicDefault.defaultCursor);
		}
	}

	public void SetCursor(CustomCursor cur)
	{
		cur?.SetCursor();
	}

	[ContextMenu("Force Default Cursor")]
	public void ForceDefaultCursor()
	{
		currentCursor = basicDefault;
		SetCursor(basicDefault.defaultCursor);
	}

	public void ChangeCursor(string id)
	{
		if (cursorDictionary.Contains(id))
		{
			currentCursor = cursorDictionary[id];
		}
		else if (id.Equals("default"))
		{
			currentCursor = basicDefault;
		}
		if (!Mouse.Left.GetButton() && !Mouse.Left.GetButtonDown())
		{
			if (!Mouse.Right.GetButton() && !Mouse.Right.GetButtonDown())
			{
				SetCursor(currentCursor.defaultCursor);
			}
			else
			{
				SetCursor(currentCursor.rightClickCursor);
			}
		}
		else
		{
			SetCursor(currentCursor.leftClickCursor);
		}
	}

	public void Update()
	{
		if (Mouse.Left.GetButtonDown())
		{
			SetCursor(currentCursor.leftClickCursor);
		}
		if (Mouse.Left.GetButtonUp())
		{
			SetCursor(currentCursor.defaultCursor);
		}
		if (Mouse.Right.GetButtonDown())
		{
			SetCursor(currentCursor.rightClickCursor);
		}
		if (Mouse.Right.GetButtonUp())
		{
			SetCursor(currentCursor.defaultCursor);
		}
	}

	public CursorItem AddCursor(string id, CustomCursor defaultCursor, CustomCursor leftClickCursor = null, CustomCursor rightClickCursor = null)
	{
		if (!cursorDictionary.Contains(id) && !id.Equals("default"))
		{
			CursorItem cursorItem = new CursorItem(defaultCursor, (leftClickCursor == null) ? defaultCursor : leftClickCursor, (rightClickCursor == null) ? defaultCursor : rightClickCursor);
			cursorDictionary.Add(id, cursorItem);
			return cursorItem;
		}
		return basicDefault;
	}

	public void Clear()
	{
		cursorDictionary.Clear();
	}

	public bool Contains(string id)
	{
		return cursorDictionary.Contains(id);
	}

	public void RemoveCursor(string id)
	{
		if (cursorDictionary.Contains(id))
		{
			cursorDictionary.Remove(id);
		}
	}
}
