using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Music : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioSource audioSource;
    public GridEditor gridEditor;
    public bool isPlay;
    public int gridNumber = 0;
    public float bpm { get; set; }
    public float spb;
    public float gridIntervel = 1f;
    public float DivisionValue = 16f;
    public float MusicTime;
    public double createGridTime;
    public float Speed;

    public bool isMusicStarted = false;
    
    public float offset = 1.3f;
    
    void Start()
    {
        offset /= gridIntervel / 4;
        DivisionValue *= 1 / 140f; //카메라크기 키워서..
        audioSource = gameObject.GetComponent<AudioSource>();
        MusicTime = audioSource.clip.length;
        //when25 = -offset - 5.0f;
        
    }

    void Update()
    {
        audioSource.pitch = Speed;
        //Debug.Log(audioSource.time);
        //Debug.Log(when25);
        if (audioSource.isPlaying)
        {
            createGridTime = - gridNumber * 2.60869 + audioSource.time;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                isPlay = false;
                audioSource.Pause();
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (!isMusicStarted)
                {
                    isPlay = true;
                    isMusicStarted = true;
                    audioSource.Play();
                    StartCoroutine(gridEditor.Syncing());
                    MusicTime -= offset;
                }
                else
                {
                    isPlay = true;
                    audioSource.Play();
                }
                

            }
        }
        
    }

    public void GridAmountFuc() //노래 길이에 맞는 그리드 개수 정하기
    {
        gridEditor.GridAmount = (int) Math.Ceiling(audioSource.clip.length / (bpm / 60));
        //Debug.Log(gridEditor.GridAmount);

    }

    public void ResetGrid()
    {
        audioSource.Stop();
        gridEditor.LoadGame();
    }
}
