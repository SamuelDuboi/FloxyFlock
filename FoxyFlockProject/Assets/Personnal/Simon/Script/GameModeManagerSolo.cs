using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameModeManagerSolo : MonoBehaviour
{
    private bool GameHasStarted;
    private float timer;
    private GameObject Player1; // Changer les Players pour les vrais script et pas les GameObject
    private bool GameHasEnded;
    private bool handsInThePlayground;

    private void Update()
    {
    }
    
    public IEnumerator StartGameMode()
    {
        yield return null;
    }
}
