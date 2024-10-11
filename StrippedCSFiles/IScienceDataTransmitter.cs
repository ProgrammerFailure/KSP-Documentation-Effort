using System.Collections.Generic;

public interface IScienceDataTransmitter
{
	float DataRate { get; }

	double DataResourceCost { get; }

	bool CanTransmit();

	bool IsBusy();

	void TransmitData(List<ScienceData> dataQueue);
}
