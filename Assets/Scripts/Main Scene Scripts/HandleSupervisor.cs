using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HandleSupervisor : MonoBehaviour {

    public GameObject frontCamera, backCamera, indoorCamera, menuPanel, gamePanel, profilePanel, objPickedUpPanel, inventoryPanel, selectedItemPanel, debugMenuPanel, playObjectsParent;

    public Button doorButton, cameraButton, playButton, profileButton, returnFromProfileButton, inventoryButton, portalButton, debugButton, returnFromInventoryButton, returnFromSelectedItemButton,
    itemBtn1, itemBtn2, itemBtn3, itemBtn4, itemBtn5, itemBtn6, itemBtn7, itemBtn8, itemBtn9, useSelectedItemButton, extraDebugBtn, otherWorldBtn, dreamWorldBtn, resetAgeBtn, clearInventoryBtn, resetAllBtn,
    returnFromDebugMenuButton, returnFromPaintCanvasViewButton;

    public Button[] itemButtonArr;

    private string[] inventoryItemsArr;

    public GameObject door;
    public Animator doorAnimator, cameraDoorAnimator, menuTextAnimator, profileTextAnimator;
    public Text titleText, profileText, playText, ageText, selectedItemText;
    public static int supervisorActionRepeatedCounter = 0, doorMovedCounter = 0, menuIdleCounter = 0;

    public static Transform lastTouchedObj, currentTouchedObj, lastTouchedPos, frontPoint, currentHeldObj;
    public Transform playerPoint, throwParent, inventoryParent;
    public static int supervisorActionRepeatedCounterMax, menuIdleCounterMax;
    public static bool doorInteractionOngoing, doorClosed, playerInteractionOngoing;
    private bool doorIsOpen;
    private int frameCtr;
    private string inventoryStringPlaceholder = "", selectedItem;
    Ray ray;
    RaycastHit hit;
    // Use this for initialization
    void Start () {
        SetUpEvents(); //Set up Buttons
        doorInteractionOngoing = false;
        doorIsOpen = false;
        gamePanel.SetActive(false);
        returnFromPaintCanvasViewButton.gameObject.SetActive(false);
        menuPanel.SetActive(true);
        backCamera.SetActive(false);
        indoorCamera.SetActive(false);
        frontCamera.SetActive(false);
        supervisorActionRepeatedCounterMax = 1000;
        menuIdleCounterMax = 600;
        cameraDoorAnimator.SetTrigger("cameraDoorIdleTrigger");
        frameCtr = 0;
        inventoryItemsArr = new string[50];
        CheckIfItemShouldBeInTheBag();
        playerInteractionOngoing = false;
    }
	
	// Update is called once per frame
	void Update () {
        

        if (supervisorActionRepeatedCounter > supervisorActionRepeatedCounterMax)
        {
            HandleAgent.agentIgnoringPlayer = true;
            supervisorActionRepeatedCounter = 0;
        }

        if (supervisorActionRepeatedCounter < 0)
        {
            HandleAgent.agentIgnoringPlayer = false;
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
            Debug.Log("Inventory Item Arr Element: " + inventoryItemsArr[i]);
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
            menuIdleCounter++;
            if (Utilities.FramesToSeconds(menuIdleCounter) > Utilities.FramesToSeconds(menuIdleCounterMax))
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
        frameCtr++;
        if (frameCtr > 59)
        {
            Utilities.UpdateAgentStats();
            frameCtr = 0;
        }
    }

    void CheckIfItemShouldBeInTheBag()
    {
        inventoryItemsArr = PlayerPrefs.GetString("Inventory").Split('|');

        for (int i = 0; i < inventoryItemsArr.Length-1; i++)
        {
            Debug.Log("Inventory Item Arr Element: " + inventoryItemsArr[i]);
        }

        for (int i = 0; i < inventoryItemsArr.Length-1; i++)
        {
            if (playObjectsParent.transform.Find(inventoryItemsArr[i]))
            {
                Debug.Log("Item in play that should be in bag: "+ playObjectsParent.transform.Find(inventoryItemsArr[i]).name);
                playObjectsParent.transform.Find(inventoryItemsArr[i]).transform.gameObject.SetActive(false);
                playObjectsParent.transform.Find(inventoryItemsArr[i]).transform.parent = inventoryParent;
            }
        }
    }

    void MakeObjectReadyToThrow()
    {
        Debug.Log("Ready TO Throw from HandleSUpervisor");
        HandleBall.readyToThrow = true;
    }

    void DropHeldObjectWhereClicked()
    {
        if (HandleBall.readyToThrow)
        {
            //Debug.Log("ready to throw now = ");
            if (Input.GetMouseButtonDown(0) || HandleSupervisor.doorClosed)
            {
                ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                //Debug.Log("Clicked Mouse BUtton");
                if (Physics.Raycast(ray, out hit) || HandleSupervisor.doorClosed)
                {
                    Debug.Log("Plane Hit");
                    if (hit.transform.name == "Actable Plane Group" || HandleSupervisor.doorClosed)
                    {
                        if (!HandleSupervisor.doorClosed)
                        {
                            currentHeldObj.position = new Vector3(hit.point.x, hit.point.y + 1f, hit.point.z);
                        }
                        currentTouchedObj = currentHeldObj;
                        currentHeldObj.gameObject.SetActive(true);
                        currentHeldObj.GetComponent<Rigidbody>().isKinematic = false;
                        currentHeldObj.transform.parent = playObjectsParent.transform;
                        
                        //Debug.Log("Object dropped");
                    }
                }
            }
            //readyToThrow = false;
        }

    }

    void SetUpEvents()
    {
        doorButton.onClick.AddListener(() =>
        {
            lastTouchedObj = currentTouchedObj;
            doorButton.interactable = false;
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
            playButton.interactable = false;
            //gamePanel.SetActive(true);
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
            if(throwParent.childCount>0)
            {
                //Debug.Log("CURRENT HELD OBJ IS: "+ currentHeldObj.name);
                Utilities.AddObjectToInventory(currentHeldObj.name);
                //inventoryStringPlaceholder = PlayerPrefs.GetString("Inventory");
                Debug.Log(inventoryStringPlaceholder);
                currentHeldObj.parent = inventoryParent;
                currentHeldObj.gameObject.SetActive(false);
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
            cameraDoorAnimator.ResetTrigger("paintCanvasTrigger");
            cameraDoorAnimator.SetTrigger("idleOutdoorTrigger");
            returnFromPaintCanvasViewButton.gameObject.SetActive(false);
        });

        #region Item Buttons
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
            currentHeldObj = inventoryParent.Find(selectedItem).transform;
            //inventoryParent.Find(selectedItem).transform.GetComponent<Rigidbody>().isKinematic = true;
            //inventoryParent.Find(selectedItem).transform.gameObject.SetActive(true);
            //inventoryParent.Find(selectedItem).transform.parent = throwParent;

            currentHeldObj.gameObject.SetActive(true);
            currentHeldObj.GetComponent<Rigidbody>().isKinematic = true;
            currentHeldObj.transform.position = throwParent.position;
            currentHeldObj.transform.parent = throwParent;
            
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

        otherWorldBtn.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("Otherworld Scene");
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

            if (doorIsOpen && !debugMenuPanel.activeInHierarchy && !returnFromPaintCanvasViewButton.gameObject.activeSelf)
            {
                gamePanel.SetActive(true);
            }

        }
    }

    void CheckForClickOnPaintCanvas()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Debug.Log("Clicked Mouse BUtton");
            if (Physics.Raycast(ray, out hit))
            {
                
                if (hit.transform.name == "Paint Canvas")
                {
                    Debug.Log("Paint Canvas Hit");
                    gamePanel.SetActive(false);
                    cameraDoorAnimator.SetTrigger("paintCanvasTrigger");
                    
                    returnFromPaintCanvasViewButton.gameObject.SetActive(true);
                }
            }
        }
        
    }
    void SetActiveProfilePanel()
    {
        menuPanel.SetActive(false);
        gamePanel.SetActive(false);
        profilePanel.SetActive(true);
    }

    void SetActiveGamePanel()
    {
        profilePanel.SetActive(false);
        menuPanel.SetActive(false);
        gamePanel.SetActive(true);
    }

    void SetActiveMenuPanel()
    {
        profilePanel.SetActive(false);
        gamePanel.SetActive(false);
        menuPanel.SetActive(true);
    } 
    #endregion
}
