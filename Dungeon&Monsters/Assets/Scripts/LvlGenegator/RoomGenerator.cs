using UnityEngine;
using System;

namespace assets
{
    public class RoomGenerator : MonoBehaviour
    {
        private static Unity.Mathematics.Random random = new Unity.Mathematics.Random();

        public static Room GenerateRandomRoom(int minSize, int maxSize)
        {
            int length = random.Next(minSize, maxSize + 1);
            int width = random.Next(minSize, maxSize + 1);
            return new Room(length, width);
        }
    }
}