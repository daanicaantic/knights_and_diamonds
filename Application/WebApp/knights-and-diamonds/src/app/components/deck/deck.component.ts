import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { CardService } from 'src/app/services/card.service';
import { Card } from 'src/classes/card';

@Component({
  selector: 'app-deck',
  templateUrl: './deck.component.html',
  styleUrls: ['./deck.component.css']
})

export class DeckComponent implements OnInit, OnDestroy {
  cards:any;
  subscriptions: Subscription[] = [];

  constructor(private cardService: CardService) { }

  ngOnInit(): void {
    this.getCards();
  }

  getCards() {
    this.subscriptions.push(
      this.cardService.getCards().subscribe({
        next: res => {
          this.cards=res;
          console.log(res);
        },
        error: err => {
        }
      })
    );
  }

  ngOnDestroy(): void {
    this.subscriptions.forEach(subscription => subscription.unsubscribe());
  }

}
