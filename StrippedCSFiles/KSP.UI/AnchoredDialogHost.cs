using System.Runtime.CompilerServices;
using UnityEngine;

namespace KSP.UI;

public class AnchoredDialogHost : MonoBehaviour
{
	public AnchoredDialog host;

	public Callback OnHostLateUpdate;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public AnchoredDialogHost()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void LateUpdate()
	{
		throw null;
	}
}
