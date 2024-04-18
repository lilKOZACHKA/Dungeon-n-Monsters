﻿using System;
using System.Collections.Generic;
using System.Linq;

public class Room
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
        var roomRepresentation = new String('0', Width) + Environment.NewLine;

        for (int i = 0; i < Length - 2; i++)
        {
            roomRepresentation += "0" + new String('.', Width - 2) + "0" + Environment.NewLine;
        }

        roomRepresentation += new String('0', Width);
        return roomRepresentation;
    }
}

public static class RoomGenerator
{
    private static Random random = new Random();

    public static Room GenerateRandomRoom(int minSize, int maxSize)
    {
        int length = random.Next(minSize, maxSize + 1);
        int width = random.Next(minSize, maxSize + 1);
        return new Room(length, width);
    }
}

public class Map
{
    public int Height { get; }
    public int Width { get; }
    private char[,] map;
    public List<Tuple<Tuple<int, int>, Room>> Rooms { get; private set; }
    private int MinX, MinY, MaxX, MaxY;

    public Map(int height, int width)
    {
        Height = height;
        Width = width;
        map = GenerateMap(Height, Width);
        Rooms = new List<Tuple<Tuple<int, int>, Room>>();
        
        MinX = int.MaxValue;
        MinY = int.MaxValue;
        MaxX = int.MinValue;
        MaxY = int.MinValue;
    }

    public string[] ToStringArray()
    {
        var trimmedMap = TrimMap();
        var result = new string[trimmedMap.GetLength(0)];

        for (int y = 0; y < trimmedMap.GetLength(0); y++)
        {
            var row = "";
            for (int x = 0; x < trimmedMap.GetLength(1); x++)
            {
                row += trimmedMap[y, x];
            }
            result[y] = row;
        }

        return result;
    }

