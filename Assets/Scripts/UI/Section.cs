using UnityEngine;
using NaughtyAttributes;

[CreateAssetMenu(fileName = "Section", menuName = "Sections/New Section")]
public class Section : ScriptableObject
{
    //Variables
    public string sectionTitle;
    public Sprite sectionIcon;
}
