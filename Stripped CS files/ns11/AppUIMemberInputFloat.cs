using ns2;

namespace ns11;

[AppUI_InputFloat]
public class AppUIMemberInputFloat : AppUIMemberInput
{
	public string guiFormat;

	public override void OnInitialized()
	{
		base.OnInitialized();
		if (_attribs is AppUI_InputFloat appUI_InputFloat)
		{
			guiFormat = (string.IsNullOrEmpty(appUI_InputFloat.guiFormat) ? "" : appUI_InputFloat.guiFormat);
		}
	}

	public override void RefreshUIInput()
	{
		input.text = KSPUtil.LocalizeNumber(GetValue<float>(), ((AppUI_InputFloat)_attribs).guiFormat);
	}
}
