using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

public interface IModel {

    int Id { get; set; }

    void Save();

}
