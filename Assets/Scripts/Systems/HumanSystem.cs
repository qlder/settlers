using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.Data.Common;
using Unity.Mathematics;
using System.Linq;

public class HumanSystem {

    public void Tick(int ticks) {
        foreach (long id in HumanData.Inst().Humans.Keys.ToList()) {
            var human = HumanData.Inst().Humans[id];
            if (human.position == null) continue;
            int moves = 0;
            for (int i = 0; i < ticks; i++) {
                if (Game.Random().NextDouble() < 0.1) {
                    moves++;
                }
            }
            float2 position = human.position.Value;
            float diffX = (float)Game.Random().NextDouble() * 2f - 1f;
            float diffY = (float)Game.Random().NextDouble() * 2f - 1f;
            position += new float2(diffX, diffY) * moves;
            human.position = position;
            HumanData.Inst().Humans[id] = human;
        }
    }

}
