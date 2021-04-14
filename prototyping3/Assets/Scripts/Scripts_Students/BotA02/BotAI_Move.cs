using UnityEngine;

[RequireComponent(typeof(BotBasic_Move))]
public class BotAI_Move : MonoBehaviour
{
	private BotBasic_Move basicMove;
	private Rigidbody rigidbody;

	private void Awake()
	{
		basicMove = GetComponent<BotBasic_Move>();
		rigidbody = GetComponent<Rigidbody>();
	}

	private void FixedUpdate()
	{
		if(basicMove.isGrabbed)
			return;

		if (basicMove.isTurtled && basicMove.canFlip)
		{
			rigidbody.AddForce(rigidbody.centerOfMass + new Vector3(basicMove.jumpSpeed / 2, 0, basicMove.jumpSpeed / 2), ForceMode.Impulse);
			transform.Rotate(150, 0, 0);
			rigidbody.velocity = Vector3.zero;
			rigidbody.angularVelocity = Vector3.zero;
			return;
		}

		if (basicMove.isGrounded)
		{
			var t = transform;
			var position = t.position;
			var forward = t.forward;
			var right = t.right;
			
			Debug.DrawLine(position, position + forward * 5 + right * 4, Color.yellow);
			Debug.DrawLine(position, position + forward * 5 + right * -4, Color.yellow);
		}
	}
}
