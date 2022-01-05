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
/*    private Vector3 v1;
    private Vector3 v2;*/
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
   public void MoveBubbles(float _playgroundRayon, float Tposition, List<GameObject> bonus, List<GameObject> malus)
    {
        playgroundRayon = _playgroundRayon;
        rayonBuble = bonus[0].GetComponent<Bubble>().radius;
        angleInDegrees = Random.Range(0, Mathf.PI*2);
       /* v1 = new Vector3(Tposition.x + Mathf.Cos(angleInDegrees), Tposition.y, Tposition.z + Mathf.Sin(angleInDegrees));
        v2 = new Vector3(Tposition.x + Mathf.Cos(angleInDegrees+Mathf.PI), Tposition.y, Tposition.z + Mathf.Sin(angleInDegrees+180));*/
        #region bonus
        for (int i = 0; i < bonus.Count; i++)
        {
            r1 = Random.Range(0, playgroundRayon - cb + 1);
            r2 = Random.Range(playgroundRayon, cbPrime + 1);
            r3 = Random.Range(hb, hbPrime + 1);
            x = (r1 + r2 )/ 2.0f;
            angleInDegrees2 = Random.Range(angleInDegrees, angleInDegrees + Mathf.PI);
            c = new Vector3( Mathf.Cos(angleInDegrees2)*x *bonus[i].transform.lossyScale.x, Tposition,  Mathf.Sin(angleInDegrees2)*x * bonus[i].transform.lossyScale.z);
            bonus[i].transform.localPosition = c + Vector3.up*r3 * bonus[i].transform.lossyScale.y; 
            if(i == 1)
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
            c = new Vector3( Mathf.Cos(angleInDegrees2) * x * malus[i].transform.lossyScale.x, Tposition,Mathf.Sin(angleInDegrees2) * x * malus[i].transform.lossyScale.z);
            d = Mathf.Abs(Vector3.Distance(c, Vector3.zero));
            //will never be equal to 0 due to float 32 round, so I added a bit more
            if (d < 0.1)
            {
                i--;
                continue;
            }
            r3 = Random.Range(0, Mathf.Abs(Vector3.Distance(c, Vector3.zero)) / 2.0f);
            cPrime = Vector3.Lerp(c,new Vector3( 0,Tposition,0), r3 / d);
            d = Mathf.Abs(Vector3.Distance(cPrime, bonus[0].transform.localPosition));
            d1= Mathf.Abs(Vector3.Distance(cPrime, bonus[1].transform.localPosition));
            if (d> d1)
            {
                d = d1;
            }
            r4 = Random.Range(minValue, d - rayonBuble + 1);
            cPrimePrime = Vector3.Lerp(cPrime, bonus[0].transform.localPosition, r4 / d);
            r5 = Random.Range(hm, hmPrime + 1);
            malus[i].transform.localPosition = cPrimePrime + Vector3.up * r5 * malus[i].transform.lossyScale.y;
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
    public void MoveBubbles(float _playgroundRayon, float Tposition, List<GameObject> bonus, List<GameObject> malus,GameObject fireball)
    {
        playgroundRayon = _playgroundRayon;
        rayonBuble = bonus[0].GetComponent<Bubble>().radius;
        angleInDegrees = Random.Range(0, Mathf.PI * 2);
      /*  v1 = new Vector3(Tposition.x + Mathf.Cos(angleInDegrees), Tposition.y, Tposition.z + Mathf.Sin(angleInDegrees));
        v2 = new Vector3(Tposition.x + Mathf.Cos(angleInDegrees + Mathf.PI), Tposition.y, Tposition.z + Mathf.Sin(angleInDegrees + 180));*/
        #region bonus
        for (int i = 0; i < bonus.Count; i++)
        {
            r1 = Random.Range(0, playgroundRayon - cb + 1);
            r2 = Random.Range(playgroundRayon, cbPrime + 1);
            r3 = Random.Range(hb, hbPrime + 1);
            x = (r1 + r2) / 2.0f;
            angleInDegrees2 = Random.Range(angleInDegrees, angleInDegrees + Mathf.PI);
            c = new Vector3( Mathf.Cos(angleInDegrees2) * x * bonus[i].transform.lossyScale.x, Tposition, Mathf.Sin(angleInDegrees2) * x * bonus[i].transform.lossyScale.z);
            bonus[i].transform.localPosition = c + Vector3.up * r3 * bonus[i].transform.lossyScale.y;
            if (i == 1)
            {
                if (Mathf.Abs(Vector3.Distance(bonus[i - 1].transform.position, bonus[i].transform.position)) < rayonBuble)
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
            c = new Vector3(Mathf.Cos(angleInDegrees2) * x * malus[i].transform.lossyScale.x, Tposition, Mathf.Sin(angleInDegrees2) * x * malus[i].transform.lossyScale.z);
            d = Mathf.Abs(Vector3.Distance(c, Vector3.zero));
            //will never be equal to 0 due to float 32 round, so I added a bit more
            if (d < 0.1)
            {
                i--;
                continue;
            }
            r3 = Random.Range(0, Mathf.Abs(Vector3.Distance(c, Vector3.zero)) / 2.0f);
            cPrime = Vector3.Lerp(c, new Vector3(0, Tposition,0), r3 / d);
            d = Mathf.Abs(Vector3.Distance(cPrime, bonus[0].transform.localPosition));
            d1 = Mathf.Abs(Vector3.Distance(cPrime, bonus[1].transform.localPosition));
            if (d > d1)
            {
                d = d1;
            }
            r4 = Random.Range(minValue, d - rayonBuble + 1);
            cPrimePrime = Vector3.Lerp(cPrime, bonus[0].transform.localPosition, r4 / d);
            r5 = Random.Range(hm, hmPrime + 1);
            malus[i].transform.localPosition = cPrimePrime + Vector3.up * r5 * malus[i].transform.lossyScale.y;
            if (i == 1)
            {
                if (Mathf.Abs(Vector3.Distance(malus[i - 1].transform.position, malus[i].transform.position)) < rayonBuble)
                {
                    i--;
                    continue;
                }
            }
        }
        #endregion
        #region fireBall
        do
        {
            r1 = Random.Range(0, playgroundRayon - cf + 1);
            r2 = Random.Range(playgroundRayon, cfPrime + 1);
            angleInDegrees2 = Random.Range(angleInDegrees, angleInDegrees + Mathf.PI);
            x = (r1 + r2) / 2.0f;
            r3 = Random.Range(hf, hfPrime + 1);
            c = new Vector3( Mathf.Cos(angleInDegrees2) * x *fireball.transform.lossyScale.x, Tposition, Mathf.Sin(angleInDegrees2) * x * fireball.transform.lossyScale.z);
            c += Vector3.up * r3 * fireball.transform.lossyScale.y;
        }
        while (IsInContact(c,bonus,malus));
        fireball.transform.localPosition = c;
        #endregion
    }
    private bool IsInContact(Vector3 pos, List<GameObject> bonus, List<GameObject> malus)
    {
        if (Mathf.Abs(Vector3.Distance(pos, bonus[0].transform.position)) < rayonBuble)
            return true;
        if (Mathf.Abs(Vector3.Distance(pos, bonus[1].transform.position)) < rayonBuble)
            return true;
        if (Mathf.Abs(Vector3.Distance(pos, malus[0].transform.position)) < rayonBuble)
            return true;
        if (Mathf.Abs(Vector3.Distance(pos, malus[1].transform.position)) < rayonBuble)
            return true;

        return false;
    }

}
