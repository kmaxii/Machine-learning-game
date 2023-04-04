using System;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Script
{
    public class MoveToGoalAgent : Agent
    {
        [SerializeField] private Transform goal;

        [SerializeField] private Material winMaterial;
        [SerializeField] private Material loseMaterial;
        [SerializeField] private MeshRenderer floorRenderer;

        public override void OnEpisodeBegin()
        {
            transform.localPosition = new Vector3(Random.Range(-2, +2f), 0, Random.Range(-2, +2));
            goal.localPosition = new Vector3(Random.Range(-2.7f, +3f), 0, Random.Range(-3, +3));
        }

        public override void CollectObservations(VectorSensor sensor)
        {
            sensor.AddObservation(transform.localPosition);
            sensor.AddObservation(goal.localPosition);
        }

        public override void OnActionReceived(ActionBuffers actions)
        {
            float moveX = actions.ContinuousActions[0];
            float moveZ = actions.ContinuousActions[1];

            float moveSpeed = 10f;
            transform.localPosition += new Vector3(moveX, 0, moveZ) * Time.deltaTime * moveSpeed;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Goal"))
            {
                SetReward(1f);   
                EndEpisode();
                floorRenderer.material = winMaterial;
                return;
            }

            if (other.CompareTag("Wall"))
            {
                SetReward(-1f);
                EndEpisode();
                floorRenderer.material = loseMaterial;
            }
        }

        public override void Heuristic(in ActionBuffers actionsOut)
        {
            ActionSegment<float> continuousActions = actionsOut.ContinuousActions;

            continuousActions[0] = Input.GetAxisRaw("Horizontal");
            continuousActions[1] = Input.GetAxisRaw("Vertical");
        }
    }
}

