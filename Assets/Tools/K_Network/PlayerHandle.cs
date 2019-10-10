using System;
using System.Collections.Generic;
using UnityEngine;

namespace KawaSquad
{
    namespace Network
    {
        public class PlayerHandle : MonoBehaviour
        {
            public PlayerController pawn;

            private Vector3 currentPosition = Vector3.zero;
            private Transform pawnTransform;

            public bool isClient = false;

            private void Start()
            {
                pawnTransform = pawn.transform;
            }
            private void Update()
            {
                if (!isClient)
                    return;

                if (currentPosition != pawnTransform.position)
                {
                    currentPosition = pawnTransform.position;
                    DataSender.SendPawnDestination(currentPosition);
                }
            }

            public void SetPawnPosition(Vector3 newPos)
            {
                currentPosition = newPos;
                pawnTransform.position = newPos;
            }
        }
    }
}