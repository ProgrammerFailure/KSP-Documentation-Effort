using System.Runtime.CompilerServices;
using TMPro;

namespace Expansions.Missions.Editor;

[MEGUI_InputField]
public class MEGUIParameterInputField : MEGUIParameter
{
	internal interface ITypeParser
	{
		object Parse(string s);

		string Convert(object o);
	}

	internal class FloatTypeParser : ITypeParser
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		public FloatTypeParser()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public object Parse(string s)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public string Convert(object o)
		{
			throw null;
		}
	}

	internal class IntTypeParser : ITypeParser
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		public IntTypeParser()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public object Parse(string s)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public string Convert(object o)
		{
			throw null;
		}
	}

	internal class DoubleTypeParser : ITypeParser
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		public DoubleTypeParser()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public object Parse(string s)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public string Convert(object o)
		{
			throw null;
		}
	}

	internal class StringTypeParser : ITypeParser
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		public StringTypeParser()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public object Parse(string s)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public string Convert(object o)
		{
			throw null;
		}
	}

	public TMP_InputField inputField;

	internal ITypeParser parser;

	protected MEGUI_Control.InputContentType contentType;

	protected bool isDirty;

	public override bool IsInteractable
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		set
		{
			throw null;
		}
	}

	public string FieldValue
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		set
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MEGUIParameterInputField()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void Setup(string name)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void LockLocalizedText()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void ResetDefaultValue(string value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void RefreshUI()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private char OnValidatePercentageInput(string text, int charIndex, char addedChar)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnSelectInput(string value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnParameterValueChanged(string value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnParameterEndEdit(string value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnHistoryValueChange(ConfigNode data, HistoryType type)
	{
		throw null;
	}
}
