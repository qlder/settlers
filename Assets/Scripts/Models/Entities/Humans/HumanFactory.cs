using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

public struct HumanFactory {


    public Sex? sex;
    public string name;

    long? fatherId;
    long? motherId;

    float2? position;

    public void SetFatherId(long fatherId) {
        this.fatherId = fatherId;
    }

    public void SetMotherId(long motherId) {
        this.motherId = motherId;
    }

    public void SetPosition(float2 position) {
        this.position = position;
    }

    public Human Spawn(GameData gameData) {
        Human human = new Human();
        human.Id = gameData.GetNextId();
        if (sex != null) {
            human.sex = sex.Value;
        } else {
            human.sex = (Sex)Game.Random().Next(0, 2);
        }
        if (name != null) {
            human.Name = name;
        } else {
            human.Name = $"Human {human.Id}";
        }
        if (position != null) {
            human.position = position.Value;
        }

        gameData.humanData.Humans[human.Id] = human;
        if (this.motherId != null) {
            gameData.humanData.Mothers.SetOwnerOf(human.Id, motherId.Value);
        }
        if (this.fatherId != null) {
            gameData.humanData.Fathers.SetOwnerOf(human.Id, fatherId.Value);
        }

        return human;
    }

}
