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

	[Header("Velocidade")]
	public float moveSpeed = 10;
	public float jumpForce = 100;

	[Header("Trigging")]
	public PlayerState currentState = PlayerState.IDDLE;
	public LayerMask layer;
	public float rayDistance = 1.18f;

	[Header("Interação")]
	public Vector3 interactionPosition;
	public float interactionRange;

	[Header("Visuais")]
	public Transform graphicsRenderer;

	private Rigidbody2D myRigidbody2D;

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
		float horizontalAxis = Input.GetAxis("Horizontal");
		transform.Translate(horizontalAxis * moveSpeed * Time.deltaTime, 0, 0);
		if (Input.GetButtonDown("Jump") && HasColliderBottom()) Jump();
	
		FlipDirection(horizontalAxis);
	}


	/// <summary>
	/// Altera a escala x do objeto atual para dar um flip em todos os elementos
	/// </summary>
	/// <param name="horizontalValue">Valor da axi horizontal</param>
	private void FlipDirection(float horizontalValue)
	{
		float direction = transform.localScale.x;
		if (horizontalValue != 0)
		{
			direction = horizontalValue < 0.0f ? -1 : 1;
			interactionPosition.x = Mathf.Abs(interactionPosition.x);

			if (interactionPosition.x > 0 && direction < 0)
				interactionPosition.x *= direction;

		}
		transform.localScale = new Vector3(direction, 1, 1);
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
		Vector3 targetRayPosition = transform.position - (transform.up * rayDistance);
		// Desenha a linha de debug do raycast
		Gizmos.color = Color.green;
		Gizmos.DrawLine(transform.position, targetRayPosition);

		// Desenha o debug da area de interação
		Gizmos.color = Color.blue;
		Gizmos.DrawWireSphere(transform.position + interactionPosition, interactionRange);
	}
}
