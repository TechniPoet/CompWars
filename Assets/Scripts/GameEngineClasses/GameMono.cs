using UnityEngine;
using System.Collections;
using System;

public class GameMono : MonoBehaviour, EngineInterface
{
	public int gameId;

	public virtual void JUpdate(){}

	public virtual void JLateUpdate(){}

	// Use this for initialization
	void Awake ()
	{
		GameEngine.Instance.Subscribe(this);
	}
}
