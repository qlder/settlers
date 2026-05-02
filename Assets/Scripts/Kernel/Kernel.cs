using UnityEngine;

public class Kernel : MonoBehaviour {



    void OnEnable() {
        GameOptions gameOptions = new GameOptions();
        gameOptions.seed = 12345;
        gameOptions.mapSize = 100;


        Game game = new Game();
        Game.Inst = game;

        GameGenerator generator = new GameGenerator();
        generator.Generate(game, gameOptions);

        Controller controller = new Controller();
        Controller.Inst = controller;

        game.Setup();
    }

    private float tickInterval = 0.01f;
    private float accumulator;
    private int frameCount;

    void Update() {

        float deltaTime = Time.deltaTime;
        Controller.Inst.DealWithInput(deltaTime);

        accumulator += deltaTime;
        int ticks = 0;
        while (accumulator >= tickInterval) {
            accumulator -= tickInterval;
            ticks++;
        }
        if (ticks > 0) {
            Game.Inst.Tick(ticks);
        }
        frameCount++;
    }


}
