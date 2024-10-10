using ns7;
using UnityEngine;

public class UnitTestingWindow : MonoBehaviour
{
	public Vector2 logScroll;

	public TestResults results;

	public Rect windowRect;

	public static UnitTestingWindow Instance { get; set; }

	public static void OpenWindow()
	{
		if (!(Instance != null))
		{
			Instance = new GameObject("UnitTestingWindow").AddComponent<UnitTestingWindow>();
		}
	}

	public void Awake()
	{
		Instance = this;
		results = TestManager.RunTests();
		foreach (TestState state in results.states)
		{
			string text = $"{state.Info.Name} by {state.Info.Author} ({state.Info.SinceVersion}): ";
			text += (state.Succeeded ? "PASS" : "FAIL!");
			text = text + "  " + state.Reason;
			if (!state.Succeeded)
			{
				Debug.LogError(text);
				if (state.Details.Length > 0)
				{
					Debug.LogError("More information: " + state.Details);
				}
			}
			else
			{
				Debug.Log(text);
			}
		}
		windowRect = new Rect((float)(Screen.width / 2) - 320f, (float)(Screen.height / 2) - 240f, 640f, 480f);
	}

	public void OnDestroy()
	{
		if (Instance != null && Instance == this)
		{
			Instance = null;
		}
	}

	public void OnGUI()
	{
		GUI.Window(1, windowRect, DrawUnitTestWindow, "Unit testing results");
	}

	public void DrawUnitTestWindow(int id)
	{
		GUI.skin.box.alignment = TextAnchor.UpperLeft;
		GUI.skin.box.wordWrap = true;
		GUILayout.BeginVertical();
		logScroll = GUILayout.BeginScrollView(logScroll, false, false, GUILayout.ExpandHeight(expand: true), GUILayout.ExpandWidth(expand: true));
		foreach (TestState state in results.states)
		{
			DrawState(state);
		}
		GUILayout.EndScrollView();
		if (GUILayout.Button("Exit"))
		{
			Instance = null;
			Object.Destroy(base.gameObject);
		}
		else
		{
			GUILayout.EndVertical();
			GUI.DragWindow();
		}
	}

	public void DrawState(TestState state)
	{
		GUILayout.BeginHorizontal();
		GUILayout.Label($"{state.Info.Name} by {state.Info.Author} ({state.Info.SinceVersion}): " + (state.Succeeded ? "PASS" : "FAIL!"));
		GUILayout.EndHorizontal();
	}
}
