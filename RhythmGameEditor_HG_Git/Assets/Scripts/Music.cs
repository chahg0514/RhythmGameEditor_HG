using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioSource audioSource;
    public GridEditor gridEditor;
    public bool isPlay;
    public int gridNumber = 0;
    public float bpm { get; set; }
    public float spb;
    public float Speed = 1f;
    public float DivisionValue = 16f;
    public float MusicTime;
    public float first;
    public float before;
    public double when25;
    
    public float offset = 1.3f;
    
    void Start()
    {
        offset /= Speed / 4;
        DivisionValue *= 1 / 140f; //카메라크기 키워서..
        audioSource = gameObject.GetComponent<AudioSource>();
        MusicTime = audioSource.clip.length;
        //when25 = -offset - 5.0f;
        
        before = 0;
    }

    void Update()
    {
        //Debug.Log(audioSource.time);
        //Debug.Log(when25);
        if (audioSource.isPlaying)
        {
            when25 = - gridNumber * 2.60869 + audioSource.time;
        }
        else
        {
            gridEditor.TrueNumber = 0;
        }
        
    }

    public void GridAmountFuc() //노래 길이에 맞는 그리드 개수 정하기
    {
        gridEditor.GridAmount = (int) Math.Ceiling(audioSource.clip.length / (bpm / 60));
        //Debug.Log(gridEditor.GridAmount);

    }

    public void StartMusic()
    {
        Debug.Log(audioSource.clip.length);
        audioSource.Play();
        StartCoroutine(gridEditor.Syncing());
        MusicTime -= offset;
        first = MusicTime;
        
    }
}
