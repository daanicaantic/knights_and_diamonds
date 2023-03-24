import { Component, OnDestroy, OnInit } from '@angular/core';
import { card } from 'src/classes/card-data';
import { IngameService } from 'src/app/services/ingame.service';

@Component({
  selector: 'app-game',
  templateUrl: './game.component.html',
  styleUrls: ['./game.component.css']
})
export class GameComponent implements OnInit, OnDestroy {
  card = card;
  numberOfCardsInEnemiesHand:Number = 6;
  
  constructor(
    public inGameService:IngameService
  ) { }

  ngOnInit(): void {
    this.setGameStatus();
  }

  ngOnDestroy(): void {
    this.inGameService.setGameOff();
  }

  setGameStatus(){
    this.inGameService.setGameOn();
  }
  
}
