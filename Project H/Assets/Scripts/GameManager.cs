using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    //싱글톤 패턴
    private static GameManager instance = null;

    void Awake()
    {
        if (null == instance)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else Destroy(this.gameObject);
    }

    public static GameManager Instance
    {
        get
        {
            if (null == instance) return null;
            return instance;
        }
    }
    //-----
    public GameObject coinManager;
    Coin coin;
    CoinTextManager CoinTM;
    TalkManager talkManager;
    PlayerController playerController;
    public Camera mainCamera;
    public bool isCamMove;

    //센터화면 주식 분류 ID
    public int nowCoinID = 0;

    //구매 개수
    public int countBuySell;
    public InputField countInput;

    //돈
    public long PlayerNowMoney;
    public long PlayerAllMoney;
    long lastMoney;

    //시간 날짜 현재장소
    public enum TimeType { morning, day, noon, evening, night }
    public TimeType timeType = TimeType.day;
    public enum PlaceType { house, street, backStreet, office }
    public PlaceType placeType = PlaceType.house;
    public float nowTime;
    public int nowDay = 0;
    public bool isTimeGo;

    //시간UI
    public Slider timeSlider;
    public Text timeText;
    public Text dayText;

    //텍스트박스 UI
    public Text talkText;
    public GameObject talkPanel;
    public GameObject selectPanel;

    public GameObject scanObject;
    public bool isAction;
    public int talkIndex;

    //페이드 이미지
    public Image FadeImg;
    public Text[] dayEndText;

    //컴퓨터 UI
    public GameObject playerComputer;
    public bool isCommputerON;

    //카메라
    public Transform target;

    public float smoothSpeed = 3;
    public Vector2 center;
    public Vector2 size;
    float heightCam;
    float widthCam;


    delegate IEnumerator BtnSelectYes();
    BtnSelectYes btnSelectYes;
    delegate IEnumerator BtnSelectNo();
    BtnSelectNo btnSelectNo;

    // Start is called before the first frame update
    void Start()
    {
        CoinTM = coinManager.GetComponent<CoinTextManager>();
        coin = coinManager.GetComponent<Coin>();
        talkManager = GetComponent<TalkManager>();
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        heightCam = Camera.main.orthographicSize;
        widthCam = heightCam * Screen.width / Screen.height;

    }

    // Update is called once per frame
    void Update()
    {
        //돈 계산 새로고침
        PlayerMoneySelect();

        //컴퓨터 화면 새로고침
        CoinTM.CenterCoinUpdate(nowCoinID);

        //시간
        TimeGoing();

        //카메라 제어
        MoveCamera();

        //가 - 속
        if (Input.GetKey("tab"))
        {
            Time.timeScale = 20;
        }
        else
        {
            Time.timeScale = 1;
        }


    }

    //플레이어 자산 계산
    public void PlayerMoneySelect()
    {
        PlayerAllMoney = PlayerNowMoney;
        
        for (int i = 0; i < coin.coinDatas.Length; i++)
        {
            coin.coinDatas[i].totalCost = coin.coinDatas[i].bringCount * coin.coinDatas[i].nowCost;
            PlayerAllMoney += coin.coinDatas[i].totalCost;
        }
    }

    //구매, 판매 개수 설정
    public void ExchangeCount()
    {
        countBuySell = int.Parse(countInput.text);
    }

    //주식 구매
    public void BuyCoin()
    {
        if(PlayerNowMoney - (coin.coinDatas[nowCoinID].nowCost * countBuySell) >= 0)
        {
            //돈 차감
            PlayerNowMoney -= (coin.coinDatas[nowCoinID].nowCost * countBuySell);
            
            //보유주식 변경
            coin.coinDatas[nowCoinID].bringCount += countBuySell;
            
            //구입액 변경
            coin.coinDatas[nowCoinID].useMoney += (coin.coinDatas[nowCoinID].nowCost * countBuySell);

            countInput.text = "";

            CoinTM.CoinTextUpdate();
        }
    }

    //주식 판매
    public void SellCoin()
    {
        if( coin.coinDatas[nowCoinID].bringCount > 0)
        {
            //돈 증가
            PlayerNowMoney += (coin.coinDatas[nowCoinID].nowCost * coin.coinDatas[nowCoinID].bringCount);

            //보유주식 변경
            coin.coinDatas[nowCoinID].bringCount = 0;

            //구입액 변경
            coin.coinDatas[nowCoinID].useMoney = 0;

            CoinTM.CoinTextUpdate();
        }

    }


    //시간 흐름
    void TimeGoing()
    {
        if (isTimeGo)
        {
            nowTime += Time.deltaTime;
            timeSlider.value = nowTime;
            switch ((int)nowTime)
            {
                case 0:
                    timeType = TimeType.morning;
                    timeText.text = "새벽";
                    lastMoney = PlayerAllMoney;
                    dayText.text = nowDay + "";
                    break;
                case 120:
                    timeType = TimeType.day;
                    timeText.text = "아침";
                    coin.UpdateCoin();
                    break;
                case 240:
                    timeType = TimeType.noon;
                    timeText.text = "점심";
                    coin.UpdateCoin();
                    break;
                case 360:
                    timeType = TimeType.evening;
                    timeText.text = "저녁";
                    coin.UpdateCoin();
                    break;
                case 480:
                    timeType = TimeType.night;
                    timeText.text = "밤";
                    coin.UpdateCoin();
                    break;
                case 600:
                    isTimeGo = false;
                    coin.UpdateCoin();
                    DayEnd();
                    break;
            }

            if (isTimeGo && nowTime >= 600)
            {
                nowTime = 0;
            }
        }
    }

    //텍스트 박스
    public void Action(GameObject scanObj)
    {
        scanObject = scanObj;
        ObjData objData = scanObject.GetComponent<ObjData>();
        Talk(objData.id, objData.isNPC);

        talkPanel.SetActive(isAction);
        if (!isAction) ObjectAction(objData.id);
    }

    void Talk(int id, bool isNPC)
    {
        string talkdata = talkManager.GetTalk(id, talkIndex);

        if (talkdata == null)
        {
            isAction = false;
            talkIndex = 0;

            return;
        }
        
        if (isNPC)
            talkText.text = talkdata;
        else
            talkText.text = talkdata;

        isAction = true;
        talkIndex += 1;
    }

    //오브젝트 기능
    void ObjectAction(int id)
    {
        //deligate 초기화
        btnSelectYes = null;
        btnSelectNo = null;

        switch (id)
        {
            case 200:
                isAction = true;
                talkPanel.SetActive(isAction);
                selectPanel.SetActive(true);
                
                //deligate
                btnSelectYes = HousePlayerOUT;
                
                btnSelectNo = BtnNull; 
                break;
            case 500:
                isCommputerON = !isCommputerON;
                isAction = isCommputerON;
                playerComputer.SetActive(isCommputerON);
                break;
            case 1000:
                isAction = true;
                talkPanel.SetActive(isAction);
                selectPanel.SetActive(true);

                btnSelectYes = HousePlayerIN; 
                btnSelectNo = BtnNull; 
                break;
            default:
                break;
        }
    }

    //집 들어가기
    public IEnumerator HousePlayerOUT()
    {
        StartCoroutine(FadePlaceINOUT());
        yield return new WaitForSeconds(1f);

        playerController.gameObject.transform.position = new Vector3(6f, -20f, 0f);
        isCamMove = true;

        placeType = PlaceType.street;

    }

    //집 나가기
    public IEnumerator HousePlayerIN()
    {
        StartCoroutine(FadePlaceINOUT());
        yield return new WaitForSeconds(1f);

        mainCamera.transform.position = new Vector3(0, 0, -10f);
        playerController.gameObject.transform.position = new Vector3(-3f, -3.5f, 0f);
        isCamMove = false;

        placeType = PlaceType.house;
    }

    public IEnumerator FadePlaceINOUT()
    {
        StartCoroutine(FadeIN());
        yield return new WaitForSeconds(1.2f);
        StartCoroutine(FadeOUT());

    }


    public void SelectYES()
    {
        isAction = false;
        talkPanel.SetActive(false);
        selectPanel.SetActive(false);

        StartCoroutine(btnSelectYes());
    }

    public void SelectNO()
    {
        isAction = false;
        talkPanel.SetActive(false);
        selectPanel.SetActive(false);

        StartCoroutine(btnSelectNo());
    }

    public IEnumerator BtnNull()
    {
        yield return null;
    }

    //시간 모두 지남
    void DayEnd()
    {
        StartCoroutine(DayEndCor());    
    }

    //날짜가 종료되었을 때
    IEnumerator DayEndCor()
    {
        StartCoroutine(FadeIN());

        yield return new WaitForSeconds(1);

        dayEndText[0].text = nowDay + "";
        dayEndText[1].text = lastMoney + "";
        dayEndText[3].text = PlayerAllMoney + "";

        for (int i = 0; i < dayEndText.Length; i++)
        {
            dayEndText[i].gameObject.SetActive(true);
            yield return new WaitForSeconds(0.1f);
        }

        yield return new WaitForSeconds(3f);
        for (int i = 0; i < dayEndText.Length; i++)
        {
            dayEndText[i].gameObject.SetActive(false);
        }

        nowTime = 0;
        nowDay += 1;
        isTimeGo = true;
        StartCoroutine(FadeOUT());
    }

    //페이드인
    IEnumerator FadeIN()
    {
        float fadeCount = 0;
        FadeImg.color = new Color(0.07f, 0.07f, 0.07f, fadeCount);
        while (fadeCount < 1.0f)
        {
            fadeCount += 0.01f;
            yield return new WaitForSeconds(0.01f);
            FadeImg.color = new Color(0.07f, 0.07f, 0.07f, fadeCount);
        }
    }

    //페이드아웃
    IEnumerator FadeOUT()
    {
        float fadeCount = 1;
        FadeImg.color = new Color(0.07f, 0.07f, 0.07f, fadeCount);
        while (fadeCount > 0f)
        {
            fadeCount -= 0.01f;
            yield return new WaitForSeconds(0.01f);
            FadeImg.color = new Color(0.07f, 0.07f, 0.07f, fadeCount);
        }
    }

    //카메라 이동 제어
    void MoveCamera()
    {
        if (placeType == PlaceType.street)
        {
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, target.position, Time.deltaTime * smoothSpeed);
            //mainCamera.transform.position = new Vector3(mainCamera.transform.position.x, mainCamera.transform.position.y, -10f);

            float lx = size.x * 0.5f - widthCam;
            float clampX = Mathf.Clamp(mainCamera.transform.position.x, -lx + center.x, lx + center.x);

            float ly = size.y * 0.5f - heightCam;
            float clampY = Mathf.Clamp(mainCamera.transform.position.y, -ly + center.y, ly + center.y);

            mainCamera.transform.position = new Vector3(clampX, clampY, -10f);

        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(center, size);
    }




}
