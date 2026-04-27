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


        human.Save();

        HumanDNA humanDna = new HumanDNA();
        humanDna.Id = human.Id;
        humanDna.hairHue = (float)Game.Random().NextDouble();
        humanDna.hairSat = (float)Game.Random().NextDouble();
        humanDna.hairDark = (float)Game.Random().NextDouble();
        humanDna.skinHue = (float)Game.Random().NextDouble();
        humanDna.skinSat = (float)Game.Random().NextDouble();
        humanDna.skinDark = (float)Game.Random().NextDouble();

        humanDna.eyeType = Game.Random().GetRandomValue<HumanDNA.EyeType>();
        humanDna.bodyType = Game.Random().GetRandomValue<HumanDNA.BodyType>();
        humanDna.faceType = Game.Random().GetRandomValue<HumanDNA.FaceType>();

        Debug.Log($"Spawned human with DNA: eyeType={humanDna.eyeType}, bodyType={humanDna.bodyType}, faceType={humanDna.faceType}");

        humanDna.Save();


        Hair hair = new Hair();
        hair.Id = human.Id;
        hair.style = Game.Random().GetRandomValue<Hair.HairStyle>();
        hair.Save();

        if (human.sex == Sex.Male) {
            Moustache moustache = new Moustache();
            moustache.Id = human.Id;
            moustache.style = Game.Random().GetRandomValue<Moustache.MoustacheStyle>();
            moustache.Save();
            Beard beard = new Beard();
            beard.Id = human.Id;
            beard.style = Game.Random().GetRandomValue<Beard.BeardStyle>();
            beard.Save();
        }


        if (this.motherId != null) {
            gameData.humanData.Mothers.SetOwnerOf(human.Id, motherId.Value);
        }
        if (this.fatherId != null) {
            gameData.humanData.Fathers.SetOwnerOf(human.Id, fatherId.Value);
        }

        return human;
    }



}
