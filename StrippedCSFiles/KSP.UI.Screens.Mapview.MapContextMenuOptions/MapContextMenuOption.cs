using System.Runtime.CompilerServices;

namespace KSP.UI.Screens.Mapview.MapContextMenuOptions;

public abstract class MapContextMenuOption : DialogGUIButton
{
	[MethodImpl(MethodImplOptions.NoInlining)]
	public MapContextMenuOption(string caption)
	{
		throw null;
	}

	protected abstract void OnSelect();

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool CheckEnabled()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual bool OnCheckEnabled(out string fbText)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual bool CheckAvailable()
	{
		throw null;
	}
}
