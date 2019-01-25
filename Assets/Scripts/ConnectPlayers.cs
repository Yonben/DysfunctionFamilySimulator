using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using XInputDotNetPure; 

public class ConnectPlayers : MonoBehaviour
{
    private List<int> notConnectedIndicies = new List<int>();
    
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < GameManager.instance.players.Count; i++)
            notConnectedIndicies.Add(i);
    }

    // Update is called once per frame
    void Update()
    {
        foreach (int index in notConnectedIndicies)
        {
            PlayerIndex testPlayerIndex = (PlayerIndex)index;
            GamePadState testState = GamePad.GetState(testPlayerIndex);
            if (testState.IsConnected)
            {
                PlayerIndex playerIndex;
//                Debug.Log(string.Format("GamePad found {0}", testPlayerIndex));
                playerIndex = testPlayerIndex;

                print(index);
                print(GameManager.instance.players.Count);
                notConnectedIndicies.RemoveAt(0);
                GameManager.instance.players[index].PlayerController.SetPlayerIndex(playerIndex);
            }
        }
        
        if (notConnectedIndicies.Count == 0)
        {
            //todo end connect players
        }
    }
}
