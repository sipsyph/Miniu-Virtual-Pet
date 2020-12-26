using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SupervisorAndUI : MonoBehaviour {

    public GameObject frontCamera, backCamera, indoorCamera, menuPanel, gamePanel, profilePanel
    , objPickedUpPanel, inventoryPanel, selectedItemPanel, debugMenuPanel, playObjectsParent
    , paintCanvasCamera, cameraModePanel, mainCamera;

    public Button doorButton, cameraButton, playButton, profileButton, returnFromProfileButton, inventoryButton, portalButton, debugButton, returnFromInventoryButton, returnFromSelectedItemButton,
    itemBtn1, itemBtn2, itemBtn3, itemBtn4, itemBtn5, itemBtn6, itemBtn7, itemBtn8, itemBtn9, useSelectedItemButton, extraDebugBtn, resetAgeBtn, clearInventoryBtn, resetAllBtn,
    returnFromDebugMenuButton, returnFromPaintCanvasViewButton, returnFromCameraModeButton,
    moveLeftCameraBtn, moveRightCameraBtn, resetCameraPosBtn;

    public Button[] itemButtonArr;

    private string[] inventoryItemsArr;

    private Vector3 origCameraPosition;

    public GameObject door;
    public Animator doorAnimator, cameraDoorAnimator, menuTextAnimator, profileTextAnimator;
    public Text titleText, profileText, playText, ageText, selectedItemText;
    public static int supervisorActionRepeatedCounter = 0, doorMovedCounter = 0;

    public static Transform lastTouchedObj, currentTouchedObj, lastTouchedPos, frontPoint, currentHeldObj;
    public Transform playerPoint, throwParent, inventoryParent;
    public static int supervisorActionRepeatedCounterMax;
    public static bool doorInteractionOngoing, doorClosed, playerInteractionOngoing, cameraMode;
    private bool doorIsOpen;
    private float frameCtr, menuIdleCounterMax, menuIdleCounter = 0;
    private string inventoryStringPlaceholder = "", selectedItem;
    Ray ray;
    RaycastHit hit;
    // Use this for initialization
    void Start () {
        SetUpEvents(); //Set up Buttons
        origCameraPosition = mainCamera.transform.position;
        doorInteractionOngoing = false;
        doorIsOpen = false;
        gamePanel.SetActive(false);
        returnFromPaintCanvasViewButton.gameObject.SetActive(false);
        cameraMode = false;
        menuPanel.SetActive(true);
        backCamera.SetActive(false);
        indoorCamera.SetActive(false);
        frontCamera.SetActive(false);
        supervisorActionRepeatedCounterMax = 1000;
        menuIdleCounterMax = 60f;
        cameraDoorAnimator.SetTrigger("cameraDoorIdleTrigger");
        frameCtr = 0.0f;
        inventoryItemsArr = new string[50];
        CheckIfItemShouldBeInTheBag();
        playerInteractionOngoing = false;
    }
	
	// Update is called once per frame
	void Update () {
        

        if (supervisorActionRepeatedCounter > supervisorActionRepeatedCounterMax)
        {
            MiniuAgent.agentIgnoringPlayer = true;
            supervisorActionRepeatedCounter = 0;
        }

        if (supervisorActionRepeatedCounter < 0)
        {
            MiniuAgent.agentIgnoringPlayer = false;
            supervisorActionRepeatedCounter = 0;
        }

        HandleMenuIdle();

        HandleObjPickedUpPanel();

        CheckForClickOnPaintCanvas();

        UpdateAgentStats();

        CheckThrowParentStatus();
    }

    void FillItemButtonTexts()
    {
        //Clear Item Buttons first
        for (int i = 0; i < itemButtonArr.Length; i++)
        {
            itemButtonArr[i].transform.Find("Text").GetComponent<Text>().text = "";
        }

        //Put the PlayerPrefs information into a string array
        inventoryItemsArr = PlayerPrefs.GetString("Inventory").Split('|');

        for (int i = 1; i < inventoryItemsArr.Length - 1; i++)
        {
            //Debug.Log("Inventory Item Arr Element: " + inventoryItemsArr[i]);
        }

        //Fill in the Item Button Texts with the string array
        for (int i = 0; i < itemButtonArr.Length; i++)
        {
            try
            {
                itemButtonArr[i].transform.Find("Text").GetComponent<Text>().text = inventoryItemsArr[i];
            }
            catch (System.Exception)
            {
                break;
            }
        }
    }

    void HandleMenuIdle()
    {
        if (!doorIsOpen)
        {
            menuIdleCounter+=Time.deltaTime;
            if (menuIdleCounter > menuIdleCounterMax)
            {
                cameraDoorAnimator.SetTrigger("cameraDoorRotateTrigger");
            }
        }
    }

    void CheckThrowParentStatus()
    {
        if(throwParent.childCount > 0)
        {
            currentHeldObj = throwParent.GetChild(0);
            DropHeldObjectWhereClicked();
        }
    }

    void UpdateAgentStats()
    {
        frameCtr = frameCtr + Time.deltaTime;
        if (frameCtr > 2f)
        {
            Utilities.UpdateAgentStats();
            frameCtr = 0f;
        }
    }

    void CheckIfItemShouldBeInTheBag()
    {
        inventoryItemsArr = PlayerPrefs.GetString("Inventory").Split('|');

        for (int i = 0; i < inventoryItemsArr.Length-1; i++)
        {
            //Debug.Log("Inventory Item Arr Element: " + inventoryItemsArr[i]);
        }

        for (int i = 0; i < inventoryItemsArr.Length-1; i++)
        {
            if (playObjectsParent.transform.Find(inventoryItemsArr[i]))
            {
                //Debug.Log("Item in play that should be in bag: "+ playObjectsParent.transform.Find(inventoryItemsArr[i]).name);
                playObjectsParent.transform.Find(inventoryItemsArr[i]).transform.gameObject.SetActive(false);
                playObjectsParent.transform.Find(inventoryItemsArr[i]).transform.parent = inventoryParent;
            }
        }
    }

    void MakeObjectReadyToThrow()
    {
        //Debug.Log("Ready TO Throw from SupervisorAndUI");
        ItemController.readyToThrow = true;
    }

    void DropHeldObjectWhereClicked()
    {
        if (ItemController.readyToThrow)
        {
            //Debug.Log("ready to throw now = ");
            if (Input.GetMouseButtonDown(0) || SupervisorAndUI.doorClosed)
            {
                ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                //Debug.Log("Clicked Mouse BUtton");
                if (Physics.Raycast(ray, out hit) || SupervisorAndUI.doorClosed)
                {
                    Debug.Log("Plane Hit");
                    if (hit.transform.name == "Actable Plane Group" 
                        || hit.transform.tag == "Play Object"
                        ||  SupervisorAndUI.doorClosed)
                    {
                        Invoke("DropHeldObject", .2f);
                    }
                }
            }
        }
    }

    void DropHeldObject()
    {
        if(ItemController.throwParent.childCount>0)
        {
            if (!SupervisorAndUI.doorClosed)
            {
                ItemController.currentHeldObj.position = new Vector3(hit.point.x, hit.point.y + 2f, hit.point.z);
            }
            ItemController.currentHeldObj.gameObject.SetActive(true);
            ItemController.currentHeldObj.GetComponent<Rigidbody>().isKinematic = false;
            ItemController.currentHeldObj.transform.parent = ItemController.playObjectsParent.transform;    
        }
    }



    void SetUpEvents()
    {
        resetCameraPosBtn.onClick.AddListener(() =>
        {
            mainCamera.transform.position = origCameraPosition;
        });

        returnFromCameraModeButton.onClick.AddListener(() =>
        {
            SetActiveGamePanel();
            cameraMode = false;
        });

        cameraButton.onClick.AddListener(() =>
        {
            SetActiveCameraModePanel();
            mainCamera.GetComponent<Animator>().enabled = false;
            cameraMode = true;
        });

        moveLeftCameraBtn.onClick.AddListener(() =>
        {
            if(MainCamera.moveCameraLeft)
            {
                MainCamera.moveCameraRight = false;
                MainCamera.moveCameraLeft = false;
            }else{
                MainCamera.moveCameraLeft = true;
            }
        });

        moveRightCameraBtn.onClick.AddListener(() =>
        {
            if(MainCamera.moveCameraRight)
            {
                MainCamera.moveCameraLeft = false;
                MainCamera.moveCameraRight = false;
            }else{
                MainCamera.moveCameraRight = true;
            }
        });

        doorButton.onClick.AddListener(() =>
        {
            mainCamera.GetComponent<Animator>().enabled = true;
            lastTouchedObj = currentTouchedObj;
            doorButton.interactable = false;
            cameraButton.interactable = false;
            debugButton.interactable = false;
            inventoryButton.interactable = false;
            playButton.interactable = true;
            //gamePanel.SetActive(false);
            if (doorIsOpen) //If Door is open
            {
                doorIsOpen = false; //Close door
                cameraDoorAnimator.SetTrigger("cameraDoorCloseTrigger");
                doorAnimator.SetTrigger("closeDoorTrigger");
                cameraDoorAnimator.SetTrigger("cameraDoorIdleTrigger");
                menuTextAnimator.SetTrigger("fadeInTrigger");
                Invoke("SetActiveMenuPanel", 1);
            }
            doorInteractionOngoing = true;
            playerInteractionOngoing = true;
            currentTouchedObj = door.transform;
            doorClosed = true;


            if (lastTouchedObj == currentTouchedObj)
            {
                supervisorActionRepeatedCounter++;
            }

            if(lastTouchedObj != currentTouchedObj)
            {
                supervisorActionRepeatedCounter--;
            }
        });

        playButton.onClick.AddListener(() =>
        {
            lastTouchedObj = currentTouchedObj;
            doorButton.interactable = true;
            cameraButton.interactable = true;
            debugButton.interactable = true;
            inventoryButton.interactable = true;
            playButton.interactable = false;
            menuIdleCounter = 0;
            doorIsOpen = true; //Open door
            cameraDoorAnimator.ResetTrigger("cameraDoorRotateTrigger");
            cameraDoorAnimator.SetTrigger("cameraDoorOpenTrigger");
            doorAnimator.SetTrigger("openDoorTrigger");
            menuTextAnimator.SetTrigger("fadeOutTrigger");
            Invoke("SetActiveGamePanel", 1);

            doorInteractionOngoing = true;
            playerInteractionOngoing = true;
            currentTouchedObj = door.transform;
            doorClosed = false;

            if (lastTouchedObj == currentTouchedObj)
            {
                supervisorActionRepeatedCounter++;
            }

            if (lastTouchedObj != currentTouchedObj)
            {
                supervisorActionRepeatedCounter--;
            }
        });

        profileButton.onClick.AddListener(() =>
        {
            menuTextAnimator.SetTrigger("fadeOutTrigger");
            profileTextAnimator.SetTrigger("fadeInTrigger");
            Invoke("SetActiveProfilePanel", 1);
            
            ageText.text = ""+PlayerPrefs.GetInt("AgeDays")+" Days, "+ PlayerPrefs.GetInt("AgeHours")+" Hours, "+ PlayerPrefs.GetInt("AgeMinutes")+" Minutes, "+ PlayerPrefs.GetInt("AgeSeconds")+" Seconds";
        });

        returnFromProfileButton.onClick.AddListener(() =>
        {
            menuTextAnimator.SetTrigger("fadeInTrigger");
            profileTextAnimator.SetTrigger("fadeOutTrigger");
            Invoke("SetActiveMenuPanel", 1);
        });

        inventoryButton.onClick.AddListener(() =>
        {
            if(ItemController.throwParent.childCount>0)
            { 
                Utilities.AddObjectToInventory(ItemController.currentHeldObj.name);
                ItemController.currentHeldObj.parent = inventoryParent;
                ItemController.currentHeldObj.gameObject.SetActive(false);
            }else{

                FillItemButtonTexts();
                inventoryPanel.SetActive(true);
            }
        });

        returnFromInventoryButton.onClick.AddListener(() =>
        {
            inventoryPanel.SetActive(false);
        });

        returnFromPaintCanvasViewButton.onClick.AddListener(() => 
        {
            //cameraDoorAnimator.ResetTrigger("paintCanvasTrigger");
            //cameraDoorAnimator.SetTrigger("idleOutdoorTrigger");
            paintCanvasCamera.SetActive(false);
            returnFromPaintCanvasViewButton.gameObject.SetActive(false);
        });

        #region Item Buttons
        //TODO: Make this shit have a dynamic amount of itemBtns bro
        itemBtn1.onClick.AddListener(() =>
        {
            selectedItem = itemBtn1.transform.Find("Text").GetComponent<Text>().text;
            OpenSelectedItemPanel();
        });

        itemBtn2.onClick.AddListener(() =>
        {
            selectedItem = itemBtn2.transform.Find("Text").GetComponent<Text>().text;
            OpenSelectedItemPanel();
        });

        itemBtn3.onClick.AddListener(() =>
        {
            selectedItem = itemBtn3.transform.Find("Text").GetComponent<Text>().text;
            OpenSelectedItemPanel();
        });

        itemBtn4.onClick.AddListener(() =>
        {
            selectedItem = itemBtn4.transform.Find("Text").GetComponent<Text>().text;
            OpenSelectedItemPanel();
        });

        itemBtn5.onClick.AddListener(() =>
        {
            selectedItem = itemBtn5.transform.Find("Text").GetComponent<Text>().text;
            OpenSelectedItemPanel();
        });

        itemBtn6.onClick.AddListener(() =>
        {
            selectedItem = itemBtn6.transform.Find("Text").GetComponent<Text>().text;
            OpenSelectedItemPanel();
        });

        itemBtn7.onClick.AddListener(() =>
        {
            selectedItem = itemBtn7.transform.Find("Text").GetComponent<Text>().text;
            OpenSelectedItemPanel();
        });

        itemBtn8.onClick.AddListener(() =>
        {
            selectedItem = itemBtn8.transform.Find("Text").GetComponent<Text>().text;
            OpenSelectedItemPanel();
        });

        itemBtn9.onClick.AddListener(() =>
        {
            selectedItem = itemBtn9.transform.Find("Text").GetComponent<Text>().text;
            OpenSelectedItemPanel();
        });

        #endregion

        useSelectedItemButton.onClick.AddListener(() =>
        {
            
            Utilities.RemoveObjectFromInventory(selectedItem);
            selectedItemPanel.SetActive(false);
            inventoryPanel.SetActive(false);
            ItemController.currentHeldObj = inventoryParent.Find(selectedItem).transform;
            //inventoryParent.Find(selectedItem).transform.GetComponent<Rigidbody>().isKinematic = true;
            //inventoryParent.Find(selectedItem).transform.gameObject.SetActive(true);
            //inventoryParent.Find(selectedItem).transform.parent = throwParent;

            ItemController.currentHeldObj.gameObject.SetActive(true);
            ItemController.currentHeldObj.GetComponent<Rigidbody>().isKinematic = true;
            ItemController.currentHeldObj.transform.position = throwParent.position;
            ItemController.currentHeldObj.transform.parent = throwParent;
            
            Invoke("MakeObjectReadyToThrow", 1/10);
        });

        returnFromSelectedItemButton.onClick.AddListener(() =>
        {
            selectedItemPanel.SetActive(false);
        });

        #region Debug Menu Stuff

        debugButton.onClick.AddListener(() =>
        {
            gamePanel.SetActive(false);
            debugMenuPanel.SetActive(true);
            
        });

        clearInventoryBtn.onClick.AddListener(() =>
        {
            //PlayerPrefs.SetString("Inventory", string.Empty);
        });

        returnFromDebugMenuButton.onClick.AddListener(() =>
        {
            debugMenuPanel.SetActive(false);
            gamePanel.SetActive(true);
        });

        #endregion
    }
    

    #region Handle methods of the different panels in the main scene
    void OpenSelectedItemPanel()
    {
        //Only "select" the item if the current selectedItem string is not null
        if (selectedItem != "")
        {
            selectedItemText.text = selectedItem;
            selectedItemPanel.SetActive(true);
        }
    }

    void HandleObjPickedUpPanel()
    {
        if (throwParent.childCount > 0)
        {
            objPickedUpPanel.SetActive(true);
        }
        else
        {
            objPickedUpPanel.SetActive(false);

            if (doorIsOpen && !debugMenuPanel.activeInHierarchy 
            && !returnFromPaintCanvasViewButton.gameObject.activeSelf
            && !cameraModePanel.activeInHierarchy)
            {
                gamePanel.SetActive(true);
            }

        }
    }

    void CheckForClickOnPaintCanvas()
    {
        if (Input.GetMouseButtonDown(0) && !PlayObject.holding && !cameraMode)
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Debug.Log("Clicked Mouse BUtton");
            if (Physics.Raycast(ray, out hit))
            {
                
                if (hit.transform.name == "Paint Canvas")
                {
                    Debug.Log("Paint Canvas Hit");
                    gamePanel.SetActive(false);
                    paintCanvasCamera.SetActive(true);
                    //cameraDoorAnimator.SetTrigger("paintCanvasTrigger");
                    
                    returnFromPaintCanvasViewButton.gameObject.SetActive(true);
                }
            }
        }
        
    }

    void SetActiveCameraModePanel()
    {
        menuPanel.SetActive(false);
        gamePanel.SetActive(false);
        profilePanel.SetActive(false);
        cameraModePanel.SetActive(true);
    }

    void SetActiveProfilePanel()
    {
        menuPanel.SetActive(false);
        gamePanel.SetActive(false);
        cameraModePanel.SetActive(false);
        profilePanel.SetActive(true);
    }

    void SetActiveGamePanel()
    {
        profilePanel.SetActive(false);
        menuPanel.SetActive(false);
        cameraModePanel.SetActive(false);
        gamePanel.SetActive(true);
    }

    void SetActiveMenuPanel()
    {
        profilePanel.SetActive(false);
        gamePanel.SetActive(false);
        cameraModePanel.SetActive(false);
        menuPanel.SetActive(true);
    } 
    #endregion
}
