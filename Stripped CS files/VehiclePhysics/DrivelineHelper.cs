using System;

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

		public float limitedSlipRatio = 5f;

		public float finalRatio = 3.7f;

		public TransferCase transferCase;

		public float transferCaseRatio = 1f;
	}

	public Settings settings = new Settings();

	public Driveline.Settings GetDrivelineSettings()
	{
		Driveline.Settings settings = new Driveline.Settings();
		switch (this.settings.driveWheels)
		{
		case DriveWheels.Rear:
			settings.drivenAxles = Driveline.DrivenAxles.SingleAxle;
			settings.firstDrivenAxle = 1;
			break;
		case DriveWheels.Front:
			settings.drivenAxles = Driveline.DrivenAxles.SingleAxle;
			settings.firstDrivenAxle = 0;
			break;
		case DriveWheels.AllWheel:
			settings.drivenAxles = Driveline.DrivenAxles.TwoAxles;
			settings.twoDrivenAxlesConfig = Driveline.TwoAxlesConfig.CenterDifferential;
			settings.firstDrivenAxle = 0;
			settings.secondDrivenAxle = 1;
			break;
		}
		return settings;
	}

	public Differential.Settings GetAxleDifferentialSettings()
	{
		Differential.Settings settings = new Differential.Settings();
		switch (this.settings.differentialType)
		{
		case DifferentialType.Open:
			settings.type = Differential.Type.Open;
			break;
		case DifferentialType.LimitedSlip:
			settings.type = Differential.Type.TorqueBias;
			settings.powerRatio = this.settings.limitedSlipRatio;
			settings.coastRatio = this.settings.limitedSlipRatio;
			break;
		case DifferentialType.Locked:
			settings.type = Differential.Type.Locked;
			break;
		}
		settings.gearRatio = this.settings.finalRatio;
		return settings;
	}

	public Differential.Settings GetCenterDifferentialSettings()
	{
		Differential.Settings settings = new Differential.Settings();
		switch (this.settings.transferCase)
		{
		case TransferCase.Locked:
			settings.type = Differential.Type.Locked;
			break;
		case TransferCase.Open:
			settings.type = Differential.Type.Open;
			break;
		}
		settings.gearRatio = this.settings.transferCaseRatio;
		return settings;
	}
}
