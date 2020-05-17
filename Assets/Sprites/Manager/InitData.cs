using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AllAsset;
//读取数据，初始化数据类
[System.Serializable]
public class InitData
{
    Dictionary<int, csvcard> csvcard = new Dictionary<int, csvcard>();
    public List<playerCard> ShowAllCards;

    public void Awake()
    {
        //CardInit();
        if (gameManager.Instance.testcard)
        {
            EditorCardInit(gameManager.Instance.TestCardEditor);
        }
        else
        {
            EditorCardInit(gameManager.Instance.CardEditorBoard);
        }

        MagicPartInit();
        BefallInit();
        MonsterInit();
    }
    //数据加载全卡
    void CardInit()
    {
        ShowAllCards = cardAsset.AllIdCards;
        playerCard newcard;
        //弃掉所有手牌，每弃一张手牌造成3点伤害，每补齐一横行或纵行，造成的伤害+1
        newcard = new playerCard(0, "弃伤补升", CardKind.AttackCard, 2, 1);
        cardEffectBase neweffect = new CardEffect_DisAllCard();
        newcard.AddEffect(neweffect);
        cardEffectBase repeateffect = new CardEffect_RepeatByEffect(neweffect);
        repeateffect.AddChildEffect(new CardEffect_DamageByJudge(3,new Judge_buqiheng(0)));
        newcard.AddEffect(repeateffect);
        cardAsset.AllIdCards.Add(newcard);

        //1费 抽2，打5
        playerCard card_jinGangQiangPo = new playerCard(3,"金刚枪破", CardKind.AttackCard, 1, 1);
        card_jinGangQiangPo.AddEffect(new Damage(5));
        card_jinGangQiangPo.AddEffect(new DrawCard(2));
        cardAsset.AllIdCards.Add(card_jinGangQiangPo);
        //2费 打8 护甲8
        playerCard attackAndDeffence = new playerCard(4, "攻击和防御", CardKind.SkillCard, 2,1);
        attackAndDeffence.AddEffect(new Damage(8));
        attackAndDeffence.AddEffect(new Armor(8));
        cardAsset.AllIdCards.Add(attackAndDeffence);
        //1费 灼烧4
        playerCard fire = new playerCard(5, "烈焰", CardKind.SkillCard, 1,1);
        fire.AddEffect(new Burn(4));
        cardAsset.AllIdCards.Add(fire);
        //1费 链接
        playerCard link = new playerCard(6, "不稳定连结", CardKind.SkillCard, 1,2);
        link.AddEffect(new LinkRandom());
        link.AddEffect(new CardEffect_ToExitLink());
        cardAsset.AllIdCards.Add(link);
        //1费 魔珠连环
        playerCard mozhulianhuan = new playerCard(7, "魔珠连环", CardKind.AttackCard, 1,2);
        mozhulianhuan.AddEffect(new Repeat(3,new Damage(3)));
        cardAsset.AllIdCards.Add(mozhulianhuan);
        //1费 降神 打7，如果敌人要攻击，则抽三张卡
        playerCard xiangshen = new playerCard(8, "降神", CardKind.AttackCard, 1,2);
        xiangshen.AddEffect(new Damage(7));
        cardEffectBase whethereffect = new CardEffect_Whether(new Judge_EnemyWillAttack(), new DrawCard(3));
        xiangshen.AddEffect(whethereffect);
        cardAsset.AllIdCards.Add(xiangshen);
        //1费 闪光爆裂 打8，抽1，弃3
        playerCard shanguangbaolie = new playerCard(9, "闪光爆裂", CardKind.AttackCard, 1,1);
        shanguangbaolie.AddEffect(new Damage(8));
        shanguangbaolie.AddEffect(new DrawCard(1));
        shanguangbaolie.AddEffect(new CardEffect_DisSomeCard(1));
        cardAsset.AllIdCards.Add(shanguangbaolie);
        //1费 飞弹 打6 
        playerCard feidan = new playerCard(10, "飞弹", CardKind.AttackCard, 1,0);
        feidan.AddEffect(new Damage(6));
        cardAsset.AllIdCards.Add(feidan);
        //1费 飞弹+ 打9
        playerCard feidanplus = new playerCard(11, "飞弹+", CardKind.AttackCard, 1,0);
        feidanplus.AddEffect(new Damage(9));
        cardAsset.AllIdCards.Add(feidanplus);
        //1费 护盾 5甲
        playerCard hudun = new playerCard(12, "护盾", CardKind.SkillCard, 1,0);
        hudun.AddEffect(new Armor(5));
        cardAsset.AllIdCards.Add(hudun);
        //1费 护盾+ 8甲
        playerCard hudunplus = new playerCard(13, "护盾+", CardKind.SkillCard, 1,0);
        hudunplus.AddEffect(new Armor(8));
        cardAsset.AllIdCards.Add(hudunplus);
        //2费 火球术 10点伤害，3层灼烧
        playerCard huoqiushu = new playerCard(14, "火球术", CardKind.AttackCard, 2,1);
        huoqiushu.AddEffect(new Damage(10));
        huoqiushu.AddEffect(new Burn(3));
        cardAsset.AllIdCards.Add(huoqiushu);
        //1费 点燃 2层灼烧，打1两次
        playerCard dianran = new playerCard(15, "点燃", CardKind.AttackCard, 1,1);
        dianran.AddEffect(new Burn(2));
        dianran.AddEffect(new Repeat(2, new Damage(1)));
        cardAsset.AllIdCards.Add(dianran);
        //2费 火焰屏障 12甲，8层灼烧
        playerCard huoyanpingzhang = new playerCard(16, "火焰屏障", CardKind.SkillCard, 2,2);
        huoyanpingzhang.AddEffect(new Armor(12));
        huoyanpingzhang.AddEffect(new Burn(8));
        cardAsset.AllIdCards.Add(huoyanpingzhang);
        //0费 火花 使敌人获得2点灼烧 
        playerCard huohua = new playerCard(17, "火花", CardKind.SkillCard, 0,1);
        huohua.AddEffect(new Burn(2));
        cardAsset.AllIdCards.Add(huohua);
        //2费 炎爆 使敌人的灼烧层数翻倍
        playerCard yanbao = new playerCard(18, "炎爆", CardKind.SkillCard, 2,2);
        yanbao.AddEffect(new DoubleBurn(2));
        cardAsset.AllIdCards.Add(yanbao);
        //0费 临时媒介 在本回合使两个部件处于连接状态
        playerCard linshimeijie = new playerCard(19, "临时媒介", CardKind.SkillCard, 0,2);
        linshimeijie.AddEffect(new LinkRandom());
        linshimeijie.AddEffect(new CardEffect_ToExitLink());
        cardAsset.AllIdCards.Add(linshimeijie);
        //1费 修整 获得8点格挡，抽两张牌
        playerCard xiuzheng = new playerCard(20, "修整", CardKind.SkillCard, 1,1);
        xiuzheng.AddEffect(new Armor(8));
        xiuzheng.AddEffect(new DrawCard(2));
        cardAsset.AllIdCards.Add(xiuzheng);
        //1费 风暴 造成3点伤害3次
        playerCard fengbao = new playerCard(21, "风暴", CardKind.AttackCard, 1,1);
        fengbao.AddEffect(new Repeat(3, new Damage(3)));
        cardAsset.AllIdCards.Add(fengbao);
        //0费 充能 抽一张牌，弃置一张牌，如果敌人灼烧层数大于等于5，额外抽两张牌
        playerCard chongneng = new playerCard(22, "充能", CardKind.SkillCard, 0,2);
        chongneng.AddEffect(new DrawCard(1));
        chongneng.AddEffect(new CardEffect_DisSomeCard(1));
        cardEffectBase whethereffect2 = new CardEffect_Whether(new Judge_BrunNumber(5), new DrawCard(2));
        cardAsset.AllIdCards.Add(chongneng);
        //1费 攻守兼备 每补齐一个横行，造成6点伤害；每补齐一个纵列，获得6点格挡
        playerCard gongshoujianbei = new playerCard(23, "攻守兼备", CardKind.SkillCard, 1, 1);
        judgeCondition judgeFillH = new Judge_buqiheng(0);
        judgeCondition judgeFillV = new Judge_buqishu(0);
        cardEffectBase whethereffect3 = new CardEffect_Whether(judgeFillH, new CardEffect_RepeatByFill(judgeFillH, new Damage(6)));
        cardEffectBase whethereffect4 = new CardEffect_Whether(judgeFillV, new CardEffect_RepeatByFill(judgeFillV, new Armor(6)));
        gongshoujianbei.AddEffect(whethereffect3);
        gongshoujianbei.AddEffect(whethereffect4);
        cardAsset.AllIdCards.Add(gongshoujianbei);
        //诅咒
        playerCard zuzhou = new playerCard(24, "诅咒", CardKind.CurseCard, 0, 0);
        cardAsset.AllIdCards.Add(zuzhou);
    }
    //“手动”加载全部件 可能是暂定
    void MagicPartInit()
    {
        int[] a = { 0, 1, 0, 0, 1, 0, 0, 0, 0 };
        MagicPart newpart;
        Reaction reaction;

        newpart = new MagicPart("护佑之石", a, 1);
        reaction = new Reaction_Affect("护盾1", new extraDeffenceUp(1), EventKind.Event_Armor, newpart);
        newpart.addReaction(reaction);
        magicpartAsset.AllMagicParts.Add(newpart);

        newpart = new MagicPart("冲击之石", a, 0);
        reaction = new Reaction_Affect("法强1", new extraAttackUp(1), EventKind.Event_Damage, newpart);
        newpart.addReaction(reaction);
        magicpartAsset.AllMagicParts.Add(newpart);

        newpart = new MagicPart("印记护佑", a, 1);
        reaction = new Reaction_Affect("护盾2", new extraDeffenceUp(2), EventKind.Event_Armor, newpart);
        newpart.addReaction(reaction);
        magicpartAsset.AllMagicParts.Add(newpart);

        newpart = new MagicPart("焰火师", a, 0);
        reaction = new Reaction_Create("火花1",new EffectEvent(new Burn(1),null), EventKind.Event_PlayCard,newpart);
        newpart.addReaction(reaction);
        magicpartAsset.AllMagicParts.Add(newpart);

        newpart = new MagicPart("弹幕齐射", a, 0);
        reaction = new Reaction_Create("伤害2", new EffectEvent(new Damage(2), null), EventKind.Event_PlayCard, newpart);
        newpart.addReaction(reaction);
        magicpartAsset.AllMagicParts.Add(newpart);

        a[2] = 1;
        newpart = new MagicPart("力场", a, 1);
        reaction = new Reaction_Create("能量护甲2", new EffectEvent(new PartEffect_Armor(2,newpart), null), EventKind.Event_PlayCard, newpart);
        newpart.addReaction(reaction);
        magicpartAsset.AllMagicParts.Add(newpart);

        newpart = new MagicPart("能量爆弹", a, 2);
        newpart.completeEvents.Add(new EffectEvent(new Damage(5), null));
        magicpartAsset.AllMagicParts.Add(newpart);

        newpart = new MagicPart("能量立场", a, 2);
        newpart.completeEvents.Add(new EffectEvent(new Armor(5), null));
        magicpartAsset.AllMagicParts.Add(newpart);

        a[2] = 0;
        a[1] = 0;
        newpart = new MagicPart("零号法术", a, 2);
        reaction = new Reaction_Create("额外的零", new EffectEvent(new CardEffect_Whether(new Judge_IsZeroCostCard(), new PartEffect_CopeLastCardEvent(1)), null), EventKind.Event_PlayCard, newpart);
        newpart.addReaction(reaction);
        magicpartAsset.AllMagicParts.Add(newpart);

        newpart = new MagicPart("魔力绽放", a, 0);
        reaction = new Reaction_Affect("法强5", new extraAttackUp(5), EventKind.Event_Damage, newpart);
        newpart.addReaction(reaction);
        magicpartAsset.AllMagicParts.Add(newpart);


    }
    //加载遭遇事件
    void BefallInit()
    {
        playerInfo player = gameManager.Instance.playerinfo;
        befallinfo befallinfo;
        //宝箱事件
        befallinfo = new befallinfo("宝箱", 1, "宝箱，不会有宝箱怪的",
            new Button_Exit("直接离开", () =>
             {

             }),
            new Button_Info("打开",()=> {              
                gameManager.Instance.uimanager.uiBefallBoard.SetActive(false);
                //随机选出可选部件
                List<MagicPart> selectparts = ListOperation.RandomValueList<MagicPart>(AllAsset.magicpartAsset.AllMagicParts, player.treasureToSelectNum);
                //打开选择部件面板
                secondBoardInfo secondBoard = new secondBoardInfo(2);
                secondBoard.onExit += () =>
                {

                };
                if (gameManager.Instance.uimanager.uiTreasurePartBoard)
                {
                    gameManager.Instance.uimanager.uiTreasurePartBoard.gameObject.SetActive(true);
                }
                else
                {
                    GameObject selectpart = instantiateManager.instance.instanSecondBoard(secondBoard);
                    UisecondBoard_SelectPart uiselectboard = selectpart.GetComponent<UisecondBoard_SelectPart>();
                    gameManager.Instance.uimanager.uiTreasurePartBoard = uiselectboard;
                    uiselectboard.EnterInit(secondBoard);
                    uiselectboard.Init(selectparts, 1);
                    uiselectboard.describeText.text = "选择1个部件";
                    uiselectboard.CancelButton.AddListener(() =>
                    {
                        selectpart.SetActive(false);
                        gameManager.Instance.uimanager.uiBefallBoard.SetActive(true);
                    });
                    uiselectboard.onSelectParts = (partlist) =>
                    {
                        foreach(MagicPart part in partlist)
                        {
                            player.AddMagicPart(part);
                        }
                        GameObject.Destroy(selectpart);
                        gameManager.Instance.mapmanager.EventWindow(false); //mapState = MapState.MainMap;
                    };
                }
            })
             );
        MapAsset.mapSystemBefall.Add(befallinfo);
        MapAsset.AllBefallInfos.Add(befallinfo);
        //营地事件
        befallinfo = new befallinfo("营火", 3, "修养生息或者稳固实力",
            new Button_Exit("直接离开",()=> { }),
            new Button_Exit("恢复血量",()=> {
                float h = player.playerHealthMax * 0.3f;
                player.RecoveryHealth((int)h);
            }),
            new Button_Info("升级卡牌",()=> {
                gameManager.Instance.uimanager.uiBefallBoard.SetActive(false);
                secondBoardInfo secondBoard = new secondBoardInfo(1);
                GameObject selectcard = instantiateManager.instance.instanSecondBoard(secondBoard);
                UisecondBoard_SelectCard uiselectBoard = selectcard.GetComponent<UisecondBoard_SelectCard>();
                uiselectBoard.EnterInit(secondBoard);
                List<playerCard> cangradeCardList = new List<playerCard>();
                foreach(playerCard card in player.playerDeck)
                {
                    if (!card.IsGrade)
                    {
                        cangradeCardList.Add(card);
                    }
                }
                uiselectBoard.Init(cangradeCardList,1);
                uiselectBoard.describeText.text = "选择1张卡升级";
                uiselectBoard.CancelButton.AddListener(() =>
                {
                    GameObject.Destroy(uiselectBoard.gameObject);
                    gameManager.Instance.uimanager.uiBefallBoard.SetActive(true);
                });
                uiselectBoard.onSelectCards = (cards) =>
                {
                    foreach(playerCard card in cards)
                    {
                        player.UpgradeCard(card);
                    }
                    GameObject.Destroy(uiselectBoard.gameObject);
                    gameManager.Instance.mapmanager.EventWindow(false); //mapState = MapState.MainMap;
                };
            })
            );
        MapAsset.mapSystemBefall.Add(befallinfo);
        MapAsset.AllBefallInfos.Add(befallinfo);

        //随机事件
        //不知名石像
        string name="";
        befallinfo = new befallinfo("不知名的石像", 1, "看起来像是某种祭祀仪式的场所，正中间的石像在月光下显得格外阴森，石像周围有很多祭品",
            new Button_Exit("离开", () => {

            }),
            new Button_Exit("拿走全部祭品", () => {
                gameManager.Instance.playerinfo.GetMoney(150);
                gameManager.Instance.playerinfo.AddCurseCard();//
            }),
            new Button_Exit("拿走一半祭品", () => {
                gameManager.Instance.playerinfo.GetMoney(75);
                gameManager.Instance.playerinfo.AddBattleBuff(new BattleBuff("疯狂", 1));//
            }),
            new Button_Exit("献上祭品("+ name + ")", () => {
                playerCard playerCard = ListOperation.RandomValue<playerCard>(player.playerDeck);
                name = playerCard.Name;
                player.RemoveCard(playerCard);
                //player.AddMagicPart();//
            })
            );
        MapAsset.AllBefallInfos.Add(befallinfo);

        //神秘法阵
        befallinfo = new befallinfo("神秘法阵", 2, "你走进一个山洞之中，石壁上好像画着什么东西，当你触碰到石壁的瞬间，一个法阵出现在你面前。你认出这是传送法阵，它可以送你去任何地方",
            new Button_Exit("进入", () =>
            {
                gameManager.Instance.mapmanager.Shenmifazhen();
            })
            );
        MapAsset.AllBefallInfos.Add(befallinfo);

        //魔法食客
        befallinfo = new befallinfo("魔法食客", 5, "一个圆滚滚的生物出现在你面前  “你好像有很多好吃的，可以分我点儿么？”",
            new Button_Exit("拒绝", () =>
            {

            }),
            new Button_Info("进行投喂", () =>
            {
                player.RecoveryHealth(-15);
                int a = Random.Range(1, 3);
                if (a == 1)
                    gameManager.Instance.playerinfo.GetMoney(100);
                if (a == 2)
                {
                    gameManager.Instance.uimanager.uiBefallBoard.SetActive(false);
                    secondBoardInfo secondBoard = new secondBoardInfo(1);
                    GameObject selectcard = instantiateManager.instance.instanSecondBoard(secondBoard);
                    UisecondBoard_SelectCard uiselectBoard = selectcard.GetComponent<UisecondBoard_SelectCard>();
                    uiselectBoard.EnterInit(secondBoard);
                    List<playerCard> cangradeCardList = new List<playerCard>();
                    foreach (playerCard card in player.playerDeck)
                    {
                        if (!card.IsGrade)
                        {
                            cangradeCardList.Add(card);
                        }
                    }
                    uiselectBoard.Init(cangradeCardList, 1);
                    uiselectBoard.describeText.text = "选择1张卡升级";
                    uiselectBoard.CancelButton.AddListener(() =>
                    {
                        GameObject.Destroy(uiselectBoard.gameObject);
                        gameManager.Instance.uimanager.uiBefallBoard.SetActive(true);
                    });
                    uiselectBoard.onSelectCards = (cards) =>
                    {
                        foreach (playerCard card in cards)
                        {
                            player.UpgradeCard(card);
                        }
                        GameObject.Destroy(uiselectBoard.gameObject);
                        gameManager.Instance.mapmanager.EventWindow(false); //mapState = MapState.MainMap;
                    };
                }
                if (a == 3)
                {
                    secondBoardInfo selectBoardInfo = new secondBoardInfo(1);
                    GameObject selectcard = instantiateManager.instance.instanSecondBoard(selectBoardInfo);
                    UisecondBoard_SelectCard uiselectboard = selectcard.GetComponent<UisecondBoard_SelectCard>();
                    uiselectboard.EnterInit(selectBoardInfo);
                    uiselectboard.Init(gameManager.Instance.playerinfo.playerDeck, 1);
                    uiselectboard.describeText.text = "删除1张卡";
                    uiselectboard.CancelButton.AddListener(() =>
                    {
                        GameObject.Destroy(uiselectboard.gameObject);
                    });
                    uiselectboard.onSelectCards = (cardlist) =>
                    {
                        foreach (playerCard card in cardlist)
                        {
                            gameManager.Instance.playerinfo.RemoveCard(card);
                        }

                        GameObject.Destroy(uiselectboard.gameObject);
                    };
                }
            })
            );
        MapAsset.AllBefallInfos.Add(befallinfo);

        //路遇不平
        befallinfo = new befallinfo("路遇不平", 6, "你看到一个邪教徒正洗劫着一家农户，他把头转向了你这边，显然，他已经注意到了你的存在“告诉你，别多管闲事啊！待会儿会留一份给你的。”",
            new Button_Exit("欣然接受", () =>
            {
                gameManager.Instance.playerinfo.GetMoney(50);
            }),
            new Button_Exit("出手相助", () =>
            {
                //gameManager.Instance.playerinfo.GetMoney(50);
                gameManager.Instance.mapmanager.EnterBattle(new battlePlace(2,1,2));
             })
            );
        MapAsset.AllBefallInfos.Add(befallinfo);

        //鲜血交易
        befallinfo = new befallinfo("鲜血交易", 0, "一个手持小刀面带微笑的男子朝你走来“我这里可是有很多好东西哦，你想看看么？只需要一点点鲜血。”",
            new Button_Exit("拒绝", () =>
            {

            }),
            new Button_Exit("接受", () =>
            {

            })
            );
        MapAsset.AllBefallInfos.Add(befallinfo);

        //静谧湖畔
        befallinfo = new befallinfo("静谧湖畔", 3, "你找到了一座湖，静谧的湖面散发着魔法的微光",
            new Button_Exit("饮下", () =>
            {
                player.RecoveryHealth(15);
            }),
            new Button_Exit("洗涤身体", () =>
            {
                secondBoardInfo selectBoardInfo = new secondBoardInfo(1);
                GameObject selectcard = instantiateManager.instance.instanSecondBoard(selectBoardInfo);
                UisecondBoard_SelectCard uiselectboard = selectcard.GetComponent<UisecondBoard_SelectCard>();
                uiselectboard.EnterInit(selectBoardInfo);
                uiselectboard.Init(gameManager.Instance.playerinfo.playerDeck, 1);
                uiselectboard.describeText.text = "删除1张卡";
                uiselectboard.CancelButton.AddListener(() =>
                {
                    GameObject.Destroy(uiselectboard.gameObject);
                });
                uiselectboard.onSelectCards = (cardlist) =>
                {
                    foreach (playerCard card in cardlist)
                    {
                        gameManager.Instance.playerinfo.RemoveCard(card);
                    }

                    GameObject.Destroy(uiselectboard.gameObject);
                };
            })
            );
        MapAsset.AllBefallInfos.Add(befallinfo);

        //微笑果农
        befallinfo = new befallinfo("微笑果农", 4, "你误入一片果林，就在这时，一个声音响起：“哎呀呀年轻人，你想尝尝哪种水果呢？”",
            new Button_Exit("苹果", () =>
            {
                player.RecoveryHealth(15);
            }),
            new Button_Exit("梨子", () =>
            {
                player.MaxHP(5);
            })
            );
        MapAsset.AllBefallInfos.Add(befallinfo);
    }
    void MonsterInit()
    {
        //普通怪物
        //1~3层(最少要有三只
        monsterInfo monster;
        monster = new monInfo_Sample(4);
        monster.Id = 0;
        monster.name = "普通怪物_1_a";
        MapAsset.nMonster1s.Add(monster);
        MapAsset.AllMonsters.Add(monster);

        monster = new monInfo_Sample(4);
        monster.Id = 0;
        monster.name = "普通怪物_1_b";
        MapAsset.nMonster1s.Add(monster);
        MapAsset.AllMonsters.Add(monster);

        monster = new monInfo_Sample(4);
        monster.Id = 0;
        monster.name = "普通怪物_1_c";
        MapAsset.nMonster1s.Add(monster);
        MapAsset.AllMonsters.Add(monster);
        //4~6层(三只
        monster = new monInfo_Sample(6);
        monster.Id = 0;
        monster.name = "普通怪物_2_a";
        MapAsset.nMonster2s.Add(monster);
        MapAsset.AllMonsters.Add(monster);

        monster = new monInfo_Sample(6);
        monster.Id = 0;
        monster.name = "普通怪物_2_b";
        MapAsset.nMonster2s.Add(monster);
        MapAsset.AllMonsters.Add(monster);

        monster = new monInfo_Sample(6);
        monster.Id = 0;
        monster.name = "普通怪物_2_c";
        MapAsset.nMonster2s.Add(monster);
        MapAsset.AllMonsters.Add(monster);
        //8~10层
        monster = new monInfo_Sample(8);
        monster.Id = 0;
        monster.name = "普通怪物_3_a";
        MapAsset.nMonster3s.Add(monster);
        MapAsset.AllMonsters.Add(monster);

        monster = new monInfo_Sample(8);
        monster.Id = 0;
        monster.name = "普通怪物_3_b";
        MapAsset.nMonster3s.Add(monster);
        MapAsset.AllMonsters.Add(monster);

        monster = new monInfo_Sample(8);
        monster.Id = 0;
        monster.name = "普通怪物_3_c";
        MapAsset.nMonster3s.Add(monster);
        MapAsset.AllMonsters.Add(monster);
        //精英怪物
        //3~6层
        monster = new monInfo_Sample(8);
        monster.Id = 0;
        monster.name = "精英怪物_1_a";
        MapAsset.hMonster1s.Add(monster);
        MapAsset.AllMonsters.Add(monster);

        monster = new monInfo_Sample(8);
        monster.Id = 0;
        monster.name = "精英怪物_1_b";
        MapAsset.hMonster1s.Add(monster);
        MapAsset.AllMonsters.Add(monster);

        //8~10层
        monster = new monInfo_Sample(8);
        monster.Id = 0;
        monster.name = "精英怪物_2_a";
        MapAsset.hMonster2s.Add(monster);
        MapAsset.AllMonsters.Add(monster);

        monster = new monInfo_Sample(8);
        monster.Id = 0;
        monster.name = "精英怪物_2_b";
        MapAsset.hMonster2s.Add(monster);
        MapAsset.AllMonsters.Add(monster);
        //boss
        monster = new monInfo_Sample(12);
        monster.Id = 0;
        monster.name = "boss";
        MapAsset.bossLists.Add(monster);
        MapAsset.AllMonsters.Add(monster);

        //normal
        MapAsset.NormalMonsterList.Add(new monInfo_Cat());
        MapAsset.NormalMonsterList.Add(new monInfo_Bunny());
        MapAsset.NormalMonsterList.Add(new monInfo_SnowMan());
        //hard
        MapAsset.HardMonsterList.Add(new monInfo_Bear());
        //boss
        MapAsset.BossMonsterList.Add(new monInfo_MoNv());
    }


