using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
	public enum PlayerType {Dad, Mum, Kid, Dog}
	
	public static GameManager instance = null;

	public List<PlayableCharacter> players;

	private int playerAliveCount;

	[SerializeField] private Text GameOverText;
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


		playerAliveCount = players.Count;
		GameOverText.enabled = false;
		Score = 0;
	}


	public void PlayerDie(PlayableCharacter player)
	{
		playerAliveCount--;
		if (playerAliveCount <= 1) //1 because of the dog that can't die
		{
			//todo
			GameOverText.enabled = true;
		}
	}
	
}
