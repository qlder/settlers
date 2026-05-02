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


        List<Entity> firstGen = new();

        for (int i = 0; i < 10; i++) {
            HumanFactory humanFactory = new HumanFactory();
            humanFactory.sex = (Sex)(i % 2);

            float2 position = MapData.Inst().tiles.Values.Where(t => t.groundType == GroundType.Earth).ToList().GetRandom().Center;
            humanFactory.SetPosition(position);

            Entity human = humanFactory.Spawn(game.data);
            Debug.Log($"Generated human {i} with sex {human.sex} at position {position}");
            firstGen.Add(human);
        }

        for (int i = 0; i < 10; i++) {
            HumanFactory humanFactory = new HumanFactory();

            Entity father = firstGen.GetRandom();
            Entity mother = firstGen.GetRandom();
            humanFactory.SetFatherId(father.Id);
            humanFactory.SetMotherId(mother.Id);
            float2 position = Position.Get(mother.Id).Value.coords;
            humanFactory.SetPosition(position);
            Entity human = humanFactory.Spawn(game.data);
            Debug.Log($"Generated sec gen human {i} with sex {human.sex} at position {position}");
        }







    }



}
