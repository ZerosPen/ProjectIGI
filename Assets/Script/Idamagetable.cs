using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Idamagetable
{
    float healthPoint { get; set; }
    public void TakeDamage(float damage);
}
