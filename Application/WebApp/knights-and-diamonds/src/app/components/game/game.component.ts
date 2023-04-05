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
  playerHand:any;
  numbers:any;

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
      this.playerHand=playerHand;
    });
  }
  getNumOfCardsInHand(){
    this.signalrService.hubConnection.on("GetNumberOfCardsInHand", (count: any) => {
      console.log(count);
      this.numbers = new Array<number>(count)
    })
  }
  
}
