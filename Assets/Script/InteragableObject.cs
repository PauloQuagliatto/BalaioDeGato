using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class InteragableObject : MonoBehaviour
{
	public bool shootable;
	private Collider2D myCollider2D;
	private Rigidbody2D myRigidbody2D;
	public Vector2 force;

	void Start()
	{
		myCollider2D = GetComponent<Collider2D>();
		myRigidbody2D = GetComponent<Rigidbody2D>();
	}

	/// <summary>
	/// Desativa a fisica do objeto
	/// </summary>
	/// <param name="value"></param>
	public void TogglePhysics(bool value)
	{
		myCollider2D.enabled = value;
		if (value) myRigidbody2D.velocity = Vector2.zero;
		myRigidbody2D.isKinematic = !value;
	}

	/// <summary>
	/// Aplica uma força baseado em uma posição
	/// </summary>
	/// <param name="position"></param>
	public void ApplyForce(Vector2 position, float direction)
	{
		if (shootable)
		{
			Vector2 targetForce = force;
			targetForce.x *= direction;
			myRigidbody2D.AddForceAtPosition(targetForce, position);
		}
	}
}
