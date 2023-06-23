import { Component, Input, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ConfirmationService, MessageService } from 'primeng/api';
import { Subscription } from 'rxjs';
import { AuthService } from 'src/app/services/auth.service';
import { CardService } from 'src/app/services/card.service';
import { DeckService } from 'src/app/services/deck.service';
import { card } from 'src/classes/card-data';
import { CardType } from 'src/classes/card-type';
import { PomType } from 'src/classes/type';

@Component({
  selector: 'app-deck-create',
  templateUrl: './deck-create.component.html',
  styleUrls: ['./deck-create.component.css'],
})
export class DeckCreateComponent implements OnInit, OnDestroy {
  @ViewChild('paginator') paginator: any;
  card = card;
  cards: any;
  form!: FormGroup;
  cardTypes: any[] = [];
  bigCard: any;
  startingvalue: any;
  cardsInDeck: any;
  userID = this.authService?.userValue?.id;
  deckID: any;
  stats: any;
  timeout: any;
  setPage: any = 0;
  sortType: any = [
    { name: 'Order by ascending', key: 'orderBy' },
    { name: 'Order by descending ', key: 'orderByDesc' },
  ];
  subscriptions: Subscription[] = [];
  
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
    this.deckID = this.route.snapshot.params['deckID'];
    console.log('ID deck', this.deckID);
    this.getFilteredCards('', '', '', 1);
    this.getCardTypes();
    this.getUsersDeck();
    this.cardCounter();

    this.form = this.fb.group({
      cardName: '',
      cardTypeID: 'AllCards',
      cardSorting: 'orderBy',
    });
  }

  goToHome() {
    this.router.navigate(['/home']);
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
    console.log('Navigating to page:', this.cards.pageNumber);

    this.setData(nextPageIndex);
  }
  getCards() {
    this.subscriptions.push(
      this.cardService.getCards().subscribe({
        next: (res) => {
          this.cards = res;
        },
        error: (err) => {},
      })
    );
  }

  getFilteredCards(
    typeFilter: any,
    nameFilter: any,
    sortOrder: any,
    pageNumber: any
  ) {
    console.log(typeFilter, nameFilter, sortOrder);
    this.subscriptions.push(
      this.cardService
        .getFilteredCards(typeFilter, nameFilter, sortOrder, pageNumber, 12)
        .subscribe({
          next: (res) => {
            this.cards = res;
          },
          error: (err) => {},
        })
    );
  }

  getCardTypes() {
    this.subscriptions.push(
      this.cardService.getCardTypes().subscribe({
        next: (res: any) => {
          this.cardTypes.push(new PomType('', 'AllCards'));
          res.forEach((element: any) => {
            this.cardTypes.push(element);
            console.log(this.cardTypes);
          });
          this.startingvalue = 0;
        },
        error: (err) => {
          console.log('neuspesno ct');
        },
      })
    );
  }

  getUsersDeck() {
    this.subscriptions.push(
      this.deckService.getUsersDeck(this.userID).subscribe({
        next: (res) => {
          console.log(res);
          this.cardsInDeck = res;
        },
        error: (err) => {
          console.log('neuspesno getUsersDeck');
        },
      })
    );
  }

  cardCounter() {
    this.subscriptions.push(
      this.deckService.cardCounter(this.deckID, this.userID).subscribe({
        next: (res) => {
          this.stats = res;
        },
        error: (err) => {
          console.log('neuspesno cardCounter');
        },
      })
    );
  }

  addCardToDeck(cardID: any) {
    this.confirmationService.confirm({
      message: 'Add this card to your deck?',
      accept: () => {
        this.subscriptions.push(
          this.deckService.addCardToDeck(cardID, this.deckID).subscribe({
            next: (res) => {
              this.getUsersDeck();
              this.cardCounter();
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

  removeCardFromDeck(cardID: any) {
    this.confirmationService.confirm({
      message: 'Remove this card from your deck?',
      accept: () => {
        this.subscriptions.push(
          this.deckService.removeCardFromDeck(cardID, this.deckID).subscribe({
            next: (res) => {
              this.getUsersDeck();
              this.cardCounter();
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
        );
      },
    });
  }

  ngOnDestroy(): void {
    this.subscriptions.forEach(subscription => subscription.unsubscribe());
  }
}