    public void EditorCardInit(CardEditorBoard cardboard)
    {
        foreach(editorCardCollect ecard in cardboard.allCards)
        {
            editorCard card = ecard.Card;
            playerCard newcard = new playerCard(card.id, card.name, card.Kind, card.cost, (int)card.Rank,false);
            newcard.TextureId = card.id;
            cardEffectBase lasteffect = new Damage(0);
            //Debug.Log(card.name);
            //Debug.Log(card.playEffects.Count);
            foreach(editorEffect eE in card.playEffects)
            {
                cardEffectBase neweffect = EffectFromInit(eE);
                newcard.AddEffect(neweffect);
                if (eE.effectKind == EnumEffect.LinkRandom| eE.effectKind == EnumEffect.PartLinkRandom)
                {
                    newcard.AddEffect(new CardEffect_ToExitLink());
                }
                if (eE.effectKind == EnumEffect.RepeatByEffect)
                {
                    (neweffect as CardEffect_RepeatByEffect).numeffect = lasteffect;
                }

                lasteffect = neweffect;
            }
            newcard.CardDescribe();
            cardAsset.AllIdCards.Add(newcard);

            card = ecard.gradeCard;
            lasteffect = new Damage(0);
            //Debug.Log(card.name);
            //Debug.Log(card.playEffects.Count);
            newcard = new playerCard(card.id, card.name, card.Kind, card.cost, (int)card.Rank,true);
            newcard.TextureId = card.id;
            foreach (editorEffect eE in card.playEffects)
            {
                cardEffectBase neweffect = EffectFromInit(eE);
                newcard.AddEffect(neweffect);
                if (eE.effectKind == EnumEffect.LinkRandom)
                {
                    newcard.AddEffect(new CardEffect_ToExitLink());
                }
                if (eE.effectKind == EnumEffect.RepeatByEffect)
                {
                    (neweffect as CardEffect_RepeatByEffect).numeffect = lasteffect;
                }

                lasteffect = neweffect;
            }
            newcard.CardDescribe();
            cardAsset.AllGradeCards.Add(newcard);
        }
        foreach(playerCard playerCard in cardAsset.AllIdCards)
        {
            if (playerCard.Rank == 0 | playerCard.Rank == 4)
            {
                cardAsset.deriveCards.Add(playerCard);
            }
            else
            {
                cardAsset.canGetCards.Add(playerCard);
                if (playerCard.Rank == 1)
                {
                    cardAsset.noramlCards.Add(playerCard);
                }
                else if (playerCard.Rank == 2)
                {
                    cardAsset.rareCards.Add(playerCard);
                }
                else if (playerCard.Rank == 3)
                {
                    cardAsset.superCards.Add(playerCard);
                }
            }
            //类型
            if (playerCard.Kind == CardKind.AttackCard)
            {
                cardAsset.attactCards.Add(playerCard);
            }
            else if (playerCard.Kind == CardKind.SkillCard)
            {
                cardAsset.skillCards.Add(playerCard);
            }
        }
    }
    cardEffectBase EffectFromInit(editorEffect editorEffect,params judgeCondition[] judges)
    {
        //Debug.Log(editorEffect.name);
        cardEffectBase Effect = new emplyPlayCard();
        switch (editorEffect.effectKind)
        {
            case EnumEffect.Damage:
                Effect = new Damage(editorEffect.num);
                break;
            case EnumEffect.DamageByJudge:
                Effect = new CardEffect_DamageByJudge(editorEffect.num);
                Debug.Log(editorEffect.judges.Count);
                foreach (editorJudge ej in editorEffect.judges)
                {
                    Effect.judgeConditions.Add(JudgeFromInit(ej));
                }
                (Effect as CardEffect_DamageByJudge).numjudge = Effect.judgeConditions[0];
                break;
            case EnumEffect.Armor:
                Effect = new Armor(editorEffect.num);
                break;
            case EnumEffect.DrawCard:
                Effect = new DrawCard(editorEffect.num);
                break;
            case EnumEffect.Repeat:
                Effect = new Repeat(editorEffect.num);
                foreach(editorEffect eE in editorEffect.childeffects)
                {
                    Effect.childeffects.Add(EffectFromInit(eE));
                }
                break;
            case EnumEffect.Burn:
                Effect = new Burn(editorEffect.num);
                break;
            case EnumEffect.LinkRandom:
                Effect = new LinkRandom(editorEffect.num);
                break;
            case EnumEffect.DoubleBurn:
                Effect = new DoubleBurn(editorEffect.num);
                break;
            case EnumEffect.Whether:
                Effect = new CardEffect_Whether();
                foreach(editorJudge ej in editorEffect.judges)
                {
                    Effect.judgeConditions.Add(JudgeFromInit(ej));
                }
                foreach (editorEffect eE in editorEffect.childeffects)
                {
                    Effect.childeffects.Add(EffectFromInit(eE,Effect.judgeConditions[0]));
                }
                break;
            case EnumEffect.DisCard:
                break;
            case EnumEffect.RepeatByFill:
                Effect = new CardEffect_RepeatByFill(judges[0]);
                foreach (editorEffect eE in editorEffect.childeffects)
                {
                    Effect.childeffects.Add(EffectFromInit(eE));
                }
                break;
            case EnumEffect.DisAllCard:
                Effect = new CardEffect_DisAllCard();
                break;
            case EnumEffect.RepeatByEffect:
                Effect = new CardEffect_RepeatByEffect();
                foreach (editorEffect eE in editorEffect.childeffects)
                {
                    Effect.childeffects.Add(EffectFromInit(eE));
                }
                break;
            case EnumEffect.Exhaust:
                Effect = new CardEffect_Exhaust();
                break;
            case EnumEffect.DamageByPartPower:
                Effect = new CardEffect_DamageByPartPower(editorEffect.num);
                break;
            case EnumEffect.ArmorByPartPower:
                Effect = new CardEffect_ArmorByPartPower(editorEffect.num);
                break;
            case EnumEffect.PartLinkRandom:
                Effect = new LinkThisWithRandom(editorEffect.num);
                break;
            case EnumEffect.RandomCardFromDiscard:
                Effect = new CardEffect_GetCardFormDiscard(editorEffect.num);
                break;
            default:
                Debug.Log("没有该EditorEffect对应的Effect转换");
                break;
        }
        return Effect;
    }
    judgeCondition JudgeFromInit(editorJudge ejudge)
    {
        judgeCondition Judge = new Judge_NullTrue();
        switch (ejudge.judgeKind)
        {
            case EnumJudge.敌人意图攻击:
                Judge = new Judge_EnemyWillAttack();
                break;
            case EnumJudge.每补齐一横行:
                Judge = new Judge_buqiheng(0);
                break;
            case EnumJudge.每补齐一纵行:
                Judge = new Judge_buqishu(0);
                break;
            case EnumJudge.每补齐一横行或纵行:
                Judge = new Judge_BuQi();
                break;
        }
        return Judge;
    }
    public static List<perform> PerformListFromInit(editorCardCollect card)
    {
        List<perform> performs = new List<perform>();
        foreach(editorPerform eP in card.performlist)
        {
            perform newperform;
            if (eP.performkind == PerformKind.动画)
            {
                newperform = new PerformAnima((int)eP.charactor,eP.animation,eP.animationSpeed);
                newperform.timeTurn = eP.time;
                performs.Add(newperform);
            }
            else if (eP.performkind == PerformKind.特效)
            {
                newperform = new PerformEffect((int)eP.effectWay, eP.effectGO, eP.movespeed,eP.effecttime);
                newperform.timeTurn = eP.time;
                performs.Add(newperform);
            }
        }
        return performs;
    }
}
