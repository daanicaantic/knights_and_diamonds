import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
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

  constructor(private cardService: CardService,
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private deckService: DeckService,
    private authService: AuthService) {
    }

  ngOnInit(): void {
    this.userID = this.route.snapshot.params['userID']
    console.log('ID usera', this.userID)
    this.getCards();
    this.getCardTypes();
    this.getUsersDeck(this.userID);

    this.form = this.fb.group({
      cardName: "",
      cardTypeID: 0,
    })
  }

  onCardTypeChange(){
    console.log(this.form.getRawValue())
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

  getUsersDeck(userID:any) {
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

}
