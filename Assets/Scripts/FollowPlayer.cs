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
                followingMe.transform.position = storedPositions[0];
                storedPositions.RemoveAt (0);
                jumping = storedJumping[0];
                running = storedRunning[0];
                storedJumping.RemoveAt (0);
                storedRunning.RemoveAt (0);
            }
        }
    }
