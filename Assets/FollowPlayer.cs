    using UnityEngine;
    using System.Collections;
    using System.Collections.Generic;
     
    public class FollowPlayer : MonoBehaviour
    {
     
        public GameObject followingMe;
        public int followDistance;
        private List<Vector3> storedPositions;
     
     
        void Awake()
        {
            storedPositions = new List<Vector3>();
        }
     
        void Update()
        {
            storedPositions.Add(transform.position);
         
            if(storedPositions.Count > followDistance)
            {
                followingMe.transform.position = storedPositions[0];
                storedPositions.RemoveAt (0);
            }
        }
    }
