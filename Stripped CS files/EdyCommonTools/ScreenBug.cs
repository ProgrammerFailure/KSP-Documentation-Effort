using UnityEngine;
using UnityEngine.Serialization;

namespace EdyCommonTools;

[ExecuteInEditMode]
public class ScreenBug : MonoBehaviour
{
	public bool hideAtEditor;

	public bool sideBySide;

	public bool showText = true;

	public string[] text;

	public GUIStyle style = new GUIStyle();

	public bool showBug = true;

	public Texture2D bug;

	public float bugSize = 150f;

	[FormerlySerializedAs("borderX")]
	public float marginX = 20f;

	[FormerlySerializedAs("borderY")]
	public float marginY;

	public void OnGUI()
	{
		if (hideAtEditor && Application.isEditor)
		{
			return;
		}
		if (showText && this.text.Length != 0)
		{
			string text = "";
			string[] array = this.text;
			foreach (string text2 in array)
			{
				text = text + text2 + "\n";
			}
			GUI.Label(new Rect(0f, 0f, Screen.width, Screen.height), text, style);
			if (sideBySide)
			{
				GUI.Label(new Rect(Screen.width / 2, 0f, Screen.width, Screen.height), text, style);
			}
		}
		if (showBug && bug != null)
		{
			GUI.DrawTexture(new Rect((float)Screen.width - bugSize - marginX, (float)Screen.height - bugSize - marginY, bugSize, bugSize), bug, ScaleMode.ScaleToFit);
			if (sideBySide)
			{
				GUI.DrawTexture(new Rect((float)(Screen.width / 2) - bugSize - marginX, (float)Screen.height - bugSize - marginY, bugSize, bugSize), bug, ScaleMode.ScaleToFit);
			}
		}
	}
}
