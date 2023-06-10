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

    int currentX; //���� x��
    int currentY; //���� y��

    int nextX; //���� x��
    int nextY; //���� y��

    bool lastMapPlaced = false; // ������ ���� ��ġ�Ǿ����� ���θ� ��Ÿ���� ����
    bool specialMapPlaced = false; // Ư�� ���� ��ġ�Ǿ����� ���θ� ��Ÿ���� ����

    //�̵� : ��,�ٿ�,����Ʈ,����Ʈ
    int[] dx = { 0, 0, 1, -1 };
    int[] dy = { 1, -1, 0, 0 };
    int udrl; //����

    int special_count;//Ư���� ���� ����(1~3)

    Vector2 startMapCoords; //���۸� ��ǥ ����
    List<Vector2Int> normalMapCoords = new List<Vector2Int>(); //�Ϲݸ� ��ǥ ���� ����Ʈ
    List<Vector2Int> specialMapCoords = new List<Vector2Int>(); //Ư���� ��ǥ ���� ����Ʈ
    Vector2 nextMapCoords; //�������� ��ǥ ����

    public static List<GameObject> portalList = new List<GameObject>();

    public GameObject Player { get => player; set => player = value; }

    private void Awake()
    {
        special_count = Random.Range(1, 3);
       // print("Ư���� ����: " + special_count);
    }

    void Start()
    {

        do //�ѹ� �����ϰ� ���������̳� Ư������ ��ġ �������� while������ ����
        {
            MapCollocate();
            if(lastMapPlaced == false || specialMapPlaced == false)
            {
               // print("���ܹ߻�");
                InitializeMapTiles();
            }
        }while(lastMapPlaced == false || specialMapPlaced == false);

    }
    private List<T> ShuffleList<T>(List<T> list) //����Ʈ ����
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
    void MapCollocate() //��Ÿ�ϻ���
    {
        //���۸� ��ġ 
        GameObject StartMapPrefab = _stage.Stage1[SceneLoadManager.currentStage].StartMapPrefab; // ���۸��� ������ ��������Ʈ ����

        var start_x = Random.Range(0, x_raw.Length); // ����x�� ����
        var start_y = Random.Range(0, x_raw[start_x].y_raw.Length); // ����y�� ����

        x_raw[start_x].y_raw[start_y]._mapObj = Instantiate(StartMapPrefab, x_raw[start_x].y_raw[start_y]._tilePosition, StartMapPrefab.transform.rotation); //���۸� ��ġ
        x_raw[start_x].y_raw[start_y]._isCheck = true; //���۸� üũ


        currentX = start_x; //���� x ��ǥ �ʱ�ȭ
        currentY = start_y; //���� y ��ǥ �ʱ�ȭ

        Player.transform.position = new Vector3(x_raw[start_x].y_raw[start_y]._tilePosition.x, Player.transform.position.y, x_raw[start_x].y_raw[start_y]._tilePosition.z);

        //print("���۸� ��ġ - ��ǥ: " + start_x + ", " + start_y);
        startMapCoords = new Vector2Int(start_x, start_y);


        //�Ϲݸ� ��ġ
        List<GameObject> NormalMapPrefabs = new List<GameObject>(_stage.Stage1[SceneLoadManager.currentStage].NormalMapPrefabs); // Ư������ ������ ��������Ʈ ����

        for (int i = 0; i < NormalMapPrefabs.Count; i++)
        {
            while (IsValidMapCoordinate(currentX, currentY))
            {

                // udrl ���� : 0 - ����, 1 - �Ʒ���, 2 - ������, 3 - ����
                udrl = Random.Range(0, 4);

                nextX = currentX + dx[udrl];
                nextY = currentY + dy[udrl];

                if (IsValidMapCoordinate(nextX, nextY))
                {
                    if (x_raw[nextX].y_raw[nextY]._isCheck) //�̹� ������ ���� ���
                    {
                        //������ġ ������ġ�� �̵�
                        currentX = nextX;
                        currentY = nextY;
                        i--;
                        //print("�ߺ��� ��ġ - ��ǥ: " + currentX + ", " + currentY + ", �̵�: " + udrl);
                        break;
                    }
                    else // �������� ���� ���� ���
                    {
                        int nm = Random.Range(0, NormalMapPrefabs.Count);
                        x_raw[nextX].y_raw[nextY]._mapObj = Instantiate(NormalMapPrefabs[nm], x_raw[nextX].y_raw[nextY]._tilePosition, NormalMapPrefabs[nm].transform.rotation);
                        x_raw[nextX].y_raw[nextY]._isCheck = true;
                        PortalCreate(udrl, currentX, currentY, nextX, nextY);

                        //print("�Ϲݸ� ��ġ - ��ǥ: " + nextX + ", " + nextY + ", �̵�: " + udrl);

                        if (!normalMapCoords.Contains(new Vector2Int(nextX, nextY)))
                        {
                            normalMapCoords.Add(new Vector2Int(nextX, nextY)); // ��ǥ �Ϲݸ� ����Ʈ�� �߰�
                        }

                        currentX = nextX; // ���� ��ġ�� ���� ��ġ�� �̵�
                        currentY = nextY;
                        break;
                    }
                }
            }
        }

        //�������� ��ġ
        GameObject NextMapPrefab = _stage.Stage1[SceneLoadManager.currentStage].NextMapPrefab; // ���������� ������ ��������Ʈ ����
        for (int i = 0; i < 4; i++)
        {
            udrl = Random.Range(0, 4);
            int endX = currentX + dx[udrl]; //������ x��
            int endY = currentY + dy[udrl]; //������ y��
            if (IsValidMapCoordinate(endX, endY) && !x_raw[endX].y_raw[endY]._isCheck)
            {
                x_raw[endX].y_raw[endY]._mapObj = Instantiate(NextMapPrefab, x_raw[endX].y_raw[endY]._tilePosition, NextMapPrefab.transform.rotation);
                x_raw[endX].y_raw[endY]._isCheck = true;
                PortalCreate(udrl, currentX, currentY, endX, endY);
               // print("�������� ��ġ - ��ǥ: " + endX + ", " + endY + ", �̵�: " + udrl);
                nextMapCoords = new Vector2(endX, endY);
                lastMapPlaced = true; // ������ ���� ��ġ�Ǿ����� ǥ��
                break;
            }
        }

        // Ư���� ��ġ
        List<GameObject> specialMapPrefabs = new List<GameObject>(_stage.Stage1[SceneLoadManager.currentStage].SpecialMapPrefabs); // Ư������ ������ ��������Ʈ ����

        normalMapCoords = ShuffleList(normalMapCoords);
        int CountNum = 0;
        for (int j = 0; j < normalMapCoords.Count; j++)
        {
            Vector2Int coord = normalMapCoords[j];

            int px = coord.x;
            int py = coord.y;

            if (CountNum != special_count)
            {
                for (int k = 0; k < 4; k++) // �����¿츦 Ȯ���Ͽ� ��ġ ������ ��ġ ã��
                {
                    int nx = px + dx[k];
                    int ny = py + dy[k];

                    if (IsValidMapCoordinate(nx, ny))
                    {
                        if (x_raw[nx].y_raw[ny]._isCheck == false)
                        {
                            int sm = Random.Range(0, specialMapPrefabs.Count); //������ Ư���� ����
                            x_raw[nx].y_raw[ny]._mapObj = Instantiate(specialMapPrefabs[sm], x_raw[nx].y_raw[ny]._tilePosition, specialMapPrefabs[sm].transform.rotation);
                            x_raw[nx].y_raw[ny]._isCheck = true;
                            CountNum++;
                            specialMapPlaced = true;
                            specialMapCoords.Add(new Vector2Int(nx, ny)); // ��ǥ Ư���� ����Ʈ�� �߰�
                            specialMapPrefabs.RemoveAt(sm); //���� Ư���� ����Ʈ���� ����
                            PortalCreate(k, px, py, nx, ny);
                           // print("Ư���� ��ġ - ��ǥ: " + nx + ", " + ny + " / ���� �Ϲ� �� ��ǥ: " + coord);
                            break;
                        }
                    }
                }
            }
        }
    }

    void PortalCreate(int udrl,int x, int y, int nx, int ny) //��Ż����
    {
            //��Ż����
        switch (udrl)
        {
            case 0:
                x_raw[x].y_raw[y]._mapObj.transform.GetChild(0).gameObject.SetActive(true);
                x_raw[nx].y_raw[ny]._mapObj.transform.GetChild(1).gameObject.SetActive(true);
                x_raw[x].y_raw[y]._mapObj.transform.GetChild(0).GetComponent<Portal>().ConnectedPortal(x_raw[nx].y_raw[ny]._mapObj.transform.GetChild(1).gameObject);
                x_raw[nx].y_raw[ny]._mapObj.transform.GetChild(1).GetComponent<Portal>().ConnectedPortal(x_raw[x].y_raw[y]._mapObj.transform.GetChild(0).gameObject);

                //��Ż ����
                portalList.Add(x_raw[x].y_raw[y]._mapObj.transform.GetChild(0).gameObject);
                portalList.Add(x_raw[nx].y_raw[ny]._mapObj.transform.GetChild(1).gameObject);
                break;
            case 1:
                x_raw[x].y_raw[y]._mapObj.transform.GetChild(1).gameObject.SetActive(true);
                x_raw[nx].y_raw[ny]._mapObj.transform.GetChild(0).gameObject.SetActive(true);
                x_raw[x].y_raw[y]._mapObj.transform.GetChild(1).GetComponent<Portal>().ConnectedPortal(x_raw[nx].y_raw[ny]._mapObj.transform.GetChild(0).gameObject);
                x_raw[nx].y_raw[ny]._mapObj.transform.GetChild(0).GetComponent<Portal>().ConnectedPortal(x_raw[x].y_raw[y]._mapObj.transform.GetChild(1).gameObject);

                //��Ż ����
                portalList.Add(x_raw[x].y_raw[y]._mapObj.transform.GetChild(1).gameObject);
                portalList.Add(x_raw[nx].y_raw[ny]._mapObj.transform.GetChild(0).gameObject);
                break;

            case 2:
                x_raw[x].y_raw[y]._mapObj.transform.GetChild(2).gameObject.SetActive(true);
                x_raw[nx].y_raw[ny]._mapObj.transform.GetChild(3).gameObject.SetActive(true);
                x_raw[x].y_raw[y]._mapObj.transform.GetChild(2).GetComponent<Portal>().ConnectedPortal(x_raw[nx].y_raw[ny]._mapObj.transform.GetChild(3).gameObject);
                x_raw[nx].y_raw[ny]._mapObj.transform.GetChild(3).GetComponent<Portal>().ConnectedPortal(x_raw[x].y_raw[y]._mapObj.transform.GetChild(2).gameObject);
                //��Ż ����
                portalList.Add(x_raw[x].y_raw[y]._mapObj.transform.GetChild(2).gameObject);
                portalList.Add(x_raw[nx].y_raw[ny]._mapObj.transform.GetChild(3).gameObject);
                break;
            case 3:
                x_raw[x].y_raw[y]._mapObj.transform.GetChild(3).gameObject.SetActive(true);
                x_raw[nx].y_raw[ny]._mapObj.transform.GetChild(2).gameObject.SetActive(true);
                x_raw[x].y_raw[y]._mapObj.transform.GetChild(3).GetComponent<Portal>().ConnectedPortal(x_raw[nx].y_raw[ny]._mapObj.transform.GetChild(2).gameObject);
                x_raw[nx].y_raw[ny]._mapObj.transform.GetChild(2).GetComponent<Portal>().ConnectedPortal(x_raw[x].y_raw[y]._mapObj.transform.GetChild(3).gameObject);
                //��Ż ����
                portalList.Add(x_raw[x].y_raw[y]._mapObj.transform.GetChild(3).gameObject);
                portalList.Add(x_raw[nx].y_raw[ny]._mapObj.transform.GetChild(2).gameObject);
                break;
        }
    }

    void InitializeMapTiles() //��Ÿ�� �ʱ�ȭ
    {
        //print("�ʱ�ȭ ����");
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
            Destroy(x_raw[x].y_raw[y]._mapObj); //Instantiate �� ����
            x_raw[x].y_raw[y]._mapObj = null;
            x_raw[x].y_raw[y]._isCheck = false;
           
            //isCheck�κ� �ʱ�ȭ
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
