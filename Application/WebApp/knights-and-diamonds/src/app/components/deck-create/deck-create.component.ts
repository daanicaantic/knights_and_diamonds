import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { CardService } from 'src/app/services/card.service';
import { card } from 'src/classes/card-data';

@Component({
  selector: 'app-deck-create',
  templateUrl: './deck-create.component.html',
  styleUrls: ['./deck-create.component.css']
})
export class DeckCreateComponent implements OnInit {
  card = card;
  cards: any;
  form!: FormGroup;
  cardTypes: any;
  
  constructor(private cardService: CardService,
    private fb: FormBuilder) { }

  ngOnInit(): void {
    this.getCards();
    this.getCardTypes();

    this.form = this.fb.group({
      cardName: "",
      cardTypeID:"",
      // numOfCardsAffected: 0,
      // pointsAddedLost: 0,
      // effectTypeID: 10,
      // cardLevel: [0, [Validators.min(0), Validators.max(11), Validators.maxLength(2)]],
      // attackPoints: [0, [Validators.min(0), Validators.max(999), Validators.maxLength(3)]],
      // defencePoints: [0, [Validators.min(0), Validators.max(999), Validators.maxLength(3)]],
      // imgPath: this.card.imgPath,
    })
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

  onCardTypeChange() {
    let ct = this.cardTypes.find((x: { id: any; }) => x.id == this.form.value["cardTypeID"]);
    this.card.cardType = ct.type;
  }

  onCardNameChange() {

  }

  getCardTypes() {
    this.cardService.getCardTypes().subscribe({
      next: res => {
        this.cardTypes = res;
        this.card.cardType=this.cardTypes[0].type;
      },
      error: err => {
        console.log("neuspesno ct")
      }
    })
  }

}
