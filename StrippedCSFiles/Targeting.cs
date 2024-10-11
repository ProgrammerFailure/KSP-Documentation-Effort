using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Smooth.Pools;

public class Targeting
{
	public struct Ray
	{
		public Vector3d p;

		public Vector3d v;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public Ray(Vector3d p, Vector3d v)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public Vector3d ClosestPoint(Vector3d point, bool forward = false)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public Vector3d ClosestPoint(Ray ray, bool forward = false)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public Ray Project(Plane plane)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public Vector3d Point(double t)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public override string ToString()
		{
			throw null;
		}
	}

	public struct Plane
	{
		public Vector3d p;

		public Vector3d n;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public Plane(Vector3d p, Vector3d n)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public Vector3d Intersept(Ray ray)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public override string ToString()
		{
			throw null;
		}
	}

	public struct Frame
	{
		public Vector3d p;

		public Vector3d t;

		public Vector3d n;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public Frame(Vector3d p, Vector3d t, Vector3d n)
		{
			throw null;
		}
	}

	public class Conic
	{
		public double l;

		public double e;

		public double a;

		public double b;

		public double pe;

		public Vector3d pe_point;

		public double ap;

		public Vector3d ap_point;

		public Vector3d X;

		public Vector3d Y;

		public Vector3d N;

		public Vector3d focus;

		public Vector3d center;

		public Plane conic_plane;

		public double min_v;

		public double max_v;

		private static readonly Pool<Conic> pool;

		[MethodImpl(MethodImplOptions.NoInlining)]
		private Conic()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		static Conic()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private static Conic Create()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private static void Reset(Conic obj)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void Release()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public static Conic Borrow(Orbit o)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public double Radius(double v)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public Vector3d Point(double v)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public double Curvature(double v)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public Vector3d Tangent(double v)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void true_anomaly_xy(Vector3d p, out double c, out double s)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public double TrueAnomaly(Vector3d p)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void tangent_xy(Vector3d p, out double vx, out double vy)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public Vector3d Tangent(Vector3d p)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public Vector3d Normal(double v)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public Vector3d Normal(Vector3d p)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public Frame GetFrame(Vector3d p)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public int Intercepts(Ray line, out Vector3d i1, out Vector3d i2)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public bool point_is_real(Vector3d p)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public bool point_inside(Vector3d p)
		{
			throw null;
		}
	}

	public class Sample
	{
		public struct Info
		{
			public Vector3d d;

			public double cos0;

			public double cos1;

			public double dot0;

			public double dot1;

			public double tangent_dot;

			[MethodImpl(MethodImplOptions.NoInlining)]
			public void Set(Frame f1, Frame f2)
			{
				throw null;
			}
		}

		public Conic orbit1;

		public Conic orbit2;

		public double v;

		public bool region_edge;

		public bool valid;

		public bool minimum;

		public Frame src;

		public Frame tgt1;

		public Frame tgt2;

		public Info info1;

		public Info info2;

		public int tgt_index;

		private static readonly Pool<Sample> pool;

		private static readonly Dictionary<int, List<Sample>> borrowed;

		public Frame tgt
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				throw null;
			}
		}

		public Info info
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				throw null;
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private Sample()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		static Sample()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private static Sample Create()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private static void Reset(Sample obj)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public static void ReleaseAllBorrowed()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public static Sample Borrow(Conic o1, Conic o2, double v)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public Info GetInfo(int index)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public Frame GetTgt(int index)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public static int get_normal_intercepts(Conic o1, Vector3d p1, Conic o2, out Vector3d i1, out Vector3d i2)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public static int get_normal_intercept_frames(Conic o1, Vector3d p1, Conic o2, out Frame f1, out Frame f2)
		{
			throw null;
		}
	}

	public class PairOfIntervals
	{
		private static readonly Pool<PairOfIntervals> pool;

		private List<Interval> A;

		private List<Interval> B;

		public List<Interval> this[int key]
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				throw null;
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private PairOfIntervals()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		static PairOfIntervals()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private static PairOfIntervals Create()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private static void Reset(PairOfIntervals obj)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public static PairOfIntervals Borrow()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void Release()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void ReleaseContent()
		{
			throw null;
		}
	}

	public class Interval
	{
		public Sample s1;

		public Sample s2;

		public int tgt_index;

		public Sample.Info info1;

		public Sample.Info info2;

		public bool void_region;

		public bool has_root;

		public bool minimum;

		public bool maximum;

		public Interval prev;

		public Interval next;

		public const double epsilon = 1E-08;

		public const double golden = 0.6180339887498949;

		private static readonly Pool<Interval> pool;

		[MethodImpl(MethodImplOptions.NoInlining)]
		private Interval()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		static Interval()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private static Interval Create()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private static void Reset(Interval obj)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void Release()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public static Interval Borrow(Sample s1, Sample s2, int tgt_index)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void Link(Interval prev, Interval next)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public List<Interval> Subdivide()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public Sample FindRoot()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public static Sample find_region_edge(Conic orbit1, Conic orbit2, double v1, double v2)
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Targeting()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static double sample_v(int index, int count, Conic o)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void add_interval(PairOfIntervals intervals, Sample start, Sample end)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static Sample check_void_transition(PairOfIntervals intervals, Sample prev, Sample samp, Sample ival_start)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static PairOfIntervals sample_orbits(Conic orbit1, Conic orbit2, int count)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static PairOfIntervals find_search_intervals(PairOfIntervals ivals)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void add_crossing_subdivisions(List<Interval> intervals, Interval ival, bool reversed)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static PairOfIntervals subdivide_intervals(PairOfIntervals ivals)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static List<Sample> Intercepts(Conic orbit1, Conic orbit2, int samples)
	{
		throw null;
	}
}
