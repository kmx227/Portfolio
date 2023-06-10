using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.UIElements;
using static Room;
using Random = UnityEngine.Random;

[Serializable]
public struct isCheck
{
    public Vector3 _tilePosition;
    public bool _isCheck;
    public GameObject _mapObj;

    public void Initialize()
    {
        _tilePosition = Vector3.zero;
        _isCheck = false;
        _mapObj = null;
    }
}

[Serializable]
public class MapTile
{
    public isCheck[] y_raw;
}


public class MapGenerator : MonoBehaviour
{
    [SerializeField] private StageManager _stage;
    [SerializeField] private Portal _portal;
    [SerializeField] private GameObject player;

    public MapTile[] x_raw;

    [SerializeField] private int sm_count;

    int currentX; //현재 x값
    int currentY; //현재 y값

    int nextX; //다음 x값
    int nextY; //다음 y값

    bool lastMapPlaced = false; // 마지막 맵이 배치되었는지 여부를 나타내는 변수
    bool specialMapPlaced = false; // 특수 맵이 배치되었는지 여부를 나타내는 변수

    //이동 : 업,다운,라이트,레프트
    int[] dx = { 0, 0, 1, -1 };
    int[] dy = { 1, -1, 0, 0 };
    int udrl; //방향

    int special_count;//특수맵 개수 랜덤(1~3)

    Vector2 startMapCoords; //시작맵 좌표 저장
    List<Vector2Int> normalMapCoords = new List<Vector2Int>(); //일반맵 좌표 저장 리스트
    List<Vector2Int> specialMapCoords = new List<Vector2Int>(); //특수맵 좌표 저장 리스트
    Vector2 nextMapCoords; //마지막맵 좌표 저장

    public static List<GameObject> portalList = new List<GameObject>();

    public GameObject Player { get => player; set => player = value; }

    private void Awake()
    {
        special_count = Random.Range(1, 3);
       // print("특수맵 개수: " + special_count);
    }

