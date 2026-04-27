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
            human.sex = Rng.Inst().EnumValue<Sex>();
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
        humanDna.hairHue = Rng.Inst().Float01();
        humanDna.hairSat = Rng.Inst().Float01();
        humanDna.hairDark = Rng.Inst().Float01();
        humanDna.skinHue = Rng.Inst().Float01();
        humanDna.skinSat = Rng.Inst().Float01();
        humanDna.skinDark = Rng.Inst().Float01();

        humanDna.eyeType = Rng.Inst().EnumValue<HumanDNA.EyeType>();
        humanDna.bodyType = Rng.Inst().EnumValue<HumanDNA.BodyType>();
        humanDna.faceType = Rng.Inst().EnumValue<HumanDNA.FaceType>();

        Debug.Log($"Spawned human with DNA: eyeType={humanDna.eyeType}, bodyType={humanDna.bodyType}, faceType={humanDna.faceType}");

        humanDna.Save();


        Hair hair = new Hair();
        hair.Id = human.Id;
        hair.style = Rng.Inst().EnumValue<Hair.HairStyle>();
        hair.Save();

        if (human.sex == Sex.Male) {
            Moustache moustache = new Moustache();
            moustache.Id = human.Id;
            moustache.style = Rng.Inst().EnumValue<Moustache.MoustacheStyle>();
            moustache.Save();
            Beard beard = new Beard();
            beard.Id = human.Id;
            beard.style = Rng.Inst().EnumValue<Beard.BeardStyle>();
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
