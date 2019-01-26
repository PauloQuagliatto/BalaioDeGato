using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class InteragableObject : MonoBehaviour
{

	private Rigidbody2D myRigidbody2D;

	void Start()
	{
		myRigidbody2D = GetComponent<Rigidbody2D>();
	}

	/// <summary>
	/// Desativa a fisica do objeto
	/// </summary>
	/// <param name="value"></param>
	public void TogglePhysics(bool value)
	{
		if (value) myRigidbody2D.velocity = Vector2.zero;
		myRigidbody2D.isKinematic = value;
	}
}
