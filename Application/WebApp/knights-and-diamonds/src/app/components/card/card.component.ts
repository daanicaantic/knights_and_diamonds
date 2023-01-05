import { Component, OnInit } from '@angular/core';
import { card } from 'src/classes/card-data';

@Component({
  selector: 'app-card',
  templateUrl: './card.component.html',
  styleUrls: ['./card.component.css']
})
export class CardComponent implements OnInit {
  card=card;
  
  constructor() { }

  ngOnInit(): void {
  }
}