    void Start()
    {

        do //한번 실행하고 마지막맵이나 특수맵을 배치 못했을때 while문에서 돌림
        {
            MapCollocate();
            if(lastMapPlaced == false || specialMapPlaced == false)
            {
               // print("예외발생");
                InitializeMapTiles();
            }
        }while(lastMapPlaced == false || specialMapPlaced == false);

    }
    private List<T> ShuffleList<T>(List<T> list) //리스트 셔플
    {
        int random1, random2;
        T temp;

        for (int i = 0; i < list.Count; ++i)
        {
            random1 = Random.Range(0, list.Count);
            random2 = Random.Range(0, list.Count);

            temp = list[random1];
            list[random1] = list[random2];
            list[random2] = temp;
        }

        return list;
    }
    void MapCollocate() //맵타일생성
    {
        //시작맵 배치 
        GameObject StartMapPrefab = _stage.Stage1[SceneLoadManager.currentStage].StartMapPrefab; // 시작맵을 참조한 지역리스트 생성

        var start_x = Random.Range(0, x_raw.Length); // 시작x값 설정
        var start_y = Random.Range(0, x_raw[start_x].y_raw.Length); // 시작y값 설정

        x_raw[start_x].y_raw[start_y]._mapObj = Instantiate(StartMapPrefab, x_raw[start_x].y_raw[start_y]._tilePosition, StartMapPrefab.transform.rotation); //시작맵 배치
        x_raw[start_x].y_raw[start_y]._isCheck = true; //시작맵 체크


        currentX = start_x; //이전 x 좌표 초기화
        currentY = start_y; //이전 y 좌표 초기화

        Player.transform.position = new Vector3(x_raw[start_x].y_raw[start_y]._tilePosition.x, Player.transform.position.y, x_raw[start_x].y_raw[start_y]._tilePosition.z);

        //print("시작맵 배치 - 좌표: " + start_x + ", " + start_y);
        startMapCoords = new Vector2Int(start_x, start_y);


        //일반맵 배치
        List<GameObject> NormalMapPrefabs = new List<GameObject>(_stage.Stage1[SceneLoadManager.currentStage].NormalMapPrefabs); // 특수맵을 참조한 지역리스트 생성

        for (int i = 0; i < NormalMapPrefabs.Count; i++)
        {
            while (IsValidMapCoordinate(currentX, currentY))
            {

                // udrl 변수 : 0 - 위쪽, 1 - 아래쪽, 2 - 오른쪽, 3 - 왼쪽
                udrl = Random.Range(0, 4);

                nextX = currentX + dx[udrl];
                nextY = currentY + dy[udrl];

                if (IsValidMapCoordinate(nextX, nextY))
                {
                    if (x_raw[nextX].y_raw[nextY]._isCheck) //이미 생성된 맵일 경우
                    {
                        //현재위치 다음위치로 이동
                        currentX = nextX;
                        currentY = nextY;
                        i--;
                        //print("중복맵 배치 - 좌표: " + currentX + ", " + currentY + ", 이동: " + udrl);
                        break;
                    }
                    else // 생성되지 않은 맵일 경우
                    {
                        int nm = Random.Range(0, NormalMapPrefabs.Count);
                        x_raw[nextX].y_raw[nextY]._mapObj = Instantiate(NormalMapPrefabs[nm], x_raw[nextX].y_raw[nextY]._tilePosition, NormalMapPrefabs[nm].transform.rotation);
                        x_raw[nextX].y_raw[nextY]._isCheck = true;
                        PortalCreate(udrl, currentX, currentY, nextX, nextY);

                        //print("일반맵 배치 - 좌표: " + nextX + ", " + nextY + ", 이동: " + udrl);

                        if (!normalMapCoords.Contains(new Vector2Int(nextX, nextY)))
                        {
                            normalMapCoords.Add(new Vector2Int(nextX, nextY)); // 좌표 일반맵 리스트에 추가
                        }

                        currentX = nextX; // 현재 위치를 다음 위치로 이동
                        currentY = nextY;
                        break;
                    }
                }
            }
        }

        //마지막맵 배치
        GameObject NextMapPrefab = _stage.Stage1[SceneLoadManager.currentStage].NextMapPrefab; // 마지막맵을 참조한 지역리스트 생성
        for (int i = 0; i < 4; i++)
        {
            udrl = Random.Range(0, 4);
            int endX = currentX + dx[udrl]; //끝지점 x값
            int endY = currentY + dy[udrl]; //끝지점 y값
            if (IsValidMapCoordinate(endX, endY) && !x_raw[endX].y_raw[endY]._isCheck)
            {
                x_raw[endX].y_raw[endY]._mapObj = Instantiate(NextMapPrefab, x_raw[endX].y_raw[endY]._tilePosition, NextMapPrefab.transform.rotation);
                x_raw[endX].y_raw[endY]._isCheck = true;
                PortalCreate(udrl, currentX, currentY, endX, endY);
               // print("마지막맵 배치 - 좌표: " + endX + ", " + endY + ", 이동: " + udrl);
                nextMapCoords = new Vector2(endX, endY);
                lastMapPlaced = true; // 마지막 맵이 배치되었음을 표시
                break;
            }
        }

        // 특수맵 배치
        List<GameObject> specialMapPrefabs = new List<GameObject>(_stage.Stage1[SceneLoadManager.currentStage].SpecialMapPrefabs); // 특수맵을 참조한 지역리스트 생성

        normalMapCoords = ShuffleList(normalMapCoords);
        int CountNum = 0;
        for (int j = 0; j < normalMapCoords.Count; j++)
        {
            Vector2Int coord = normalMapCoords[j];

            int px = coord.x;
            int py = coord.y;

            if (CountNum != special_count)
            {
                for (int k = 0; k < 4; k++) // 상하좌우를 확인하여 배치 가능한 위치 찾기
                {
                    int nx = px + dx[k];
                    int ny = py + dy[k];

                    if (IsValidMapCoordinate(nx, ny))
                    {
                        if (x_raw[nx].y_raw[ny]._isCheck == false)
                        {
                            int sm = Random.Range(0, specialMapPrefabs.Count); //랜덤한 특수맵 선택
                            x_raw[nx].y_raw[ny]._mapObj = Instantiate(specialMapPrefabs[sm], x_raw[nx].y_raw[ny]._tilePosition, specialMapPrefabs[sm].transform.rotation);
                            x_raw[nx].y_raw[ny]._isCheck = true;
                            CountNum++;
                            specialMapPlaced = true;
                            specialMapCoords.Add(new Vector2Int(nx, ny)); // 좌표 특수맵 리스트에 추가
                            specialMapPrefabs.RemoveAt(sm); //사용된 특수맵 리스트에서 제거
                            PortalCreate(k, px, py, nx, ny);
                           // print("특수맵 배치 - 좌표: " + nx + ", " + ny + " / 랜덤 일반 맵 좌표: " + coord);
                            break;
                        }
                    }
                }
            }
        }
    }

