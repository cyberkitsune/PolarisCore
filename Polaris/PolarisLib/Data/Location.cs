using System;

namespace Polaris.Data
{
    public struct Location
    {
        // RotX, RotY, RotZ, and RotW make up a Quaternion
        public float RotX { get; set; }
        public float RotY { get; set; }
        public float RotZ { get; set; }
        public float RotW { get; set; }

        public float PosX { get; set; }
        public float PosY { get; set; }
        public float PosZ { get; set; }

        public Location(float RotX, float RotY, float RotZ, float RotW, float PosX, float PosY, float PosZ)
        {
            this.RotX = RotX;
            this.RotY = RotY;
            this.RotZ = RotZ;
            this.RotW = RotW;

            this.PosX = PosX;
            this.PosY = PosY;
            this.PosZ = PosZ;
        }

        public override string ToString()
        {
            return String.Format("Rot: ({0}, {1}, {2}, {3}) Pos: ({4}, {5}, {6})", RotX, RotY, RotZ, RotW, PosX, PosY, PosZ);
        }
    }
}
