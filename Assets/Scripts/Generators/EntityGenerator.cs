using System;
using UnityEngine;

public class EntityGenerator {

    public static void Generate(Game game) {
        GenerateEntities(game);
    }

    private static void GenerateEntities(Game game) {
        Map map = game.map;

        for (int i = 0; i < 1000; i++) {
            bool found = false;
            Tile tile = null;
            while (!found) {
                int x = UnityEngine.Random.Range(0, map.tileLength);
                int z = UnityEngine.Random.Range(0, map.tileLength);
                tile = Tile.Get(x, z);
                found = tile.isWalkable;
            }
            Human human = HumanFactory.CreateHuman();
            human.currentPosition = tile.GetPosition();
            game.humans.Add(human);
        }

    }



}
