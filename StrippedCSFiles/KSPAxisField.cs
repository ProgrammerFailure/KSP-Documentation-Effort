using System.Runtime.CompilerServices;

public class KSPAxisField : KSPField
{
	public float minValue;

	public float maxValue;

	public float incrementalSpeed;

	public float incrementalSpeedMultiplier;

	public bool ignoreClampWhenIncremental;

	public bool ignoreIncrementByZero;

	public KSPAxisGroup axisGroup;

	public KSPAxisMode axisMode;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public KSPAxisField()
	{
		throw null;
	}
}
