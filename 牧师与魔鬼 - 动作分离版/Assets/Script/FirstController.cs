using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Interfaces;

public class FirstController : MonoBehaviour, ISceneController, UserAction
{
    InteracteGUI UserGUI;
    public CoastController fromCoast;
    public CoastController toCoast;
    public BoatController boat;
    private GameObjects[] GameObjects;

    AutoMove auto;

    public Referee referee_=new Referee();

    private FirstSceneActionManager FCActionManager;

    void Start()
    {
        FCActionManager = GetComponent<FirstSceneActionManager>();
    }

    void Awake()
    {
        SSDirector director = SSDirector.getInstance();
        director.currentScenceController = this;
        UserGUI = gameObject.AddComponent<InteracteGUI>() as InteracteGUI;
        GameObjects = new GameObjects[6];
        LoadResources();
        AutoMove.autoMove.firstScence = this;
    }

    public void LoadResources()
    {
        fromCoast = new CoastController("from");
        toCoast = new CoastController("to");
        boat = new BoatController();
        GameObject water = Instantiate(Resources.Load("Perfabs/Water", typeof(GameObject)), new Vector3(0, 0.5F, 0), Quaternion.identity, null) as GameObject;
        water.name = "water";
        for (int i = 0; i < 3; i++)
        {
            GameObjects s = new GameObjects("priest");
            s.setName("priest" + i);
            s.setPosition(fromCoast.getEmptyPosition());
            s.getOnCoast(fromCoast);
            fromCoast.getOnCoast(s);
            GameObjects[i] = s;
        }

        for (int i = 0; i < 3; i++)
        {
            GameObjects s = new GameObjects("devil");
            s.setName("devil" + i);
            s.setPosition(fromCoast.getEmptyPosition());
            s.getOnCoast(fromCoast);
            fromCoast.getOnCoast(s);
            GameObjects[i + 3] = s;
        }
    }

	public void ObjectIsClicked(GameObjects Objects)
    {
        if (FCActionManager.Complete == SSActionEventType.Started) return;
        if (Objects.isOnBoat())
        {
            CoastController whichCoast;
            if (boat.get_State() == -1)
            { // to->-1; from->1
                whichCoast = toCoast;
            }
            else
            {
                whichCoast = fromCoast;
            }

            boat.GetOffBoat(Objects.getName());
            FCActionManager.GameObjectsMove(Objects,whichCoast.getEmptyPosition());
            Objects.getOnCoast(whichCoast);
            whichCoast.getOnCoast(Objects);

        }
        else
        {
            Debug.Log("On Coast!");
            CoastController whichCoast = Objects.getCoastController(); // obejects on coast

            if (boat.getEmptyIndex() == -1)
            {      
                return;
            }

            if (whichCoast.get_State() != boat.get_State())   // boat is not on the side of character
                return;

            whichCoast.getOffCoast(Objects.getName());
            FCActionManager.GameObjectsMove(Objects, boat.getEmptyPosition());
            Objects.getOnBoat(boat);
            boat.GetOnBoat(Objects);
        }
        //UserGUI.SetState = Check();
    }

    public void MoveBoat()
    {   UserGUI.SetState = referee_.Check(fromCoast, toCoast, boat);
        if (FCActionManager.Complete == SSActionEventType.Started || boat.isEmpty()) return;
        FCActionManager.BoatMove(boat);

    }



    public void Restart()
    {
        fromCoast.reset();
        toCoast.reset();
        foreach (GameObjects gameobject in GameObjects)
        {
            gameobject.reset();
        }
        boat.reset();
        AutoMove.autoMove.restart();
    }
    public void Next(){
        AutoMove.autoMove.move();
        Debug.Log("next");
    }
}