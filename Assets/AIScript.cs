using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class AIScript : Agent
{
    private Transform goalPosition;
    private Vector3 m_position;

    public float speed = 2.0f;
    private void Start()
    {
        goalPosition = GameObject.Find("Goal").GetComponent<Transform>();
        m_position = transform.position;
    }
    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.position);
        sensor.AddObservation(goalPosition.position);
    }

    public override void OnEpisodeBegin()
    {
        transform.position = m_position;
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continousActions = actionsOut.ContinuousActions;

        continousActions[0] = Input.GetAxisRaw("Horizontal");
        continousActions[1] = Input.GetAxisRaw("Vertical");
    }
    public override void OnActionReceived(ActionBuffers actions)
    {
        float moveX = actions.ContinuousActions[0];
        float moveY = actions.ContinuousActions[1];

        transform.position += speed * Time.deltaTime * new Vector3(moveX, 0.0f, moveY);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Goal>(out Goal goal))
        {
            SetReward(10.0f);
            EndEpisode();
            Debug.Log("Goal");
        }

        if(other.TryGetComponent<Wall>(out Wall wall))
        {
            SetReward(-10.0f);
            EndEpisode();
            Debug.Log("Wall");

        }
    }
}
