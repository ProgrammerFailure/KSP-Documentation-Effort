public interface IDateTimeFormatter
{
	int Minute { get; }

	int Hour { get; }

	int Day { get; }

	int Year { get; }

	string PrintTimeLong(double time);

	string PrintTimeStamp(double time, bool days = false, bool years = false);

	string PrintTimeStampCompact(double time, bool days = false, bool years = false);

	string PrintTime(double time, int valuesOfInterest, bool explicitPositive);

	string PrintTime(double time, int valuesOfInterest, bool explicitPositive, bool logEnglish);

	string PrintTimeCompact(double time, bool explicitPositive);

	string PrintDateDelta(double time, bool includeTime, bool includeSeconds, bool useAbs);

	string PrintDateDeltaCompact(double time, bool includeTime, bool includeSeconds, bool useAbs);

	string PrintDateDeltaCompact(double time, bool includeTime, bool includeSeconds, bool useAbs, int interestedPlaces);

	string PrintDate(double time, bool includeTime, bool includeSeconds = false);

	string PrintDateNew(double time, bool includeTime);

	string PrintDateCompact(double time, bool includeTime, bool includeSeconds = false);
}
