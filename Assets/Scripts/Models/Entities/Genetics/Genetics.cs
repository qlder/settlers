using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Unity.Mathematics;
using Unity.VisualScripting;
using Newtonsoft.Json.Linq;


public struct Genetics : IModel {

    #region GetSet
    public static Genetics? Get(long id) {
        //try to get or return null
        if (!LivingData.Inst().Genetics.TryGetValue(id, out var genetics)) {
            return null;
        }
        return genetics;
    }

    public void Save() {
        LivingData.Inst().Genetics[Id] = this;
    }
    #endregion 

    public long Id { get; set; }
    public HSDGene hairGene;
    public HSDGene skinGene;
    public Dictionary<string, string> geneCodes;

    // public BodyType bodyType { get; set; }
    // public enum BodyType
    // {
    //     Type1 = 1,
    //     Type2 = 2,
    //     Type3 = 3,
    // }

    // public EyeType eyeType { get; set; }
    // public enum EyeType
    // {
    //     Type1 = 1,
    //     Type2 = 2,
    //     Type3 = 3,
    //     Type4 = 4,
    //     Type5 = 5,
    //     Type6 = 6,
    // }

    // public FaceType faceType { get; set; }
    // public enum FaceType
    // {
    //     Type1 = 1,
    //     Type2 = 2,
    //     Type3 = 3,
    //     Type4 = 4,
    //     Type5 = 5,
    //     Type6 = 6,
    // }

    // public float hairHue; // 0~1 is 0.02 ~ 0.15
    // public float hairSat; // 0~1 is 0.3 ~ 0.9
    // public float hairDark; // 0~1 is 0.1 ~ 0.9

    // public float skinHue; // 0.04f ~ 0.08f
    // public float skinSat; // = 0.4f ~ 0.6f;
    // public float skinDark; // = 0.2f ~ 0.9f;


    //TODO: Move somewhere else for other animals
    public Color HairColor() {
        float h = Mathf.Lerp(0.02f, 0.15f, hairGene.hue);
        float s = Mathf.Lerp(0.3f, 0.9f, hairGene.sat);
        float v = Mathf.Lerp(0.1f, 0.9f, 1f - hairGene.dark);
        return Color.HSVToRGB(h, s, v);
    }

    public Color SkinColor() {
        float h = Mathf.Lerp(0.04f, 0.08f, skinGene.hue);
        float s = Mathf.Lerp(0.4f, 0.6f, skinGene.sat);
        float v = Mathf.Lerp(0.2f, 0.9f, 1f - (skinGene.dark * skinGene.dark));
        return Color.HSVToRGB(h, s, v);
    }



}
