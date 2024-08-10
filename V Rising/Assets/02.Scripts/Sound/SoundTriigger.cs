using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundTriigger : MonoBehaviour
{
    public void ActiveSound(string soundEnum)
    {
        SoundManager.instance.ActiveOnShotSFXSound(SoundManager.instance.enumToString[soundEnum], transform, Vector3.zero);
    }
}
