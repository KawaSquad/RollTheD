using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KawaSquad.Network;

public class AdventureManager : MonoBehaviour
{
    public MapLoader mapLoader;
    public Camera mainCamera;

    TokenPawn tokenSelected = null;

    [Header("GameMaster")]
    public SObject_Player sessionData;
    public Transform listCharacters;

    public CanvasGroup characterList;
    public CharacterSelector characterSample;
    public EnemySelector enemySample;
    public ItemSelector itemSample;

    [System.Serializable]
    public class ColorSelection
    {
        public Color unselected = Color.clear;
        public Color highlightedLocal = Color.blue;//Mine
        public Color highlightedServer = Color.red;//Them
        public Color active = Color.green;
    }
    public ColorSelection colorSelections;
    public static AdventureManager Instance;

    public enum MouseStep
    {
        NONE,
        RELEASED,
        JUST_PRESSED,
        PRESSED,
    }

    private void Awake()
    {
        if (Instance != null)
            Debug.Log("Instance already assigned");
        Instance = this;

        HideCharacterList();
    }

    private void Update()
    {
        //Ping debug
        if (Input.GetKeyDown(KeyCode.P))
        {
            NetworkManager.PingServer();
        }

        MouseStep mouseLeftStep = MouseStep.NONE;
        if (Input.GetMouseButtonUp(0))
            mouseLeftStep = MouseStep.RELEASED;
        else if (Input.GetMouseButtonDown(0))
            mouseLeftStep = MouseStep.JUST_PRESSED;
        else if (Input.GetMouseButton(0))
            mouseLeftStep = MouseStep.PRESSED;

        MouseStep mouseRightStep = MouseStep.NONE;
        if (Input.GetMouseButtonUp(1))
            mouseRightStep = MouseStep.RELEASED;
        else if (Input.GetMouseButtonDown(1))
            mouseRightStep = MouseStep.JUST_PRESSED;
        else if (Input.GetMouseButton(1))
            mouseRightStep = MouseStep.PRESSED;


        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo))
        {
            if (TokenPawn.activeToken != null)
            {
                switch (mouseLeftStep)
                {
                    case MouseStep.RELEASED:
                       TokenPawn.activeToken.SetActiveToken(false);
                        break;
                    case MouseStep.PRESSED:
                        TokenPawn.activeToken.MovePawn(hitInfo.point, Vector3.zero, Vector3.one);
                        break;
                    default:
                        break;
                }
            }
            else
            {
                if (hitInfo.collider.tag == "Pawn")
                {
                    TokenPawn pawn = hitInfo.collider.GetComponent<TokenPawn>();
                    if (pawn != null)
                    {
                        //if a token is selected and is not the token
                        if (tokenSelected != null && tokenSelected != pawn)
                        {
                            tokenSelected.UnselectToken();
                            tokenSelected = null;
                        }

                        switch (mouseLeftStep)
                        {
                            case MouseStep.NONE:
                                tokenSelected = pawn;
                                tokenSelected.SelectToken();
                                break;
                            case MouseStep.RELEASED:
                                pawn.SetActiveToken(false);
                                break;
                            case MouseStep.JUST_PRESSED:
                                pawn.SetActiveToken(true);
                                break;
                            case MouseStep.PRESSED:
                                if (TokenPawn.activeToken != null)
                                    TokenPawn.activeToken.MovePawn(hitInfo.point, Vector3.zero, Vector3.one);
                                break;
                            default:
                                break;
                        }

                        if (mouseRightStep == MouseStep.JUST_PRESSED)
                            pawn.DestroyPawn();

                    }
                }
                else
                {
                    if (tokenSelected != null)
                    {
                        tokenSelected.UnselectToken();
                        tokenSelected = null;
                    }
                }
            }
        }
    }


    public void CreateCharacter(int connectionID, int id_Character, int id_Token)
    {
        Content_Lobby character = null;
        for (int i = 0; i < sessionData.Content_Lobby.characters.Count; i++)
        {
            if (sessionData.Content_Lobby.characters[i].ID_Character == id_Character)
                character = sessionData.Content_Lobby.characters[i];
        }
        if (character != null)
        {
            Pawn.Server_PawnData data = new Pawn.Server_PawnData();
            data.ID_Handler = connectionID;

            data.position = CastCenterCamera();
            data.rotation = Vector3.zero;
            data.scale = Vector3.one;

            data.pawnType = Pawn.PawnPackets.P_Player;

            PlayerController.PlayerController_Data dataToParse = new PlayerController.PlayerController_Data();
            dataToParse.id_Character = character.ID_Character;
            dataToParse.id_Token = character.ID_Token;
            dataToParse.className = character.Class_Character;

            data.classParsed = JsonUtility.ToJson(dataToParse);

            DataSender.SendNewPawn(data);
        }

    }

    public void CreateEnemy(int connectionID, int id_Token)
    {
        Pawn.Server_PawnData data = new Pawn.Server_PawnData();
        data.ID_Handler = connectionID;

        data.position = CastCenterCamera();
        data.rotation = Vector3.zero;
        data.scale = Vector3.one;

        data.pawnType = Pawn.PawnPackets.P_Ennemy;

        EnemyController.EnemyController_Data dataToParse = new EnemyController.EnemyController_Data();
        dataToParse.id_Token = id_Token;

        data.classParsed = JsonUtility.ToJson(dataToParse);

        DataSender.SendNewPawn(data);
    }

    public void CreateItem(int connectionID, int id_Token)
    {
        Pawn.Server_PawnData data = new Pawn.Server_PawnData();
        data.ID_Handler = connectionID;

        data.position = CastCenterCamera();
        data.rotation = Vector3.zero;
        data.scale = Vector3.one;

        data.pawnType = Pawn.PawnPackets.P_Items;

        ItemPawn.ItemPawn_Data dataToParse = new ItemPawn.ItemPawn_Data();
        dataToParse.id_Token = id_Token;

        data.classParsed = JsonUtility.ToJson(dataToParse);

        DataSender.SendNewPawn(data);
    }


    public void SessionCharacterList()
    {
        if (characterList.alpha == 1f)
        {
            HideCharacterList();
            return;
        }

        CharacterSelector[] characters = listCharacters.GetComponentsInChildren<CharacterSelector>();
        for (int i = 0; i < characters.Length; i++)
        {
            Destroy(characters[i].gameObject);
        }

        characterList.alpha = 1f;
        characterList.blocksRaycasts = true;
        characterList.interactable = true;

        for (int i = 0; i < sessionData.Content_Lobby.characters.Count; i++)
        {
            Content_Lobby character = sessionData.Content_Lobby.characters[i];
            CharacterSelector characterSelect = Instantiate(characterSample, listCharacters);
            characterSelect.name = "Character_" + character.ID_Character;
            characterSelect.id_Character = character.ID_Character;
            characterSelect.SetVisual(character.ID_Token);
        }

        //SAMPLES
        for (int i = 0; i < 4; i++)// 4 samples enemy
        {
            EnemySelector enemySelect = Instantiate(enemySample, listCharacters);
            enemySelect.name = "Ennemy_" + i;
            enemySelect.SetVisual(i);
        }
        ItemSelector chestSelect = Instantiate(itemSample, listCharacters);
        chestSelect.name = "Chest";
        chestSelect.SetVisual(0);
    }
    public void HideCharacterList()
    {
        characterList.alpha = 0f;
        characterList.blocksRaycasts = false;
        characterList.interactable = false;
    }

    Vector3 CastCenterCamera()
    {
        Ray ray = mainCamera.ScreenPointToRay(new Vector2(Screen.width, Screen.height) / 2f);
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo))
        {
            return hitInfo.point;
        }
        return Vector3.zero;
    }
}