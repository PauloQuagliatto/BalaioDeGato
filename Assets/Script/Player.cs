using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

	[Header("Configurações do player")]
	public float moveSpeed = 10;

	public PlayerState currentState = PlayerState.IDDLE;

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
	}

	void FixedUpdate()
	{
		switch (currentState)
		{
			case PlayerState.MOVE: Move(); break;
		}
	}



}
