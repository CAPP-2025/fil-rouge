    using UnityEngine;
    using System.Collections;
    using System.Collections.Generic;
     
    public class FollowPlayer : MonoBehaviour
    {
     
        public GameObject followingMe;
        public int followDistance;
        private List<Vector3> storedPositions;
        private List<bool> storedRunning;
        private List<bool> storedJumping;
        public PlayerMovement movement;
     
        public bool jumping;
        public bool running;
     
        void Awake()
        {
            storedPositions = new List<Vector3>();
            storedRunning = new List<bool>();
            storedJumping = new List<bool>();
        }
     
        void Update()
        {
            storedPositions.Add(transform.position);
            storedJumping.Add(movement.jumping);
            storedRunning.Add(movement.running);
         
            if(storedPositions.Count > followDistance)
            {
                // if going to the left, mirror the sprite (flip it on the x axis)
                // if (storedPositions[0].x > followingMe.transform.position.x && (storedJumping[0] || storedRunning[0]))
                // {
                //     followingMe.transform.eulerAngles = Vector3.zero;
                // } else {
                //     followingMe.transform.eulerAngles = new Vector3(0f, 180f, 0f);
                // }

                if (storedPositions.Count > 1 && storedPositions[0].x < storedPositions[1].x  && (storedJumping[0] || storedRunning[0]))
                {
                    followingMe.transform.eulerAngles = Vector3.zero;
                } else {
                    followingMe.transform.eulerAngles = new Vector3(0f, 180f, 0f);
                }

                followingMe.transform.position = storedPositions[0];
                storedPositions.RemoveAt (0);
                jumping = storedJumping[0];
                running = storedRunning[0];
                storedJumping.RemoveAt (0);
                storedRunning.RemoveAt (0);
            }
        }
    }
