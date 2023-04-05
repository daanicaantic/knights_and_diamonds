import { Component, OnInit } from '@angular/core';
import { CardService } from 'src/app/services/card.service';
import { Card } from 'src/classes/card';

@Component({
  selector: 'app-deck',
  templateUrl: './deck.component.html',
  styleUrls: ['./deck.component.css']
})

export class DeckComponent implements OnInit {
  cards:any;
  constructor(private cardService: CardService) { }

  ngOnInit(): void {
    this.getCards();
  }

  getCards() {
    this.cardService.getCards().subscribe({
      next: res => {
        this.cards=res;
        console.log(res);
      },
      error: err => {
        
      }
    })
  }

}
