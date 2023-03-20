import { Component, OnDestroy, OnInit } from '@angular/core';
import { card } from 'src/classes/card-data';

@Component({
  selector: 'app-game',
  templateUrl: './game.component.html',
  styleUrls: ['./game.component.css']
})
export class GameComponent implements OnInit, OnDestroy {
  card=card;
  constructor() { }

  ngOnInit(): void {

  }

  ngOnDestroy(): void {

  }
  
}
