using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedicineChest : MonoBehaviour
{
    public float lowMedChest = 15f;
    public float middleMedChest = 35f;
    public float hightMedChest = 65f;

    public TypeMedChest typeMedChest;

    public enum TypeMedChest
    {
        LowHelp,
        MiddleHelp,
        HightHelp
    }
}
