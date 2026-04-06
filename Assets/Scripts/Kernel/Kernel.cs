using UnityEngine;

public class Kernel : MonoBehaviour {


    void OnEnable() {
        GameOptions gameOptions = new GameOptions();
        Game game = new Game(gameOptions);
        Game.Inst = game;
        GameGenerator.Generate(game);
    }

    private float tickInterval = 0.02f;
    private float accumulator;
    private int tickCount;
    private int frameCount;

    void Update() {
        accumulator += Time.deltaTime;
        bool firstTick = true;
        while (accumulator >= tickInterval) {
            accumulator -= tickInterval;
            tickCount++;
            Game.Inst.Tick(tickCount, frameCount, firstTick);
            firstTick = false;
        }
        frameCount++;
    }


}
