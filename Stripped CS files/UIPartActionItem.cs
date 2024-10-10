using UnityEngine;

public class UIPartActionItem : MonoBehaviour
{
	public Part part;

	public PartModule partModule;

	public bool isModule;

	public UIPartActionWindow window;

	public UI_Scene scene;

	public UI_Control control;

	public Part Part => part;

	public PartModule PartModule => partModule;

	public bool IsModule => isModule;

	public UIPartActionWindow Window => window;

	public UI_Scene Scene => scene;

	public UI_Control Control => control;

	public void SetupItem(UIPartActionWindow window, Part part, PartModule partModule, UI_Scene scene, UI_Control control)
	{
		this.window = window;
		this.part = part;
		this.partModule = partModule;
		this.scene = scene;
		this.control = control;
		if (this.control != null)
		{
			this.control.partActionItem = this;
		}
		isModule = partModule != null;
	}

	public virtual bool IsItemValid()
	{
		if (isModule)
		{
			if (part == null || part.State == PartStates.DEAD || partModule == null)
			{
				return false;
			}
		}
		else if (part == null || part.State == PartStates.DEAD)
		{
			return false;
		}
		return true;
	}

	public virtual void UpdateItem()
	{
	}

	public virtual void AddInputFieldLock(string val)
	{
		InputLockManager.SetControlLock(ControlTypes.KEYBOARDINPUT, "UIPartActionFieldTextInput");
	}

	public virtual void RemoveInputfieldLock()
	{
		InputLockManager.RemoveControlLock("UIPartActionFieldTextInput");
	}
}
