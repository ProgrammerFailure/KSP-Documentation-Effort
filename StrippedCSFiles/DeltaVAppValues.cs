using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class DeltaVAppValues
{
	public class InfoLine
	{
		public string name;

		public string displayAppName;

		public string displayStageName;

		public Func<DeltaVStageInfo, DeltaVSituationOptions, string> infoValue;

		public string valueSuffix;

		private bool enabled;

		public bool Enabled
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				throw null;
			}
			[MethodImpl(MethodImplOptions.NoInlining)]
			set
			{
				throw null;
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public InfoLine(string name, string appName, string stageName, Func<DeltaVStageInfo, DeltaVSituationOptions, string> value)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public InfoLine(string name, string appName, string stageName, Func<DeltaVStageInfo, DeltaVSituationOptions, string> value, string valueSuffix)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public InfoLine(string name, string appName, string stageName, Func<DeltaVStageInfo, DeltaVSituationOptions, string> value, bool enabled)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public InfoLine(string name, string appName, string stageName, Func<DeltaVStageInfo, DeltaVSituationOptions, string> value, string valueSuffix, bool enabled)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		internal void InitEnabledValue(bool enabled)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public string GetValue(DeltaVStageInfo stage, DeltaVSituationOptions situation)
		{
			throw null;
		}
	}

	public CelestialBody body;

	public DeltaVSituationOptions situation;

	public double altitude;

	public List<InfoLine> infoLines;

	public List<string> infoLinesEnabled;

	public float atmPressure
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public double atmDensity
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public DeltaVAppValues()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void LoadEnabledInfoLines(string settingString)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void UpdateEnabledInfoItems()
	{
		throw null;
	}
}
