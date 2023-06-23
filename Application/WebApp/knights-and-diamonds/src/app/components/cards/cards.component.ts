import {
  AfterViewInit,
  Component,
  ElementRef,
  OnDestroy,
  OnInit,
  ViewChild,
} from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ConfirmationService, MessageService } from 'primeng/api';
import { Subscription } from 'rxjs';
import { AuthService } from 'src/app/services/auth.service';
import { CardService } from 'src/app/services/card.service';
import { DeckService } from 'src/app/services/deck.service';
import { PomType } from 'src/classes/type';

@Component({
  selector: 'app-cards',
  templateUrl: './cards.component.html',
  styleUrls: ['./cards.component.css'],
})
export class CardsComponent implements OnInit, OnDestroy {
  @ViewChild('paginator') paginator: any;
  rightListOFCards: any;
  form!: FormGroup;
  changeCardForm!: FormGroup;
  subscriptions: Subscription[] = [];
  cardTypes: any[] = [];
  bigCard: any;
  startingvalue: any;
  middleListOfCards: any;
  userID = this.authService?.userValue?.id;
  deckID: any;
  stats: any;
  timeout: any;
  setPage: any = 0;
  sortType: any = [
    { name: 'Order by ascending', key: 'orderBy' },
    { name: 'Order by descending ', key: 'orderByDesc' },
  ];
  pageSize: any;
  totalItems: any;
  pageNumber: any;
  cardsPerPage: any;
  user: any;
  readonly: any = true;
  readonlySpell: any = true;

  constructor(
    private cardService: CardService,
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private deckService: DeckService,
    private authService: AuthService,
    private confirmationService: ConfirmationService,
    private messageService: MessageService,
    private router: Router,

  ) {}

  ngOnInit(): void {
    this.user = this.authService?.userValue;
    console.log('pom', this.user);
    this.deckID = this.route.snapshot.params['deckID'];
    if (this.deckID == undefined) {
      this.cardsPerPage = 50;
    } else {
      this.getUsersDeck();
      this.cardsPerPage = 12;
    }
    console.log('ID deck', this.deckID);
    this.getFilteredCards('', '', '', 1);
    this.getCardTypes();
    this.getCardsCounter();

    this.form = this.fb.group({
      cardName: '',
      cardTypeID: 'AllCards',
      cardSorting: 'orderBy',
    });
    this.changeCardForm = this.fb.group({
      id: '',
      cardName: '',
      imgPath: '',
      cardLevel: '',
      attackPoints: '',
      defencePoints: '',
    });
  }

  setData(pageNumber: any) {
    console.log(this.form.getRawValue());
    var cardName = this.form.value['cardName'].toString();
    var cardTypeID = this.form.value['cardTypeID'].toString();
    if (cardTypeID == 'AllCards') {
      cardTypeID = '';
    }
    var cardSorting = this.form.value['cardSorting'].toString();

    this.getFilteredCards(cardTypeID, cardSorting, cardName, pageNumber);
  }
  
  onCardTypeChange() {
    this.setData(1);
    this.paginator.changePage(0);
    console.log(this.setPage);
  }

  onCardSortChange() {
    this.setData(1);
    this.paginator.changePage(0);
  }

  onTextChange(event: any) {
    clearTimeout(this.timeout);
    this.timeout = setTimeout(() => {
      this.setData(1);
      this.paginator.changePage(0);
    }, 500);
  }

  onPageChanged(event: any) {
    const nextPageIndex = event.page + 1;
    event.page = 0;
    console.log('Navigating to page:', nextPageIndex);
    console.log('Navigating to page:', this.pageNumber);

    this.setData(nextPageIndex);
  }
  // getCards() {
  //   this.cardService.getCards().subscribe({
  //     next: (res) => {
  //       this.cards = res;
  //     },
  //     error: (err) => {},
  //   });
  // }
  getFilteredCards(
    typeFilter: any,
    nameFilter: any,
    sortOrder: any,
    pageNumber: any
  ) {
    console.log(typeFilter, nameFilter, sortOrder);
    this.cardService
      .getFilteredCards(
        typeFilter,
        nameFilter,
        sortOrder,
        pageNumber,
        this.cardsPerPage
      )
      .subscribe({
        next: (res: any) => {
          if (this.deckID == undefined) {
            this.middleListOfCards = res.cards;
          } else {
            this.rightListOFCards = res.cards;
          }
          this.pageNumber = res.pageNumber;
          this.pageSize = res.pageSize;
          this.totalItems = res.totalItems;
          console.log(this.middleListOfCards);
        },
        error: (err) => {},
      });
  }

  getCardTypes() {
    this.subscriptions.push(
      this.cardService.getCardTypes().subscribe({
        next: (res: any) => {
          var pom = res;
          pom.push({ id: 0, type: 'AllCards', imgPath: 'string' });
          this.cardTypes = pom;
          // res.forEach((element: any) => {
          //   this.cardTypes.push(element);
          //   console.log(this.cardTypes);
          // });
          // this.startingvalue = 'AllCards';
        },
        error: (err) => {
          console.log('neuspesno ct');
        },
      })
    );
  }

