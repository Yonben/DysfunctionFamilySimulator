using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

[CreateAssetMenu]
public class PlayerController : ScriptableObject
{
	PlayerIndex playerIndex;
	GamePadState state;
	GamePadState prevState;

	public void SetPlayerIndex(PlayerIndex playerIndex)
	{
		this.playerIndex = playerIndex;
		Debug.Log("player get index" + this.playerIndex.ToString());
	}
}
