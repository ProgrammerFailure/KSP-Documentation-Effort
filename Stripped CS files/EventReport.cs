public class EventReport
{
	public FlightEvents eventType;

	public Part origin;

	public string sender;

	public string other;

	public int stage;

	public string msg;

	public float param;

	public EventReport(FlightEvents type, Part eventCreator, string name = "an unidentified object", string otherName = "an unidentified object", int stageNumber = 0, string customMsg = "", float param = 0f)
	{
		origin = eventCreator;
		eventType = type;
		sender = name;
		other = otherName;
		stage = stageNumber;
		msg = customMsg;
		this.param = param;
	}
}
