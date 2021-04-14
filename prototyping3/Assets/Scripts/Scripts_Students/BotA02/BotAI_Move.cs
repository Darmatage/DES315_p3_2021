using System.Linq;
using UnityEngine;

[RequireComponent(typeof(BotBasic_Move))]
public class BotAI_Move : MonoBehaviour
{
	private BotBasic_Move _basicMove;
	private Rigidbody _rigidbody;
	private Collider[] _overlapSphereResults = new Collider[20];
	private float[] _moved;
	private int _samples, _currentSample;
	private Vector3 _lastPos;
	private Animator _animator;
	private GameObject _enemy;

	private void Awake()
	{
		_basicMove = GetComponent<BotBasic_Move>();
		_rigidbody = GetComponent<Rigidbody>();
		_animator = GetComponent<Animator>();
	}

	private GameObject GetEnemy()
	{
		GameHandler handler = GameObject.FindWithTag("GameHandler").GetComponent<GameHandler>();
		if (transform.root.CompareTag("Player1"))
		{
			return handler.Player2Holder.transform.GetChild(0).gameObject;
		}

		return handler.Player1Holder.transform.GetChild(0).gameObject;
	}
	
	private void Start()
	{
		_samples = Mathf.FloorToInt(1.0f / Time.deltaTime);
		_moved = new float[_samples];
		for (int i = 0; i < _samples; ++i)
		{
			_moved[i] = 1;
		}

		_lastPos = transform.position;

		_enemy = GetEnemy();
	}

	private void LateUpdate()
	{
		if(_basicMove.isGrabbed)
			return;

		if (_basicMove.isGrounded)
		{
			if (_moved.Average() < .0001f)
			{
				transform.Rotate(0, _basicMove.rotateSpeed * Time.deltaTime, 0);
				transform.Translate(0, 0, -_basicMove.moveSpeed * Time.deltaTime);
			}
			else
			{
				Move();
			}
		}
		else if (_basicMove.isTurtled && _basicMove.canFlip)
		{
			_rigidbody.AddForce(_rigidbody.centerOfMass + new Vector3(_basicMove.jumpSpeed / 2, 0, _basicMove.jumpSpeed / 2), ForceMode.Impulse);
			transform.Rotate(150, 0, 0);
			_rigidbody.velocity = Vector3.zero;
			_rigidbody.angularVelocity = Vector3.zero;
		}
		
		var position = transform.position;
		_moved[_currentSample] = (_lastPos - position).sqrMagnitude;
		if (++_currentSample >= _samples)
			_currentSample = 0;
		_lastPos = position;
	}

	private void Move()
	{
		var t = transform;
		var position = t.position;
		var forward = t.forward;
		var right = t.right;
		var up = t.up;
		
		// check forward
		int overlaps = Physics.OverlapSphereNonAlloc(position + forward * 3.25f + up, 1, _overlapSphereResults);

		if (overlaps == 0)
		{
			t.Translate(0, 0, _basicMove.moveSpeed * Time.deltaTime);
			Vector3 toEnemy = _enemy.transform.position - position;
			float angleTo = Vector3.SignedAngle(forward, toEnemy, up);
			if (Mathf.Abs(angleTo) > 45)
			{
				int turnDirection = angleTo > 0 ? 1 : -1;
				t.Rotate(0, _basicMove.rotateSpeed * turnDirection * Time.deltaTime, 0);
			}
			
			return;
		}
		
		if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Default") && OverlappingT<BotBasic_Move>(overlaps))
		{
			_animator.Play("Hammer");
			return;
		}

		if (OverlappingT<BotBasic_Damage>(overlaps))
		{
			t.Translate(0, 0, -_basicMove.moveSpeed * Time.deltaTime);
			return;
		}

		// check right
		overlaps = Physics.OverlapSphereNonAlloc(position + forward * 3 + right * 3 + up, 1, _overlapSphereResults);

		if (overlaps > 0)
		{
			if (OverlappingT<BotBasic_Move>(overlaps))
			{
				t.Rotate(0, _basicMove.rotateSpeed * Time.deltaTime, 0);
				return;
			}
			
			t.Rotate(0, -_basicMove.rotateSpeed * Time.deltaTime, 0);
			return;
			
		}

		// check left
		overlaps = Physics.OverlapSphereNonAlloc(position + forward * 3 + right * -3 + up, 1, _overlapSphereResults);

		if (overlaps > 0 && OverlappingT<BotBasic_Move>(overlaps))
		{
			t.Rotate(0, -_basicMove.rotateSpeed * Time.deltaTime, 0);
			return;
		}
		
		t.Rotate(0, _basicMove.rotateSpeed * Time.deltaTime, 0);
	}
	
	private bool OverlappingT<T>(int overlaps)
	{
		for (int i = 0; i < overlaps; i++)
		{
			if (_overlapSphereResults[i].TryGetComponent(out T _))
				return true;
		}

		return false;
	}
	
	
}
