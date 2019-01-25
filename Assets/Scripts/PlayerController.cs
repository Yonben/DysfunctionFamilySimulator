using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public class PlayerController : MonoBehaviour
{
	public enum Button {A,B,X,Y}
	
	PlayerIndex playerIndex;
	GamePadState state;
	GamePadState prevState;

	public void SetPlayerIndex(PlayerIndex playerIndex)
	{
		this.playerIndex = playerIndex;
		Debug.Log("player get index" + this.playerIndex.ToString());
	}

	private void Update()
	{
		prevState = state;
		state = GamePad.GetState(playerIndex);
	}

	public bool isPressedThisFrame(Button button)
	{
		return getStateButton(prevState, button) == ButtonState.Released &&
		       getStateButton(state, button) == ButtonState.Pressed;
	}
	
	public bool isReleasedThisFrame(Button button)
	{
		return getStateButton(prevState, button) == ButtonState.Pressed &&
		       getStateButton(state, button) == ButtonState.Released;
	}

	public float getLeftStickHorizontal()
	{
		return state.ThumbSticks.Left.X;
	}
	
	public float getLeftStickVertical()
	{
		return state.ThumbSticks.Left.Y;
	}

	private ButtonState getStateButton(GamePadState state, Button button)
	{
		switch (button)
		{
			case Button.A:
				return state.Buttons.A;
			case Button.B:
				return state.Buttons.B;
			case Button.X:
				return state.Buttons.X;
			case Button.Y:
				return state.Buttons.Y;
			default:
				throw new ArgumentOutOfRangeException(nameof(button), button, null);
		}
	}
}
