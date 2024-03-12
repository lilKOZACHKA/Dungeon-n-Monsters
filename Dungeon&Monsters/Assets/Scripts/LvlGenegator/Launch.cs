using UnityEngine;

namespace assets
{
    public class Launch : MonoBehaviour
    {
        [SerializeField] private Vector2Int _size;
        private int nRooms;
        private int minRoomSize;
        private int maxRoomSize;

        private void Start()
        {
            Map gameMap = new Map(_size.x, _size.y);

            gameMap.GenerateConnectedRooms(nRooms, minRoomSize, maxRoomSize, 1);
            gameMap.PlaceDoors(1);
        }
    }
}