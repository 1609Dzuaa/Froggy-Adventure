using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Dùng SO để hạn chế việc Tight Coupling (Phụ thuộc chặt chẽ lẫn nhau giữa các class)
//và Generally speaking, the main purpose of SO is to store instances of data outside of a scene
//NOTE: "since all each script does is read from the player data asset."
//Docs:
//"Every time you instantiate that Prefab, it will get its own copy of that data.
//Instead of using this method, and storing duplicated data,
//you can use a ScriptableObject to store the data and then access it by
//reference from all of the Prefabs. This means that there is ONE copy of the data in memory."
//https://docs.unity3d.com/Manual/class-ScriptableObject.html
//https://gamedevbeginner.com/scriptable-objects-in-unity/#what_is_a_scriptable_object

//NOTE:
//Nên dùng SO dưới dạng (Read Only), hạn chế hoặc 0 modify trực tiếp nó trong quá trình Runtime
//https://www.reddit.com/r/Unity2D/comments/mayjhy/should_i_edit_scriptable_objects_at_runtime/
//https://forum.unity.com/threads/change-scriptableobject-at-runtime.1008376/

[CreateAssetMenu(fileName ="ScriptableObject", menuName = "ScriptableObject/PlayerStats")]
public class PlayerStats : ScriptableObject
{
    [SerializeField] private float _speedX;
    [SerializeField] private float _speedY;

    public float SpeedX { get { return _speedX; } }

    public float SpeedY { get { return _speedY; } }

}
