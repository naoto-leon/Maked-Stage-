using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseApp : MonoBehaviour
{
    // Start is called before the first frame update

    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape)) Quit();
    }



    void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;

#elif UNITY_STANDALONE
    UnityEngine.Application.Quit();
#endif
    }
}