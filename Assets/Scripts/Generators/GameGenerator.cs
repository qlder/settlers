using UnityEngine;

public class GameGenerator {

    public void Generate(Game game, GameOptions options) {
        GameData gameData = new GameData();
        game.data = gameData;
        game.data.gameOptions = options;
        MapGenerator mapGenerator = new MapGenerator();
        mapGenerator.Generate(game);
        HumanGenerator humanGenerator = new HumanGenerator();
        humanGenerator.Generate(game);

        Debug.Log("Game generated.");
        Debug.LogWarning(game.data.humanData.Humans.Count);
    }

}
