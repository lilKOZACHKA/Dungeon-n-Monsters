using System;
using UnityEngine;


namespace assets
{
    public class Room : MonoBehaviour
    {
        public int Length { get; set; }
        public int Width { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        public Room(int length, int width, int x = 0, int y = 0)
        {
            Length = length;
            Width = width;
            X = x;
            Y = y;
        }

        public void Rotate()
        {
            int temp = Length;
            Length = Width;
            Width = temp;
        }

        public override string ToString()
        {
            var roomRepresentation = new String('#', Width) + Environment.NewLine;

            for (int i = 0; i < Length - 2; i++)
            {
                roomRepresentation += "#" + new String('.', Width - 2) + "#" + Environment.NewLine;
            }

            roomRepresentation += new String('#', Width);
            return roomRepresentation;
        }
    }
}