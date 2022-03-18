using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteEditorController : MonoBehaviour //이 스크립트는 에디터를 켰을 때만 활성화해야함(계속 레이를 쏘기 때문에)
{
    
    public Camera mainCam;
    public Music music;
    float rayDistance = 15f;
    
    public RaycastHit2D mRay;

    public bool isDetected;
    public bool isScrolling;
    public int mouseClickNum = 0;
    public int scrollDir;
    //private Vector2 wheelInput2 = Input.mouseScrollDelta;

    public Vector3 CursurEffectPos { get; set; }

    // Update is called once per frame

    private void Update()
    {
        if (music.audioSource.isPlaying)
        {
            
        }
        else
        {
            OnMouseClick();
            OnMouseScroll();
        }
        
    }

    void LateUpdate()
    {
        OnMouseRay();
        
    }
    
    
    void OnMouseClick()
    {
        if (Input.GetMouseButtonDown(0)) mouseClickNum = 0; 
        else if (Input.GetMouseButtonDown(1)) mouseClickNum = 1;
        else if (Input.GetMouseButtonDown(2)) mouseClickNum = 2;
        else mouseClickNum = -1;
    }
    
    void OnMouseScroll() // =>이거 안되는 이유 걍 Scroll 주석처리해서 그런거였음
    {
        
        if (Input.mouseScrollDelta.y < 0) // 아래
        {
            isScrolling = true;
            scrollDir = -1;
        }
        else if (Input.mouseScrollDelta.y > 0) // 위
        {
            isScrolling = true;
            scrollDir = 1;
        }
        else
        {
            isScrolling = false;
            scrollDir = 0;
        }
    }
    
    void OnMouseRay() //마우스 움직임에 따라서 콜라이더 충돌을 감지하고 충돌된 콜라이더 정보를 mRay변수에 저장
    {
        Vector3 mousePos = Input.mousePosition; 
        //mousePos.z = mainCam.farClipPlane;
        
        Vector3 dir = mainCam.ScreenToWorldPoint(mousePos); //씬에서의 마우스 좌표

        RaycastHit2D hit = Physics2D.Raycast(dir, transform.forward, rayDistance);
        Debug.DrawRay(transform.position, dir, Color.red, 0.2f); //빨간줄 표시하는 코드
        if (hit)
        {
            mRay = hit;
            isDetected=true;
            //Debug.Log("레이어 " + mRay.transform.gameObject.layer);
        }
        else
        {
            isDetected = false;
        }
        /*
        if (Physics2D.Raycast(mousePos,transform.forward,rayDistance))// Layer))//충돌되는 콜라이더의 정보를 hit에 보관
        {
            mRay = hit;
            Debug.Log("레이어 " + mRay.transform.gameObject.layer);
            //Debug.Log("월드 마우스 : " + hit.point);
            //Debug.Log("그리드 포지 : " + hit.transform.position);
        }
        */
        
    }
}
