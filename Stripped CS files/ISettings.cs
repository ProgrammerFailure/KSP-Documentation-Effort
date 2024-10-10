public interface ISettings
{
	void OnUpdate();

	string GetName();

	void GetSettings();

	void DrawSettings();

	DialogGUIBase[] DrawMiniSettings();

	void ApplySettings();
}
