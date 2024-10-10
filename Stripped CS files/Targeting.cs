using System;
using System.Collections.Generic;
using System.Threading;
using Smooth.Pools;

public class Targeting
{
	public struct Ray
	{
		public Vector3d p;

		public Vector3d v;

		public Ray(Vector3d p, Vector3d v)
		{
			this.p = p;
			this.v = v;
		}

		public Vector3d ClosestPoint(Vector3d point, bool forward = false)
		{
			double num = Vector3d.Dot(point - p, v) / v.sqrMagnitude;
			if (forward && num < 0.0)
			{
				return p;
			}
			return p + v * num;
		}

		public Vector3d ClosestPoint(Ray ray, bool forward = false)
		{
			Vector3d rhs = v - ray.v * Vector3d.Dot(v, ray.v) / ray.v.sqrMagnitude;
			if (rhs.sqrMagnitude == 0.0)
			{
				return p;
			}
			double num = Vector3d.Dot(ray.p - p, rhs) / Vector3d.Dot(v, rhs);
			if (forward && num < 0.0)
			{
				return p;
			}
			return p + v * num;
		}

		public Ray Project(Plane plane)
		{
			Vector3d vector3d = p - plane.n * Vector3d.Dot(p - plane.p, plane.n) / plane.n.sqrMagnitude;
			Vector3d vector3d2 = v - plane.n * Vector3d.Dot(v, plane.n) / plane.n.sqrMagnitude;
			return new Ray(vector3d, vector3d2);
		}

		public Vector3d Point(double t)
		{
			return p + t * v;
		}

		public override string ToString()
		{
			return string.Concat("{", p, ", ", v, "}");
		}
	}

	public struct Plane
	{
		public Vector3d p;

		public Vector3d n;

		public Plane(Vector3d p, Vector3d n)
		{
			this.p = p;
			this.n = n;
		}

		public Vector3d Intersept(Ray ray)
		{
			double num = Vector3d.Dot(p - ray.p, n);
			double num2 = Vector3d.Dot(ray.v, n);
			if (num2 == 0.0)
			{
				return ray.p;
			}
			return ray.p + ray.v * num / num2;
		}

		public override string ToString()
		{
			return string.Concat("{", p, ", ", n, "}");
		}
	}

	public struct Frame
	{
		public Vector3d p;

		public Vector3d t;

		public Vector3d n;

