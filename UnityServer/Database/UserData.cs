﻿using System;

public enum CastleState
{
    Peace = 0,
    Famine,
    Attacked,
    BeingAttacked
}

public enum HeroState
{
    Stationed = 0,
    Attack,
    Return,
    Dead,
}

[Serializable]
public class UserData
{
    string Id;
    int heroId;
    int heroLevel;
    int[] equipment;
    int[] inventoryId;
    int[] inventoryNum;
    int[] skill;
    int unitKind;
    int createUnitKind;
    int attackUnitKind;
    Unit[] unit;
    Unit[] createUnit;
    Unit[] attackUnit;
    DateTime unitCreateTime;
    int[] building;
    DateTime buildTime;
    int buildBuilding;
    int[] upgrade;
    int resource;
    HeroState heroState;
    CastleState castleState;

    public string ID { get { return Id; } }
    public int HeroId { get { return heroId; } }
    public int HeroLevel { get { return heroLevel; } }
    public int[] Equipment { get { return equipment; } }
    public int[] InventoryId { get { return inventoryId; } }
    public int[] InventoryNum { get { return inventoryNum; } }
    public Unit[] Unit { get { return unit; } }
    public Unit[] CreateUnit { get { return createUnit; } }
    public Unit[] AttackUnit { get { return attackUnit; } }
    public int UnitKind
    {
        get
        {
            unitKind = 0;

            for (int i = 0; i < unitNum; i++)
            {
                if (unit[i].num != 0)
                    unitKind++;
            }
            return unitKind;
        }
    }
    public int CreateUnitKind
    {
        get
        {
            createUnitKind = 0;

            createUnit = new Unit[unitNum];
            for (int i = 0; i < unitNum; i++) { createUnit[i] = new Unit(); }

            for (int i = 0; i < unitNum; i++)
            {
                if (createUnit[i].num != 0)
                    createUnitKind++;
            }
            return createUnitKind;
        }
    }
    public int AttackUnitKind
    {
        get
        {
            attackUnitKind = 0;

            for (int i = 0; i < unitNum; i++)
            {
                if (attackUnit[i].num != 0)
                    attackUnitKind++;
            }
            return attackUnitKind;
        }
    }
    public DateTime UnitCreateTime { get { return unitCreateTime; } }
    public int[] Skill { get { return skill; } }
    public int[] Building { get { return building; } }
    public DateTime BuildTime { get { return buildTime; } }
    public int BuildBuilding { get { return buildBuilding; } }
    public int[] Upgrade { get { return upgrade; } }
    public int Resource { get { return resource; } }
    public HeroState HState { get { return heroState; } }
    public CastleState CState { get { return castleState; } }

    public const int equipNum = 7;
    public const int invenNum = 16;
    public const int skillNum = 15;
    public const int unitNum = 5;
    public const int buildingNum = 6;

    public UserData(string newId)
    {
        Id = newId;
        heroId = 1;
        heroLevel = 1;
        equipment = new int[equipNum];
        inventoryId = new int[invenNum];
        inventoryNum = new int[invenNum];
        skill = new int[skillNum];
        unit = new Unit[unitNum];
        createUnit = new Unit[unitNum];
        attackUnit = new Unit[unitNum];
        building = new int[buildingNum];
        buildTime = new DateTime();
        buildBuilding = buildingNum;
        upgrade = new int[unitNum];
        resource = 0;
        heroState = HeroState.Stationed;
        castleState = CastleState.Peace;

        for (int i = 0; i < unitNum; i++)
        {
            unit[i] = new Unit();
            createUnit[i] = new Unit();
            attackUnit[i] = new Unit();
            upgrade[i] = 1;
        }
    }

    //레벨 변경
    public void SetLevel(int newLevel)
    {
        heroLevel = newLevel;
    }

    //클라이언트에서 미리 그곳에 아이템이 있는지 확인해서 데이터를 전송
    //장비 장착
    public void Equip(int itemId, int type)
    {
        equipment[type] = (byte) itemId;
    }

    //장비 해제
    public void UnEquip(int index)
    {
        equipment[index] = 0;
    }

    //아이템 획득
    public void AddItem(int Id)
    {
        int index = FindSlotWithId(Id);

        if(index != -1)
        {
            inventoryNum[index] ++;
        }
        else
        {
            index = FindEmptySlot(inventoryNum);

            if (index != -1)
            {
                inventoryId[index] = (byte) Id;
                inventoryNum[index] ++;
            }
        }
    }

    //아이템 빼기
    public void AbstractItem(int index)
    {
        if(index != 0)
            inventoryNum[index] --;
        if (inventoryNum[index] == 0)
            inventoryId[index] = 0;
    }

    //빈칸찾기
    public int FindEmptySlot(int[] arrange)
    {
        for (int i = 0; i < arrange.Length; i++)
        {
            if(arrange[i] == 0)
            {
                return i;
            }
        }
        return -1;
    }

    //아이템용 아이템 인덱스 찾기
    public int FindSlotWithId(int Id)
    {
        for(int i =0; i< invenNum; i++)
        {
            if(inventoryId[i] == Id)
            {
                return i;
            }
        }

        return -1;
    }

    //자원 사용
    public void UseResources (int amount)
    {
        resource -= amount;
    }

    //영웅 상태 변경
    public void ChangeHeroState(HeroState state)
    {
        heroState = state;
    }

    //성 상태 변경
    public void ChangeCastleState(CastleState state)
    {
        castleState = state;
    }

    //유닛숫자변경
    public void AddUnit(int unitId, int unitNum)
    {
        unit[unitId - 1].num += (byte) unitNum;
    }

    //건물건설
    public void Build(int buildingId)
    {
        buildBuilding = buildingId;
        buildTime = DateTime.Now + BuildingDatabase.Instance.buildingData[buildingId].GetLevelData(building[buildingId] + 1).BuildTime;
    }

    //건설 취소
    public void BuildCancel()
    {
        buildBuilding = buildingNum;
        buildTime = DateTime.Now;
    }

    //건설 완료
    public void Buildcomplete()
    {
        building[buildBuilding]++;
        buildBuilding = buildingNum;
        buildTime = DateTime.Now;
    }
    
    //유닛생산
    public void UnitCreate(UnitCreate unitCreate)
    {
        int[] unit = new int[unitNum];

        for (int i = 0; i < unitNum; i++)
        {
            unit[i] = createUnit[i].num;
        }

        int index = FindEmptySlot(unit);
        createUnit[index].Id = unitCreate.Id;
        createUnit[index].num = unitCreate.num;
        unitCreateTime = DateTime.Now + MultiplyTime(UnitDatabase.Instance.unitData[unitCreate.Id].CreateTime, unitCreate.num);
    }

    public TimeSpan MultiplyTime(TimeSpan time, int num)
    {
        return new TimeSpan(time.Hours * num, time.Minutes * num, time.Seconds * num);
    }

    //유닛공격, 복귀

}