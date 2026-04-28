using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.Data.Common;
using Unity.Mathematics;
using System.Linq;

public class HumanSystem
{

    public void Tick(int ticks)
    {
        foreach (long id in LivingData.Inst().Entities.Keys.ToList())
        {
            var human = LivingData.Inst().Entities[id];
            if (human.position == null) continue;
            int moves = 1;
            // for (int i = 0; i < ticks; i++) {
            //     if (RngData.Inst().Float01() < 0.1f) {
            //         moves++;
            //     }
            // }
            float2 position = human.position.Value;
            float diffX = Rng.Inst().Float(-1f, 1f) * 0.05f;
            float diffY = Rng.Inst().Float(-1f, 1f) * 0.05f;
            position += new float2(diffX, diffY) * moves;
            human.position = position;
            human.Save();
        }
    }

}
