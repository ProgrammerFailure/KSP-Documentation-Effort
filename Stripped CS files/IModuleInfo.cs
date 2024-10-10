using UnityEngine;

public interface IModuleInfo
{
	string GetModuleTitle();

	string GetInfo();

	Callback<Rect> GetDrawModulePanelCallback();

	string GetPrimaryField();
}
