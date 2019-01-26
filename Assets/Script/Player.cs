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
		INTERACT,
		DIE
	}

	[Header("Configurações de velocidade")]
	public float moveSpeed = 10;
	public float jumpForce = 100;

	[Header("Configurações de trigging")]
	public LayerMask layer;
	public float rayDistance = 1.18f;

	private Rigidbody2D myRigidbody2D;

	public PlayerState currentState = PlayerState.IDDLE;

	private void Start()
	{
		myRigidbody2D = GetComponent<Rigidbody2D>();
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
		if (Input.GetButtonDown("Jump") && HasColliderBottom()) Jump();
		transform.Translate(Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime, 0, 0);
	}

	/// <summary>
	/// Aplica uma força no eixo y do rigidbody, fazendo que o o player salte
	/// </summary>
	private void Jump()
	{
		myRigidbody2D.velocity = new Vector2(myRigidbody2D.velocity.x, 0);
		myRigidbody2D.AddForce(new Vector2(0, jumpForce * Time.deltaTime), ForceMode2D.Impulse);
	}

	/// <summary>
	/// Verifica se possui algum collider embaixo, respeitando a layermask definida
	/// </summary>
	/// <returns>Se o player está no chão ou não</returns>
	private bool HasColliderBottom()
	{
		Vector3 targetPosition = transform.position - (transform.up * rayDistance);
		RaycastHit2D hit = Physics2D.Raycast(transform.position, targetPosition, rayDistance, layer);

		return hit.collider != null;
	}

	void Update()
	{
		switch (currentState)
		{
			case PlayerState.MOVE: Move(); break;
		}
	}

	private void OnDrawGizmos()
	{
		Vector3 targetPosition = transform.position - (transform.up * rayDistance);
		// Desenha a linha de debug do raycast
		Gizmos.DrawLine(transform.position, targetPosition);
	}
}
