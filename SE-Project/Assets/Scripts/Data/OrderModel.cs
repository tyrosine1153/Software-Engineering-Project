using System;

[Serializable]
public class OrderOption
{
    // PotionID에 따라 결정되는 NextScenarioID, Result의 모임 자료형
    // -1의 경우 정의된 옵션 외의 값을 뜻함
    public int potionId;
    public int nextScenarioID;
    public int result;  // 결과값 (임시) => 엔딩 분기를 결정하는 분기 포인트? : 아직 미구현 기능이기에 result
    
    public OrderOption(int potionId, int nextScenarioID, int result)
    {
        this.potionId = potionId;
        this.nextScenarioID = nextScenarioID;
        this.result = result;
    }
}

[Serializable]
public class EndingPoint
{
    public int id;
    public int nextScenarioID;
    public int result;  // 결과값 (임시) => 엔딩 분기를 결정하는 분기 포인트? : 아직 엔딩 결정이 미구현 임으로 result로 해둠
}