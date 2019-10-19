using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KawaSquad.Network;

public class AdventureManager : MonoBehaviour
{
    public MapLoader mapLoader;
    public Camera mainCamera;

    PlayerController playerSelected = null;

    [Header("GameMaster")]
    public SObject_Player sessionData;
    public Transform listCharacters;

    public CanvasGroup characterList;
    public CharacterSelector characterSample;

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

    private void Awake()
    {
        if (Instance != null)
            Debug.Log("Instance already assigned");
        Instance = this;

        HideCharacterList();
    }

    /*
    void Start()
    {
        mapLoader.LoadMap(sessionData.GM_Url, false);
    }
     */

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            NetworkManager.PingServer();
        }

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo))
        {
            if (PlayerController.activeController != null)
            {
                if (Input.GetMouseButton(0))
                {
                    if (PlayerController.activeController != null)
                    {
                        PlayerController.activeController.MovePawn(hitInfo.point, Vector3.zero, Vector3.one);
                    }
                }
                else if (Input.GetMouseButtonUp(0))
                {
                    PlayerController.activeController.SetActiveCharacter(false);
                }
            }
            else
            {
                if (hitInfo.collider.tag == "Player")
                {
                    PlayerController player = hitInfo.collider.GetComponent<PlayerController>();
                    if (player != null)
                    {
                        if (playerSelected != null && playerSelected != player)
                        {
                            playerSelected.UnselectCharacter();
                            playerSelected = null;
                        }

                        player.SelectCharacter();
                        playerSelected = player;

                        if (Input.GetMouseButtonDown(0))
                        {
                            player.SetActiveCharacter(true);
                        }
                        else if (Input.GetMouseButtonDown(1))
                        {
                            player.DestroyPawn();
                        }
                        else if (Input.GetMouseButton(0))
                        {
                            if (PlayerController.activeController != null)
                            {
                                PlayerController.activeController.MovePawn(hitInfo.point, Vector3.zero, Vector3.one);
                            }
                        }
                        else if (Input.GetMouseButtonUp(0))
                        {
                            player.SetActiveCharacter(false);
                        }
                    }
                }
                else
                {
                    if (playerSelected != null)
                    {
                        playerSelected.UnselectCharacter();
                        playerSelected = null;
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
            //data.ID_Character = id_Character;
            data.ID_Handler = connectionID;


            data.position = Vector3.zero;
            data.rotation = Vector3.zero;
            data.scale = Vector3.one;

            data.pawnType = Pawn.Server_PawnData.PawnPackets.P_Player;

            PlayerController.PlayerController_Data dataToParse = new PlayerController.PlayerController_Data();
            dataToParse.id_Character= character.ID_Character;
            dataToParse.id_Token= character.ID_Token;
            dataToParse.className = character.Class_Character;

            data.classParsed = JsonUtility.ToJson(dataToParse);

            DataSender.SendNewPawn(data);

            //if (sendToServer)
        }
    }

    public void SessionCharacterList()
    {
        if (characterList.alpha == 1f)
        {
            HideCharacterList();
            return;
        }

        CharacterSelector[] characters = characterList.GetComponentsInChildren<CharacterSelector>();
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
            CharacterSelector characterSelect = Instantiate(characterSample, characterList.transform);
            characterSelect.name = "Character_" + character.ID_Character;
            characterSelect.id_Character = character.ID_Character;
            characterSelect.SetVisual(character.ID_Token);
        }
    }
    public void HideCharacterList()
    {
        characterList.alpha = 0f;
        characterList.blocksRaycasts = false;
        characterList.interactable = false;
    }
}
