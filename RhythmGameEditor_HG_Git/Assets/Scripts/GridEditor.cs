using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridEditor : MonoBehaviour
{
    // Start is called before the first frame update
    public NoteEditorController noteEditorController;
    public Music music;
    public MusicInfo musicInfo;
    public NoteEditor noteEditor;
    public List<GameObject> grids = new List<GameObject>();

    public float HBND; //HeightBetweenNoteDivision; => (1f/music.DivisionValue) 으로 대체
                       //(노트 사이에 거리를 잴 때 쓰는 변수인데, spb와 speed를 눈으로 봤을 때 어느정도의 거리가 좋을것 같은지 눈대중으로 계산해서 나눠주는 값

    public GameObject gridObject;

    private int SecondStartNumber;

    public int GridAmount;
    public int TrueNumber;
    public bool isLoad;
    public bool isPlay;
    public double curruntTime ;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Scroll();
    }
    void Scroll()
    {
        if(noteEditorController.isScrolling /*&& !sheetController.isLeftCtrl*/)
            ChangePos(noteEditorController.scrollDir);
    }
    public void ChangePos(float dir)
    {
        for (int i = 0; i < grids.Count; i++)
        {
            GameObject obj = grids[i];

            obj.transform.Translate(new Vector3(0f, dir * 280, 0f));
        }
        //InterpolPos(dir);
    }
    
    public void StartMusic()
    {
        isPlay = true;
        Debug.Log("dd");
        //if(isLoad)
            //StartCoroutine(Generate());
    }

    public void LoadGame()
    {
        Destroy();
        music.spb = 60f / music.bpm;
        music.GridAmountFuc();
        GridGenerateInGame1();
        
        Debug.Log("spb"+music.spb);
        

    }
    
    public void LoadEditor()
    {
        Destroy();
        musicInfo.LoadNoteData();
        music.spb = 60f / music.bpm;
        music.GridAmountFuc();
        GridGenerateInEditor();
        
        //Debug.Log("spb"+music.spb);
        

    }

    public IEnumerator Syncing()//노래가 2.608초 지날 때마다 특정 y좌표에 그리드 설치해주면 그 좌표부터 판정선을 지나가는 순간까지는 크게 싱크가 밀리지 않는다는 것으로부터 생각한 방법
    {
        TrueNumber = 1;
        GridGenerateInGame2(0);
        int i = 2;
        while (TrueNumber == 1)
        {
            //Debug.Log("지금 생성중");
            if (music.when25 >= 2.60869)
            {
                GridGenerateInGame2(i);
                i++;
                music.gridNumber++;
                yield return new WaitForSeconds(1f);
            }
            
            if (i>GridAmount)
            {
                TrueNumber = 0;
            }

            yield return null;
            

        }
        
    }
    
    
    
    public void GridGenerateInEditor()
    {
        for (int i = 0; i < GridAmount; i++)
        {
            //Debug.Log(i * music.spb * 4 * music.Speed);
            GameObject obj = Instantiate(gridObject, new Vector3(0f,- music.offset + i * music.spb * 32f * music.Speed * (1f/music.DivisionValue)), Quaternion.identity);     //4는 노트줄 4개
            Process32rd(obj);
            Grid grid = obj.GetComponent<Grid>();
            BoxCollider2D coll = obj.GetComponent<BoxCollider2D>();
            coll.size = new Vector2(1122f, music.spb * music.Speed * 31f * (1f/music.DivisionValue));
            coll.offset = new Vector2(0f, music.spb * music.Speed * 31f * (1f/music.DivisionValue) * 0.5f);
            grid.barNumber = i;
            SecondStartNumber = i;
            grids.Add(obj);
            musicInfo.DisPoseNote(musicInfo.FindGridNote(i), i);
        }

        isLoad = true;

    }

    
    
    public void GridGenerateInGame1()
    {
        for (int i = 0; i < 2; i++)
        {
            //Debug.Log(i * music.spb * 4 * music.Speed);
            GameObject obj = Instantiate(gridObject, new Vector3(0f,- music.offset + i * music.spb * 32f * music.Speed * (1f/music.DivisionValue)), Quaternion.identity);     //4는 노트줄 4개
            Process32rd(obj);
            Grid grid = obj.GetComponent<Grid>();
            BoxCollider2D coll = obj.GetComponent<BoxCollider2D>();
            coll.size = new Vector2(1122f, music.spb * music.Speed * 31f * (1f/music.DivisionValue));
            coll.offset = new Vector2(0f, music.spb * music.Speed * 31f * (1f/music.DivisionValue) * 0.5f);
            grid.barNumber = i;
            SecondStartNumber = i;
            grids.Add(obj);
        }

        isLoad = true;

    }

    public void GridGenerateInGame2(int i)
    {
        //Debug.Log(i * music.spb * 4 * music.Speed);
        GameObject obj = Instantiate(gridObject, new Vector3(0f, 18.38522f * 140 + 1.3f), Quaternion.identity);  //4는 노트줄 4개
        Process32rd(obj);
        Grid grid = obj.GetComponent<Grid>();
        BoxCollider2D coll = obj.GetComponent<BoxCollider2D>();
        coll.size = new Vector2(1122f, music.spb * music.Speed * 31f * (1f/music.DivisionValue));
        coll.offset = new Vector2(0f, music.spb * music.Speed * 31f * (1f/music.DivisionValue) * 0.5f);
        grid.barNumber = i;
        SecondStartNumber = i;
        grids.Add(obj);
    }

    IEnumerator Generate()
    {
        for (int i = SecondStartNumber + 1; i < (music.MusicTime + 1) / 4; i++)
        {
            GameObject obj = Instantiate(gridObject, new Vector3(0f, music.offset + (SecondStartNumber+1) * music.spb * 32f * music.Speed * (1f/music.DivisionValue)), Quaternion.identity);
            Process32rd(obj);
            Grid grid = obj.GetComponent<Grid>();
            BoxCollider2D coll = obj.GetComponent<BoxCollider2D>();
            coll.size = new Vector2(1122f, music.spb * music.Speed * 31f * (1f/music.DivisionValue));
            coll.offset = new Vector2(0f, music.spb * music.Speed * 31f * (1f/music.DivisionValue) * 0.5f);
            grid.barNumber = i;
            grids.Add(obj);

            yield return new WaitForSeconds(2.5f);
        }
    }

    IEnumerator SetPosition()
    {
        yield return new WaitForSeconds(2.5f);
    }
    
    
    
    void Process32rd(GameObject grid)
    {
        for(int i = 0; i < 32; i++)
        {
            GameObject obj = grid.transform.GetChild(i).gameObject;
            obj.transform.localPosition = new Vector3(0f, music.spb * i * (1f/music.DivisionValue) * music.Speed, -0.1f);// 한 노트가 떨어지는데 걸리는 시간이 music.spb이고, 그 간격을 얼마나 넓힐지가 Speed
        }
    }
    
    void Destroy()
    {
        for(int i = 0; i < grids.Count; i++)
        {
            if (grids[i] != null)
            {
                GameObject obj = grids[i];
                Destroy(obj);
            }
        }
        grids.Clear();
    }
    
}
