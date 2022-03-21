using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class NoteEditor : MonoBehaviour
{

    public NoteEditorController noteEditorController;
    public Music music;
    public GridEditor gridEditor;
    public MusicInfo musicInfo;
    public GameObject note;
    public GameObject PinkNote;
    public GameObject BlueNote;
    GameObject seletedObject; // 배치 단계에서 선택된 오브젝트
    private GameObject obj;
    
    int currentSelectedLine;
    public int curruentSelectedGrid;
    
    public Vector3 snapPos;
    public float snapPosX = 0f;
    public int currentColor;
    private float halfSnapAmount;
    
    void Update()
    {
        //KeyDownDisPose();
        if (noteEditorController.isDetected)
        {
            DisposePreObject();
        }
           
        else
        {
            note.SetActive(false);
        }
    }
    public void Pink()
    {
        //note.GetComponent<SpriteRenderer>().color = Color.red;
        note = PinkNote;
        currentColor = 0;
    }

    public void Blue()
    {
        //note.GetComponent<SpriteRenderer>().color = Color.blue;
        note = BlueNote;
        currentColor = 1;
    }

    void KeyDownDisPose()
    {
        //music.gridNumber
        if (Input.GetKeyDown(KeyCode.Q))
        {
            //obj = Instantiate(PinkNote, snapPos, Quaternion.identity, noteContainer.transform);
            if (Input.GetKeyDown(KeyCode.W))
            {

            }

            if (Input.GetKeyDown(KeyCode.E))
            {

            }

            if (Input.GetKeyDown(KeyCode.R))
            {

            }
        }
    }


    void DisposePreObject()
    {
        if (noteEditorController.mRay.transform.gameObject.layer == 6)
        {
            GameObject gridObject;
            Grid grid;
            gridObject = noteEditorController.mRay.transform.gameObject; // 레이받은(클릭된) 그리드의 게임오브젝트를 가져온다.
            grid = gridObject.GetComponent<Grid>(); // 그리고 해당 오브젝트의 스크릡트를 가져옴
            curruentSelectedGrid = grid.barNumber;

            Vector2 hitToGrid; 
            hitToGrid = noteEditorController.mRay.point - new Vector2(gridObject.transform.position.x, gridObject.transform.position.y);

            ProcessSnapPos(hitToGrid, gridObject); //이게 마우스 이동에 따라서 노트 실시간 움직임

            note.SetActive(true);
            //Debug.Log(snapPos);
            note.transform.position = snapPos;//노트 위치 실시간으로 바꿔주는 코드(OnCursurEffect 안쓰고 이거씀)

            if (noteEditorController.mouseClickNum == 0)
                DisposeObject(gridObject,grid);
            else if (noteEditorController.mouseClickNum == 1)
                UnDisposeObject(gridObject);
            
            if (music.audioSource.isPlaying)//음악 재생중이 아닐때만 커서에 있는 오브젝트를 활성화함
            {
                //note.SetActive(false);
            }
            else//음악 재생중 아닐 때
            {
                
            }
        }
        else
            note.SetActive(false);
        
    }
    void ProcessSnapPos(Vector3 hitToGrid, GameObject gridObject)//에임에 따라 설치할 노트 위치를 잡아주는 함수
    {
        // 현재 스냅양에 따라 스냅될 위치를 계산한다. (x값)
        
        if (noteEditorController.mRay.point.x > -600f && noteEditorController.mRay.point.x < -300f)
        {
            snapPosX = -450f;
            currentSelectedLine = 1;
        }
        else if (noteEditorController.mRay.point.x > -300f && noteEditorController.mRay.point.x < 0f)
        {
            snapPosX = -150f;
            currentSelectedLine = 2;
        }
        else if (noteEditorController.mRay.point.x > 0f && noteEditorController.mRay.point.x < 300f)
        {
            snapPosX = 150f;
            currentSelectedLine = 3;
        }
        else if (noteEditorController.mRay.point.x > 300f && noteEditorController.mRay.point.x < 600f)
        {
            snapPosX = 450f;
            currentSelectedLine = 4;
        }
        // 현재 스냅양에 따라 스냅될 위치를 계산한다. (y값)

        float snapAmount = 1f * music.spb * music.Speed * (1f/music.DivisionValue);
        halfSnapAmount = snapAmount / 2;

        float snapPosY = hitToGrid.y;
        for (int i = 0; i < 32; i++)
        {
            if (snapPosY >= (snapAmount * i) - halfSnapAmount && snapPosY <= (snapAmount * i) + halfSnapAmount)
            {
                //Debug.Log("최소 : " + ((snapAmount * i) - halfSnapAmount) + " 최대 : " + ((snapAmount * i) + halfSnapAmount));
                //Debug.Log("걸린 곳 : " + i);
                snapPos = new Vector3(snapPosX, gridObject.transform.position.y + i * snapAmount, -0.1f);
                
                break;
            }
        }
        
    }
    void DisposeObject(GameObject gridObject,Grid grid) //노트 배치
    {
        GameObject noteContainer = gridObject.transform.GetChild(32).gameObject; //이건 굳이 따로 안만들고 함수에 바로 집어넣어도 되지만 가독성위해 남겨두겠음

        if (!CheckObject(gridObject))//세이브 안눌려도 저장되도록
        {
            if (currentColor == 0) 
            {
                obj = Instantiate(PinkNote, snapPos, Quaternion.identity, noteContainer.transform);
            }
            else
            {
                obj = Instantiate(BlueNote, snapPos, Quaternion.identity, noteContainer.transform);
            }
            
            Debug.Log("dfddfdf");
            musicInfo.InputData(obj, currentColor,grid.barNumber);
        }
        
        

    }
    
    void UnDisposeObject(GameObject gridObject) //설치할 때는 버튼을 눌러서 선택한 색대로 설치만하면 되지만, 없앨때는 그 자리에 있던 오브젝트의 색깔이 뭐였는지 비교해야하기 때문에 태그로 하기로함
    {
        if (CheckObject(gridObject))
        {
            Debug.Log(seletedObject == PinkNote);
            //DeleteObject(currentSelectedLine, pos); 여기에 제이슨파일 삭제하는거만 넣으면됨
            musicInfo.WhatInMyBag(new Note(0, CheckColor(seletedObject), curruentSelectedGrid, seletedObject.transform.position.x,
                seletedObject.transform.localPosition.y));

            Destroy(seletedObject);
        }
    }

    int CheckColor(GameObject selectedObject)
    {
        if (selectedObject.transform.CompareTag("PinkNote"))
        {
            Debug.Log("빨간색");
            return 0;
        }

        return 1; //핑크노트가 아니면 당연히 블루니깐 엘스이프 생략가능

    }
    
    bool CheckObject(GameObject gridObject)//0이면 설치, 1이면 삭제할때 함수호출한다는 뜻
    {
        GameObject noteContainer = gridObject.transform.GetChild(32).gameObject;
        bool isOverlap = false;

        if (noteContainer.transform.childCount == 1)
        {
            
        }
        for (int i = 0; i < noteContainer.transform.childCount; i++) //-1을 해주는 이유: 이미 인스턴시에이트를 하고 난 뒤이기 때문 => -1뺌
        {
            isOverlap = false;
            Vector2 note = noteContainer.transform.GetChild(i).transform.position;
            
            //Debug.Log(snapPos.y - halfSnapAmount+ "-" + note.y +"-"+ snapPos.y + halfSnapAmount);
            if (Mathf.Approximately(note.x, snapPos.x) && Mathf.Approximately(note.y,snapPos.y))
            {
                Debug.Log(note.y + "+" + snapPos.y);
                Debug.Log("이미 노트가 있습니다.");
                seletedObject = noteContainer.transform.GetChild(i).transform.gameObject;
                isOverlap = true;
                break;
                
            }
        }
        return isOverlap;
    }

}
