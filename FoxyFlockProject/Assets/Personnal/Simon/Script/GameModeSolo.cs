using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameModeSolo : MonoBehaviour
{
    private bool gameHasStarted;
    private float timer;
    private GameObject player; // Changer les Players pour les vrais script et pas les GameObject
    public bool playerWin;
    public HandsPlayground hands;
    public List<GameObject> Tables;

    public int batchesNumbers;
    public string tip;
    //public List<Batches> batches;


    public virtual void FixedUpdate()
    {
        if (playerWin == true)
        {
            StartCoroutine(EndGameMode());
        }
    }

    public IEnumerator StartGameMode()
    {
        gameHasStarted = true;
        yield return null;
    }

    public IEnumerator EndGameMode()
    {
        Debug.Log(player + " win !");
        yield return null;
    }


}
