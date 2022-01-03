using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBubble : MonoBehaviour
{
    float rayonBuble;
    float angleInDegrees;
    float angleInDegrees2;
    float r1;
    float r2;
    float r3;
    float r4;
    float r5;
    float x;
    float d;
    float d1;
    private Vector3 v1;
    private Vector3 v2;
    private Vector3 c;
    private Vector3 cPrime;
    private Vector3 cPrimePrime;
    private float playgroundRayon;
    public float cb;
    public float cbPrime;
    public float hb;
    public float hbPrime;
    public float cm;
    public float cmPrime;
    public float minValue;
    public float hm;
    public float hmPrime;
    public float cf;
    public float cfPrime;
    public float hf;
    public float hfPrime;
   public void MoveBubbles(float _playgroundRayon, Vector3 Tposition, List<GameObject> bonus, List<GameObject> malus)
    {
        playgroundRayon = _playgroundRayon;
        rayonBuble = bonus[0].GetComponent<Bubble>().radius;
        angleInDegrees = Random.Range(0, Mathf.PI*2);
        v1 = new Vector3(Tposition.x + Mathf.Cos(angleInDegrees), Tposition.y, Tposition.z + Mathf.Sin(angleInDegrees));
        v2 = new Vector3(Tposition.x + Mathf.Cos(angleInDegrees+Mathf.PI), Tposition.y, Tposition.z + Mathf.Sin(angleInDegrees+180));
        #region bonus
        for (int i = 0; i < bonus.Count; i++)
        {
            r1 = Random.Range(0, playgroundRayon - cb + 1);
            r2 = Random.Range(playgroundRayon, cbPrime + 1);
            r3 = Random.Range(hb, hbPrime + 1);
            x = (r1 + r2 )/ 2.0f;
            angleInDegrees2 = Random.Range(angleInDegrees, angleInDegrees + Mathf.PI);
            c = new Vector3(Tposition.x + Mathf.Cos(angleInDegrees2)*x, Tposition.y, Tposition.z + Mathf.Sin(angleInDegrees2)*x);
            bonus[i].transform.position = c + Vector3.up*r3; 
            if(i == 0)
            {
                if(Mathf.Abs( Vector3.Distance(bonus[i-1].transform.position, bonus[i].transform.position)) < rayonBuble)
                {
                    i--;
                }
            }
        }
        #endregion
        #region malus
        for (int i = 0; i < malus.Count; i++)
        {
            r1 = Random.Range(0, playgroundRayon - cm + 1);
            r2 = Random.Range(playgroundRayon, cmPrime + 1);
            x = (r1 + r2) / 2.0f;
            angleInDegrees2 = Random.Range(angleInDegrees, angleInDegrees + Mathf.PI);
            c = new Vector3(Tposition.x + Mathf.Cos(angleInDegrees2) * x, Tposition.y, Tposition.z + Mathf.Sin(angleInDegrees2) * x);
            d = Mathf.Abs(Vector3.Distance(c, Vector3.zero));
            //will never be equal to 0 due to float 32 round, so I added a bit more
            if (d < 0.1)
            {
                i--;
                continue;
            }
            r3 = Random.Range(0, Mathf.Abs(Vector3.Distance(c, Vector3.zero)) / 2.0f);
            cPrime = Vector3.Lerp(c, Tposition, r3 / d);
            d = Mathf.Abs(Vector3.Distance(cPrime, bonus[0].transform.position));
            d1= Mathf.Abs(Vector3.Distance(cPrime, bonus[1].transform.position));
            if (d> d1)
            {
                d = d1;
            }
            r4 = Random.Range(minValue, d - rayonBuble + 1);
            cPrimePrime = Vector3.Lerp(cPrime, bonus[0].transform.position, r4 / d);
            r5 = Random.Range(hm, hmPrime + 1);
            malus[i].transform.position = cPrimePrime + Vector3.up * r5;
            if (i == 1)
            {
                if (Mathf.Abs(Vector3.Distance(malus[i - 1].transform.position, malus[i].transform.position)) < rayonBuble)
                {
                    i--;
                }
            }
        }
        #endregion
    }

}