    private char[,] GenerateMap(int height, int width)
    {
        var newMap = new char[height, width];
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                newMap[y, x] = '.';
            }
        }
        return newMap;
    }

    public bool PlaceRoom(Room room, int startX, int startY, int padding = 1)
    {
        if (startX + room.Width + padding > Width || startY + room.Length + padding > Height)
            return false;

        for (int y = startY - padding; y < startY + room.Length + padding; y++)
        {
            for (int x = startX - padding; x < startX + room.Width + padding; x++)
            {
                if (x < 0 || y < 0 || x >= Width || y >= Height || map[y, x] != '.')
                    return false;
            }
        }

        for (int y = 0; y < room.Length; y++)
        {
            for (int x = 0; x < room.Width; x++)
            {
                int mapX = startX + x, mapY = startY + y;
                map[mapY, mapX] = x == 0 || x == room.Width - 1 || y == 0 || y == room.Length - 1 ? '0' : '0'; 
            }
        }

        Rooms.Add(new Tuple<Tuple<int, int>, Room>(new Tuple<int, int>(startX, startY), room));

        MinX = Math.Min(MinX, startX - padding);
        MinY = Math.Min(MinY, startY - padding);
        MaxX = Math.Max(MaxX, startX + room.Width + padding - 1);
        MaxY = Math.Max(MaxY, startY + room.Length + padding - 1);

        return true;
    }

    public Tuple<int, int> FindAdjacentPosition(Tuple<Tuple<int, int>, Room> existingRoom, Room newRoom, int padding = 1)
    {
        var (existingPosition, existingRoomObj) = existingRoom;
        var (x, y) = existingPosition;
        var directions = new List<Tuple<int, int>>
        {
            Tuple.Create(x + existingRoomObj.Width + padding, y), 
            Tuple.Create(x - newRoom.Width - padding, y), 
            Tuple.Create(x, y + existingRoomObj.Length + padding), 
            Tuple.Create(x, y - newRoom.Length - padding) 
        };

        Shuffle(directions); 

        foreach (var newPosition in directions)
        {
            if (PlaceRoom(newRoom, newPosition.Item1, newPosition.Item2, padding))
            {
                return newPosition;
            }
        }

        return null;
    }

    public void GenerateConnectedRooms(int numRooms, int minRoomSize, int maxRoomSize, int padding = 1, int startRoomWidth = 5, int startRoomLength = 5)
    {
        if (numRooms < 1) return;

        
        var firstRoom = new Room(startRoomLength, startRoomWidth);
        int startX = random.Next(0, Width - firstRoom.Width);
        int startY = random.Next(0, Height - firstRoom.Length);
        PlaceRoom(firstRoom, startX, startY, padding);

        
        for (int i = 1; i < numRooms; i++)
        {
            var newRoom = RoomGenerator.GenerateRandomRoom(minRoomSize, maxRoomSize);
            foreach (var existingRoom in Rooms)
            {
                var adjacentPosition = FindAdjacentPosition(existingRoom, newRoom, padding);
                if (adjacentPosition != null)
                {
                    break;
                }
            }
        }
    }

    public char[,] TrimMap()
    {
        int trimmedHeight = MaxY - MinY;
        int trimmedWidth = MaxX - MinX;

        
        if (trimmedHeight <= 0 || trimmedWidth <= 0 || trimmedHeight > Height || trimmedWidth > Width)
        {
            throw new InvalidOperationException($"Invalid trimmed map dimensions: {trimmedHeight}x{trimmedWidth}");
        }

        var trimmedMap = new char[trimmedHeight, trimmedWidth];

        for (int y = 0; y < trimmedHeight; y++)
        {
            for (int x = 0; x < trimmedWidth; x++)
            {
                trimmedMap[y, x] = map[MinY + y, MinX + x];
            }
        }

        return trimmedMap;
    }

    public void PlaceDoors(int padding = 1)
    {
        for (int i = 0; i < Rooms.Count; i++)
        {
            var ((startX1, startY1), room1) = Rooms[i];
            for (int j = i + 1; j < Rooms.Count; j++)
            {
                var ((startX2, startY2), room2) = Rooms[j];

                
                if (startY1 < startY2 + room2.Length + padding && startY1 + room1.Length + padding > startY2)
                {
                    if (startX1 + room1.Width + padding == startX2) 
                    {
                        int sharedStart = Math.Max(startY1, startY2) + 1;
                        int sharedEnd = Math.Min(startY1 + room1.Length, startY2 + room2.Length) - 1;
                        if (sharedEnd > sharedStart)
                        {
                            int doorY = (sharedStart + sharedEnd) / 2;
                            map[doorY, startX2 - 1] = 'D';
                        }
                    }
                    else if (startX2 + room2.Width + padding == startX1) 
                    {
                        int sharedStart = Math.Max(startY1, startY2) + 1;
                        int sharedEnd = Math.Min(startY1 + room1.Length, startY2 + room2.Length) - 1;
                        if (sharedEnd > sharedStart)
                        {
                            int doorY = (sharedStart + sharedEnd) / 2;
                            map[doorY, startX1 - 1] = 'D';
                        }
                    }
                }

                
                if (startX1 < startX2 + room2.Width + padding && startX1 + room1.Width + padding > startX2)
                {
                    if (startY1 + room1.Length + padding == startY2) 
                    {
                        int sharedStart = Math.Max(startX1, startX2) + 1;
                        int sharedEnd = Math.Min(startX1 + room1.Width, startX2 + room2.Width) - 1;
                        if (sharedEnd > sharedStart)
                        {
                            int doorX = (sharedStart + sharedEnd) / 2;
                            map[startY2 - 1, doorX] = 'D';
                        }
                    }
                    else if (startY2 + room2.Length + padding == startY1) 
                    {
                        int sharedStart = Math.Max(startX1, startX2) + 1;
                        int sharedEnd = Math.Min(startX1 + room1.Width, startX2 + room2.Width) - 1;
                        if (sharedEnd > sharedStart)
                        {
                            int doorX = (sharedStart + sharedEnd) / 2;
                            map[startY1 - 1, doorX] = 'D';
                        }
                    }
                }
            }
        }
    }

    public override string ToString()
    {
        var trimmedMap = TrimMap();
        var result = "";
        for (int y = 0; y < trimmedMap.GetLength(0); y++)
        {
            for (int x = 0; x < trimmedMap.GetLength(1); x++)
            {
                result += trimmedMap[y, x];
            }
            result += Environment.NewLine;
        }
        return result;
    }

    private static Random random = new Random();

    private static void Shuffle<T>(IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = random.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    public void PlaceTraps(int trapCount)
    {
        for (int roomIndex = 0; roomIndex < Rooms.Count; roomIndex++)
        {
            if (roomIndex == 0) continue; 

            var (position, room) = Rooms[roomIndex];
            var (startX, startY) = position;

            HashSet<Tuple<int, int>> placedTraps = new HashSet<Tuple<int, int>>();

            for (int i = 0; i < trapCount; i++)
            {
                int trapX = startX;
                int trapY = startY;
                int attempts = 0;
                do
                {
                    if (++attempts > 100) break; 

                    trapX = random.Next(startX, startX + room.Width);
                    trapY = random.Next(startY, startY + room.Length);
                } while (placedTraps.Contains(Tuple.Create(trapX, trapY)) || 
                        map[trapY, trapX] != '0' || 
                        IsAdjacentToDoor(trapX, trapY));

                if (attempts <= 100)
                {
                    map[trapY, trapX] = 'T';
                    placedTraps.Add(Tuple.Create(trapX, trapY));
                }
            }
        }
    }

    private bool IsAdjacentToDoor(int x, int y)
    {
        int[] dx = {1, -1, 0, 0};
        int[] dy = {0, 0, 1, -1};

        for (int i = 0; i < 4; i++)
        {
            int checkX = x + dx[i];
            int checkY = y + dy[i];
            if (checkX >= 0 && checkX < Width && checkY >= 0 && checkY < Height && map[checkY, checkX] == 'D')
            {
                return true;
            }
        }
        return false;
    }


    public void PlaceEntities(int entityCount)
    {
        for (int roomIndex = 0; roomIndex < Rooms.Count; roomIndex++)
        {
            var (position, room) = Rooms[roomIndex];
            var (startX, startY) = position;

            if (roomIndex == 0) 
            {
                map[startY + room.Length / 2, startX + room.Width / 2] = 'H';
                continue;
            }

            HashSet<Tuple<int, int>> placedEntities = new HashSet<Tuple<int, int>>();
            for (int j = 0; j < entityCount; j++)
            {
                int entityX = startX; 
                int entityY = startY;
                int attempts = 0;
                do
                {
                    if (++attempts > 100) break; 

                    entityX = random.Next(startX, startX + room.Width);
                    entityY = random.Next(startY, startY + room.Length);
                } while (placedEntities.Contains(Tuple.Create(entityX, entityY)) ||
                        map[entityY, entityX] != '0' ||
                        IsAdjacentToDoor(entityX, entityY));

                if (attempts <= 100)
                {
                    map[entityY, entityX] = 'E'; 
                    placedEntities.Add(Tuple.Create(entityX, entityY));
                }
            }
        }
    }

    public void PlaceChests(int maxChestsPerRoom)
    {
        if (Rooms.Count <= 1 || maxChestsPerRoom <= 0) return;

        for (int i = 1; i < Rooms.Count; i++) 
        {
            var (position, room) = Rooms[i];
            var (startX, startY) = position;

            bool placeAtWalls = random.Next(2) == 0;
            int chestsToPlace = random.Next(1, maxChestsPerRoom + 1);

            for (int j = 0; j < chestsToPlace; j++)
            {
                int chestX = startX; 
                int chestY = startY;
                int attempts = 0;
                do
                {
                    if (++attempts > 100) break; 

                    if (!placeAtWalls)
                    {
                        int centerX = startX + room.Width / 2;
                        int centerY = startY + room.Length / 2;
                        int radius = Math.Min(room.Width, room.Length) / 4;

                        chestX = random.Next(centerX - radius, centerX + radius + 1);
                        chestY = random.Next(centerY - radius, centerY + radius + 1);
                    }
                    else
                    {
                        switch (random.Next(4))
                        {
                            case 0: chestX = random.Next(startX + 1, startX + room.Width - 1); chestY = startY; break;
                            case 1: chestX = random.Next(startX + 1, startX + room.Width - 1); chestY = startY + room.Length - 1; break;
                            case 2: chestX = startX; chestY = random.Next(startY + 1, startY + room.Length - 1); break;
                            default: chestX = startX + room.Width - 1; chestY = random.Next(startY + 1, startY + room.Length - 1); break;
                        }
                    }
                } while (map[chestY, chestX] != '0' || IsAdjacentToDoor(chestX, chestY));

                if (attempts <= 100)
                {
                    map[chestY, chestX] = 'C'; 
                }
            }
        }
    }

    public List<(int startX, int startY, int length, int width)> GetAllRoomCoordinates()
    {
        List<(int startX, int startY, int length, int width)> roomDetails = new List<(int startX, int startY, int length, int width)>();
        foreach (var roomData in Rooms)
        {
            var ((startX, startY), room) = roomData;
            int adjustedStartX = startX - MinX;
            int adjustedStartY = startY - MinY;
            roomDetails.Add((adjustedStartX, adjustedStartY, room.Length, room.Width));
        }
        return roomDetails;
    }
}


public class Program
{
    public static void Main(string[] args)
    {
        Map gameMap = new Map(100, 100);
        int nRooms = 10, minRoomSize = 5, maxRoomSize = 10;

        gameMap.GenerateConnectedRooms(nRooms, minRoomSize, maxRoomSize, 1);
        gameMap.PlaceDoors(1);
        gameMap.PlaceChests(5);
        gameMap.PlaceTraps(5);
        gameMap.PlaceEntities(5);

        var roomCoordinates = gameMap.GetAllRoomCoordinates();
        foreach (var room in roomCoordinates)
        {
            Console.WriteLine($"Room at ({room.startX}, {room.startY}) with size {room.length}x{room.width}");
        }

        Console.WriteLine(gameMap);
        Console.ReadLine();
    }
}