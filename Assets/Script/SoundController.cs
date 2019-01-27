using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundController : MonoBehaviour
{

	public enum Sound
	{
		KILLED,
		INTERACT
	}

	[Header("Audio clips do player")]
	public AudioClip[] killed;
	public AudioClip[] interact;

	private AudioSource audioSource;
	public AudioSource musicAudioSource;

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
			case Sound.KILLED:
				{
					int randomizedIndex = Random.Range(0, killed.Length);
					audioSource.PlayOneShot(killed[randomizedIndex]);
					break;
				}
			case Sound.INTERACT:
				{
					int randomizedIndex = Random.Range(0, interact.Length);
					audioSource.PlayOneShot(interact[randomizedIndex]);
					break;
				}
		}
	}

	public void StopMusic()
	{
		musicAudioSource.Stop();
	}
}
