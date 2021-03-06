using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
public class Note
{
    public int Type; // 0-> short 1-> long
    public int Color; // 0-> red 1-> blue
    public int GridNum;
    public float XCoordinate;
    public float YCoordinate; //long��Ʈ ���� ���

    public Note(int type,int color, int gridNum, float xCoordinate, float yCoordinate)
    {
        Type = type;
        Color = color;
        GridNum = gridNum;
        XCoordinate = xCoordinate;
        YCoordinate = yCoordinate;
        /*StartTime = start;
        EndTime = end;*/
    }
}

[System.Serializable]
public class MusicData
{
    
    public string Music;
    public float BPM;
    public int noteCount;//중복의 문제때문에 ++해주면서 이 변수 쓰는것보다 그냥 note.Count 쓰면 됨
    public List<Note> note = new List<Note>();
    private HashSet<Note> note12 = new HashSet<Note>();
}


public class MusicInfo : MonoBehaviour
{
    private string path;
    public MusicData musicData;
    public GridEditor gridEditor;
    public NoteEditor noteEditor;
    public Music music;
    public HashSet<Note> hashNote = new HashSet<Note>();
    
    GameObject obj;

    private string jsonData;
    
    void Start()
    {
        path = Application.dataPath + "/Audio/" + musicData.Music + "Data.json";
    }
    
    public void InputData(GameObject Note, int color, int gridnum)
    {
        musicData.note.Add(new Note(0, color, gridnum, NoteLine(Note), Note.transform.localPosition.y));
        musicData.note = musicData.note.OrderBy(x => x.GridNum).ToList();
        
        jsonData = JsonUtility.ToJson(musicData, true);
        File.WriteAllText(path, jsonData);
        musicData.noteCount = musicData.note.Count;
    }

    public void WhatInMyBag(Note noteDataInGrid)
    {
        for (int i = 0; i < musicData.note.Count; i++)
        {
            if (noteDataInGrid.GridNum == musicData.note[i].GridNum)
            {
                if (noteDataInGrid.Color == musicData.note[i].Color)
                {
                    if (Mathf.Approximately(noteDataInGrid.XCoordinate,musicData.note[i].XCoordinate))
                    {
                        if (Mathf.Approximately(noteDataInGrid.YCoordinate,musicData.note[i].YCoordinate))
                        {
                            musicData.note.Remove(musicData.note[i]);
                            jsonData = JsonUtility.ToJson(musicData, true);
                            File.WriteAllText(path, jsonData);
                            musicData.noteCount = musicData.note.Count;
                            break;
                        }
                    }
                }
                
            }
        }
    }
    
    public void LoadNoteData() //어쨌든 로드를 해야 위에 두 함수를 쓸 수 있으니
    {
        jsonData = File.ReadAllText(path);
        musicData = JsonUtility.FromJson<MusicData>(jsonData);
        music.bpm = musicData.BPM;
        musicData.noteCount = musicData.note.Count;
    }
    public List<Note> FindGridNote(int i) //노트 배치하는 작업인데, 오래걸릴거 같으므로 씬 넘어갈때 이걸 하자.,gridNumber가 i인 노트데이터들을 리스트에 저장
    {
        List<Note> noteList1 = new List<Note>();
        
        for (int j = 0; j < musicData.note.Count; j++)
        {
            if (musicData.note[j].GridNum == i)
            {
                noteList1.Add(musicData.note[j]);
            }
            
        }
        
        return noteList1;
    }
    
    public void DisPoseNote(List<Note> noteList2,int i)
    {
        GameObject noteContainer = gridEditor.grids[i].transform.GetChild(32).gameObject;

        for (int j = 0; j < noteList2.Count; j++)
        {
            if (noteList2[j].Color == 0)
            {
                obj = Instantiate(noteEditor.PinkNote, new Vector3(0,0), Quaternion.identity,noteContainer.transform);
            }
            else
            {
                obj = Instantiate(noteEditor.BlueNote, new Vector3(0,0), Quaternion.identity,noteContainer.transform);
            }
            //Debug.Log(noteList2[j].GridNum);
            obj.transform.localPosition = new Vector3(noteList2[j].XCoordinate, noteList2[j].YCoordinate);
            obj.SetActive(true);
        }

    }
    
    
    public int NoteLine(GameObject NoteClone)
    {
        int LineNum = 0;
        switch(NoteClone.transform.position.x)
        {
            case (-450f):
                LineNum = -450;
                break;
            case (-150f):
                LineNum = -150;
                break;
            case (150f):
                LineNum = 150;
                break;
            case (450f):
                LineNum = 450;
                break;
        }
        return LineNum;
    }
    
}
