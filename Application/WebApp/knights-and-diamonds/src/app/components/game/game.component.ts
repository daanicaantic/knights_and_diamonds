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
  userID = this.authService?.userValue?.id;
  playerID=this.authService?.userValue?.player;  
  enemiesID=this.authService?.userValue?.enemie; 
  curentPhase="DP";

  playerField:any;
  enemiesField:any;
  card = card;
  // numberOfCardsInEnemiesHand:Number = 6;
  isLoadingOver=false;
  loadingType="game";
  gameID:any
  playerHand:any[]=[];
  enemiesHand:any[]=[];
  drawingTimer:any;
  mediumCard=card; 
  graveCard:any;
  iscardshowen=false;
  constructor(
    public inGameService:IngameService,
    public gameService:GameService,
    private router: Router,
    private route: ActivatedRoute,
    private authService:AuthService,
    private signalrService:SignalrService
  ) { }

  ngOnInit(): void {
    
    console.log(this.authService.userValue);
    this.gameID = this.route.snapshot.params['gameID'];
    // this.getPlayersField();
    // this.getEnemiesField();
    this.getStartingField();

    this.getStartingDrawing();

  }

  ngOnDestroy(): void {
    this.inGameService.setGameOff();
  }

  setGameStatus(){
    this.inGameService.setGameOn();
  }
  onLoadingOver(event:any){
    this.isLoadingOver=event;
    if(this.playerField.gameStarted==false){
      this.gameService.startingDrawingInv(Number(this.gameID),this.playerID); 
    }
  }

  //koristimo fju prilikom ngOnInita vraca ceo prikaz table
  //preko kontrolera,preko signalr-a,ukoliko dodje do refresha
  //dolazi do gresaka
  getStartingField(){
    this.gameService.getPlayersField(this.playerID).subscribe({
      next: res => {
        this.playerField=res;
        console.log(this.playerField)
   
        this.playerField.cardFields[3].cardOnField=this.playerField.hand[0]
        this.playerField.cardFields[9].cardOnField=this.playerField.hand[2]
        this.playerField.cardFields[9].cardShowen=false;

      },
      error: err => {
        console.log(err.error);
      }
    })
    this.gameService.getEnemiesField(this.enemiesID).subscribe({
      next: res => {
        this.enemiesField=res;
        this.enemiesHand=Array.from(Array(this.enemiesField.handCount).keys())
        this.enemiesField.cardFields[4].cardOnField=this.mediumCard
        this.enemiesField.cardFields[0].cardOnField=this.mediumCard
        this.enemiesField.cardFields[9].cardOnField=this.mediumCard

        this.enemiesField.cardFields[9].cardShowen=false;
        console.log( this.enemiesField.cardFields[9])

      },
      error: err => {
        console.log(err.error);
      }
    })
  }
  


  getStartingDrawing() {
    this.signalrService.hubConnection.on("GetFirstCards", (playerHand: any) => {
      this.getStartingDrawingEffect(0,this.playerField.hand,playerHand);
    });
    this.signalrService.hubConnection.on("GetNumberOfCardsInHand", (count: any) => {
      this.getEnemiesDrawingEffect(0,this.enemiesHand,count);
    })
  }

  getPlayersField() {
    this.signalrService.hubConnection.on("GetYourField", (field: any) => {
      this.playerField=field;
      console.log(this.playerField);
    });
  }
  getEnemiesField() {
    this.signalrService.hubConnection.on("GetEnemiesField", (enemiesField: any) => {
      this.enemiesField=enemiesField;
      console.log(this.enemiesField);
    });
  }



  //koristimo fju za prikaz pocetnog izvlacenja karata;
  getStartingDrawingEffect(index:any,selectedArray:any,Array2:any) {
    var timer=setInterval(() => {
      if(index == Array2.length){
        clearInterval(timer)
        return;
      }
      selectedArray.unshift(Array2[index]);
      this.playerField.deckCount=this.playerField.deckCount-1;
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
      this.getEnemiesNumberOfCards(selectedArray);
      index++;
    }, 300);
  }

  getEnemiesNumberOfCards(selectedArray:any){
    selectedArray.unshift(new Array<number>(0));
    this.enemiesField.deckCount=this.enemiesField.deckCount-1;
  }
  onEnemiesCardMouseOver(field:any){
    this.mediumCard=field.cardOnField;
    if(field.cardShowen==true) 
    {
      this.iscardshowen=true;
    }
    else{
      this.iscardshowen=false;
    }
    
  }
}