		public Frame(Vector3d p, Vector3d t, Vector3d n)
		{
			this.p = p;
			this.t = t;
			this.n = n;
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

		public Vector3d vector3d_0;

		public Vector3d vector3d_1;

		public Vector3d vector3d_2;

		public Vector3d focus;

		public Vector3d center;

		public Plane conic_plane;

		public double min_v;

		public double max_v;

		public static readonly Pool<Conic> pool = new Pool<Conic>(Create, Reset);

		public static Conic Create()
		{
			return new Conic();
		}

		public static void Reset(Conic obj)
		{
		}

		public void Release()
		{
			pool.Release(this);
		}

		public static Conic Borrow(Orbit o)
		{
			Conic conic = pool.Borrow();
			conic.l = o.semiLatusRectum;
			conic.e = o.eccentricity;
			conic.vector3d_0 = o.OrbitFrame.vector3d_0;
			conic.vector3d_1 = o.OrbitFrame.vector3d_1;
			conic.vector3d_2 = o.OrbitFrame.vector3d_2;
			conic.focus = Vector3d.zero;
			conic.conic_plane = new Plane(conic.focus, conic.vector3d_2);
			if (conic.e != 1.0)
			{
				conic.a = conic.l / (1.0 - conic.e * conic.e);
				conic.b = conic.l / Math.Sqrt(Math.Abs(1.0 - conic.e * conic.e));
				conic.center = -conic.vector3d_0 * conic.l * conic.e / (1.0 - conic.e * conic.e);
			}
			else
			{
				conic.center = conic.vector3d_0 * conic.l / 2.0;
				double num = double.PositiveInfinity;
				conic.b = double.PositiveInfinity;
				conic.a = num;
			}
			conic.pe = conic.l / (1.0 + conic.e);
			conic.pe_point = conic.vector3d_0 * conic.pe;
			if (conic.e < 1.0)
			{
				conic.ap = conic.l / (1.0 - conic.e);
				conic.ap_point = (0.0 - conic.ap) * conic.vector3d_0;
				conic.max_v = Math.PI;
			}
			else
			{
				conic.ap = double.PositiveInfinity;
				conic.ap_point = (0.0 - conic.ap) * conic.vector3d_0;
				conic.max_v = Math.Acos(-1.0 / conic.e);
			}
			conic.min_v = 0.0 - conic.max_v;
			return conic;
		}

		public double Radius(double v)
		{
			return l / (1.0 + e * Math.Cos(v));
		}

		public Vector3d Point(double v)
		{
			double num = Math.Cos(v);
			double num2 = Math.Sin(v);
			return l / (1.0 + e * num) * (num * vector3d_0 + num2 * vector3d_1);
		}

		public double Curvature(double v)
		{
			double num = Math.Cos(v);
			double num2 = 1.0 + e * num;
			double num3 = 1.0 + 2.0 * num * num + e * e;
			return Math.Pow(num2 * num2 / num3, 1.5) / l;
		}

		public Vector3d Tangent(double v)
		{
			double num = 0.0 - Math.Sin(v);
			double num2 = Math.Cos(v) + e;
			return num * vector3d_0 + num2 * vector3d_1;
		}

		public void true_anomaly_xy(Vector3d p, out double c, out double s)
		{
			Vector3d normalized = p.normalized;
			c = Vector3d.Dot(normalized, vector3d_0);
			s = Vector3d.Dot(normalized, vector3d_1);
		}

		public double TrueAnomaly(Vector3d p)
		{
			true_anomaly_xy(p, out var c, out var s);
			return Math.Atan2(s, c);
		}

		public void tangent_xy(Vector3d p, out double vx, out double vy)
		{
			Vector3d normalized = p.normalized;
			if (e > 1.0 && Vector3d.Dot(p - center, vector3d_0) > 0.0)
			{
				vx = Vector3d.Dot(normalized, vector3d_1);
				vy = 0.0 - Vector3d.Dot(normalized, vector3d_0) + e;
			}
			else
			{
				vx = 0.0 - Vector3d.Dot(normalized, vector3d_1);
				vy = Vector3d.Dot(normalized, vector3d_0) + e;
			}
		}

		public Vector3d Tangent(Vector3d p)
		{
			tangent_xy(p, out var vx, out var vy);
			return vx * vector3d_0 + vy * vector3d_1;
		}

		public Vector3d Normal(double v)
		{
			double num = 0.0 - Math.Sin(v);
			return (Math.Cos(v) + e) * vector3d_0 - num * vector3d_1;
		}

		public Vector3d Normal(Vector3d p)
		{
			tangent_xy(p, out var vx, out var vy);
			return vy * vector3d_0 - vx * vector3d_1;
		}

		public Frame GetFrame(Vector3d p)
		{
			tangent_xy(p, out var vx, out var vy);
			Vector3d t = vx * vector3d_0 + vy * vector3d_1;
			Vector3d n = vy * vector3d_0 - vx * vector3d_1;
			return new Frame(p, t, n);
		}

		public int Intercepts(Ray line, out Vector3d i1, out Vector3d i2)
		{
			line = line.Project(conic_plane);
			Vector3d lhs = line.p - center;
			double num = Vector3d.Dot(lhs, vector3d_0);
			double num2 = Vector3d.Dot(lhs, vector3d_1);
			double num3 = Vector3d.Dot(line.v, vector3d_0);
			double num4 = Vector3d.Dot(line.v, vector3d_1);
			double num5;
			double num6;
			if (e == 1.0)
			{
				if (num4 == 0.0)
				{
					num5 = (0.0 - (num2 * num2 + 2.0 * l * num)) / (2.0 * l * num3);
					num6 = 0.0;
				}
				else
				{
					double num7 = num4 * num4;
					num5 = (0.0 - (l * num3 + num2 * num4)) / num7;
					num6 = (l * l * num3 * num3 + 2.0 * l * (num2 * num3 * num4 - num * num4 * num4)) / (num7 * num7);
				}
			}
			else
			{
				double num8 = a * a;
				double num9 = b * b;
				double num10;
				double num11;
				double num12;
				if (e < 1.0)
				{
					num10 = num9 * num3 * num3 + num8 * num4 * num4;
					num11 = num9 * num * num3 + num8 * num2 * num4;
					num12 = num9 * num * num + num8 * num2 * num2 - num8 * num9;
				}
				else
				{
					num10 = num9 * num3 * num3 - num8 * num4 * num4;
					num11 = num9 * num * num3 - num8 * num2 * num4;
					num12 = num9 * num * num - num8 * num2 * num2 - num8 * num9;
				}
				if (num10 == 0.0)
				{
					num6 = 0.0;
					num5 = (0.0 - num12) / (2.0 * num11);
				}
				else
				{
					num6 = num11 * num11 / (num10 * num10) - num12 / num10;
					num5 = (0.0 - num11) / num10;
				}
			}
			if (num6 < 0.0)
			{
				i1 = Vector3d.zero;
				i2 = Vector3d.zero;
				return 0;
			}
			if (num6 > 0.0)
			{
				num6 = Math.Sqrt(num6);
				i1 = line.Point(num5 + num6);
				i2 = line.Point(num5 - num6);
				return 2;
			}
			i1 = line.Point(num5);
			i2 = Vector3d.zero;
			return 1;
		}

		public bool point_is_real(Vector3d p)
		{
			if (e <= 1.0)
			{
				return true;
			}
			return Vector3d.Dot(p - center, vector3d_0) < 0.0;
		}

		public bool point_inside(Vector3d p)
		{
			Vector3d lhs = p - center;
			double num = Vector3d.Dot(lhs, vector3d_0);
			double num2 = Vector3d.Dot(lhs, vector3d_1);
			if (e < 1.0)
			{
				return num * num / (a * a) + num2 * num2 / (b * b) <= 1.0;
			}
			if (e > 1.0)
			{
				if (num < 0.0)
				{
					return num * num / (a * a) - num2 * num2 / (b * b) >= 1.0;
				}
				return false;
			}
			return num2 * num2 + 2.0 * l * num < 0.0;
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

			public void Set(Frame f1, Frame f2)
			{
				d = f2.p - f1.p;
				cos0 = Vector3d.Dot(f1.t.normalized, d.normalized);
				cos1 = Vector3d.Dot(f2.t.normalized, d.normalized);
				dot0 = Vector3d.Dot(f1.t.normalized, d);
				dot1 = Vector3d.Dot(f2.t.normalized, d);
				tangent_dot = Vector3d.Dot(f1.t, f2.t);
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

		public static readonly Pool<Sample> pool = new Pool<Sample>(Create, Reset);

		public static readonly Dictionary<int, List<Sample>> borrowed = new Dictionary<int, List<Sample>>();

		public Frame tgt => GetTgt(tgt_index);

		public Info info => GetInfo(tgt_index);

		public static int normal_intercepts
		{
			get
			{
				Frame frame = o1.GetFrame(p1);
				Vector3d p2 = o2.conic_plane.Intersept(new Ray(p1, frame.n));
				Vector3d vector3d = Vector3d.Cross(frame.t, o2.vector3d_2);
				if (vector3d.sqrMagnitude == 0.0)
				{
					p2 = p1;
					vector3d = frame.n;
				}
				return o2.Intercepts(new Ray(p2, vector3d), out i1, out i2);
			}
		}

		public static int normal_intercept_frames
		{
			get
			{
				Vector3d i;
				Vector3d i2;
				int num = Sample.get_normal_intercepts(o1, p1, o2, out i, out i2);
				switch (num)
				{
				case 1:
					if (!o2.point_is_real(i))
					{
						goto default;
					}
					goto IL_0080;
				case 2:
					if (!o2.point_is_real(i2))
					{
						if (!o2.point_is_real(i))
						{
							goto default;
						}
					}
					else
					{
						if (o2.point_is_real(i))
						{
							f1 = o2.GetFrame(i);
							f2 = o2.GetFrame(i2);
							break;
						}
						i = i2;
					}
					goto IL_0080;
				default:
					{
						f1 = new Frame(Vector3d.zero, Vector3d.zero, Vector3d.zero);
						f2 = new Frame(Vector3d.zero, Vector3d.zero, Vector3d.zero);
						break;
					}
					IL_0080:
					f1 = (f2 = o2.GetFrame(i));
					break;
				}
				return num;
			}
		}

		public static Sample Create()
		{
			return new Sample();
		}

		public static void Reset(Sample obj)
		{
		}

		public static void ReleaseAllBorrowed()
		{
			if (borrowed.ContainsKey(Thread.CurrentThread.ManagedThreadId))
			{
				List<Sample> list = borrowed[Thread.CurrentThread.ManagedThreadId];
				int i = 0;
				for (int count = list.Count; i < count; i++)
				{
					pool.Release(list[i]);
				}
				borrowed.Remove(Thread.CurrentThread.ManagedThreadId);
			}
		}

		public static Sample Borrow(Conic o1, Conic o2, double v)
		{
			Sample sample = pool.Borrow();
			if (!borrowed.ContainsKey(Thread.CurrentThread.ManagedThreadId))
			{
				borrowed.Add(Thread.CurrentThread.ManagedThreadId, new List<Sample>());
			}
			borrowed[Thread.CurrentThread.ManagedThreadId].Add(sample);
			sample.orbit1 = o1;
			sample.orbit2 = o2;
			sample.v = v;
			sample.region_edge = false;
			sample.tgt_index = -1;
			if (o1.e >= 1.0 && (v <= o1.min_v || v >= o1.max_v))
			{
				sample.valid = false;
				return sample;
			}
			Vector3d vector3d = o1.Point(v);
			sample.src = o1.GetFrame(vector3d);
			int num = Sample.get_normal_intercept_frames(o1, vector3d, o2, out sample.tgt1, out sample.tgt2);
			sample.valid = num != 0;
			if (sample.valid)
			{
				sample.info1.Set(sample.src, sample.tgt1);
				sample.info2.Set(sample.src, sample.tgt2);
			}
			return sample;
		}

		public Info GetInfo(int index)
		{
			if (index <= 0)
			{
				return info1;
			}
			return info2;
		}

		public Frame GetTgt(int index)
		{
			if (index <= 0)
			{
				return tgt1;
			}
			return tgt2;
		}
	}

	public class PairOfIntervals
	{
		public static readonly Pool<PairOfIntervals> pool = new Pool<PairOfIntervals>(Create, Reset);

		public List<Interval> list_0 = new List<Interval>();

		public List<Interval> list_1 = new List<Interval>();

		public List<Interval> this[int key] => key switch
		{
			0 => list_0, 
			1 => list_1, 
			_ => throw new ArgumentOutOfRangeException("PairOfIntervals asked for key " + key + " but only allows 0 or 1"), 
		};

		public static PairOfIntervals Create()
		{
			return new PairOfIntervals();
		}

		public static void Reset(PairOfIntervals obj)
		{
			obj.list_0.Clear();
			obj.list_1.Clear();
		}

		public static PairOfIntervals Borrow()
		{
			return pool.Borrow();
		}

		public void Release()
		{
			pool.Release(this);
		}

		public void ReleaseContent()
		{
			int i = 0;
			for (int count = list_0.Count; i < count; i++)
			{
				list_0[i].Release();
			}
			int j = 0;
			for (int count2 = list_1.Count; j < count2; j++)
			{
				list_1[j].Release();
			}
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

		public static readonly Pool<Interval> pool = new Pool<Interval>(Create, Reset);

		public static Interval Create()
		{
			return new Interval();
		}

		public static void Reset(Interval obj)
		{
		}

		public void Release()
		{
			pool.Release(this);
		}

		public static Interval Borrow(Sample s1, Sample s2, int tgt_index)
		{
			Interval interval = pool.Borrow();
			interval.s1 = s1;
			interval.s2 = s2;
			interval.tgt_index = tgt_index;
			interval.info1 = s1.GetInfo(tgt_index);
			interval.info2 = s2.GetInfo(tgt_index);
			interval.void_region = s1.region_edge && s2.region_edge;
			interval.has_root = !interval.void_region && interval.info1.dot1 * interval.info2.dot1 <= 0.0;
			if (interval.has_root)
			{
				double num = interval.info1.dot1 * interval.info1.tangent_dot;
				double num2 = interval.info2.dot1 * interval.info2.tangent_dot;
				interval.maximum = num >= 0.0 && num2 <= 0.0;
				interval.minimum = num <= 0.0 && num2 >= 0.0;
			}
			interval.prev = (interval.next = null);
			return interval;
		}

		public void Link(Interval prev, Interval next)
		{
			this.prev = prev;
			this.next = next;
			if (prev != null)
			{
				prev.next = this;
			}
			if (next != null)
			{
				next.prev = this;
			}
		}

		public List<Interval> Subdivide()
		{
			double num = s2.v - s1.v;
			if (num < 0.0)
			{
				num += Math.PI * 2.0;
			}
			double num2 = s1.v + num * 0.6180339887498949 * 0.6180339887498949;
			double num3 = s1.v + num * 0.6180339887498949;
			if (num2 >= Math.PI)
			{
				num2 -= Math.PI;
			}
			if (num3 >= Math.PI)
			{
				num3 -= Math.PI;
			}
			Conic orbit = s1.orbit1;
			Conic orbit2 = s1.orbit2;
			Sample sample = Sample.Borrow(orbit, orbit2, num2);
			Sample sample2 = Sample.Borrow(orbit, orbit2, num3);
			List<Interval> list = ListPool<Interval>.Instance.Borrow();
			if (!sample.valid && !sample2.valid)
			{
				sample = find_region_edge(orbit, orbit2, s1.v, num2);
				sample2 = find_region_edge(orbit, orbit2, num3, s2.v);
				list.Add(Borrow(s1, sample, tgt_index));
				list.Add(Borrow(sample, sample2, tgt_index));
				list.Add(Borrow(sample2, s2, tgt_index));
			}
			else if (!sample.valid && sample2.valid)
			{
				Sample sample3 = find_region_edge(orbit, orbit2, s1.v, num2);
				sample = find_region_edge(orbit, orbit2, num2, num3);
				list.Add(Borrow(s1, sample3, tgt_index));
				list.Add(Borrow(sample3, sample, tgt_index));
				list.Add(Borrow(sample, sample2, tgt_index));
				list.Add(Borrow(sample2, s2, tgt_index));
			}
			else if (sample.valid && !sample2.valid)
			{
				sample2 = find_region_edge(orbit, orbit2, num2, num3);
				Sample sample3 = find_region_edge(orbit, orbit2, num3, s2.v);
				list.Add(Borrow(s1, sample, tgt_index));
				list.Add(Borrow(sample, sample2, tgt_index));
				list.Add(Borrow(sample2, sample3, tgt_index));
				list.Add(Borrow(sample3, s2, tgt_index));
			}
			else
			{
				list.Add(Borrow(s1, sample, tgt_index));
				list.Add(Borrow(sample, sample2, tgt_index));
				list.Add(Borrow(sample2, s2, tgt_index));
			}
			Interval interval = prev;
			int i;
			for (i = 0; i < list.Count - 1; i++)
			{
				list[i].Link(interval, list[i + 1]);
				interval = list[i];
			}
			list[i].Link(interval, next);
			return list;
		}

		public Sample FindRoot()
		{
			Sample sample = s1;
			Sample sample2 = s2;
			double num = sample.v;
			double num2 = sample2.v;
			Conic orbit = sample.orbit1;
			Conic orbit2 = sample.orbit2;
			if (num2 < num)
			{
				num2 += Math.PI * 2.0;
			}
			Sample sample3;
			while (true)
			{
				double num3 = (num + num2) / 2.0;
				sample3 = Sample.Borrow(orbit, orbit2, num3);
				if (sample3.valid)
				{
					Sample.Info info = sample3.GetInfo(tgt_index);
					double num4 = info.dot1 * info.tangent_dot;
					if (num2 - num < 1E-08 || num4 == 0.0)
					{
						break;
					}
					if (num4 < 0.0)
					{
						if (minimum)
						{
							sample = sample3;
							num = num3;
						}
						else
						{
							sample2 = sample3;
							num2 = num3;
						}
					}
					else if (num4 > 0.0)
					{
						if (minimum)
						{
							sample2 = sample3;
							num2 = num3;
						}
						else
						{
							sample = sample3;
							num = num3;
						}
					}
					continue;
				}
				Sample sample4 = find_region_edge(orbit, orbit2, num, num3);
				Sample sample5 = find_region_edge(orbit, orbit2, num3, num2);
				Interval interval = Borrow(sample, sample4, tgt_index);
				Interval interval2 = Borrow(sample5, sample2, tgt_index);
				if (interval.has_root)
				{
					Sample result = interval.FindRoot();
					interval.Release();
					return result;
				}
				if (interval2.has_root)
				{
					Sample result2 = interval2.FindRoot();
					interval2.Release();
					return result2;
				}
				return null;
			}
			sample3.tgt_index = tgt_index;
			sample3.minimum = minimum;
			return sample3;
		}

		public static Sample find_region_edge(Conic orbit1, Conic orbit2, double v1, double v2)
		{
			if (v2 < v1)
			{
				v2 += Math.PI * 2.0;
			}
			Sample sample = Sample.Borrow(orbit1, orbit2, v1);
			Sample sample2 = Sample.Borrow(orbit1, orbit2, v2);
			Sample sample3;
			while (true)
			{
				double num = (v1 + v2) / 2.0;
				if (v2 - v1 < 1E-08)
				{
					break;
				}
				sample3 = Sample.Borrow(orbit1, orbit2, num);
				if (!sample.valid)
				{
					if (sample3.valid)
					{
						v2 = num;
						sample2 = sample3;
					}
					else
					{
						v1 = num;
						sample = sample3;
					}
				}
				else if (sample3.valid)
				{
					v1 = num;
					sample = sample3;
				}
				else
				{
					v2 = num;
					sample2 = sample3;
				}
			}
			sample3 = ((!sample2.valid) ? sample : sample2);
			sample3.region_edge = true;
			return sample3;
		}
	}

	public static double sample_v(int index, int count, Conic o)
	{
		return (double)(2 * index - count) * o.max_v / (double)count;
	}

	public static void add_interval(PairOfIntervals intervals, Sample start, Sample end)
	{
		intervals[0].Add(Interval.Borrow(start, end, 0));
		intervals[1].Add(Interval.Borrow(start, end, 1));
	}

	public static Sample check_void_transition(PairOfIntervals intervals, Sample prev, Sample samp, Sample ival_start)
	{
		if (prev != null && prev.valid != samp.valid)
		{
			Sample sample = Interval.find_region_edge(prev.orbit1, prev.orbit2, prev.v, samp.v);
			if (ival_start != null)
			{
				add_interval(intervals, ival_start, sample);
			}
			ival_start = sample;
		}
		return ival_start;
	}

	public static PairOfIntervals sample_orbits(Conic orbit1, Conic orbit2, int count)
	{
		PairOfIntervals pairOfIntervals = PairOfIntervals.Borrow();
		Sample sample = null;
		Sample sample2 = null;
		Sample sample3 = null;
		Sample sample4 = null;
		Sample sample5 = null;
		int count2 = count;
		if (orbit1.e >= 1.0)
		{
			count++;
		}
		for (int i = 0; i < count; i++)
		{
			double v = sample_v(i, count2, orbit1);
			Sample sample6 = Sample.Borrow(orbit1, orbit2, v);
			if (sample6.valid)
			{
				if (sample3 == null)
				{
					sample3 = sample6;
				}
				sample = check_void_transition(pairOfIntervals, sample5, sample6, sample);
				sample2 = sample6;
			}
			else
			{
				sample = check_void_transition(pairOfIntervals, sample5, sample6, sample);
				sample2 = null;
			}
			if (sample != null && sample2 != null)
			{
				add_interval(pairOfIntervals, sample, sample2);
			}
			if (sample2 != null)
			{
				sample = sample2;
			}
			sample2 = null;
			sample5 = sample6;
			if (i == 0)
			{
				sample4 = sample5;
			}
		}
		if (orbit1.e < 1.0)
		{
			if ((sample5.valid && !sample4.valid) || (!sample5.valid && sample4.valid))
			{
				Sample sample7 = Interval.find_region_edge(orbit1, orbit2, sample5.v, sample4.v);
				if (sample != null)
				{
					add_interval(pairOfIntervals, sample, sample7);
				}
				sample = sample7;
			}
			if (sample != null && sample3 != null)
			{
				add_interval(pairOfIntervals, sample, sample3);
			}
		}
		return pairOfIntervals;
	}

	public static PairOfIntervals find_search_intervals(PairOfIntervals ivals)
	{
		PairOfIntervals pairOfIntervals = PairOfIntervals.Borrow();
		for (int i = 0; i < 2; i++)
		{
			for (int j = 0; j < ivals[i].Count; j++)
			{
				Interval interval = ivals[i][j];
				if (interval.has_root)
				{
					pairOfIntervals[i].Add(interval);
				}
				else
				{
					interval.Release();
				}
			}
		}
		ivals.Release();
		return pairOfIntervals;
	}

	public static void add_crossing_subdivisions(List<Interval> intervals, Interval ival, bool reversed)
	{
		Vector3d p = ival.s1.src.p;
		Vector3d p2 = ival.s2.src.p;
		if (ival.s1.orbit2.point_inside(p) == ival.s1.orbit2.point_inside(p2))
		{
			intervals.Add(ival);
			return;
		}
		List<Interval> list = ival.Subdivide();
		if (reversed)
		{
			int i;
			for (i = 0; i < list.Count - 1; i++)
			{
				intervals.Add(list[i]);
			}
			add_crossing_subdivisions(intervals, list[i], reversed);
		}
		else
		{
			add_crossing_subdivisions(intervals, list[0], reversed);
			for (int i = 1; i < list.Count; i++)
			{
				intervals.Add(list[i]);
			}
		}
		ListPool<Interval>.Instance.Release(list);
	}

	public static PairOfIntervals subdivide_intervals(PairOfIntervals ivals)
	{
		PairOfIntervals pairOfIntervals = PairOfIntervals.Borrow();
		int num = 0;
		int num2 = ivals[0].Count - 1;
		int num3 = ivals[0].Count / 2 - 1;
		int num4 = num3 + 1;
		for (int i = 0; i < 2; i++)
		{
			add_crossing_subdivisions(pairOfIntervals[i], ivals[i][num], reversed: false);
			for (int j = num + 1; j < num3; j++)
			{
				pairOfIntervals[i].Add(ivals[i][j]);
			}
			add_crossing_subdivisions(pairOfIntervals[i], ivals[i][num3], reversed: false);
			add_crossing_subdivisions(pairOfIntervals[i], ivals[i][num4], reversed: true);
			for (int k = num4 + 1; k < num2; k++)
			{
				pairOfIntervals[i].Add(ivals[i][k]);
			}
			add_crossing_subdivisions(pairOfIntervals[i], ivals[i][num2], reversed: true);
			int l = 0;
			for (int count = ivals[i].Count; l < count; l++)
			{
				Interval interval = ivals[i][l];
				if (!pairOfIntervals[i].Contains(interval))
				{
					interval.Release();
				}
			}
		}
		ivals.Release();
		return pairOfIntervals;
	}

	public static List<Sample> Intercepts(Conic orbit1, Conic orbit2, int samples)
	{
		List<Sample> list = ListPool<Sample>.Instance.Borrow();
		PairOfIntervals ivals = sample_orbits(orbit1, orbit2, samples);
		ivals = subdivide_intervals(ivals);
		ivals = find_search_intervals(ivals);
		for (int i = 0; i < 2; i++)
		{
			for (int num = ivals[i].Count - 1; num >= 0; num--)
			{
				Sample sample = ivals[i][num].FindRoot();
				if (sample != null)
				{
					list.Add(sample);
				}
			}
		}
		ivals.ReleaseContent();
		ivals.Release();
		return list;
	}
}
