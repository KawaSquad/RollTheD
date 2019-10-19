using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace KawaSquad
{
    namespace Network
    {
        public class Pawn
        {
            public Guid server_Ref;
            public int ID_Hanlder;

            public Transform transform;

            public int pawnType;
            public string classParsed;
        }
    }
}