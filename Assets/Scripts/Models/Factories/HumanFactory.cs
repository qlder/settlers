using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

public struct HumanFactory {

    public enum BodyType {
        Type1 = 1,
        Type2 = 2,
        Type3 = 3,
    }

    public enum EyeType {
        Type1 = 1,
        Type2 = 2,
        Type3 = 3,
        Type4 = 4,
        Type5 = 5,
        Type6 = 6,
    }

    public enum FaceType {
        Type1 = 1,
        Type2 = 2,
        Type3 = 3,
        Type4 = 4,
        Type5 = 5,
        Type6 = 6,
    }


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

    public Entity Spawn(GameData gameData) {
        Entity human = new Entity();
        human.Id = gameData.GetNextId();
        human.species = SpeciesType.Human;
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

        Genetics genetics = new Genetics();
        genetics.Id = human.Id;
        genetics.hairGene = new HSDGene(Rng.Inst().Float01(), Rng.Inst().Float01(), Rng.Inst().Float01());
        genetics.skinGene = new HSDGene(Rng.Inst().Float01(), Rng.Inst().Float01(), Rng.Inst().Float01());

        string eyeType = ((int)Rng.Inst().EnumValue<HumanFactory.EyeType>()).ToString();
        string bodyType = ((int)Rng.Inst().EnumValue<HumanFactory.BodyType>()).ToString();
        string faceType = ((int)Rng.Inst().EnumValue<HumanFactory.FaceType>()).ToString();


        genetics.geneCodes = new Dictionary<string, string>
        {
            { "eyeType", eyeType },
            { "bodyType", bodyType },
            { "faceType", faceType },
        };

        Debug.Log($"Spawned human with DNA: eyeType={eyeType}, bodyType={bodyType}, faceType={faceType}");
        genetics.Save();


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
            gameData.livingData.Mothers.SetOwnerOf(human.Id, motherId.Value);
        }
        if (this.fatherId != null) {
            gameData.livingData.Fathers.SetOwnerOf(human.Id, fatherId.Value);
        }

        return human;
    }



}
