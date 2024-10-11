using System.Runtime.CompilerServices;

namespace KSP.UI.Screens;

public class KbApp_Null : KbApp
{
	[MethodImpl(MethodImplOptions.NoInlining)]
	public KbApp_Null()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void ActivateApp(MapObject target)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void DisplayApp()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void HideApp()
	{
		throw null;
	}
}
