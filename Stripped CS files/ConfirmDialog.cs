using UnityEngine;

public class ConfirmDialog
{
	public UISkinDef skin;

	public Callback confirmed;

	public Callback abort;

	public Rect dialogRect;

	public string windowTitle = "Confirmation Needed";

	public string message = "Are you sure you want to proceed?";

	public string confirmButtonText = "Yes";

	public string abortButtonText = "Cancel";

	public Callback DrawCustomContent = delegate
	{
		GUILayout.FlexibleSpace();
	};

	public ConfirmDialog(Callback confirmCallback, Callback abortCallback, UISkinDef UISkinDef = null)
	{
		skin = UISkinDef;
		confirmed = confirmCallback;
		abort = abortCallback;
		dialogRect = new Rect(Screen.width / 2 - 150, Screen.height / 2 - 75, 300f, 100f);
	}
}
