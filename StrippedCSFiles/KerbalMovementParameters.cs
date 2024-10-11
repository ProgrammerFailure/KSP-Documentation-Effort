using System;
using System.Runtime.CompilerServices;

[Serializable]
public class KerbalMovementParameters
{
	public float walkSpeed;

	public float runSpeed;

	public float jumpForce;

	public float strafeSpeed;

	public float backwardsSpeed;

	public float turnRate;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public KerbalMovementParameters()
	{
		throw null;
	}
}
