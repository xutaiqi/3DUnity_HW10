using UnityEngine;
using Interfaces;
using System.Collections.Generic;
using System.Collections;
using System.Threading;

 

public class AutoMove 
{
    public static AutoMove autoMove = new AutoMove();
    public FirstController firstScence;
    private int devilNum;
    private int priestNum;
    private int BoatCoast;//-1 ->left 1->right

    private enum Boataction { empty, P, D, PP, DD, PD };
    private bool isFinished = true;
    private Boataction nextState;
    private int count = 0;
    private int num = 0;

    private AutoMove() { }
    public void move()
    {
        Debug.Log("isFinished is true, " + count);
        if(isFinished){
            isFinished = false;
            Debug.Log(count);
            int[] fromCount = firstScence.fromCoast.GetobjectsNumber();
            priestNum = fromCount[0];
            devilNum = fromCount[1];
            BoatCoast = firstScence.boat.get_State();


            if(count==0)
            {
                    nextState = getNext();
                    if ((int)nextState >= 3)
                    {
                        num = 2;
                    }
                    else if ((int)nextState > 0) num = 1;
                    else num = 0;
                    count++;
                }
                Debug.Log(nextState+"move");
                DoAction();


        } 
    }
    private void DoAction()
    {
        if (count == 1 && num != 0)
        {
            if (nextState == Boataction.D)
            {
                devilOnBoat();
            }
            else if (nextState == Boataction.DD)
            {
                devilOnBoat();
            }
            else if (nextState == Boataction.P)
            {
                priestOnBoat();
            }
            else if (nextState == Boataction.PP)
            {
                priestOnBoat();
            }
            else if (nextState == Boataction.PD)
            {
                priestOnBoat();
            }
            count++;
        }
        else if (num == 2 && count == 2)
        {
            if (nextState == Boataction.DD)
            {
                devilOnBoat();
            }
            else if (nextState == Boataction.PP)
            {
                priestOnBoat();
            }
            else if (nextState == Boataction.PD)
            {
                devilOnBoat();
            }
            count++;
        }
        else if ((num == 1 && count == 2) || (num == 2 && count == 3) || (num == 0 && count == 1))
        {
            firstScence.MoveBoat();
            count++;
        }
        else if ((num == 1 && count >= 3) || (num == 2 && count >= 4) || (num == 0 && count >= 2))
        {
            GetOffBoat();
        }
        isFinished = true;
    }
    private void GetOffBoat()
    {
        if ((priestNum == 0 && devilNum == 2) || (priestNum == 0 && devilNum == 0))
        {
            if (firstScence.boat.get_State() == -1)
            {
                foreach (var x in firstScence.boat.passenger)
                {
                    if (x != null)
                    {
                        firstScence.ObjectIsClicked(x);
                        break;
                    }
                }
                if (firstScence.boat.isEmpty())
                {
                    count = 0;
                }
            }
            else
            {
                count = 0;
            }
        }
        else if (((priestNum == 0 && devilNum == 1)) && firstScence.boat.get_State() == 1)
        {
            count = 0;
        }
        else
        {
            foreach (var x in firstScence.boat.passenger){
                if(x!=null &&x.getType()==1){
                    firstScence.ObjectIsClicked(x);
                    count = 0;
                    break;
                }
            }
            if (count != 0)
            {
                foreach (var x in firstScence.boat.passenger)
                {
                    if (x != null)
                    {
                        firstScence.ObjectIsClicked(x);
                        count = 0;
                        break;
                    }
                }
            }
        }
    }
    private void priestOnBoat()
    {
        if (BoatCoast == 1)
        {
            foreach (var x in firstScence.fromCoast.passengerPlaner)
            {
                if (x != null && x.getType() == 0)
                {
                    firstScence.ObjectIsClicked(x);
                    return;
                }
            }
        }
        else
        {
            foreach (var x in firstScence.toCoast.passengerPlaner)
            {
                if (x != null && x.getType() == 0)
                {
                    firstScence.ObjectIsClicked(x);
                    return;
                }
            }
        }
    }
    private void devilOnBoat()
    {
        if(BoatCoast==1){
            foreach(var x in firstScence.fromCoast.passengerPlaner){
                if(x!=null && x.getType()==1){
                    firstScence.ObjectIsClicked(x);
                    return;
                }
            }
        }
        else{
            foreach(var x in firstScence.toCoast.passengerPlaner){
                if(x!=null && x.getType()==1){
                    firstScence.ObjectIsClicked(x);
                    return;
                }
            }
        }
    }
    private Boataction getNext(){
        Boataction next = Boataction.empty;
        if(BoatCoast ==1){
            if (devilNum == 3 && priestNum == 3)
            {
                next = Boataction.PD;
            }
            else if (devilNum == 2 && priestNum == 3)
            {
                next = Boataction.DD;
            }
            else if (devilNum == 1 && priestNum == 3)
            {
                next = Boataction.PP;
            }
            else if (devilNum == 2 && priestNum == 2)
            {
                next = Boataction.PP;
            }
            else if (devilNum == 3 && priestNum == 0)
            {
                next = Boataction.DD;
            }
            else if (devilNum == 1 && priestNum == 1)
            {
                next = Boataction.PD;
            }
            else if (devilNum == 2 && priestNum == 0)
            {
                next = Boataction.D;
            }
            else if (devilNum == 1 && priestNum == 2)
            {
                next = Boataction.P;
            }
            else if (devilNum == 2 && priestNum == 1)
            {
                next = Boataction.P;
            }
            else if (devilNum == 1 && priestNum == 0)
            {
                next = Boataction.D;
            }
            else if (devilNum == 3 && priestNum == 2)
            {
                next = Boataction.D;
            }
            else next = Boataction.empty;
        }else{
            if (devilNum == 2 && priestNum == 2)//2P2D
            {
                next = Boataction.empty;
            }
            else if (devilNum == 1 && priestNum == 3)//3P1D
            {
                next = Boataction.empty;
            }
            else if (devilNum == 2 && priestNum == 3)//3P2D
            {
                next = Boataction.D;
            }
            else if (devilNum == 0 && priestNum == 3)//3P
            {
                next = Boataction.empty;
            }
            else if (devilNum == 1 && priestNum == 1)//1P1D
            {
                next = Boataction.D;
            }
            else if (devilNum == 2 && priestNum == 0)//2D
            {
                next = Boataction.D;
            }
            else if (devilNum == 1 && priestNum == 0)//1D
            {
                next = Boataction.empty;
            }
            else next = Boataction.empty;
        }
        return next;
    }
    public void restart(){
        count = 0;
        num = 0;
    }
}
