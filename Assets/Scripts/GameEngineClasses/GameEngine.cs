using UnityEngine;
using System.Collections;

/// <summary>
/// We're running our own updates now!
/// </summary>
public class GameEngine : UnitySingleton<GameEngine>
{
	int currId;
	GameMono[] monos;
	GameMono[] Monos
	{
		get
		{
			if (monos == null)
			{
				monos = new GameMono[] { };
			}
			return monos;
		}
		set
		{
			monos = value;
		}
	}

	// Use this for initialization
	void Awake ()
	{
		currId = 0;
	}
	
	// Update is called once per frame
	void Update () {
		for (int i = 0; i < Monos.Length; i++)
		{
			if (Monos[i].enabled)
			{
				Monos[i].JUpdate();
			}
		}
	}


	void LateUpdate()
	{
		for (int i = 0; i < Monos.Length; i++)
		{
			if (Monos[i].enabled)
			{
				Monos[i].JLateUpdate();
			}
		}
	}


	public void Subscribe(GameMono obj)
	{
		GameMono[] temp = Monos;
		Monos = new GameMono[temp.Length + 1];
		for (int i = 0; i < temp.Length; i++)
		{
			Monos[i] = temp[i];
		}
		obj.gameId = currId;
		Monos[temp.Length] = obj;
		currId++;
	}


	public void Unsubscribe(GameMono obj)
	{
		int tempId = 0;
		GameMono[] temp = Monos;
		Monos = new GameMono[temp.Length - 1];
		for (int i = 0; i < temp.Length; i++)
		{
			if (temp[i].gameId != obj.gameId)
			{
				Monos[tempId] = temp[i];
				tempId++;
			}
		}
	}
}
