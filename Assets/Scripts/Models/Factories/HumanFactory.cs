using System.Collections.Generic;
using UnityEngine;

public class HumanFactory {


    public static Human CreateHuman() {
        Human human = new Human();
        human.name = $"Human_{System.Guid.NewGuid().ToString().Substring(0, 10)}";
        return human;
    }

    public static Human CreateHuman(Human father, Human mother) {
        Human human = new Human();
        human.name = $"Human_{System.Guid.NewGuid().ToString().Substring(0, 8)}";
        return human;
    }




}
