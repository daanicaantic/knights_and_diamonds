import { Component, OnDestroy, OnInit } from '@angular/core';
import { card } from 'src/classes/card-data';
import { IngameService } from 'src/app/services/ingame.service';
import { GameService } from 'src/app/services/game.service';
import { AuthService } from 'src/app/services/auth.service';
import { ActivatedRoute, Router } from '@angular/router';
import { SignalrService } from 'src/app/services/signalr.service';

@Component({
  selector: 'app-game',
  templateUrl: './game.component.html',
  styleUrls: ['./game.component.css']
})
export class GameComponent implements OnInit, OnDestroy {
  card = card;
  numberOfCardsInEnemiesHand:Number = 6;
  isLoadingOver=false;
  loadingType="game";
  userID = this.authService?.userValue?.id;
  gameID:any;
  playerID:any;
  enemiesID:any;
  playerHand:any[]=[];
  numOfEnemiesCards:any[]=[];
  drawingTimer:any;
  mediumCard:any;

  constructor(
    public inGameService:IngameService,
    public gameService:GameService,
    private router: Router,
    private route: ActivatedRoute,
    private authService:AuthService,
    private signalrService:SignalrService
  ) { }

  ngOnInit(): void {
    this.gameID = this.route.snapshot.params['gameID'];
    this.setGameStatus();
    this.getGame();
    this.startingDrawing();
    this.getNumOfCardsInHand();

    
  }

  ngOnDestroy(): void {
    this.inGameService.setGameOff();
  }

  setGameStatus(){
    this.inGameService.setGameOn();
  }
  getTimer(event:any){
    console.log("ovdeee",event);
    this.isLoadingOver=event;

    if(this.userID!=undefined){
      if (this.signalrService.hubConnection.state=="Connected") {
        this.startingDrawingInv(); 
      }
      else{
        this.signalrService.ssSubj.subscribe((obj: any) => {
          if (obj.type == "HubConnStarted") {
            this.startingDrawingInv(); 
          }
        });
      }
    }
  }
  
  getGame(){
    this.gameService.getGame(this.gameID, this.userID).subscribe({
      next: (res:any) => {
        this.playerID=res.playerID;
        this.enemiesID=res.enemiePlayerID;
        console.log("resko reskovic",res)
        // this.startingDrawingInv();

      },
      error: err => {
        console.log(err)
      }
    })
  }

  startingDrawingInv(): void {
    this.signalrService.hubConnection.invoke("StartingDrawing",Number(this.gameID),this.playerID)
      .catch(err => console.error(err));
  }

  startingDrawing() {
    this.signalrService.hubConnection.on("GetFirstCards", (playerHand: any) => {
      console.log(playerHand);
      this.getStartingDrawingEffect(0,this.playerHand,playerHand);
      // this.playerHand=playerHand;
    });
  }
  getNumOfCardsInHand(){
    this.signalrService.hubConnection.on("GetNumberOfCardsInHand", (count: any) => {
      console.log(count);
      this.getEnemiesDrawingEffect(0,this.numOfEnemiesCards,count);
      // this.getArrayValues(0,this.numbers,)
    })
  }
  //koristimo fju za prikaz pocetnog izvlacenja karata;
  getStartingDrawingEffect(index:any,selectedArray:any,Array2:any) {
    var timer=setInterval(() => {
      if(index == Array2.length){
        clearInterval(timer)
        return;
      }
      selectedArray.unshift(Array2[index]);
      index++;
    }, 300);
  }
  //koristimo fju za prikaz pocetnog broja protivnickih karata;
  getEnemiesDrawingEffect(index:any,selectedArray:any,count:any) {
    var timer=setInterval(() => {
      console.log(index)
      if(index == count){
        clearInterval(timer)
        return;
      }
      selectedArray.unshift(new Array<number>(index));
      index++;
    }, 300);
  }

}
