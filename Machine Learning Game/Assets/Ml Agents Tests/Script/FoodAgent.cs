using System;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Script
{
    public class FoodAgent : Agent
    {
        
        [SerializeField] private FoodButton foodButton;
        [SerializeField] private FoodSpawner foodSpawner;

        private Rigidbody _rigidbody;
        
        [SerializeField] private float moveSpeed = 5f;


        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        public override void OnEpisodeBegin(){
            
            transform.localPosition = new Vector3(Random.Range(-2, +2f), 0, Random.Range(-2, +2));

            _rigidbody.velocity = Vector3.zero;
            
            transform.rotation = Quaternion.identity;

            foodButton.ResetButton();
            foodSpawner.ResetFoodSpawner();
        }

        public override void CollectObservations(VectorSensor sensor)
        {
            sensor.AddObservation(foodButton.CanUseButton ? 1: 0);


            Vector3 dirToFoodButton = (foodButton.transform.localPosition - transform.localPosition).normalized;
            sensor.AddObservation(dirToFoodButton.x);
            sensor.AddObservation(dirToFoodButton.z);
            
            sensor.AddObservation(foodSpawner.HasSpawnedFood ? 1: 0);

            if (foodSpawner.HasSpawnedFood)
            {
                Vector3 dirToFood = (foodSpawner.GetLastFoodTransform.localPosition - transform.localPosition)
                    .normalized;
                
                sensor.AddObservation(dirToFood.x);
                sensor.AddObservation(dirToFood.z);
            }
            else
            {
                //Food not spawned
                sensor.AddObservation(0f);   //X
                sensor.AddObservation(0f);   //Z
            }
        }

        public override void OnActionReceived(ActionBuffers actions)
        {
            int moveX = actions.DiscreteActions[0]; //0 = Don't move; 1 = Left; 2 = Right
            int moveZ = actions.DiscreteActions[1]; //0 = Don't move; 1 = Back; 2 = Forward  

            Vector3 addForce = Vector3.zero;

            switch (moveX)
            {
                case  0: addForce.x = 0f; break;
                case  1: addForce.x = -1f; break;
                case  2: addForce.x = +1f; break;
            }
            
            switch (moveZ)                          
            {                                       
                case  0: addForce.z = 0f; break;    
                case  1: addForce.z = -1f; break;   
                case  2: addForce.z = +1f; break;   
            }

            _rigidbody.velocity = addForce * moveSpeed + new Vector3(0, _rigidbody.velocity.y, 0);

            bool isUseButtonDown = actions.DiscreteActions[2] == 1;

            if (isUseButtonDown)
            {
                Collider[] colliderArray = Physics.OverlapBox(transform.position, Vector3.one * 0.5f);

                foreach (var collider1 in colliderArray)
                {
                    if (collider1.TryGetComponent<FoodButton>(out FoodButton foodButtonFound))
                    {
                        if (foodButtonFound.CanUseButton)
                        {
                            foodButtonFound.UseButton();
                            AddReward(1f);
                        }
                    }
                }
            }
            
            AddReward(-1f / MaxStep);
        }



        public override void Heuristic(in ActionBuffers actionOut)
        {
            ActionSegment<int> discreteActions = actionOut.DiscreteActions;

            switch (Mathf.RoundToInt(Input.GetAxisRaw("Horizontal")))
            {
                case  -1: discreteActions[0] = 1; break;     
                case  0: discreteActions[0] = 0; break;    
                case  +1: discreteActions[0] = 2; break;    
            }
            
            switch (Mathf.RoundToInt(Input.GetAxisRaw("Vertical")))     
            {                                                             
                case  -1: discreteActions[1] = 1; break;                  
                case  0:  discreteActions[1] = 0; break;                   
                case  +1: discreteActions[1] = 2; break;                  
            }

            discreteActions[2] = Input.GetKey(KeyCode.E) ? 1 : 0; //Use action
        }


        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Goal"))
            {
                AddReward(1f);

                EndEpisode();
            }
        }
    }







}
