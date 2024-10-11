using System.Runtime.CompilerServices;
using TMPro;

public class InputFieldHandler
{
	public delegate double GetValueDelegate();

	public delegate void SetValueDelegate(double newValue);

	protected ManeuverNodeEditorTabVectorInput inputTab;

	protected TMP_InputField inputField;

	private bool fieldChangeInProgress;

	private GetValueDelegate getValue;

	private SetValueDelegate setValue;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public InputFieldHandler(ManeuverNodeEditorTabVectorInput owner, TMP_InputField field, GetValueDelegate getValue, SetValueDelegate setValue)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual string GetFormattedValue(double value, bool fullPrecision)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual bool ParseText(string text, out double newValue)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnValueChanged(string text)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnFieldSelected(string text)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnFieldDeselected(string text)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdateField()
	{
		throw null;
	}
}
