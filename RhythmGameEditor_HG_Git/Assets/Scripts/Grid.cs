using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.TerrainAPI;

public class Grid : MonoBehaviour
{
    public GridEditor gridEditor;
    public Music music;
    public int bpm = 93;
    public int barNumber;
    
    public float spb;
    


    private void Start()
    {
        
    }

    void Update()
    {

        if (music.isPlay)
            transform.Translate(Vector3.down * Time.smoothDeltaTime * music.gridIntervel * 140 * music.Speed);
    }

    
}