  uploadFinished = (event: any) => {
    console.log(event.dbPath);
    this.changeCardForm.patchValue({
      imgPath: event.dbPath,
    });
    console.log(this.changeCardForm.getRawValue());
  };

  getUsersDeck() {
    this.subscriptions.push(
      this.deckService.getUsersDeck(this.userID).subscribe({
        next: (res) => {
          console.log(res);
          this.middleListOfCards = res;
        },
        error: (err) => {
          console.log('neuspesno getUsersDeck');
        },
      })
    );
  }

  deckCounter() {
    this.subscriptions.push(
      this.deckService.cardCounter(this.deckID, this.userID).subscribe({
        next: (res) => {
          this.stats = res;
        },
        error: (err) => {
          console.log('neuspesno deckCounter');
        },
      })
    );
  }

  cardCounter() {
    this.subscriptions.push(
      this.cardService.cardCount().subscribe({
        next: (res) => {
          this.stats = res;
        },
        error: (err) => {
          console.log('neuspesno cardCounter');
        },
      })
    );
  }

  getCardsCounter() {
    if(this.deckID == undefined)
      this.cardCounter();
    else
      this.deckCounter();
  }

  addCardToDeck(cardID: any) {
    this.confirmationService.confirm({
      message: 'Add this card to your deck?',
      accept: () => {
        this.subscriptions.push(
          this.deckService.addCardToDeck(cardID, this.deckID).subscribe({
            next: (res) => {
              this.getUsersDeck();
              this.getCardsCounter();
              this.messageService.add({
                key: 'br',
                severity: 'success',
                summary: 'Success',
                detail: 'Card added!',
              });
            },
            error: (err) => {
              this.messageService.add({
                key: 'br',
                severity: 'error',
                summary: 'Error',
                detail: err.error,
              });
            },
          })
        );
      },
    });
  }

  onMidleCardsClick(card: any) {
    if (this.deckID != undefined) {
      this.removeCardFromDeck(card.id);
    }
    if (this.user != undefined) {
      if (this.user.role == 'Admin') {

        this.setFormData(card);
        this.changeReadonlyParrametar(card);
      }
    }
  }

  changeReadonlyParrametar(card: any) {
    if (this.changeCardForm.value['id'] == '') {
      this.readonly = true;
      this.readonlySpell = true;
    } else {
      if (card.cardType == 'MonsterCard') {
        this.readonlySpell = false;
      } else {
        this.readonlySpell = true;
      }
      this.readonly = false;
    }
  }

  setFormData(card: any) {
    this.changeCardForm.setValue({
      id: card.id,
      cardName: card.cardName,
      cardLevel: card.cardLevel,
      imgPath: card.imgPath,
      attackPoints: card.attackPoints,
      defencePoints: card.defencePoints,
    });
    console.log(this.changeCardForm.getRawValue());
  }

  updateCard() {
    this.subscriptions.push(
      this.cardService.updateCard(this.changeCardForm.getRawValue()).subscribe({
        next: (res: any) => {
          this.setData(1);
          this.changeCardForm.setValue({
            id: '',
            cardName: '',
            cardLevel: '',
            imgPath: '',
            attackPoints: '',
            defencePoints: '',
          });
          this.changeReadonlyParrametar(null);
        },
        error: (err: any) => {},
      })
    );
  }

  deleteCard() {
    var id=this.changeCardForm.value['id'];
    if(id!=''){
      this.subscriptions.push(
        this.cardService.deleteCard(id).subscribe({
          next: (res: any) => {
            this.setData(1);
            this.changeCardForm.setValue({
              id: '',
              cardName: '',
              cardLevel: '',
              imgPath: '',
              attackPoints: '',
              defencePoints: '',
            });
            this.changeReadonlyParrametar(null);
            this.getCardsCounter();
            this.bigCard = undefined;
          },
          error: (err: any) => {},
        })
      );
    }
  }
  
  removeCardFromDeck(cardID: any) {
    this.confirmationService.confirm({
      message: 'Remove this card from your deck?',
      accept: () => {
        this.subscriptions.push(
          this.deckService.removeCardFromDeck(cardID, this.deckID).subscribe({
            next: (res) => {
              this.getUsersDeck();
              this.getCardsCounter();
              this.messageService.add({
                key: 'br',
                severity: 'success',
                summary: 'Success',
                detail: 'Card removed!',
              });
            },
            error: (err) => {
              console.log(err.error);
              this.messageService.add({
                key: 'br',
                severity: 'error',
                summary: 'Error',
                detail: err.error,
              });
            },
          })
        )
      },
    });
  }
  goToHome() {
    this.router.navigate(['/home']);
  }
  ngOnDestroy(): void {
    this.subscriptions.forEach(subscription => subscription.unsubscribe());
  }
}
