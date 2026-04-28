using UnityEngine;
using Unity.Mathematics;
using System.Collections.Generic;

public class HumanMono : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField]
    private SpriteRenderer bodyRend;
    [SerializeField]
    private SpriteRenderer faceRend;
    [SerializeField]
    private SpriteRenderer[] handRends; //0=left, 1=right

    [SerializeField]
    private SpriteRenderer hairRend;
    [SerializeField]
    private SpriteRenderer eyesRend;
    [SerializeField]
    private SpriteRenderer moustacheRend;
    [SerializeField]
    private SpriteRenderer beardRend;

    private List<SpriteRenderer> allRends = new();

    [TextArea(10, 10)]
    public string debugText;

    public void OnEnable()
    {
        allRends.Clear();
        allRends.Add(bodyRend);
        allRends.Add(faceRend);
        allRends.AddRange(handRends);
        allRends.Add(hairRend);
        allRends.Add(eyesRend);
        allRends.Add(moustacheRend);
        allRends.Add(beardRend);
    }


    public void UpdateVisuals(long id)
    {


        Human human = Human.Get(id).Value;
        HumanDNA humanDna = HumanDNA.Get(id).Value;

        float posY = human.position.Value.y;

        int sortingOrder = (int)(-posY * 100) + 1000000;
        foreach (var rend in allRends)
        {
            rend.sortingOrder = sortingOrder;
        }

        debugText = $"Human ID: {id}\n" +
                    $"Name: {human.Name}\n";

        bodyRend.sprite = HumanRenderer.Inst.humanSprites[$"human_body_{(int)humanDna.bodyType}_front"];
        faceRend.sprite = HumanRenderer.Inst.humanSprites[$"human_face_{(int)humanDna.faceType}_front"];
        eyesRend.sprite = HumanRenderer.Inst.humanSprites[$"human_eyes_{(int)humanDna.eyeType}_front"];

        bodyRend.color = humanDna.SkinColor();
        faceRend.color = humanDna.SkinColor();
        foreach (var hand in handRends)
        {
            hand.color = humanDna.SkinColor();
        }


        Hair? hair = Hair.Get(id);
        if (hair.HasValue)
        {
            string sexStr = human.sex == Sex.Male ? "m" : "f";
            string hairStr = $"human_{sexStr}_hair_{(int)hair.Value.style}_front";
            hairRend.sprite = HumanRenderer.Inst.humanSprites[hairStr];
            hairRend.color = humanDna.HairColor();
        }
        else
        {
            hairRend.sprite = null;
        }

        Beard? beard = Beard.Get(id);
        if (beard.HasValue)
        {
            string beardStr = $"human_beard_{(int)beard.Value.style}_front";
            beardRend.sprite = HumanRenderer.Inst.humanSprites[beardStr];
            beardRend.color = humanDna.HairColor();
        }
        else
        {
            beardRend.sprite = null;
        }

        Moustache? moustache = Moustache.Get(id);
        if (moustache.HasValue)
        {
            string moustacheStr = $"human_moust_{(int)moustache.Value.style}_front";
            moustacheRend.sprite = HumanRenderer.Inst.humanSprites[moustacheStr];
            moustacheRend.color = humanDna.HairColor();
        }
        else
        {
            moustacheRend.sprite = null;
        }

    }
}
