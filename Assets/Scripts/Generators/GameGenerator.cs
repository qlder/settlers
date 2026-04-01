using UnityEngine;

public class GameGenerator {

    public static void Generate(Game game) {
        MapGenerator.Generate(game);
        EntityGenerator.Generate(game);
    }

}
