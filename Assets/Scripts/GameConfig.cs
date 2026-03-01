/* This script is adapted from https://gist.github.com/kurtdekker/775bb97614047072f7004d6fb9ccce30
 * to prepare a game manager singleton - a class instantiation that can only
 * only exist once - and mark it with DontDestroyOnLoad().
 * 
 * The game config keeps track of the game settings between the main menu 
 * and the game scene
 *
 * To access this object from another script, use GameConfig.Instance and
 * do not add it to a game scene.
 */

using System.Runtime.CompilerServices;
using UnityEngine;

public class GameConfig : MonoBehaviour
{
    private static GameConfig _Instance;
    public static GameConfig Instance
    {
        get
        {
            if (!_Instance)
            {
                _Instance = new GameObject().AddComponent<GameConfig>();
                // name it for easy recognition
                _Instance.name = _Instance.GetType().ToString();
                // mark root as DontDestroyOnLoad();
                DontDestroyOnLoad(_Instance.gameObject);
            }
            return _Instance;
        }
    }

    private string Player1 = "";
    private string Player2 = "";

    public void setPlayer(int playerNum, string input)
    {
        if (playerNum == 1)
        {
            Player1 = input;
            Debug.Log("Player 1 is " + input);
        }
        else if (playerNum == 2)
        {
            Player2 = input;
            Debug.Log("Player 2 is " + input);
        }
        else
        {
            Debug.Log("invalid call to setPlayer(), player num can only be 1 or 2");
        }
    }
}

