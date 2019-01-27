using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{

	public static Player instance;

	public enum PlayerState
	{
		IDLE,
		MOVE,
		INTERACT,
		DIE,
		JUMP,
		SMALL_JUMP
	}

	[Header("Velocidade")]
	public float moveSpeed = 10;
	public float jumpForce = 100;
	public float smallJumpForce = 70;

	[HideInInspector]
	public float direction = 1;

	[Header("Trigging")]
	public PlayerState currentState = PlayerState.IDLE;
	private PlayerState lastState = PlayerState.IDLE;
	public LayerMask layer;
	public float rayDistance = 1.18f;

	[Header("Interação")]
	public LayerMask layerMaskObjects;
	public Vector3 interactionPosition;
	public float interactionRange;
	private bool biting = false;
	private InteragableObject interagableObj;

	public Animator myAnimator;

	private Rigidbody2D myRigidbody2D;

	private GameController gameController;

	private void Awake()
	{
		instance = this;
	}

	private void Start()
	{
		gameController = GameController.instance;
		myRigidbody2D = GetComponent<Rigidbody2D>();
	}

	void Update()
	{
		if (!gameController.gameOver && !gameController.gameWin && currentState != PlayerState.DIE) {
			float horizontalAxis = Input.GetAxis("Horizontal");
			Collider2D[] interactColliders = getCollidersAroundInteractPoint();

			if (biting)
			{
				interagableObj.transform.position = transform.position + interactionPosition;
				if (Input.GetButtonDown("Fire2")) DetachBitedObject();
			}

			switch (currentState)
			{
				case PlayerState.IDLE:
					{
						// Chamada das funções principais
						Idle();
						myAnimator.SetTrigger("idle");
						// Gatilhos para mudar de estado
						if (horizontalAxis != 0) ChangeState(PlayerState.MOVE);
						if (Input.GetButtonDown("Jump") && HasColliderBottom()) ChangeState(PlayerState.JUMP);
						if (Input.GetButtonDown("Fire1") && interactColliders.Length != 0 && !biting) ChangeState(PlayerState.INTERACT);
						if (Input.GetButtonDown("Fire2") && interactColliders.Length != 0 && !biting) Bite(interactColliders[0]);
						break;
					}
				case PlayerState.MOVE:
					{
						// Chamada das funções principais
						Move(horizontalAxis);
						myAnimator.SetTrigger("walking");
						// Gatilhos para mudar de estado
						if (horizontalAxis == 0) ChangeState(PlayerState.IDLE);
						if (Input.GetButtonDown("Jump") && HasColliderBottom()) ChangeState(PlayerState.JUMP);
						if (Input.GetButtonDown("Fire1") && interactColliders.Length != 0 && !biting) ChangeState(PlayerState.INTERACT);
						if (Input.GetButtonDown("Fire2") && interactColliders.Length != 0 && !biting) Bite(interactColliders[0]);
						break;
					}
				case PlayerState.JUMP:
					{
						// Chamada das funções principais
						Jump();
						myAnimator.SetTrigger("idle");
						// Gatilhos para mudar de estado
						ChangeState(lastState);
						break;
					}
				case PlayerState.SMALL_JUMP:
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
						myAnimator.SetTrigger("interact");
						// Gatilhos para mudar de estado
						ChangeState(lastState);
						break;
					}
			}
			FlipDirection(horizontalAxis);
		}
	}

	/// <summary>
	/// Remove o objeto da boca do gato
	/// </summary>
	private void DetachBitedObject()
	{
		biting = false;
		if (interagableObj) interagableObj.TogglePhysics(true);
	}

	/// <summary>
	/// Adiciona um objeto a boca do gato para ser carregado
	/// </summary>
	/// <param name="collider">collider que será interagido</param>
	private void Bite(Collider2D collider)
	{
		biting = true;
		interagableObj = collider.GetComponent<InteragableObject>();
		interagableObj.TogglePhysics(false);
	}

	private void OnTriggerEnter2D(Collider2D collider)
	{
		if (collider.tag == "Damagable")
		{
			ChangeState(PlayerState.DIE);
			GameController.instance.Killed();
		}
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.tag == "Damagable")
		{
			ChangeState(PlayerState.DIE);
			GameController.instance.Killed();
		}
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
	/// Estado de idle da maquina de estados, aqui o jogador está totalmente parado
	/// aguardando por inputs para se mover
	/// </summary>
	private void Idle()
	{
		// TODO - aplicar animação e funções de idle aqui
	}

	/// <summary>
	/// Movimenta o player na cena
	/// </summary>
	/// <param name="horizontalMove">Movimento horizontal que será aplicado</param>
	private void Move(float horizontalMove)
	{
		float move = biting ? moveSpeed / 2 : moveSpeed;
		transform.Translate(horizontalMove * move * Time.deltaTime, 0, 0);
	}

	/// <summary>
	/// Aplica uma força no eixo y do rigidbody, fazendo que o o player salte
	/// </summary>
	private void Jump()
	{
		float force = biting ? jumpForce / 2 : jumpForce;
		myRigidbody2D.velocity = new Vector2(myRigidbody2D.velocity.x, 0);
		myRigidbody2D.AddForce(new Vector2(0, force * Time.deltaTime), ForceMode2D.Impulse);
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
		RaycastHit2D hit = Physics2D.Raycast(transform.position, -transform.up, rayDistance, layer);

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
