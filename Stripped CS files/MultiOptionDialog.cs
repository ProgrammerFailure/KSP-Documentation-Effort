using System.Collections.Generic;
using UnityEngine;

public class MultiOptionDialog : DialogGUIBase
{
	public DialogGUIBase[] Options;

	public UISkinDef UISkinDef;

	public string name = "";

	public string title = "";

	public string message = "Choose an option:";

	public Rect dialogRect;

	public Callback DrawCustomContent = delegate
	{
		GUILayout.FlexibleSpace();
	};

	public int id;

	public MultiOptionDialog(string name, string msg, string windowTitle, UISkinDef skin, params DialogGUIBase[] options)
	{
		this.name = name;
		UISkinDef = skin;
		Options = options;
		title = windowTitle;
		message = msg;
		dialogRect = new Rect(0.5f, 0.5f, 300f, 100f);
	}

	public MultiOptionDialog(string name, string msg, string windowTitle, UISkinDef skin, float width, params DialogGUIBase[] options)
	{
		this.name = name;
		UISkinDef = skin;
		Options = options;
		title = windowTitle;
		message = msg;
		dialogRect = new Rect(0.5f, 0.5f, width, 100f);
	}

	public MultiOptionDialog(string name, string msg, string windowTitle, UISkinDef skin, Rect rct, params DialogGUIBase[] options)
	{
		this.name = name;
		UISkinDef = skin;
		Options = options;
		title = windowTitle;
		message = msg;
		dialogRect = rct;
	}

	public override GameObject Create(ref Stack<Transform> layouts, UISkinDef skin)
	{
		DialogGUIBase[] options = Options;
		for (int i = 0; i < options.Length; i++)
		{
			options[i].Create(ref layouts, skin);
		}
		return uiItem;
	}

	public override void Update()
	{
		OnUpdate();
		DialogGUIBase[] options = Options;
		for (int i = 0; i < options.Length; i++)
		{
			options[i].Update();
		}
		base.Update();
	}

	public void FixedUpdate()
	{
		DialogGUIBase[] options = Options;
		for (int i = 0; i < options.Length; i++)
		{
			options[i].OnFixedUpdate();
		}
	}

	public void LateUpdate()
	{
		DialogGUIBase[] options = Options;
		for (int i = 0; i < options.Length; i++)
		{
			options[i].OnLateUpdate();
		}
	}

	public new void OnRenderObject()
	{
		DialogGUIBase[] options = Options;
		for (int i = 0; i < options.Length; i++)
		{
			options[i].OnRenderObject();
		}
	}

	public new void Resize()
	{
		OnResize();
		DialogGUIBase[] options = Options;
		for (int i = 0; i < options.Length; i++)
		{
			options[i].Resize();
		}
	}
}
