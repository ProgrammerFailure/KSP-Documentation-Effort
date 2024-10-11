using System.Runtime.CompilerServices;

namespace KSP.UI.Screens.DebugToolbar.Screens.Cheats;

public class NoCrashDamage : DebugScreenToggle
{
	[MethodImpl(MethodImplOptions.NoInlining)]
	public NoCrashDamage()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void SetupValues()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnToggleChanged(bool state)
	{
		throw null;
	}
}
