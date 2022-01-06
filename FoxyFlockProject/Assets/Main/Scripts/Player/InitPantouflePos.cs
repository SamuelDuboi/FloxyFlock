using UnityEngine;
using System.Collections;
public class InitPantouflePos : MonoBehaviour
{
    public Transform headSettTransform;
    public Transform headSettTransformR;
    // Start is called before the first frame update
    IEnumerator Start()
    {
        yield return new WaitForSeconds(3.0f);
        transform.localPosition = new Vector3(headSettTransform.localPosition.x, 0, headSettTransform.localPosition.z);
        transform.rotation = Quaternion.Euler(90, headSettTransformR.rotation.eulerAngles.y,0);
    }


}
