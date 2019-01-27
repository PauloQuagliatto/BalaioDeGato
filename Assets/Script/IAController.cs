using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAController : MonoBehaviour
{

	public enum IAState
	{
		IDLE,
		MOVING,
		INTERACT,
		BITED,
		DIE,
		HITED
	}

	[Header("Informações dos sensores")]
	public float maxMoveDistance = 2;
	public float maxInteractDistance = 5;
		
	public IAState currentState = IAState.IDLE;

	private void OnDrawGizmos()
	{
		Vector3 currentPosition = transform.position + (-Vector3.right * maxMoveDistance);
		Vector3 targetPosition = transform.position + (Vector3.right * maxMoveDistance);
		Gizmos.color = Color.yellow;
		Gizmos.DrawLine(currentPosition, targetPosition);

		Gizmos.color = Color.blue;
		Gizmos.DrawWireSphere(transform.position, maxInteractDistance);
	}


}
