using System.Collections.Generic;

public interface IResourceConsumer
{
	List<PartResourceDefinition> GetConsumedResources();
}
