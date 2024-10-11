using System.Runtime.CompilerServices;

public class AudioFXSettings : DialogGUIVerticalLayout, ISettings
{
	private float masterVolume;

	private float shipVolume;

	private float ambienceVolume;

	private float musicVolume;

	private float voicesVolume;

	private float uiVolume;

	private bool normalizerEnabled;

	private float threshold;

	private float sharpness;

	private int skipsample;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public AudioFXSettings()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void GetSettings()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void DrawSettings()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public DialogGUIBase[] DrawMiniSettings()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ApplySettings()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public string GetName()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public new void OnUpdate()
	{
		throw null;
	}
}
