using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundController : MonoBehaviour
{

	public enum Sound
	{
		JUMP,
		DEAD
	}

	[Header("Audio clips do player")]
	public AudioClip[] playerJump;
	public AudioClip[] playerDead;

	private AudioSource audioSource;

	/// <summary>
	/// Instancia atual do SoundController
	/// </summary>
	public static SoundController instance;

	private void Awake()
	{
		instance = this;
		// Pegamos o audio source
		audioSource = GetComponent<AudioSource>();
	}
	
	/// <summary>
	/// Reproduz um efeito sonoro a nível global
	/// </summary>
	/// <param name="soundType">Efeito sonoro que será reproduzido</param>
	public void PlaySound(Sound soundType)
	{
		switch (soundType)
		{
			case Sound.JUMP:
				{
					int randomizedIndex = Random.Range(0, playerJump.Length);
					audioSource.PlayOneShot(playerJump[randomizedIndex]);
					break;
				}
			case Sound.DEAD:
				{
					int randomizedIndex = Random.Range(0, playerDead.Length);
					audioSource.PlayOneShot(playerDead[randomizedIndex]);
					break;
				}
		}
	}
}
