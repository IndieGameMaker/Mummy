using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageMgr : MonoBehaviour
{
    //싱글톤 변수 선언
    public static StageMgr instance = null;
    //버섯, 몬스터 프리팹 로드
    private GameObject mushroomPrefab;
    private GameObject monsterPrefab;

    //버섯, 몬스터의 최대 생성 갯수
    public int maxMushroom = 30;
    public int maxMonster  = 20;

    public Transform[] agents;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        mushroomPrefab = Resources.Load<GameObject>("Mushroom");
        monsterPrefab  = Resources.Load<GameObject>("Monster");
        InitStage();
    }

    void CreateMushroom()
    {
        //기존의 버섯을 모두 삭제
        foreach (var obj in GetComponentsInChildren<Transform>())
        {
            if (obj.name == "MUSHROOM")
            {
                Destroy(obj.gameObject);
            }
        }

        for (int i=0; i<maxMushroom; i++)
        {
            GameObject obj = Instantiate<GameObject>(mushroomPrefab, transform);
            obj.name = "MUSHROOM";
            //위치, 각도 설정
            Vector3 pos = new Vector3(Random.Range(-50.0f, 50.0f)
                                    , 0.0f
                                    , Random.Range(-50.0f, 50.0f));
            Quaternion rot = Quaternion.Euler(0, Random.Range(0, 360), 0);

            obj.transform.localPosition = pos;
            obj.transform.localRotation = rot;
        }
    }

    void CreateMonster()
    {
        //기존의 몬스터을 모두 삭제
        foreach (var obj in GetComponentsInChildren<Transform>())
        {
            if (obj.name == "MONSTER")
            {
                Destroy(obj.gameObject);
            }
        }

        for (int i=0; i<maxMonster; i++)
        {
            GameObject obj = Instantiate<GameObject>(monsterPrefab, transform);
            obj.name = "MONSTER";
            //위치, 각도 설정
            Vector3 pos = new Vector3(Random.Range(-50.0f, 50.0f)
                                    , 0.0f
                                    , Random.Range(-50.0f, 50.0f));
            Quaternion rot = Quaternion.Euler(0, Random.Range(0, 360), 0);

            obj.transform.localPosition = pos;
            obj.transform.localRotation = rot;
        }
    }

    public void InitAgents()
    {
        foreach(var agent in agents)
        {
            //위치, 각도 설정
            Vector3 pos = new Vector3(Random.Range(-50.0f, 50.0f)
                                    , 0.0f
                                    , Random.Range(-50.0f, 50.0f));
            Quaternion rot = Quaternion.Euler(0, Random.Range(0, 360), 0);

            agent.localPosition = pos;
            agent.localRotation = rot;            
        }

    }

    public void InitStage()
    {
        CreateMonster();
        CreateMushroom();
        InitAgents();
    }
}
