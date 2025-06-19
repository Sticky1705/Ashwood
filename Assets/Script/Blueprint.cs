using UnityEngine;

public class Blueprint
{
    public string itemName;
    public string Req1, Req2;
    public int Req1Amount, Req2Amount;
    public int numOfRequirement;

    public Blueprint(string name, int reqNum, string R1, int R1num, string R2, int R2num)
    {
        itemName = name;
        numOfRequirement = reqNum;
        Req1 = R1;
        Req2 = R2;
        Req1Amount = R1num;
        Req2Amount = R2num;
    }
}