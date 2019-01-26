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
		DIE,
		JUMP
	}

	[Header("Velocidade")]
	public float moveSpeed = 10;
	public float jumpForce = 100;
	private float direction = 1;

	[Header("Trigging")]
	public PlayerState currentState = PlayerState.IDDLE;
	private PlayerState lastState = PlayerState.IDDLE;
	public LayerMask layer;
	public float rayDistance = 1.18f;

	[Header("Interação")]
	public LayerMask layerMaskObjects;
	public Vector3 interactionPosition;
	public float interactionRange;

	[Header("Visuais")]
	public Transform graphicsRenderer;

	private Rigidbody2D myRigidbody2D;

	private void Start()
	{
		myRigidbody2D = GetComponent<Rigidbody2D>();
	}

	void Update()
	{
		float horizontalAxis = Input.GetAxis("Horizontal");
		Collider2D[] interactColliders = getCollidersAroundInteractPoint();

		switch (currentState)
		{
			case PlayerState.IDDLE:
				{
					// Chamada das funções principais
					Iddle();
					// Gatilhos para mudar de estado
					if (horizontalAxis != 0) ChangeState(PlayerState.MOVE);
					if (Input.GetButton("Jump") && HasColliderBottom()) ChangeState(PlayerState.JUMP);
					if (Input.GetButton("Fire1") && interactColliders.Length != 0) ChangeState(PlayerState.INTERACT);
					break;
				}
			case PlayerState.MOVE:
				{
					// Chamada das funções principais
					Move(horizontalAxis);
					// Gatilhos para mudar de estado
					if (horizontalAxis == 0) ChangeState(PlayerState.IDDLE);
					if (Input.GetButton("Jump") && HasColliderBottom()) ChangeState(PlayerState.JUMP);
					if (Input.GetButton("Fire1") && interactColliders.Length != 0) ChangeState(PlayerState.INTERACT);
					break;
				}
			case PlayerState.JUMP:
				{
					// Chamada das funções principais
					Jump();
					// Gatilhos para mudar de estado
					ChangeState(lastState);
					break;
				}
			case PlayerState.INTERACT:
				{
					// Chamada das funções princiais
					Interact(interactColliders);
					// Gatilhos para mudar de estado
					ChangeState(lastState);
					break;
				}
		}
		FlipDirection(horizontalAxis);
	}

	#region Maquina de estado

	/// <summary>
	/// Altera o estado da maquina de estados
	/// </summary>
	/// <param name="newState">Novo estado que será aplicado</param>
	void ChangeState(PlayerState newState)
	{
		lastState = currentState;
		currentState = newState;
	}

	/// <summary>
	/// Aplica um efeito nos elementos
	/// </summary>
	private void Interact(Collider2D[] colliders)
	{
		foreach (Collider2D collider in colliders)
		{
			InteragableObject interagableObject = collider.GetComponent<InteragableObject>();
			// Aplicamos uma força
			interagableObject.ApplyForce(transform.position + interactionPosition, direction);
		}
	}

	/// <summary>
	/// Estado de iddle da maquina de estados, aqui o jogador está totalmente parado
	/// aguardando por inputs para se mover
	/// </summary>
	private void Iddle()
	{
		// TODO - aplicar animação e funções de idle aqui
	}

	/// <summary>
	/// Movimenta o player na cena
	/// </summary>
	/// <param name="horizontalMove">Movimento horizontal que será aplicado</param>
	private void Move(float horizontalMove)
	{
		transform.Translate(horizontalMove * moveSpeed * Time.deltaTime, 0, 0);
	}

	/// <summary>
	/// Aplica uma força no eixo y do rigidbody, fazendo que o o player salte
	/// </summary>
	private void Jump()
	{
		myRigidbody2D.velocity = new Vector2(myRigidbody2D.velocity.x, 0);
		myRigidbody2D.AddForce(new Vector2(0, jumpForce * Time.deltaTime), ForceMode2D.Impulse);
	}

	#endregion

	#region Funções auxiliares

	/// <summary>
	/// Pega os colisores de objetos de interação ao redor do player
	/// </summary>
	private Collider2D[] getCollidersAroundInteractPoint()
	{
		return Physics2D.OverlapCircleAll(transform.position + interactionPosition, interactionRange, layerMaskObjects);
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

	/// <summary>
	/// Checa a direção atual da sprite da axis e altera a escala x
	/// do objeto atual para dar um flip em todos os elementos
	/// </summary>
	/// <param name="horizontalValue">Valor da axi horizontal</param>
	private void FlipDirection(float horizontalValue)
	{
		direction = transform.localScale.x;
		if (horizontalValue != 0)
		{
			direction = horizontalValue < 0.0f ? -1 : 1;
			interactionPosition.x = Mathf.Abs(interactionPosition.x);

			if (interactionPosition.x > 0 && direction < 0)
				interactionPosition.x *= direction;

		}
		transform.localScale = new Vector3(direction, 1, 1);
	}

	#endregion

	#region Funções de debug

	/// <summary>
	/// Utilizamos o Gizmoz do unity para debugar os raios de colisao
	/// </summary>
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

	#endregion
}
