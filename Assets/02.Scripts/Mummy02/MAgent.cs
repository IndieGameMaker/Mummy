﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;

public class MAgent : Agent
{
    private Transform tr;
    private Rigidbody rb;
    private RayPerception3D ray;
    private int getMushroom = 0;

    //이동 속도
    private float moveSpeed = 10.0f;

    //광선의 거리
    public float rayDistance = 50.0f;
    //광선의 발사 각도 (7개의 광선)
    public float[] rayAngles = {20.0f, 45.0f, 70.0f, 90.0f, 110.0f, 135.0f, 160.0f};
    //광선의 검출 대상 (4개의 검출 대상)
    public string[] detectObjects = {"MUSHROOM", "MONSTER", "WALL", "AGENT"};

    public bool isVisualObs = false;

    public override void InitializeAgent()
    {
        base.InitializeAgent();

        tr = GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();
        ray = GetComponent<RayPerception3D>();
    }

    public override void CollectObservations()
    {
        if (isVisualObs == false)
        {
            //광선 7 , 대상 (4 + 2)
            //Observation Size = 7 * 6 = 42
            AddVectorObs(ray.Perceive(rayDistance, rayAngles, detectObjects, 0.5f, 0.5f));

            Vector3 localVelocity = tr.InverseTransformDirection(rb.velocity);
            AddVectorObs(localVelocity.x);
            AddVectorObs(localVelocity.z);
        }
    }

    public override void AgentAction(float[] vectorAction, string textAction)
    {
        Vector3 rot = tr.up * vectorAction[0];      //0 : Horizontal 좌/우 회전
        Vector3 dir = tr.forward * vectorAction[1]; //1 : Vertical 전진/후진

        rb.AddForce(dir * moveSpeed, ForceMode.VelocityChange);
        tr.Rotate(rot * Time.fixedDeltaTime * 200.0f);

        //일정 속도이상이면 감속
        if (rb.velocity.sqrMagnitude > 5.0f)
        {
            rb.velocity *= 0.8f;
        }

        AddReward(-0.001f);

        if (tr.position.y <= -10.0f)
        {
            Vector3 pos = new Vector3(Random.Range(-50.0f, 50.0f)
                        , 0.0f
                        , Random.Range(-50.0f, 50.0f));
            Quaternion turn = Quaternion.Euler(0, Random.Range(0, 360), 0);
            tr.localPosition = pos;
            tr.localRotation = turn;
        }
    }

    void OnCollisionEnter(Collision coll)
    {
        if (coll.collider.CompareTag("MUSHROOM"))
        {
            AddReward(+1.0f);
            Destroy(coll.gameObject);
            if (++getMushroom == StageMgr.instance.maxMushroom)
            {
                ResetStage();
            }
        }

        if (coll.collider.CompareTag("MONSTER"))
        {
            AddReward(-1.0f);
            Done();
        }
    }

    void ResetStage()
    {
        // getMushroom = 0;
        // rb.velocity = Vector3.zero;

        StageMgr.instance.InitStage();
    }

    public override void AgentReset()
    {
        ResetStage();
    }
}
