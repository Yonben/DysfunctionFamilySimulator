﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
	public enum PlayerType {Dad, Mum, Kid, Dog}
	
	public static GameManager instance = null;

	public List<PlayableCharacter> players;

	public int playerAliveCount;

	[SerializeField] private Text GameOverText;
	[SerializeField] private GameObject GameOverGO;
	[SerializeField] private Text ScoreText;

	private int score = 0;
	public int Score
	{
		get { return score; }
		set
		{
			score = value;
			ScoreText.text = score.ToString();
		}
	}

	private void Awake()
	{
		if (instance == null)     
			//if not, set instance to this
			instance = this; 
		//If instance already exists and it's not this:
		else if (instance != this)      
			//Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
			Destroy(gameObject);      
		//Sets this to not be destroyed when reloading scene
		DontDestroyOnLoad(gameObject);


		playerAliveCount = 4;//players.Count;
		GameOverText.enabled = false;
		GameOverGO.SetActive(false);
		Score = 0;
	}

	private void Start()
	{
		StartCoroutine(nameof(getIntervalPoints));
	}


	public void PlayerDie(PlayableCharacter player)
	{
		playerAliveCount--;
		if (playerAliveCount <= 1) //1 because of the dog that can't die
		{
			Invoke(nameof(GameOver), 2);
			
		}
	}

	private void GameOver()
	{
		GameOverText.enabled = true;
		GameOverGO.SetActive(true);
	}

	IEnumerator getIntervalPoints()
	{
		yield return new WaitForSeconds(2f);
		while (!GameOverGO.activeSelf)
		{
			Score += 1;
			yield return new WaitForSeconds(2f);
		}
	}
	
}
