using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;

public class MummyAgent : Agent
{
    public Transform planeTr;  //바닥의 Transform
    public Transform goalTr;   //목적지의 Transform

    private Transform mummyTr;  //Agents의 Transform

    public override void InitializeAgent()
    {
        mummyTr = GetComponent<Transform>();
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
    }
}
