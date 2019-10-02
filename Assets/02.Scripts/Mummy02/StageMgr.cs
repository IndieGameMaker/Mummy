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

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        mushroomPrefab = Resources.Load<GameObject>("MushRoom");
        monsterPrefab  = Resources.Load<GameObject>("Monster");        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
