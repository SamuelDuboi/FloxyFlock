using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalInfoText : MonoBehaviour
{
    public GameModeSolo gm;
    public TextMesh info;
    void Start()
    {
        gameObject.transform.position = gm.Tables[0].transform.GetChild(0).transform.position + Vector3.up;
        info = gameObject.GetComponent<TextMesh>();
    }

    void Update()
    {
        transform.forward = gameObject.transform.position - Camera.main.transform.position;
        info.text = gm.tip;
    }
}
