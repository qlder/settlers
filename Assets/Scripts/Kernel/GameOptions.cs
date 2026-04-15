using UnityEngine;

[System.Serializable]
public class GameOptions {


    public int seed;
    public int mapSize = 10;

    public GameOptions() {
        seed = 1234;
    }

}
