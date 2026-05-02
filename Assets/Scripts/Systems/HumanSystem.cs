using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.Data.Common;
using Unity.Mathematics;
using System.Linq;

public class HumanSystem {



    public void Tick(int ticks) {

        // Move humans randomly for now, TODO - FIX later


        foreach (int id in LivingData.Inst().Entities.Keys.ToList()) {

            if (Rng.Inst().Float(0f, 1f) < 0.95f) continue;

            Entity entity = LivingData.Inst().Entities[id];
            if (LivingData.Inst().Positions.TryGetValue(id, out var position) == false) continue;
            if (LivingData.Inst().Paths.TryGetValue(id, out var path)) {
                if (path.tile_keys.Count > 0) {
                    string nextTileKey = path.tile_keys.Peek();
                    Tile nextTile = MapData.Inst().tiles[nextTileKey];
                    position.coords = nextTile.Center;
                    position.Save();
                    path.tile_keys.Dequeue();
                    LivingData.Inst().Paths[id] = path;
                } else {
                    // Reached destination
                    LivingData.Inst().Paths.Remove(id);
                    Job job = JobData.Inst().Jobs[LivingData.Inst().JobIds[id]];
                    JobData.Inst().Jobs.Remove(job.Id);
                    LivingData.Inst().JobIds.Remove(id);

                }

            }

        }
    }

}
