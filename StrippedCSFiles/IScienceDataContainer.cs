public interface IScienceDataContainer
{
	ScienceData[] GetData();

	void ReturnData(ScienceData data);

	void DumpData(ScienceData data);

	void ReviewData();

	void ReviewDataItem(ScienceData data);

	int GetScienceCount();

	bool IsRerunnable();
}
