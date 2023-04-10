import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { ConfirmationService, MessageService } from 'primeng/api';
import { AuthService } from 'src/app/services/auth.service';
import { CardService } from 'src/app/services/card.service';
import { DeckService } from 'src/app/services/deck.service';
import { card } from 'src/classes/card-data';
import { CardType } from 'src/classes/card-type';
import { PomType } from 'src/classes/type';

@Component({
  selector: 'app-deck-create',
  templateUrl: './deck-create.component.html',
  styleUrls: ['./deck-create.component.css']
})
export class DeckCreateComponent implements OnInit {
  card = card;
  cards: any;
  form!: FormGroup;
  cardTypes: any[]=[];
  bigCard: any;
  startingvalue: any;
  cardsInDeck: any;
  userID = this.authService?.userValue?.id;
  deckID: any;
  sortType: any[]=[];
  stats: any;

  constructor(private cardService: CardService,
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private deckService: DeckService,
    private authService: AuthService,
    private confirmationService: ConfirmationService,
    private messageService: MessageService) {
    }

  ngOnInit(): void {
    this.deckID = this.route.snapshot.params['deckID']
    console.log('ID deck', this.deckID)
    this.getCards();
    this.getCardTypes();
    this.getUsersDeck();
    this.cardCounter();

    this.form = this.fb.group({
      cardName: "",
      cardTypeID: 0,
      cardSorting: 0,
    })
  }

  onCardTypeChange(){
    console.log(this.form.getRawValue())
  }

  onCardSortChange() {

  }

  getCards() {
    this.cardService.getCards().subscribe({
      next: res => {
        this.cards=res;
      },
      error: err => {
      }
    })
  }

  getCardTypes() {
    this.cardService.getCardTypes().subscribe({
      next: (res:any) => {
        this.cardTypes.push(new PomType(0,"AllCards"))
        res.forEach((element:any) => {
          this.cardTypes.push(element);
        });
        this.startingvalue=0;
      },
      error: err => {
        console.log("neuspesno ct")
      }
    })
  }

  getUsersDeck() {
    this.deckService.getUsersDeck(this.userID).subscribe({
      next: res => {
        console.log(res)
        this.cardsInDeck=res;
      },
      error: err => {
        console.log("neuspesno getUsersDeck")
      }
    })
  }

  cardCounter() {
    this.deckService.cardCounter(this.deckID, this.userID).subscribe({
      next: res => {
        this.stats = res;
      },
      error: err => {
        console.log("neuspesno cardCounter")
      }
    })
  }

  addCardToDeck(cardID:any) {
    this.confirmationService.confirm({
      message: 'Add this card to your deck?',
      accept: () => {
        this.deckService.addCardToDeck(cardID, this.deckID).subscribe({
          next: res => {
            this.getUsersDeck();
            this.cardCounter();
            this.messageService.add({ key: 'br', severity: 'success', summary: 'Success', detail: 'Card added!'});
          },
          error: err => {
            this.messageService.add({ key: 'br', severity: 'error', summary: 'Error', detail: err.error });
          }
        })
      }
    });
  }

  removeCardFromDeck(cardID:any) {
    this.confirmationService.confirm({
      message: 'Remove this card from your deck?',
      accept: () => {
        this.deckService.removeCardFromDeck(cardID, this.deckID).subscribe({
          next: res => {
            this.getUsersDeck();
            this.cardCounter();
            this.messageService.add({ key: 'br', severity: 'success', summary: 'Success', detail: 'Card removed!'});
          },
          error: err => {
            console.log(err.error)
            this.messageService.add({ key: 'br', severity: 'error', summary: 'Error', detail: err.error });
          }
        })
      }
    });
  }

}
