using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;

public class MummyAgent : Agent
{
    public Transform planeTr;  //바닥의 Transform
    public Transform goalTr;   //목적지의 Transform
    
    public Material goalMt;
    public Material deadMt;

    private Transform mummyTr;  //Agents의 Transform
    private MeshRenderer planeRender;

    public override void InitializeAgent()
    {
        mummyTr = GetComponent<Transform>();
        planeRender = planeTr.GetComponent<MeshRenderer>();
    }

    public override void CollectObservations()
    {
        //#1 관측정보 
        //바닥의 중심과 에이전트의 거리
        Vector3 dist1 = planeTr.position - mummyTr.position;

        //(바닥의 가로길이/2) : 관측 데이터의 정규화(Normalized)
        float norX1 = Mathf.Clamp(dist1.x / 5.0f, -1.0f, +1.0f);
        float norZ1 = Mathf.Clamp(dist1.z / 5.0f, -1.0f, +1.0f);

        //#2 관측정보
        //에이전트와 목적지(Goal)간의 거리가 가까울 수록 + 보상을 받을 가능성이 커진다.
        Vector3 dist2 = goalTr.position - mummyTr.position;

        float norX2 = Mathf.Clamp(dist2.x / 5.0f, -1.0f, +1.0f);
        float norZ2 = Mathf.Clamp(dist2.z / 5.0f, -1.0f, +1.0f); 

        //브레인에 관측 정보를 전달
        AddVectorObs(norX1);
        AddVectorObs(norZ1);
        AddVectorObs(norX2);
        AddVectorObs(norZ2);
    }

    //브레인으로 부터 결정된 명령을 전달하는 함수
    public override void AgentAction(float[] vectorAction, string textAction)
    {
        float h = vectorAction[0];
        float v = vectorAction[1];

        //이동할 방향 벡터를 계산
        Vector3 dir = (Vector3.forward * v) + (Vector3.right * h);
        //에이전트 이동 처리
        mummyTr.Translate(dir);

        AddReward(-0.001f);
    }

    void OnTriggerEnter(Collider coll)
    {
        //목적지에 도착 : + 보상
        if (coll.CompareTag("GOAL"))
        {
            AddReward(+1.0f);
            ResetStage();
        }
        //벽에 충돌하면 : - 보상
        if (coll.CompareTag("DEAD_ZONE"))
        {
            AddReward(-1.0f);
            Done();
        }
    }

    void OnTriggerStay(Collider coll)
    {
        if (coll.CompareTag("GOAL"))
            ResetStage();
    }

    public override void AgentReset()
    {
        ResetStage();
    }

    void ResetStage()
    {
        //에이전트의 위치를 변경
        mummyTr.localPosition = new Vector3(Random.Range(-4.0f, 4.0f)
                                            , 0.0f
                                            ,Random.Range(-4.0f, 4.0f));

        //목적지의 위치를 변경
        goalTr.localPosition =  new Vector3(Random.Range(-4.0f, 4.0f)
                                            , 0.55f
                                            ,Random.Range(-4.0f, 4.0f));
    }

}
