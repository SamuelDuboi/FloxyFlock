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
    float angle=0;
    float minAngle;
    float maxAngle;
    float J;

    /*    private Vector3 v1;
        private Vector3 v2;*/
    private Vector3 c;
    private Vector3 cPrime;
    private Vector3 cPrimePrime;
    private float playgroundRayon;
    public float spawnMax;
    public float cb;
    public float cbPrime;
    public float cbMin;
    public float hb;
    public float hbPrime;
    public float cm;
    public float cmPrime;
    public float cmMin;
    public float minValue;
    public float hm;
    public float hmPrime;
    public float cf;
    public float cfPrime;
    public float cfMin;
    public float hf;
    public float hfPrime;
    public float offset;
    private bool moveFireBall;
    private bool isNotFirstBacth;
    private int iterationNumber;
   public void MoveBubbles(float _playgroundRayon, float Tposition, Vector3 pPos, List<GameObject> bonus, List<GameObject> malus, List<Vector3> position)
    {
        playgroundRayon = _playgroundRayon;
        rayonBuble = bonus[0].GetComponent<Bubble>().radius+offset;
        /* v1 = new Vector3(Tposition.x + Mathf.Cos(angleInDegrees), Tposition.y, Tposition.z + Mathf.Sin(angleInDegrees));
         v2 = new Vector3(Tposition.x + Mathf.Cos(angleInDegrees+Mathf.PI), Tposition.y, Tposition.z + Mathf.Sin(angleInDegrees+180));*/

        J = Mathf.Abs(Vector3.Distance(Vector3.up* Tposition, pPos+Vector3.up*(Tposition-pPos.y)));
        minAngle = 0;
        maxAngle = Mathf.PI;


        if (J >= spawnMax)
        {
            // security until Simon find the probleme
            float angleValue = (spawnMax * spawnMax - _playgroundRayon * _playgroundRayon - J * J) / (-2 * _playgroundRayon * J);
            /*if (angleValue > 1)
                angleValue = 1;
            if (angleValue < -1)
                angleValue = -1;*/
            angle = Mathf.Acos(angleValue);
            minAngle = angle;
            maxAngle = Mathf.PI - angle;

        }
        angleInDegrees = Random.Range(minAngle, maxAngle);
        int index = 0;
        for (int i = 0; i < bonus.Count; i++)
        {
            if (bonus[i].GetComponent<Bubble>().OnDestroyed(isNotFirstBacth)) 
            {
                if(isNotFirstBacth)
                bonus[i].GetComponent<Bubble>().particuleBubble.GetComponent<BubbleToDispenser>().Move(position[index]);
                index++;
            }
        }
        for (int i = 0; i < malus.Count; i++)
        {
            if (malus[i].GetComponent<Bubble>().OnDestroyed(isNotFirstBacth))
            {
                if (isNotFirstBacth)
                    malus[i].GetComponent<Bubble>().particuleBubble.GetComponent<BubbleToDispenser>().Move(position[index]);
                index++;
            }
        }

        iterationNumber = 0;

        #region bonus
        for (int i = 0; i < bonus.Count; i++)
        {
            r1 = Random.Range(0, playgroundRayon - cb );
            r2 = Random.Range(cbMin, playgroundRayon - cbPrime);
            r3 = Random.Range(hb, hbPrime );
            x = (r1 + r2 )/ 2.0f;
            angleInDegrees2 = Random.Range(angleInDegrees, angleInDegrees + Mathf.PI);
            c = new Vector3( Mathf.Cos(angleInDegrees2)*x *bonus[i].transform.lossyScale.x, Tposition,  Mathf.Sin(angleInDegrees2)*x * bonus[i].transform.lossyScale.z);
            bonus[i].transform.localPosition = c + Vector3.up*r3 * bonus[i].transform.lossyScale.y; 
            if(i == 1)
            {
                if(Mathf.Abs( Vector3.Distance(bonus[i-1].transform.position, bonus[i].transform.position)) < rayonBuble*2 && iterationNumber<20)
                {
                    iterationNumber++;
                    i--;
                }
            }
        }
        #endregion
        iterationNumber = 0;
        #region malus
        for (int i = 0; i < malus.Count; i++)
        {
            r1 = Random.Range(0, playgroundRayon - cm);
            r2 = Random.Range(cmMin, playgroundRayon - cmPrime);
            x = (r1 + r2) / 2.0f;
            angleInDegrees2 = Random.Range(angleInDegrees, angleInDegrees + (Mathf.PI / 3) * 2);
            c = new Vector3( Mathf.Cos(angleInDegrees2) * x * malus[i].transform.lossyScale.x, Tposition,Mathf.Sin(angleInDegrees2) * x * malus[i].transform.lossyScale.z);
            d = Mathf.Abs(Vector3.Distance(c, Vector3.zero));
            //will never be equal to 0 due to float 32 round, so I added a bit more
            if (d < 0.1f && iterationNumber < 20)
            {
                iterationNumber++;
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
            if (d < rayonBuble*2&& iterationNumber < 20)
            {
                iterationNumber++;
                i--;
                continue;
            }
            r4 = Random.Range(minValue, d - rayonBuble*2);
            cPrimePrime = Vector3.Lerp(cPrime, bonus[0].transform.localPosition, r4 / d);
            r5 = Random.Range(hm, hmPrime);
           
           
            malus[i].transform.localPosition = cPrimePrime + Vector3.up * r5 * malus[i].transform.lossyScale.y;
            if (IsInContactBonus(malus[i].transform.localPosition, bonus, malus, i) && iterationNumber<20)
            {
                iterationNumber++;
                i--;
                continue;
            }
        }
        for (int i=0;i<bonus.Count; i++)
        {
            Vector3.MoveTowards(bonus[i].transform.position, Vector3.up*Tposition - pPos, J);
        }
        for (int i = 0; i < malus.Count; i++)
        {
            Vector3.MoveTowards(malus[i].transform.position, Vector3.up * Tposition - pPos, J);
        }
        if (!isNotFirstBacth)
            isNotFirstBacth = true;
        #endregion
    }
    public void MoveBubbles(float _playgroundRayon, float Tposition, Vector3 pPos, List<GameObject> bonus, List<GameObject> malus,GameObject fireball, List<Vector3> position)
    {
        playgroundRayon = _playgroundRayon;
        rayonBuble = bonus[0].GetComponent<Bubble>().radius;
        J = Mathf.Abs(Vector3.Distance(Vector3.up * Tposition, pPos));
        minAngle = 0;
        maxAngle = Mathf.PI;
        iterationNumber = 0;
        if (J >= spawnMax)
         {
                    float angleValue = (spawnMax * spawnMax - _playgroundRayon * _playgroundRayon - J * J) / (-2 * _playgroundRayon * J);
             if (angleValue > 1)
                 angleValue = 1;
             if (angleValue < -1)
                 angleValue = -1;
             angle = Mathf.Acos(angleValue);
            
             minAngle = angle;
             maxAngle = Mathf.PI - angle;

         }
        angleInDegrees = Random.Range(minAngle, maxAngle);
        /*  v1 = new Vector3(Tposition.x + Mathf.Cos(angleInDegrees), Tposition.y, Tposition.z + Mathf.Sin(angleInDegrees));
          v2 = new Vector3(Tposition.x + Mathf.Cos(angleInDegrees + Mathf.PI), Tposition.y, Tposition.z + Mathf.Sin(angleInDegrees + 180));*/
        int index = 0;
        for (int i = 0; i < bonus.Count; i++)
        {
            if (bonus[i].GetComponent<Bubble>().OnDestroyed(isNotFirstBacth))
            {
                if (isNotFirstBacth)
                    bonus[i].GetComponent<Bubble>().particuleBubble.GetComponent<BubbleToDispenser>().Move(position[index]);
                index++;
            }
        }
        for (int i = 0; i < malus.Count; i++)
        {
            if (malus[i].GetComponent<Bubble>().OnDestroyed(isNotFirstBacth))
            {
                if (isNotFirstBacth)
                    malus[i].GetComponent<Bubble>().particuleBubble.GetComponent<BubbleToDispenser>().Move(position[index]);
                index++;
            }
        }
        if (moveFireBall)
        {
            fireball.SetActive(true);
            if (fireball.GetComponent<Bubble>().OnDestroyed(isNotFirstBacth))
            {
                if (position.Count!= index)
                {
                    if (isNotFirstBacth)
                        fireball.GetComponent<Bubble>().particuleBubble.GetComponent<BubbleToDispenser>().Move(position[index]);
                    index++;
                }
            }
        }
   
        #region bonus
        for (int i = 0; i < bonus.Count; i++)
        {
            r1 = Random.Range(0, playgroundRayon - cb);
            r2 = Random.Range(cbMin, playgroundRayon - cbPrime);
            r3 = Random.Range(hb, hbPrime);
            x = (r1 + r2) / 2.0f;
            angleInDegrees2 = Random.Range(angleInDegrees, angleInDegrees + (Mathf.PI / 3) * 2);
            c = new Vector3(Mathf.Cos(angleInDegrees2) * x * bonus[i].transform.lossyScale.x, Tposition, Mathf.Sin(angleInDegrees2) * x * bonus[i].transform.lossyScale.z);
            bonus[i].transform.localPosition = c + Vector3.up * r3 * bonus[i].transform.lossyScale.y;
            if (i == 1)
            {
                if (Mathf.Abs(Vector3.Distance(bonus[i - 1].transform.position, bonus[i].transform.position)) < rayonBuble * 2 && iterationNumber < 20)
                {
                    iterationNumber++;
                    i--;
                }
            }
        }
        #endregion
        #region malus
        iterationNumber = 0;
        for (int i = 0; i < malus.Count; i++)
        {
            r1 = Random.Range(0, playgroundRayon - cm );
            r2 = Random.Range(cmMin, playgroundRayon - cmPrime);
            x = (r1 + r2) / 2.0f;
            angleInDegrees2 = Random.Range(angleInDegrees, angleInDegrees + (Mathf.PI / 3) * 2);
            c = new Vector3(Mathf.Cos(angleInDegrees2) * x * malus[i].transform.lossyScale.x, Tposition, Mathf.Sin(angleInDegrees2) * x * malus[i].transform.lossyScale.z);
            d = Mathf.Abs(Vector3.Distance(c, Vector3.zero));
            //will never be equal to 0 due to float 32 round, so I added a bit more
            if (d < 0.1 && iterationNumber < 20)
            {
                iterationNumber++;
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
            r4 = Random.Range(minValue, d - rayonBuble*2 );
            cPrimePrime = Vector3.Lerp(cPrime, bonus[0].transform.localPosition, r4 / d);
            r5 = Random.Range(hm, hmPrime);


            malus[i].transform.localPosition = cPrimePrime + Vector3.up * r5 * malus[i].transform.lossyScale.y;
            if (IsInContactBonus(malus[i].transform.localPosition, bonus, malus, i) && iterationNumber < 20)
            {
                iterationNumber++;
                i--;
                continue;
            }
        }
        #endregion
        #region fireBall
        iterationNumber = 0;
        if (moveFireBall)
        {
            do
            {
                r1 = Random.Range(0, playgroundRayon - cf);
                r2 = Random.Range(cfMin, playgroundRayon - cfPrime);
                angleInDegrees2 = Random.Range(angleInDegrees, angleInDegrees + (Mathf.PI / 3) * 2);
                x = (r1 + r2) / 2.0f;
                r3 = Random.Range(hf, hfPrime);
                c = new Vector3(Mathf.Cos(angleInDegrees2) * x * fireball.transform.lossyScale.x, Tposition, Mathf.Sin(angleInDegrees2) * x * fireball.transform.lossyScale.z);
                c += Vector3.up * r3 * fireball.transform.lossyScale.y;
                iterationNumber++;
                if (iterationNumber > 20)
                    break;
            }
            while (IsInContact(c, bonus, malus));
            fireball.transform.localPosition = c;
            for (int i = 0; i < bonus.Count; i++)
            {
                Vector3.MoveTowards(bonus[i].transform.position, Vector3.up * Tposition - pPos, J);
            }
            for (int i = 0; i < malus.Count; i++)
            {
                Vector3.MoveTowards(malus[i].transform.position, Vector3.up * Tposition - pPos, J);
            }
            Vector3.MoveTowards(fireball.transform.position, Vector3.up * Tposition - pPos, J);
        }
        else
        {
            fireball.SetActive(false);
        }
        moveFireBall = !moveFireBall;
        if(!isNotFirstBacth)
        isNotFirstBacth = true;
        #endregion
    }
    private bool IsInContact(Vector3 pos, List<GameObject> bonus, List<GameObject> malus)
    {
        if (Mathf.Abs(Vector3.Distance(pos, bonus[0].transform.localPosition)) < rayonBuble*2)
            return true;
        if (Mathf.Abs(Vector3.Distance(pos, bonus[1].transform.localPosition)) < rayonBuble*2)
            return true;
        if (Mathf.Abs(Vector3.Distance(pos, malus[0].transform.localPosition)) < rayonBuble*2)
            return true;
        if (Mathf.Abs(Vector3.Distance(pos, malus[1].transform.localPosition)) < rayonBuble*2)
            return true;

        return false;
    }
    private bool IsInContactBonus(Vector3 pos, List<GameObject> bonus, List<GameObject> malus,int index)
    {
        if (Mathf.Abs(Vector3.Distance(pos, bonus[0].transform.localPosition)) < rayonBuble * 2)
            return true;
        if (Mathf.Abs(Vector3.Distance(pos, bonus[1].transform.localPosition)) < rayonBuble * 2)
            return true;
        if (index == 1)
            if (Mathf.Abs(Vector3.Distance(pos, malus[0].transform.localPosition)) < (rayonBuble /*+ offset*/) * 2)
                return true;

        return false;
    }


}
