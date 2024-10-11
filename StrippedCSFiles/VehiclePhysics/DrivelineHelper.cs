using System;
using System.Runtime.CompilerServices;

namespace VehiclePhysics;

public class DrivelineHelper
{
	public enum DriveWheels
	{
		Rear,
		Front,
		AllWheel
	}

	public enum DifferentialType
	{
		Open,
		LimitedSlip,
		Locked
	}

	public enum TransferCase
	{
		Open,
		Locked
	}

	[Serializable]
	public class Settings
	{
		public DriveWheels driveWheels;

		public DifferentialType differentialType;

		public float limitedSlipRatio;

		public float finalRatio;

		public TransferCase transferCase;

		public float transferCaseRatio;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public Settings()
		{
			throw null;
		}
	}

	public Settings settings;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public DrivelineHelper()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Driveline.Settings GetDrivelineSettings()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Differential.Settings GetAxleDifferentialSettings()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Differential.Settings GetCenterDifferentialSettings()
	{
		throw null;
	}
}
