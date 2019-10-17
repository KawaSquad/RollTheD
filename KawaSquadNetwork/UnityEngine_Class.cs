using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnityEngine
{
    public class Transform
    {
        public Vector3 position;
        public Vector3 rotation;
        public Vector3 scale;
        public Transform(Vector3 position, Vector3 rotation, Vector3 scale)
        {
            this.position = position;
            this.rotation = rotation;
            this.scale = scale;
        }

        public override string ToString()
        {
            return position.ToString() + rotation.ToString() + scale.ToString();
        }
    }

    public class Vector3
    {
        public float x;
        public float y;
        public float z;

        public Vector3()
        {
            this.x = 0f;
            this.y = 0f;
            this.z = 0f;
        }
        public Vector3(float x, float y)
        {
            this.x = x;
            this.y = y;
            this.z = 0f;
        }
        public Vector3(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
        public static Vector3 zero
        {
            get
            {
                return new Vector3(0, 0, 0);
            }
        }
        public static Vector3 one
        {
            get
            {
                return new Vector3(1, 1, 1);
            }
        }

        public override string ToString()
        {
            return "(" + x + "," + y + "," + z + ")";
        }
    }
}
