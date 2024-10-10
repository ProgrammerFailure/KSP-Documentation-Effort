using TMPro;

public class InputFieldHandler
{
	public delegate double GetValueDelegate();

	public delegate void SetValueDelegate(double newValue);

	public ManeuverNodeEditorTabVectorInput inputTab;

	public TMP_InputField inputField;

	public bool fieldChangeInProgress;

	public GetValueDelegate getValue;

	public SetValueDelegate setValue;

	public InputFieldHandler(ManeuverNodeEditorTabVectorInput owner, TMP_InputField field, GetValueDelegate getValue, SetValueDelegate setValue)
	{
		inputTab = owner;
		this.getValue = getValue;
		this.setValue = setValue;
		inputField = field;
		inputField.onEndEdit.AddListener(OnValueChanged);
		inputField.onSelect.AddListener(OnFieldSelected);
		inputField.onDeselect.AddListener(OnFieldDeselected);
		UpdateField();
	}

	public virtual string GetFormattedValue(double value, bool fullPrecision)
	{
		if (fullPrecision)
		{
			return value.ToString("G15");
		}
		return value.ToString("F3");
	}

	public virtual bool ParseText(string text, out double newValue)
	{
		return double.TryParse(text, out newValue);
	}

	public void OnValueChanged(string text)
	{
		if (!fieldChangeInProgress && ParseText(inputField.text, out var newValue))
		{
			setValue(newValue);
		}
	}

	public void OnFieldSelected(string text)
	{
		fieldChangeInProgress = true;
		inputField.text = GetFormattedValue(getValue(), fullPrecision: true);
		fieldChangeInProgress = false;
		inputTab.isEditing = true;
		InputLockManager.SetControlLock("mannodeInputEdit");
	}

	public void OnFieldDeselected(string text)
	{
		fieldChangeInProgress = true;
		inputField.text = GetFormattedValue(getValue(), fullPrecision: false);
		fieldChangeInProgress = false;
		inputTab.isEditing = false;
		InputLockManager.RemoveControlLock("mannodeInputEdit");
	}

	public void OnDestroy()
	{
		inputField.onEndEdit.RemoveListener(OnValueChanged);
		inputField.onSelect.RemoveListener(OnFieldSelected);
		inputField.onDeselect.RemoveListener(OnFieldDeselected);
	}

	public void UpdateField()
	{
		fieldChangeInProgress = true;
		inputField.text = GetFormattedValue(getValue(), fullPrecision: false);
		fieldChangeInProgress = false;
	}
}
