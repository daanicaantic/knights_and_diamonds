import { card } from 'src/classes/card-data';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ConfirmationService, MessageService } from 'primeng/api';
import {
  HttpClient,
  HttpEventType,
  HttpErrorResponse,
} from '@angular/common/http';
import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { CardService } from 'src/app/services/card.service';
import {
  BehaviorSubject,
  elementAt,
  Observable,
  Subject,
  Subscription,
} from 'rxjs';

@Component({
  selector: 'app-card-create',
  templateUrl: './card-create.component.html',
  styleUrls: ['./card-create.component.css'],
})
export class CardCreateComponent implements OnInit {
  card = card;
  form!: FormGroup;
  cardTypes: any;
  cardEffects: any;
  pickur: any;
  defaultType: any;
  subscripions: Subscription[] = [];
  isEffectNone = false;
  isSpellOrTrapCard = false;
  imgPath: any;

  constructor(
    private fb: FormBuilder,
    private http: HttpClient,
    private messageService: MessageService,
    private cardService: CardService,
    private confirmationService: ConfirmationService
  ) {}

  ngOnInit(): void {
    this.isEffectNone = true;
    this.isSpellOrTrapCard = false;
    this.getCardTypes();
    this.getEffectTypes();

    this.form = this.fb.group({
      cardName: '',
      cardTypeID: '',
      numOfCardsAffected: 0,
      pointsAddedLost: 0,
      effectTypeID: 10,
      cardLevel: [
        0,
        [Validators.min(0), Validators.max(11), Validators.maxLength(2)],
      ],
      attackPoints: [
        0,
        [Validators.min(0), Validators.max(999), Validators.maxLength(3)],
      ],
      defencePoints: [
        0,
        [Validators.min(0), Validators.max(999), Validators.maxLength(3)],
      ],
      imgPath: this.card.imgPath,
    });
  }

  onCardNameChange(): void {
    this.card.cardName = this.form.value['cardName'];
  }

  onCardTypeChange() {
    let ct = this.cardTypes.find(
      (x: { id: any }) => x.id == this.form.value['cardTypeID']
    );
    this.card.cardType = ct.type;
    if (ct.type == 'MonsterCard') {
      console.log(ct.type);
      this.isSpellOrTrapCard = false;
    } else this.isSpellOrTrapCard = true;
  }

  onCardsAffectedChange() {
    this.card.numberOfCardsAffected = this.form.value['numberOfCardsAffected'];
  }

  onPointsAddedLostChange() {
    this.card.pointsAddedLost = this.form.value['pointsAddedLost'];
  }

  onCardEffectChange() {
    let ce = this.cardEffects.find(
      (x: { id: any }) => x.id == this.form.value['effectTypeID']
    );
    this.card.cardEffectID = ce.type;
    console.log(ce);
    if (ce.type == 'none') {
      this.isEffectNone = true;
    } else this.isEffectNone = false;
  }

  onCardLevelChange() {
    this.card.cardLevel = this.form.value['cardLevel'];
  }

  onAttackChange() {
    this.card.attackPoints = this.form.value['attackPoints'];
  }

  onDefenceChange() {
    this.card.defencePoints = this.form.value['defencePoints'];
  }

  uploadFinished = (event: any) => {
    this.card.imgPath = event.dbPath;
    console.log(event.dbPath);
    this.imgPath = event.dbPath;

    console.log(this.form.getRawValue());
  };

  getCardTypes() {
    this.cardService.getCardTypes().subscribe({
      next: (res) => {
        this.cardTypes = res;
        this.card.cardType = this.cardTypes[0].type;
      },
      error: (err) => {
        console.log('neuspesno ct');
      },
    });
  }

  getEffectTypes() {
    this.cardService.getEffectTypes().subscribe({
      next: (res) => {
        console.log(res);
        this.cardEffects = res;
        console.log(this.cardEffects);
      },
      error: (err) => {
        console.log('neuspesno ce');
      },
    });
  }

  handleClick() {
    let p = this.form.getRawValue();
    p.imgPath = this.imgPath;
    console.log('pikcur', p);

    this.cardService.addCard(p).subscribe({
      next: (res: any) => {
        this.messageService.add({
          key: 'br',
          severity: 'success',
          summary: 'Success',
          detail: p.cardName + ' card added!',
        });
      },
      error: (err: any) => {
        this.messageService.add({
          key: 'br',
          severity: 'error',
          summary: 'Error',
          detail: err.error,
        });
        console.log(err);
      },
    });
  }
}
