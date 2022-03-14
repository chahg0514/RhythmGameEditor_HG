using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class Note
{
    public int Type; // 0-> short 1-> long
    public int Color; // 0-> red 1-> blue
    public int GridNum;
    public int LineNum;
    public float HorizonNum; //수평 숫자(1~32)
    public double StartTime;
    public double EndTime; //long��Ʈ ���� ���

    public Note(int type,int color, int gridNum, int Line, float horizonNumber)
    {
        Type = type;
        Color = color;
        GridNum = gridNum;
        LineNum = Line;
        HorizonNum = horizonNumber;
        /*StartTime = start;
        EndTime = end;*/
    }
}

[System.Serializable]
public class MusicData
{
    public string Music;
    public float BPM;
    public int noteCount;
    public List<Note> note = new List<Note>();
}

public class MusicInfo : MonoBehaviour
{
    public MusicData musicData;
    public GridEditor gridEditor;
    public NoteEditor noteEditor;
    public void InputData(GameObject Note, int color, int gridnum)
    {
        musicData.noteCount++;
        musicData.note.Add(new Note(0, color, gridnum, NoteLine(Note), Note.transform.localPosition.y));
        musicData.note = musicData.note.OrderBy(x => x.GridNum).ToList();
    }
    public void SaveNoteData()
    {
        string jsonData = JsonUtility.ToJson(musicData, true);
        string path = Path.Combine(Application.dataPath + "/Audio/" + musicData.Music + "Data.json");
        File.WriteAllText(path, jsonData);
    }
    public void LoadNoteData()
    {
        string path = Path.Combine(Application.dataPath + "/Audio/" + musicData.Music + "Data.json");
        string jsonData = File.ReadAllText(path);
        musicData = JsonUtility.FromJson<MusicData>(jsonData);
    }
    public List<Note> FindGridNote(int i) //노트 배치하는 작업인데, 오래걸릴거 같으므로 씬 넘어갈때 이걸 하자.
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
            GameObject obj = Instantiate(noteEditor.note, new Vector3(0,0), Quaternion.identity,noteContainer.transform);
            Debug.Log(noteList2[j].GridNum);
            obj.transform.localPosition = new Vector3(noteList2[j].LineNum, noteList2[j].HorizonNum);
            obj.SetActive(true);
        }

    }
    
    
    public int NoteLine(GameObject NoteClone)
    {
        int LineNum = 0;
        switch(NoteClone.transform.position.x)
        {
            case (-3f):
                LineNum = -3;
                break;
            case (-1f):
                LineNum = -1;
                break;
            case (1f):
                LineNum = 1;
                break;
            case (3f):
                LineNum = 3;
                break;
        }
        return LineNum;
    }
    
}