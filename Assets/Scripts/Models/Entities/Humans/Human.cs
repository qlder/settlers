using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Unity.Mathematics;
using Unity.VisualScripting;
using Newtonsoft.Json.Linq;

public struct Human : IModel, ILiving
{

    #region GetSet
    public static Human? Get(long id)
    {
        //try to get or return null
        if (!LivingData.Inst().Humans.TryGetValue(id, out var human))
        {
            return null;
        }
        return human;
    }

    public void Save()
    {
        LivingData.Inst().Humans[Id] = this;
    }
    #endregion 

    public long Id { get; set; }
    public string Name;
    public Sex sex { get; set; }
    public float2? position { get; set; }

}

public struct HumanDNA : IModel
{

    #region GetSet
    public static HumanDNA? Get(long id)
    {
        //try to get or return null
        if (!LivingData.Inst().HumanDNA.TryGetValue(id, out var humanDna))
        {
            return null;
        }
        return humanDna;
    }

    public void Save()
    {
        LivingData.Inst().HumanDNA[Id] = this;
    }
    #endregion 

    public long Id { get; set; }
    public float hairHue; // 0~1 is 0.02 ~ 0.15
    public float hairSat; // 0~1 is 0.3 ~ 0.9
    public float hairDark; // 0~1 is 0.1 ~ 0.9

    public float skinHue; // 0.04f ~ 0.08f
    public float skinSat; // = 0.4f ~ 0.6f;
    public float skinDark; // = 0.2f ~ 0.9f;

    public Color HairColor()
    {
        float h = Mathf.Lerp(0.02f, 0.15f, hairHue);
        float s = Mathf.Lerp(0.3f, 0.9f, hairSat);
        float v = Mathf.Lerp(0.1f, 0.9f, 1f - hairDark);
        return Color.HSVToRGB(h, s, v);
    }

    public Color SkinColor()
    {
        float h = Mathf.Lerp(0.04f, 0.08f, skinHue);
        float s = Mathf.Lerp(0.4f, 0.6f, skinSat);
        float v = Mathf.Lerp(0.2f, 0.9f, 1f - (skinDark * skinDark));
        return Color.HSVToRGB(h, s, v);
    }

    public BodyType bodyType { get; set; }
    public enum BodyType
    {
        Type1 = 1,
        Type2 = 2,
        Type3 = 3,
    }

    public EyeType eyeType { get; set; }
    public enum EyeType
    {
        Type1 = 1,
        Type2 = 2,
        Type3 = 3,
        Type4 = 4,
        Type5 = 5,
        Type6 = 6,
    }

    public FaceType faceType { get; set; }
    public enum FaceType
    {
        Type1 = 1,
        Type2 = 2,
        Type3 = 3,
        Type4 = 4,
        Type5 = 5,
        Type6 = 6,
    }

}
