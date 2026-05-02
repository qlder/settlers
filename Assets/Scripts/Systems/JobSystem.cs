using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.Data.Common;
using Unity.Mathematics;
using System.Linq;

public class JobSystem {

    public void Tick(int ticks) {


        foreach (int id in LivingData.Inst().Entities.Keys.ToList()) {
            Entity entity = LivingData.Inst().Entities[id];
            if (LivingData.Inst().Positions.TryGetValue(id, out var position) == false) continue;
            if (LivingData.Inst().JobIds.TryGetValue(id, out var jobid) == false) {
                //Doesnt have a job. Assign a random job for now, TODO - FIX later
                Tile entityTile = position.GetTile();
                Sector sector = entityTile.GetSector();
                string moveToTile = Rng.Inst().Scramble(sector.tileKeys).FirstOrDefault();

                Job job = new Job() {
                    Id = JobData.Inst().GetNextId(),
                    tile_key = moveToTile,
                };
                job.Save();
                LivingData.Inst().JobIds[id] = job.Id;

                Path path = PathSystem.Inst().FindPath(entityTile.key, moveToTile);
                if (path.status == Path.PathStatus.Complete) {
                    // Debug.Log($"Assigned job {job.Id} to entity {id} to move to tile {moveToTile} with path length {path.tile_keys.Count}");
                    LivingData.Inst().Paths[id] = path;
                }
            }
        }
    }

}
