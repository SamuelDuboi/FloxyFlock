
using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(SoundList))]
public class SoundListEditor : Editor
{

    private SoundList soundList;
    private Vector2 scrollPosition;
    private void OnEnable()
    {
        soundList = target as SoundList;
    }



    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        var _rect = EditorGUILayout.BeginHorizontal();
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
        DisplayMusic();
        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndVertical();
        serializedObject.ApplyModifiedProperties();
        EditorUtility.SetDirty(soundList);
        Repaint();

    }


    private void DisplayMusic()
    {
       
            var _rect = EditorGUILayout.BeginVertical();
            GUI.Box(_rect, " ");
            EditorGUILayout.Space(20);
            foreach (var soundClassic in soundList.music)
            {
                EditorGUILayout.BeginHorizontal();
                soundClassic.name = EditorGUILayout.TextField(soundClassic.name);
                soundClassic.clip = (AudioClip)EditorGUILayout.ObjectField(soundClassic.clip, typeof(AudioClip), true);
                soundClassic.volume = EditorGUILayout.Slider(soundClassic.volume, 0, 1);
                EditorGUILayout.EndHorizontal();

            }
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Actualise music")) { ActualiseMusicList(); }
            if (GUILayout.Button("AddMusic")) { soundList.music.Add(new Music()); }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
        
        EditorGUILayout.EndFoldoutHeaderGroup();
    }

    private void ActualiseMusicList()
    {
        var sound = Resources.LoadAll("Sound", typeof(AudioClip));
        Debug.Log(sound.Length);
        foreach (AudioClip soundFile in Resources.LoadAll("Sound", typeof(AudioClip)))
        {
            bool cantAdd = false;
            for (int i = 0; i < soundList.music.Count; i++)
            {
                if (soundList.music[i].clip == soundFile)
                {
                    cantAdd = true;
                    break;
                }
                else if(soundList.music[i].clip == null)
                {
                    soundList.music.RemoveAt(i);                    
                }
            }
            if (!cantAdd)
            {
                soundList.music.Add(new Music());
                soundList.music[soundList.music.Count - 1].clip = soundFile;
            }
        }
    }
}

