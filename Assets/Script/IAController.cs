﻿using System.Collections;
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
		DIE
	}

	public IAState currentState = IAState.IDLE;
	[Header("Movimentação")]
	public float moveSpeed = 5;
	public float maxMoveDistance = 2;

	[Header("Temporização")]
	public float minRandomTime = 0;
	public float maxRandomTime = 7;

	[Header("Interação")]
	public LayerMask layerMaskObjects;
	public float maxInteractDistance = 5;
	public Vector3 interactionPosition;
	public float interactionRange = 1;

	private float targetTime = 0;
	private Vector2 targetPosition = Vector2.zero;

	/// <summary>
	/// Altera o estado da maquina de estados
	/// </summary>
	/// <param name="newState">Novo estado que será definido</param>
	private void ChangeState(IAState newState)
	{
		currentState = newState;
	}

	private void Start()
	{
		UpdateTargetTime();
	}

	/// <summary>
	/// Atualiza o tempo randomico
	/// </summary>
	private void UpdateTargetTime()
	{
		targetTime = Time.time + Random.Range(minRandomTime, maxRandomTime);
	}

	private void Update()
	{
		switch (currentState)
		{
			case IAState.IDLE:
				{
					// Gatilhos para mudar de estado
					if (targetTime <= Time.time) {
						targetPosition = transform.position;
						targetPosition.x = Random.Range(transform.position.x-maxMoveDistance, transform.position.x + maxMoveDistance);
						FlipDirection();
						UpdateTargetTime();
						ChangeState(IAState.MOVING);
					}
					break;
				}
			case IAState.MOVING:
				{
					// Chamada das funções principais
					Move();
					// Gatilhos para mudar de estado
					if (targetTime <= Time.time)
					{
						UpdateTargetTime();
						ChangeState(IAState.IDLE);
					}
					break;
				}
		}
	}

	private void OnTriggerEnter2D (Collider2D collider)
	{
		if (currentState == IAState.MOVING && collider.tag != "Ground") {
			UpdateTargetTime();
			ChangeState(IAState.IDLE);
		}
	}

	/// <summary>
	/// Move o player até uma posição especifica
	/// </summary>
	private void Move()
	{
		transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed*Time.deltaTime);
	}

	/// <summary>
	/// Checa a direção atual da sprite da axis e altera a escala x
	/// do objeto atual para dar um flip em todos os elementos
	/// </summary>
	private void FlipDirection()
	{
		int direction = 1;
		if (targetPosition.x < transform.position.x)
			direction = -1;

		transform.localScale = new Vector3(direction, 1, 1);

		interactionPosition.x = Mathf.Abs(interactionPosition.x);

		if (interactionPosition.x > 0 && direction < 0)
			interactionPosition.x *= direction;
	}

	/// <summary>
	/// Pega os colisores de objetos de interação ao redor do player
	/// </summary>
	private Collider2D[] getCollidersAroundInteractPoint()
	{
		return Physics2D.OverlapCircleAll(transform.position + interactionPosition, interactionRange, layerMaskObjects);
	}

	private void OnDrawGizmos()
	{
		Vector3 currentPosition = transform.position + (-Vector3.right * maxMoveDistance);
		Vector3 targetPosition = transform.position + (Vector3.right * maxMoveDistance);
		Gizmos.color = Color.yellow;
		Gizmos.DrawLine(currentPosition, targetPosition);

		// Desenha o debug da area de interação
		Gizmos.color = Color.blue;
		Gizmos.DrawWireSphere(transform.position + interactionPosition, interactionRange);
	}


}
