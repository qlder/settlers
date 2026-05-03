using UnityEngine;

public class GameGenerator {

    public void Generate(Game game, GameOptions options) {
        game.data.gameOptions = options;
        MapGenerator mapGenerator = new MapGenerator();
        mapGenerator.Generate(game);
        HumanGenerator humanGenerator = new HumanGenerator();
        humanGenerator.Generate(game);


        game.data.cameraData.position = new Unity.Mathematics.float2(game.data.gameOptions.MapSize / 2f, game.data.gameOptions.MapSize / 2f);

        Debug.Log("Game generated.");
        Debug.LogWarning(game.data.livingData.Entities.Count);
    }

}