    void PortalCreate(int udrl,int x, int y, int nx, int ny) //포탈생성
    {
            //포탈생성
        switch (udrl)
        {
            case 0:
                x_raw[x].y_raw[y]._mapObj.transform.GetChild(0).gameObject.SetActive(true);
                x_raw[nx].y_raw[ny]._mapObj.transform.GetChild(1).gameObject.SetActive(true);
                x_raw[x].y_raw[y]._mapObj.transform.GetChild(0).GetComponent<Portal>().ConnectedPortal(x_raw[nx].y_raw[ny]._mapObj.transform.GetChild(1).gameObject);
                x_raw[nx].y_raw[ny]._mapObj.transform.GetChild(1).GetComponent<Portal>().ConnectedPortal(x_raw[x].y_raw[y]._mapObj.transform.GetChild(0).gameObject);

                //포탈 저장
                portalList.Add(x_raw[x].y_raw[y]._mapObj.transform.GetChild(0).gameObject);
                portalList.Add(x_raw[nx].y_raw[ny]._mapObj.transform.GetChild(1).gameObject);
                break;
            case 1:
                x_raw[x].y_raw[y]._mapObj.transform.GetChild(1).gameObject.SetActive(true);
                x_raw[nx].y_raw[ny]._mapObj.transform.GetChild(0).gameObject.SetActive(true);
                x_raw[x].y_raw[y]._mapObj.transform.GetChild(1).GetComponent<Portal>().ConnectedPortal(x_raw[nx].y_raw[ny]._mapObj.transform.GetChild(0).gameObject);
                x_raw[nx].y_raw[ny]._mapObj.transform.GetChild(0).GetComponent<Portal>().ConnectedPortal(x_raw[x].y_raw[y]._mapObj.transform.GetChild(1).gameObject);

                //포탈 저장
                portalList.Add(x_raw[x].y_raw[y]._mapObj.transform.GetChild(1).gameObject);
                portalList.Add(x_raw[nx].y_raw[ny]._mapObj.transform.GetChild(0).gameObject);
                break;

            case 2:
                x_raw[x].y_raw[y]._mapObj.transform.GetChild(2).gameObject.SetActive(true);
                x_raw[nx].y_raw[ny]._mapObj.transform.GetChild(3).gameObject.SetActive(true);
                x_raw[x].y_raw[y]._mapObj.transform.GetChild(2).GetComponent<Portal>().ConnectedPortal(x_raw[nx].y_raw[ny]._mapObj.transform.GetChild(3).gameObject);
                x_raw[nx].y_raw[ny]._mapObj.transform.GetChild(3).GetComponent<Portal>().ConnectedPortal(x_raw[x].y_raw[y]._mapObj.transform.GetChild(2).gameObject);
                //포탈 저장
                portalList.Add(x_raw[x].y_raw[y]._mapObj.transform.GetChild(2).gameObject);
                portalList.Add(x_raw[nx].y_raw[ny]._mapObj.transform.GetChild(3).gameObject);
                break;
            case 3:
                x_raw[x].y_raw[y]._mapObj.transform.GetChild(3).gameObject.SetActive(true);
                x_raw[nx].y_raw[ny]._mapObj.transform.GetChild(2).gameObject.SetActive(true);
                x_raw[x].y_raw[y]._mapObj.transform.GetChild(3).GetComponent<Portal>().ConnectedPortal(x_raw[nx].y_raw[ny]._mapObj.transform.GetChild(2).gameObject);
                x_raw[nx].y_raw[ny]._mapObj.transform.GetChild(2).GetComponent<Portal>().ConnectedPortal(x_raw[x].y_raw[y]._mapObj.transform.GetChild(3).gameObject);
                //포탈 저장
                portalList.Add(x_raw[x].y_raw[y]._mapObj.transform.GetChild(3).gameObject);
                portalList.Add(x_raw[nx].y_raw[ny]._mapObj.transform.GetChild(2).gameObject);
                break;
        }
    }

    void InitializeMapTiles() //맵타일 초기화
    {
        //print("초기화 시작");
        DestroyMapObject(startMapCoords);

        foreach (Vector2Int coord in normalMapCoords)
        {
            DestroyMapObject(coord);
        }

        foreach (Vector2Int coord in specialMapCoords)
        {
            DestroyMapObject(coord);
        }

        DestroyMapObject(nextMapCoords);

        normalMapCoords.Clear();
        specialMapCoords.Clear();
    }

    void DestroyMapObject(Vector2 coord)
    {
        int x = (int)coord.x;
        int y = (int)coord.y;

        if (IsValidMapCoordinate(x, y) && x_raw[x].y_raw[y]._mapObj != null)
        {
            Destroy(x_raw[x].y_raw[y]._mapObj); //Instantiate 맵 제거
            x_raw[x].y_raw[y]._mapObj = null;
            x_raw[x].y_raw[y]._isCheck = false;
           
            //isCheck부분 초기화
        }
    }
    bool IsValidMapCoordinate(int x, int y) 
    {
        return x >= 0 && y >= 0 && x < x_raw.Length && y < x_raw[x].y_raw.Length;
    }

    public static void OnOffPortal(List<GameObject> portalList)
    {
        foreach(GameObject portals in portalList)
        {
            if(portals.activeSelf == true)
            {
                portals.SetActive(false);
            }
            else
            {
                portals.SetActive(true);
            }

        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)){
            OnOffPortal(portalList);
        }
    }
}
