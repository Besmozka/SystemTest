using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "DogsData", menuName = "DogsDataSO")]
public class DogsData : ScriptableObject
{
    [Header("Dogs data")]
    public string DogsApiUrl;
    public string DogsApiKey;
}