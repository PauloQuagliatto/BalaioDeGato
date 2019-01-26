using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
	public enum PlayerState
	{
		IDDLE,
		MOVE,
		JUMP,
		INTERACT,
		DIE
	}

	[Header("Configurações de velocidade")]
	public float moveSpeed = 10;
	public float jumpForce = 100;

	[Header("Configurações de trigging")]
	public float groundRayDistance = 3;

	private Rigidbody2D rigidbody2D;

	// flags
	private bool inGround = false;

	public PlayerState currentState = PlayerState.IDDLE;

	private void Start()
	{
		rigidbody2D = GetComponent<Rigidbody2D>();
	}

	/// <summary>
	/// Altera o estado da maquina de estados
	/// </summary>
	/// <param name="newState">Novo estado que será aplicado</param>
	void ChangeState(PlayerState newState)
	{
		currentState = newState;
	}

	/// <summary>
	/// Movimenta o player na cena
	/// </summary>
	private void Move()
	{
		transform.Translate(Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime, 0, 0);
		if (Input.GetButtonDown("Jump")) Jump();
	}

	/// <summary>
	/// Faz o player dar um pulo
	/// </summary>
	private void Jump()
	{
		rigidbody2D.velocity = new Vector2(0, 0);
		rigidbody2D.AddForce(new Vector2(0, jumpForce * Time.deltaTime), ForceMode2D.Impulse);
	}

	private void CheckIsInGround()
	{
		// RayCastHit2D hit = Physics2D.Raycast(transform.position, transform.position * (Vector2.up * groundRayDistance), groundRayDistance);
	}

	void FixedUpdate()
	{
		switch (currentState)
		{
			case PlayerState.MOVE: Move(); break;
		}
	}

	private void OnDrawGizmos()
	{
		// Desenha a linha de debug do raycast
		Gizmos.DrawLine(transform.position, transform.position * (Vector2.up * groundRayDistance));
	}



}
