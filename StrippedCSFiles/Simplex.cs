using System.Runtime.CompilerServices;

public class Simplex
{
	private static int[][] grad3;

	private static int[] p;

	private int[] perm;

	private double n0;

	private double n1;

	private double n2;

	private double n3;

	private double F3;

	private double s;

	private int i;

	private int j;

	private int k;

	private double G3;

	private double t;

	private double X0;

	private double Y0;

	private double Z0;

	private double x0;

	private double y0;

	private double z0;

	private int i1;

	private int j1;

	private int k1;

	private int i2;

	private int j2;

	private int k2;

	private double x1;

	private double y1;

	private double z1;

	private double x2;

	private double y2;

	private double z2;

	private double x3;

	private double y3;

	private double z3;

	private int ii;

	private int jj;

	private int kk;

	private int gi0;

	private int gi1;

	private int gi2;

	private int gi3;

	private double t0;

	private double t1;

	private double t2;

	private double t3;

	private double itr;

	private double total;

	private double amplitude;

	private double maxAmplitude;

	private double f;

	public double octaves;

	public double persistence;

	public double frequency;

	public int seed
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		set
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Simplex()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Simplex(int seed)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Simplex(int seed, double octaves, double persistence, double frequency)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static Simplex()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetupPermTable()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static int fastfloor(double x)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static double dot(int[] g, double x, double y)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static double dot(int[] g, double x, double y, double z)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static double dot(int[] g, double x, double y, double z, double w)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private double value(double xin, double yin, double zin)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public double noiseNormalized(Vector3d v3d)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public double noise(Vector3d v3d)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public double noiseNormalized(double x, double y, double z)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public double noise(double x, double y, double z)
	{
		throw null;
	}
}
