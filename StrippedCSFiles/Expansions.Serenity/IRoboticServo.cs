using UnityEngine;

namespace Expansions.Serenity;

internal interface IRoboticServo
{
	Rigidbody AttachServoRigidBody(AttachNode node);

	bool ServoTransformCollider(string colName);

	GameObject MovingObject();

	GameObject BaseObject();

	Rigidbody NodeRigidBody(AttachNode node);
}
