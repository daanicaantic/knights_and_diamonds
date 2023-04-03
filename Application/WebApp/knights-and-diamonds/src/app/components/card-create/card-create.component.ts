import { card } from 'src/classes/card-data';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Card } from 'src/classes/card';
import { STCard } from 'src/classes/stcard';
import { CardType } from 'src/classes/card-type';
import { ConfirmationService, MessageService } from 'primeng/api';
import { Message } from 'primeng//api';
import { HttpClient, HttpEventType, HttpErrorResponse } from '@angular/common/http';
import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { CardService } from 'src/app/services/card.service';
import { BehaviorSubject, elementAt, Observable, Subject, Subscription } from 'rxjs';


@Component({
  selector: 'app-card-create',
  templateUrl: './card-create.component.html',
  styleUrls: ['./card-create.component.css']
})
export class CardCreateComponent implements OnInit {
  card = card;
  form!: FormGroup;
  cardTypes: any;
  cardEffects: any;
  pickur: any;
  // @Output() public onUploadFinished = new EventEmitter();
  subscripions: Subscription[] = [];
  isEffectNone = false;
  isSpellOrTrapCard = false;

  constructor(private fb: FormBuilder,
    private http: HttpClient,
    private messageService: MessageService,
    private cardService: CardService,
    private confirmationService: ConfirmationService) {
  }

  ngOnInit(): void {
    this.isEffectNone = true;
    this.isSpellOrTrapCard = false;

    this.getCardTypes();
    this.getEffectTypes();

    // this.card.cardType=this.cardTypes.type;
    console.log("ovdeeeee", this.card)

    this.form = this.fb.group({
      cardName: "",
      cardTypeID: 3,
      numOfCardsAffected: 0,
      pointsAddedLost: 0,
      effectTypeID: 10,
      cardLevel: [0, [Validators.min(0), Validators.max(11), Validators.maxLength(2)]],
      attackPoints: [0, [Validators.min(0), Validators.max(999), Validators.maxLength(3)]],
      defencePoints: [0, [Validators.min(0), Validators.max(999), Validators.maxLength(3)]],
      imgPath: "",
    })
  }

  onCardNameChange(): void {
    this.card.cardName = this.form.value["cardName"];
  }

  onCardTypeChange() {
    let ct = this.cardTypes.find((x: { id: any; }) => x.id == this.form.value["cardTypeID"]);
    this.card.cardType = ct.type;
    if(ct.type == "MonsterCard"){
      console.log(ct.type)
      this.isSpellOrTrapCard = false;
    }
    else this.isSpellOrTrapCard = true;
  }
  
  onCardsAffectedChange() {
    this.card.numberOfCardsAffected = this.form.value["numberOfCardsAffected"];
  }

  onPointsAddedLostChange() {
    this.card.pointsAddedLost = this.form.value["pointsAddedLost"];
  }

  onCardEffectChange() {
    let ce = this.cardEffects.find((x: { id: any; }) => x.id == this.form.value["effectTypeID"]);
    this.card.cardEffect = ce.type;
    console.log(ce)
    if(ce.type == "none") {
      this.isEffectNone = true;
    }
    else this.isEffectNone = false;
  }

  onCardLevelChange() {
    this.card.cardLevel = this.form.value["cardLevel"];
  }

  onAttackChange() {
    this.card.attackPoints = this.form.value["attackPoints"]
  }

  onDefenceChange() {
    this.card.defencePoints = this.form.value["defencePoints"]
  }

  uploadFinished = (event: any) => {
    this.card.imgPath = event.dbPath;
    console.log(event.dbPath)
    this.form.value["imgPath"] = event.dbPath;
  }

  getCardTypes() {
    this.cardService.getCardTypes().subscribe({
      next: res => {
        console.log(res);
        this.cardTypes = res;
        console.log(this.cardTypes)
      },
      error: err => {
        console.log("neuspesno ct")
      }
    })
  }

  getEffectTypes() {
    this.cardService.getEffectTypes().subscribe({
      next: res => {
        console.log(res);
        this.cardEffects = res;
        console.log(this.cardEffects)
      },
      error: err => {
        console.log("neuspesno ce")
      }
    })
  }

  handleClick() {
    let p = this.form.getRawValue()
    p.imgPath = this.form.value["imgPath"]

    this.cardService.addCard(p).subscribe({
      next: (res: any) => {
        this.messageService.add({key: 'br', severity:'success', summary: 'Success', detail: p.cardName + ' card added!'});
      },
      error: (err: any) => {
        this.messageService.add({key: 'br', severity:'error', summary: 'Error', detail: err.error});
        console.log(err)
      }
    })

    // if (p.cardType !== 3) {
    //   this.card.cardName = this.form.value["cardName"];
    //   this.card.cardType = this.form.value["cardTypeID"];
    //   this.card.imgPath = this.form.value["imgPath"];

    //   this.cardService.addCard(this.stcard).subscribe({
    //     next: (res: any) => {
    //       console.log(res);
    //       this.monsterTypes = res;
    //     },
    //     error: (err: any) => {
    //       console.log(err)
    //     }
    //   })

    //   console.log("ovdeeeeeeeeeeeeeeeeeeeeeee", this.stcard)
    // }
    // else {
    //   p.imgPath = this.form.value["imgPath"]
    //   console.log(p)
    //   this.cardService.addCard(p).subscribe({
    //     next: (res: any) => {
    //       console.log(res);
    //       this.monsterTypes = res;
    //     },
    //     error: (err: any) => {
    //       console.log(err)
    //     }
    //   })
    // }
  }
}
