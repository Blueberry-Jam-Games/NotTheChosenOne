using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class StoryGetArrow : MonoBehaviour
{
    private StoryController storyController;

    public DoorScript exitDoor;
    public GameObject playerStart;
    public GameObject arrowItem;
    public RPGTalk levelMasterTalk;

    private PlayerMovement player;

    // Start is called before the first frame update
    void Start()
    {
        GameObject sc = GameObject.FindWithTag("StoryController");
        if(sc != null)
        {
            storyController = sc.GetComponent<StoryController>();
        }

        if(sc == null || storyController == null || storyController.StoryStage != StoryController.STAGE_GET_ARROW)
        {
            Debug.Log("StoryGetArrow detected missing elements, deleting");
            this.gameObject.SetActive(false);
            arrowItem.SetActive(false);
            return;
        }
        else
        {
            Debug.Log("Initializing Story Get arrow stage");
            exitDoor.gameObject.SetActive(false);
            GameObject playerObj = GameObject.FindWithTag("Player");
            player = playerObj.GetComponent<PlayerMovement>();
            player.transform.position = playerStart.transform.position;
            //Play intro cutscene
            player.lockedControls = true;
            levelMasterTalk.OnEndTalk += OnCutsceneEnd;
            levelMasterTalk.NewTalk("IntroCutscene", "IntroCutsceneE");
        }
    }

    private void OnCutsceneEnd()
    {
        player.lockedControls = false;
        levelMasterTalk.OnEndTalk -= OnCutsceneEnd;
    }

    public void ArrowRetrieved()
    {
        Debug.Log("Retrieve arrow completed, advance to the next stage.");
        storyController.StoryStage = StoryController.STAGE_HAS_ARROW; //TODO this has to do more
        player.ResetDoor();
        exitDoor.gameObject.SetActive(true);
        exitDoor.ResetDoorUsedRecently();
    }
}
