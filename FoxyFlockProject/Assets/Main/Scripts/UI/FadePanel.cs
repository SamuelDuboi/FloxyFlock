using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class FadePanel : MonoBehaviour
{
    public Image panel;
    // Start is called before the first frame update
    IEnumerator Start()
    {
        yield return new WaitForSeconds(1f);
        float timer = 0;
        while (timer < 1f)
        {
            timer += 0.01f;
            panel.color -= Color.black * 1f / 255f;
            yield return new WaitForSeconds(0.01f);
        }
    }

   
}
