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
    GameObject seletedObject; // 배치 단계에서 선택된 오브젝트
    
    int currentSelectedLine;
    private int curruentSelectedGrid;
    
    public Vector3 snapPos;
    public float snapPosX = 0f;
    public int currentColor;
    private float halfSnapAmount;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(noteEditorController.isDetected)
            DisposePreObject();
        else
        {
            noteEditorController.cursurObj.SetActive(false);
        }

    }
    

    public void Pink()
    {
        note.GetComponent<SpriteRenderer>().color = Color.red;
        currentColor = 0;
    }

    public void Blue()
    {
        note.GetComponent<SpriteRenderer>().color = Color.blue;
        currentColor = 1;
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

            if (music.audioSource.isPlaying)//음악 재생중이 아닐때만 커서에 있는 오브젝트를 활성화함
            {
                noteEditorController.cursurObj.SetActive(false);
            }
            else//음악 재생중 아닐 때
            {
                noteEditorController.cursurObj.SetActive(true);
                //Debug.Log(snapPos);
                noteEditorController.cursurObj.transform.position = snapPos;//노트 위치 실시간으로 바꿔주는 코드(OnCursurEffect 안쓰고 이거씀)

                if (noteEditorController.mouseClickNum == 0)
                    DisposeObject(gridObject,grid);
                /*else if (noteEditorController.mouseClickNum == 1)
                    UnDisposeObject(gridObject);*/
            }
        }
        else
            noteEditorController.cursurObj.SetActive(false);
        
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
        GameObject noteContainer = gridObject.transform.GetChild(32).gameObject;

        GameObject obj = Instantiate(note, snapPos, Quaternion.identity, noteContainer.transform);
        
        if (!CheckObject(gridObject))//세이브 안눌려도 저장되도록
        {
            musicInfo.InputData(obj, currentColor,grid.barNumber);
        }
        

    }
    // 오브젝트 언배치
    
    
    /*void UnDisposeObject(GameObject gridObject)
    {
        if (CheckObject(gridObject))
        {
            float pos = seletedObject.transform.localPosition.y + (currentBarNumber * music.BarPerSec * Speed);
            DeleteObject(currentSelectedLine, pos);

            Destroy(seletedObject);
        }
    }*/
    

    bool CheckObject(GameObject gridObject)
    {
        GameObject noteContainer = gridObject.transform.GetChild(32).gameObject;
        bool isOverlap = false;

        if (noteContainer.transform.childCount == 0)
        {
            //Debug.Log("0인데 추가");
            return isOverlap;
        }
        else
        {
            for (int i = 0; i < noteContainer.transform.childCount; i++)
            {
                isOverlap = false;
                Vector2 note = noteContainer.transform.GetChild(i).transform.position;
                Debug.Log(snapPos.y - halfSnapAmount+ "-" + note.y +"-"+ snapPos.y + halfSnapAmount);
                if (Mathf.Approximately(note.x, snapPos.x) && (note.y >= snapPos.y - halfSnapAmount && note.y <= snapPos.y + halfSnapAmount))
                {
                    
                    Debug.Log("이미 노트가 있습니다.");
                    seletedObject = noteContainer.transform.GetChild(i).transform.gameObject;
                    isOverlap = true;
                    break;
                }
            }
            return isOverlap;
        }
    }  
    
    public void SaveNote()
    {
        if (gridEditor.grids != null)
        {
            for (int i = 0; i < gridEditor.grids.Count; i++)
            {
                for (int j = 0; j < gridEditor.grids[i].transform.GetChild(32).transform.childCount; j++)
                {
                    musicInfo.InputData(gridEditor.grids[i].transform.GetChild(32).transform.GetChild(j).gameObject, currentColor,i);
                    Debug.Log(i);
                    
                }
                
            }
            musicInfo.SaveNoteData();
        }
    }
    
    
    
}
