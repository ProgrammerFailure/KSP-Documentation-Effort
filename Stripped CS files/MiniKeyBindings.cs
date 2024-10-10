using System.Collections.Generic;
using ns9;
using UnityEngine;
using UnityEngine.UI;

public class MiniKeyBindings : MonoBehaviour
{
	public Callback OnDismissCallback;

	public Vector2 scrollPos;

	public Rect windowRect;

	public UISkinDef skin;

	public InputSettings keyBindings;

	public static MiniKeyBindings Create(Callback onDismiss)
	{
		MiniKeyBindings miniKeyBindings = new GameObject("Mini Settings Dialog").AddComponent<MiniKeyBindings>();
		miniKeyBindings.OnDismissCallback = onDismiss;
		miniKeyBindings.windowRect = new Rect(0.5f, 0.5f, 400f, 500f);
		miniKeyBindings.skin = UISkinManager.GetSkin("MiniSettingsSkin");
		miniKeyBindings.keyBindings = new InputSettings();
		return miniKeyBindings;
	}

	public void Start()
	{
		keyBindings.GetSettings();
		PopupDialog.SpawnPopupDialog(new MultiOptionDialog("KeyBindings", "", Localizer.Format("#autoLOC_149391"), skin, windowRect, drawWindow()), persistAcrossScenes: false, skin).OnDismiss = Dismiss;
	}

	public void Update()
	{
		keyBindings.OnUpdate();
	}

	public DialogGUIBase[] drawWindow()
	{
		List<DialogGUIBase> list = new List<DialogGUIBase>();
		DialogGUIVerticalLayout dialogGUIVerticalLayout = new DialogGUIVerticalLayout(-1f, 450f, 4f, new RectOffset(8, 24, 16, 16), TextAnchor.UpperLeft, new DialogGUIContentSizer(ContentSizeFitter.FitMode.Unconstrained, ContentSizeFitter.FitMode.PreferredSize, useParentSize: true));
		dialogGUIVerticalLayout.AddChildren(keyBindings.DrawMiniSettings());
		list.Add(new DialogGUIScrollList(-Vector2.one, hScroll: false, vScroll: true, dialogGUIVerticalLayout));
		list.Add(new DialogGUIHorizontalLayout(new DialogGUIFlexibleSpace(), new DialogGUIButton(Localizer.Format("#autoLOC_149410"), delegate
		{
			Dismiss();
		}, 80f, 30f, true)));
		return list.ToArray();
	}

	public void Dismiss()
	{
		OnDismissCallback();
		Object.Destroy(base.gameObject);
	}
}
