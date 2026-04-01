using System;
using UnityEngine;

public class EntityGenerator {

    public static void Generate(Game game) {
        GenerateEntities(game);
    }

    private static void GenerateEntities(Game game) {
        Map map = game.map;

        for (int i = 0; i < 1000; i++) {
            int x = UnityEngine.Random.Range(0, map.tileLength);
            int z = UnityEngine.Random.Range(0, map.tileLength);
            Tile tile = Tile.Get(x, z);
            Human human = HumanFactory.CreateHuman();
            human.currentTile = tile;
            game.humans.Add(human);
        }

    }



}
