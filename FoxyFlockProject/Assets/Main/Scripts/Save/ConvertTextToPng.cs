using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class ConvertTextToPng : MonoBehaviour
{

    private void Start()
    {
        RenderTexToPNG();
    }
    void RenderTexToPNG( )
    {

        Camera myCam = GetComponent<Camera>();

        RenderTexture renderTexture = RenderTexture.active;
        RenderTexture.active = myCam.targetTexture;

        myCam.Render();

        Texture2D Image = new Texture2D(myCam.targetTexture.width, myCam.targetTexture.height);
        Image.ReadPixels(new Rect(0, 0, myCam.targetTexture.width, myCam.targetTexture.height), 0, 0);
        Image.Apply();
        RenderTexture.active = renderTexture;

        var Bytes = Image.EncodeToPNG();
        Destroy(Image);
        var dataPath = Path.Combine(Application.persistentDataPath, "background.png");
        File.WriteAllBytes(dataPath, Bytes);
    }
}
