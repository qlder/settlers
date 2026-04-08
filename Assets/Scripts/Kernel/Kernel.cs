using UnityEngine;

public class Kernel : MonoBehaviour {


    void OnEnable() {
        GameOptions gameOptions = new GameOptions();
        Game game = new Game(gameOptions);
        Game.Inst = game;
        GameGenerator.Generate(game);
    }

    private float tickInterval = 0.01f;
    private float accumulator;
    private int frameCount;

    void Update() {
        accumulator += Time.deltaTime;
        int ticks = 0;
        while (accumulator >= tickInterval) {
            accumulator -= tickInterval;
            ticks++;
        }
        Game.Inst.Tick(ticks);
        frameCount++;
    }


}
