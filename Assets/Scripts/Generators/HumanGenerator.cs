using System;
using System.Linq;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class HumanGenerator {

    public void Generate(Game game) {
        GenerateHumans(game);
    }

    private void GenerateHumans(Game game) {
        HumanData humanData = new HumanData();
        game.data.humanData = humanData;


        List<Human> firstGen = new();
        for (int i = 0; i < 1000; i++) {
            HumanFactory humanFactory = new HumanFactory();
            humanFactory.sex = (Sex)(i % 2);

            float2 position = MapData.Inst().tiles.Values.Where(t => t.groundType == GroundType.Earth).ToList().GetRandom().Center;
            humanFactory.SetPosition(position);

            Human human = humanFactory.Spawn(game.data);
            Debug.Log($"Generated human {human.Name} with sex {human.sex} at position {position}");
            firstGen.Add(human);
        }



    }



}
