using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
	public static Game instance;

	private void Awake()
	{
		if(Game.instance !=null)
		instance = this;
	} 
}
