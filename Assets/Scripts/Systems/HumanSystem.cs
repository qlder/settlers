using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.Data.Common;
using Unity.Mathematics;
using System.Linq;

public class HumanSystem {

    public void Tick(int ticks) {

        // Move humans randomly for now, TODO - FIX later
        foreach (long id in LivingData.Inst().Entities.Keys.ToList()) {
            Entity entity = LivingData.Inst().Entities[id];
            if (LivingData.Inst().Positions.TryGetValue(id, out var position) == false) continue;
            int moves = 1;
            float diffX = Rng.Inst().Float(-1f, 1f) * 0.05f;
            float diffY = Rng.Inst().Float(-1f, 1f) * 0.05f;
            position.coords += new float2(diffX, diffY) * moves;
            position.Save();
        }
    }

}
