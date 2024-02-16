    using UnityEngine;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
     
    public class FollowPlayer : MonoBehaviour
    {
     
        public GameObject pet;
        public int followDistance;
        private Queue<Vector3> storedPositions;
        private Queue<bool> storedRunning;
        private Queue<bool> storedJumping;
        public PlayerMovement movement;
     
        public bool jumping;
        public bool running;
     
        void Awake()
        {
            storedPositions = new Queue<Vector3>();
            storedRunning = new Queue<bool>();
            storedJumping = new Queue<bool>();
        }
     
        void Update()
        {
            storedPositions.Enqueue(transform.position);
            storedJumping.Enqueue(movement.jumping);
            storedRunning.Enqueue(movement.running);
         
            if(storedPositions.Count > followDistance)
            {
                // if going to the left, mirror the sprite (flip it on the x axis)
                // if (storedPositions[0].x > pet.transform.position.x && (storedJumping[0] || storedRunning[0]))
                // {
                //     pet.transform.eulerAngles = Vector3.zero;
                // } else {
                //     pet.transform.eulerAngles = new Vector3(0f, 180f, 0f);
                // }

                if (storedPositions.Count > 1 && storedPositions.Peek().x < storedPositions.ElementAt(1).x  && (storedJumping.Peek() || storedRunning.Peek()))
                {
                    pet.transform.eulerAngles = Vector3.zero;
                } else {
                    pet.transform.eulerAngles = new Vector3(0f, 180f, 0f);
                }

                pet.transform.position = storedPositions.Dequeue();
                pet.transform.position = new Vector3(pet.transform.position.x, pet.transform.position.y - 0.1f, pet.transform.position.z);
                jumping = storedJumping.Dequeue();
                running = storedRunning.Dequeue();
            }
        }

        void OnDisable()
        {
            storedPositions.Clear();
            storedJumping.Clear();
            storedRunning.Clear();
            jumping = false;
            running = false;
        }
    }
