using System.Collections;
using System.Collections.Generic;
using UnityEngine;


    public class PositionComparer : IComparer<Transform>
    {
        private Vector3 _serverPosition;

        public PositionComparer(Vector3 serverPosition)
        {
            _serverPosition = serverPosition;
        }
        
        public int Compare(Transform client1, Transform client2)
        {
            return
                (client1.position - _serverPosition).sqrMagnitude <
                (client2.position - _serverPosition).sqrMagnitude
                    ? 1
                    : -1;
        }
    }