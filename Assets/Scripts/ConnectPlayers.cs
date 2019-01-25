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
        for (int i = 0; i < 4; ++i)
        {
            PlayerIndex testPlayerIndex = (PlayerIndex)i;
            GamePadState testState = GamePad.GetState(testPlayerIndex);
            if (testState.IsConnected)
            {
                PlayerIndex playerIndex;
//                Debug.Log(string.Format("GamePad found {0}", testPlayerIndex));
                playerIndex = testPlayerIndex;

                int listIndex = notConnectedIndicies[0];
                notConnectedIndicies.RemoveAt(0);
                GameManager.instance.players[listIndex].PlayerController.SetPlayerIndex(playerIndex);
            }
        }
        
        if (notConnectedIndicies.Count == 0)
        {
            //end connect players
        }
    }
}
