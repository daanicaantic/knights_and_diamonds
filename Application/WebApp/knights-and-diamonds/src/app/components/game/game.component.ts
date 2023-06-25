import {
  AfterViewChecked,
  AfterViewInit,
  Component,
  ElementRef,
  OnDestroy,
  OnInit,
  ViewChild,
} from '@angular/core';
import { card } from 'src/classes/card-data';
import { IngameService } from 'src/app/services/ingame.service';
import { GameService } from 'src/app/services/game.service';
import { AuthService } from 'src/app/services/auth.service';
import { ActivatedRoute, Router } from '@angular/router';
import { SignalrService } from 'src/app/services/signalr.service';
import { TmplAstRecursiveVisitor } from '@angular/compiler';
import { DOCUMENT } from '@angular/common';
import { Subscription } from 'rxjs';
import { ConfirmationService } from 'primeng/api';

@Component({
  selector: 'app-game',
  templateUrl: './game.component.html',
  styleUrls: ['./game.component.css'],
})
export class GameComponent implements OnInit, OnDestroy {
  @ViewChild('enemiesLpChange', { static: false })
  enemiesLpChange!: ElementRef;
  @ViewChild('playersLpChange', { static: false })
  playersLpChange!: ElementRef;
  @ViewChild('activateTrapCard', { static: false })
  activateTrapCard!: ElementRef;
  @ViewChild('pom', { static: false })
  winnerMessageDiv!: ElementRef;

  userID = this.authService?.userValue?.id;
  playerID = this.authService?.userValue?.player;
  enemiesID = this.authService?.userValue?.enemie;
  curentPhase: any;
  isPlayerOnTurn: any;

  playerField: any;
  enemiesField: any;
  grave: any;
  card = card;
  // numberOfCardsInEnemiesHand:Number = 6;
  isLoadingOver = false;
  loadingType = 'game';
  gameID: any;
  playerHand: any[] = [];
  enemiesHand: any[] = [];
  drawingTimer: any;
  mediumCard = card;
  graveCard: any;
  iscardshowen = false;
  phases: any[] = [
    { name: 'DP', key: 0, status: false },
    { name: 'MP', key: 1, status: false },
    { name: 'BP', key: 2, status: false },
    { name: 'EP', key: 3, status: false },
  ];
  cardToBeSummoned: any;
  fieldsAbleToAttack: any;
  swordRadyToAttack: any;
  angle: any;
  transitionIsActive: any = false;
  turnInfo: any;
  fieldReadyAttack: any;
  fieldThatLostPoints: any;
  pointsLost: any;
  phaseMessage: any;
  isPhaseMassageOver: any = false;
  areaOfClicking: any;
  isEnemiesHandShown: any = false;
  doYouNeedToWaitEnemiesChoice: any = false;
  fieldsThatCanActivateTrapCard: any;
  canYouActivateTrapCard: any;
  doYouWantToActivateTrapCard: any;
  cardActivated: any;
  textLeft = 0;
  textTop = 0;
  lastSummonedMonster: any;
  listOfIds: any = [0];
  subscriptions: Subscription[] = [];
  winnerMessage: any;
  didYouWin: any;
  listOfFiledThatYouCanChangePosition: any[] = [];
  onCardOnFieldMouseOver: any;
  graveStats: any;
  isGraveShown:any=false;
  curentFilter: any;
  isExitGraveShown:any=true;
  constructor(
    public inGameService: IngameService,
    public gameService: GameService,
    private router: Router,
    private route: ActivatedRoute,
    private authService: AuthService,
    private signalrService: SignalrService,
    private confirmationService: ConfirmationService
  ) {}

  ngOnInit(): void {
    console.log(this.authService.userValue);
    this.gameID = this.route.snapshot.params['gameID'];
    this.getPlayersField();
    this.getEnemiesField();
    this.getStartingField();
    this.getGrave();
    this.getHands();
    this.getTurnInfo();
    this.getDataAffterPlayedEffectCard();
    this.getGraveFromHub();
    this.getFieldsAbleToAttack();
    this.getFieldsIncludedInAttack();
    this.getFieldThatLostPoints();
    this.getFieldsIDsThatCanActivateTrapCard();
    this.getFieldThatMonsterIsSummonedOn();
    this.getIfYouCanContinuePlaying();
    this.getWinner();
    // this.getStartingDrawing();
    if (this.playerID != undefined) {
      if (this.signalrService.hubConnection.state == 'Connected') {
        this.gameService.getTurnInfoInv(Number(this.gameID), this.playerID);
      } else {
        this.subscriptions.push(
          this.signalrService.ssSubj.subscribe((obj: any) => {
            if (obj.type == 'HubConnStarted') {
              this.gameService.getTurnInfoInv(
                Number(this.gameID),
                this.playerID
              );
            }
          })
        );
      }
    }
  }
  // setGameStatus() {
  //   this.inGameService.setGameOn();
  // }
  onLoadingOver(event: any) {
    this.isLoadingOver = event;
    if (
      this.playerField != undefined &&
      this.playerField.gameStarted == false
    ) {
      this.gameService.startingDrawingInv(Number(this.gameID), this.playerID);
      if (this.isPlayerOnTurn) {
        this.gameService.drawPhaseInv(Number(this.gameID), this.playerID);
      }
    }
    this.getStartingTurnInfo();
  }
  //koristimo fju prilikom ngOnInita vraca ceo prikaz table
  getStartingField() {
    this.subscriptions.push(
      this.gameService
        .getStartingField(this.playerID, this.enemiesID)
        .subscribe({
          next: (res: any) => {
            console.log(res);
            this.playerField = res.playerField;
            this.enemiesField = res.enemiesField;
            this.playerHand = res.playerField.hand;
            this.enemiesHand = res.enemiesField.hand;
          },
          error: (err) => {
            console.log(err.error);
          },
        })
    );
  }
  //players field is huba
  getPlayersField() {
    this.signalrService.hubConnection.on('GetYourField', (field: any) => {
      if (field.lifePoints <= 0) {
        field.lifePoints = 0;
      }
      this.getPointsReduce(
        this.playersLpChange,
        this.playerField.lifePoints,
        field.lifePoints
      );
      this.playerField = field;
      this.playerHand = field.hand;
      console.log(this.playerField);
    });
  }

  //enemies field iz huba
  getEnemiesField() {
    this.signalrService.hubConnection.on(
      'GetEnemiesField',
      (enemiesField: any) => {
        if (enemiesField.lifePoints <= 0) {
          enemiesField.lifePoints = 0;
        }
        this.getPointsReduce(
          this.enemiesLpChange,
          this.enemiesField.lifePoints,
          enemiesField.lifePoints
        );
        this.enemiesField = enemiesField;
        this.enemiesHand = enemiesField.hand;
        console.log(this.enemiesField);
        if (this.enemiesField.lifePoints == 0 || this.playerField == 0) {
          this.gameService.getWinnerInv(this.gameID, this.playerID);
        }
      }
    );
  }

  getWinner() {
    this.signalrService.hubConnection.on(
      'WinnerMessage',
      (WinnerMessage: any) => {
        console.log(WinnerMessage);
        if (WinnerMessage == 'You Won') {
          this.didYouWin = true;
        } else {
          this.didYouWin = false;
        }
        this.winnerMessage = WinnerMessage;
        console.log(this.winnerMessage);
        setTimeout(() => {
          this.router.navigate(['/home']);
        }, 2000);
      }
    );
  }

  //ruke iz huba
  getHands() {
    this.signalrService.hubConnection.on(
      'GetCardsInYourHand',
      (playerHand: any) => {
        if (this.playerField.gameStarted == false) {
          this.getStartingDrawingEffect(0, this.playerHand, playerHand);
        } else {
          this.playerHand = playerHand;
        }
      }
    );
    this.signalrService.hubConnection.on(
      'GetCardsInEnemiesHand',
      (enemiesHand: any) => {
        if (this.playerField.gameStarted == false) {
          this.getStartingDrawingEffect(0, this.enemiesHand, enemiesHand);
        } else {
          this.enemiesHand = enemiesHand;
        }
      }
    );
  }

  //koristimo fju za prikaz pocetnog izvlacenja karata;targetArray=nasa ili protivnikova ruka,souceArray=nova ruka stigla sa servera
  getStartingDrawingEffect(i: any, targetArray: any, sourceArray: any) {
    var timer = setInterval(() => {
      if (i == sourceArray.length) {
        clearInterval(timer);
        return;
      }
      const exists = targetArray.some(
        (obj: any) => obj.id === sourceArray[i].id
      );
      if (!exists) {
        targetArray.push(sourceArray[i]);
      }
      i++;
    }, 300);
  }
  //grave iz controllera
  getGrave() {
    this.subscriptions.push(
      this.gameService.getGrave(this.gameID).subscribe({
        next: (res: any) => {
          this.grave = res;
        },
        error: (err) => {
          console.log(err.error);
        },
      })
    );
  }
  //grave iz huba
  getGraveFromHub() {
    this.signalrService.hubConnection.on('GetGraveData', (grave: any) => {
      this.grave = grave;
    });
  }

  //turnInfoIz contrlera
  getStartingTurnInfo() {
    this.subscriptions.push(
      this.gameService.getTurnInfo(this.gameID, this.playerID).subscribe({
        next: (res: any) => {
          this.setTurnInfo(res);
        },
        error: (err) => {
          console.log(err.error);
        },
      })
    );
  }
  //turninfo iz huba
  getTurnInfo() {
    this.signalrService.hubConnection.on('GetTurnInfo', (turnInfo: any) => {
      console.log(turnInfo);
      this.turnInfo = turnInfo;
      this.setTurnInfo(turnInfo);
      this.isPhaseMassageOver = false;
      if (this.turnInfo.turnPhase == 0) {
        this.phaseMessage = 'Draw Phase';
      } else if (this.turnInfo.turnPhase == 1) {
        this.phaseMessage = 'Main Phase';
        console.log('grejala pa gradila');
        this.listOfFiledThatYouCanChangePosition = [];
        this.fieldsThatCanChangePosition();
      } else if (this.turnInfo.turnPhase == 2) {
        this.phaseMessage = 'Battle Phase';
      } else if (this.turnInfo.turnPhase == 3) {
        this.phaseMessage = 'End Phase';
      } else {
        this.phaseMessage = undefined;
      }
      if (this.turnInfo.turnPhase == 1 || this.turnInfo.turnPhase == 2) {
        setTimeout(() => {
          this.phaseMessage = undefined;
          if (this.turnInfo.turnPhase == 1) {
            this.isPhaseMassageOver = true;
          }
        }, 1000);
      }
    });
  }
  //setujemo trenutnu fazu,i menjamo prethodnu
  setTurnInfo(turnInfo: any) {
    this.curentPhase = turnInfo.turnPhase;
    if (turnInfo.playerOnTurn == this.playerID) {
      this.isPlayerOnTurn = true;
    } else {
      this.isPlayerOnTurn = false;
    }
    this.phases.forEach((element) => {
      element.status = false;
    });
    //!=4 potrebno samo kod prvog poteza znaci da jos ne postoji potez
    if (turnInfo.turnPhase != 4) {
      let phase = this.phases.find((x) => x.key == turnInfo.turnPhase);
      phase.status = true;
      this.curentPhase = phase;
    }
  }
  fieldsThatCanChangePosition() {
    if (this.playerField != undefined) {
      this.playerField.cardFields.forEach((element: any) => {
        if (element.cardOnField != null) {
          if (element.fieldIndex < 5) {
            var exists = this.listOfFiledThatYouCanChangePosition.some(
              (el) => el == element.fieldID
            );
            console.log(exists);
            if (!exists) {
              this.listOfFiledThatYouCanChangePosition.push(element.fieldID);
            }
            console.log('ovdeeeeee', this.listOfFiledThatYouCanChangePosition);
          }
        }
      });
    }
  }
  //menjanje faze
  changePhaseInv(phase: any) {
    //moras prvo izbaciti kartu iz ruke
    if (this.playerHand.length < 7) {
      if (phase.name == 'BP' && this.curentPhase.name == 'MP') {
        this.gameService.battlePhaseInv(this.gameID, this.playerID);
      } else if (
        (phase.name == 'EP' && this.curentPhase.name == 'MP') ||
        this.curentPhase.name == 'BP'
      ) {
        this.gameService.endPhaseInv(
          this.gameID,
          this.playerID,
          this.enemiesID
        );
      }
    }
  }
  //click na nasu ruku
  onPlayersHandClick(card: any) {
    if (this.playerHand.length < 7) {
      if (
        card.cardType == 'MonsterCard' &&
        this.turnInfo.isMonsterSummoned == false &&
        this.curentPhase.name == 'MP' &&
        this.areaOfClicking ==undefined

      ) {
        this.openSummoningMonsterWindow(card);
      }
      if (
        (card.cardType == 'SpellCard' || card.cardType == 'TrapCard') &&
        this.curentPhase.name == 'MP' &&
        this.isPlayerOnTurn &&
        this.areaOfClicking ==undefined
        
      ) {
        this.playSpellTrapCard(card.id, card.cardEffectID, card.cardType);
      }
    } else if (this.isPlayerOnTurn) {
      this.gameService.removeCardFromHandToGraveInv(
        this.playerID,
        card.id,
        this.gameID
      );
    }
  }
  onEnemiesHandClick(card: any) {
    if (this.areaOfClicking != undefined && this.isEnemiesHandShown == true) {
      console.log(this.areaOfClicking);
      if (this.areaOfClicking.areaOfClicking == 5) {
        this.gameService.executeEffectInv(
          [card.id],
          this.areaOfClicking.fieldID,
          this.playerID,
          this.enemiesID,
          Number(this.gameID)
        );
        this.areaOfClicking = undefined;
        this.isEnemiesHandShown = false;
      }
    }
  }
  onEnemiesCardMouseOver(field: any) {
    this.mediumCard = field.cardOnField;
    if (field.cardShowen == true) {
      this.iscardshowen = true;
    } else {
      this.iscardshowen = false;
    }
  }
  onPlayersFieldMouseOver(field: any) {
    this.mediumCard = field.cardOnField;
    this.iscardshowen = true;
    if (
      this.fieldsThatCanActivateTrapCard != undefined &&
      this.doYouWantToActivateTrapCard == true
    ) {
      if (this.fieldsThatCanActivateTrapCard.indexOf(field.fieldID) >= 0) {
        this.onCardOnFieldMouseOver = 'Activate';
        this.cardActivated = true;
      }
    } else if (
      this.listOfFiledThatYouCanChangePosition != undefined &&
      this.isPlayerOnTurn &&
      this.curentPhase.name == 'MP'
    ) {
      if (
        this.listOfFiledThatYouCanChangePosition.indexOf(field.fieldID) >= 0
      ) {
        this.onCardOnFieldMouseOver = 'ChangePosition';
        this.cardActivated = true;
      }
    }
  }
  updateTextPosition(event: MouseEvent) {
    this.textLeft = event.pageX;
    this.textTop = event.pageY;
  }
  onPlayersFieldClick(field: any) {
    console.log(field);
    if (field.cardOnField != undefined) {
      if (field.cardOnField.cardType == 'TrapCard') {
        if (this.cardActivated) {
          this.canYouActivateTrapCard = false;
          this.fieldsThatCanActivateTrapCard = undefined;
          this.doYouWantToActivateTrapCard = undefined;
          this.listOfIds = [this.lastSummonedMonster];
          this.cardActivated = false;
          this.gameService.activateTrapCard(
            this.gameID,
            this.playerID,
            field.fieldID
          );
          this.gameService.didTrapEffectExecutedInv(this.gameID, this.playerID);
        }
      } else if (
        field.cardOnField.cardType == 'MonsterCard' &&
        this.curentPhase.name == 'MP' &&
        this.isPlayerOnTurn
      ) {
        if (this.listOfFiledThatYouCanChangePosition.includes(field.fieldID)) {
          this.gameService.changeMonsterPositionInv(
            this.playerID,
            field.fieldID,
            this.gameID
          );
          const index = this.listOfFiledThatYouCanChangePosition.findIndex(
            (item) => item === field.fieldID
          );
          this.listOfFiledThatYouCanChangePosition.splice(index, 1);
          this.cardActivated = false;
        }
      }
    }
  }
  //click na protivnicko polje
  onEnemiesFieldClick(field: any) {
    if (
      this.isPlayerOnTurn == true &&
      this.curentPhase.name == 'BP' &&
      this.fieldReadyAttack != undefined
    ) {
      console.log(field);
      this.attackField(field);
    }
    if (this.areaOfClicking != undefined && field.cardOnField != undefined) {
      console.log(this.areaOfClicking);
      if (this.areaOfClicking.areaOfClicking == 4) {
        this.gameService.executeEffectInv(
          [field.fieldID],
          this.areaOfClicking.fieldID,
          this.playerID,
          this.enemiesID,
          Number(this.gameID)
        );
        this.areaOfClicking = undefined;
      }
    }
  }
  openSummoningMonsterWindow(card: any) {
    if (this.cardToBeSummoned == undefined && this.isPlayerOnTurn == true) {
      this.cardToBeSummoned = card;
    }
  }
  closeSummoningMonsterWindow() {
    this.cardToBeSummoned = undefined;
    this.turnInfo.isMonsterSummoned = false;
  }
  summonMonsterInAttackPosition() {
    this.normalSummon(this.cardToBeSummoned.id, true);
  }
  summonMonsterInDeffencePosition() {
    this.normalSummon(this.cardToBeSummoned.id, false);
  }
  //pozivanje cudovista
  normalSummon(cardID: any, position: any) {
    console.log(cardID);
    this.gameService.normalSummonInv(
      Number(this.gameID),
      this.playerID,
      cardID,
      position
    );
    this.cardToBeSummoned = undefined;
    this.turnInfo.isMonsterSummoned = true;
  }
  //igranje spell karte
  playSpellTrapCard(cardID: any, cardEffectID: any, cardType: any) {
    this.gameService.playSpellTrapCardInv(
      Number(this.gameID),
      this.playerID,
      cardID,
      cardEffectID,
      cardType
    );
  }
  //afterData -areaaOfClicking-gte treba da kliknes,i fieldID-polje na kome je karta odigrana
  getDataAffterPlayedEffectCard() {
    this.signalrService.hubConnection.on(
      'GetAreaOfClicking',
      (affterData: any) => {
        console.log('AREAOFCLICKING', affterData);
        this.setAfterData(affterData);
      }
    );
  }

  setAfterData(affterData: any) {
    this.areaOfClicking = affterData;
    //ako je 0 znaci da ne treba nigde da kliknemo
    //[0] na beken za parametar CardIDs jer ne treba nista;
    if (affterData.areaOfClicking == 0) {
      this.gameService.executeEffectInv(
        this.listOfIds,
        affterData.fieldID,
        this.playerID,
        this.enemiesID,
        Number(this.gameID)
      );
      this.areaOfClicking = undefined;
      this.listOfIds = [0];
    }
    this.chackAreaOfCliciking();
    if (this.areaOfClicking.areaOfClicking == 5) {
      this.isEnemiesHandShown = true;
    }
  }
  //nakon ulaska u bp dobijamo koja polja mogu da napadnu u ovom potezu,tj pojavlju se macevi
  getFieldsAbleToAttack() {
    this.signalrService.hubConnection.on(
      'GetFieldsAbleToAttack',
      (listOFfieldsIDs: any) => {
        this.fieldsAbleToAttack = listOFfieldsIDs;
      }
    );
  }

  //dobijamo centar elementa
  getCenter(element: any) {
    const { left, top, width, height } = element.getBoundingClientRect();
    return { x: left + width / 2, y: top + height / 2 };
  }
  //biramo mac tj polje tj kartu kojom zelimo da napadnemo
  chooseSwordForAttack(swordDiv: any, fieldID: any) {
    console.log(swordDiv);
    this.fieldReadyAttack = fieldID; //setujemo id polja koje smo kliknuli
    this.swordRadyToAttack = swordDiv;
    if (this.swordRadyToAttack != undefined) {
      const arrowCenter = this.getCenter(this.swordRadyToAttack);
      addEventListener('mousemove', ({ clientX, clientY }) => {
        if (this.transitionIsActive == false) {
          const angle = Math.atan2(
            clientY - arrowCenter.y,
            clientX - arrowCenter.x
          );
          this.swordRadyToAttack.style.transform = `rotate(${angle - 30}rad)`;
          this.angle = angle;
        }
      });
    }
  }
  //nakon sto smo izabrali mac biramo polje koje zelimo da napadnemo
  attackField(field: any) {
    var canYouAttackThisField: any = false;
    var counter = this.checkNumberOfMonstersOnField(
      this.enemiesField.cardFields
    );
    field.cardShowen = true;
    //klikcemo na spellTrap polje tj napadamo direktno
    if (counter == 0 && field.fieldIndex > 5) {
      canYouAttackThisField = true;
    }
    //klikcemo na monsterField znaci napadamo kartu
    else if (
      counter > 0 &&
      field.fieldIndex <= 5 &&
      field.cardOnField != null
    ) {
      canYouAttackThisField = true;
    }
    if (canYouAttackThisField == true) {
      this.gameService.attackEnemiesFieldInv(
        Number(this.gameID),
        this.playerID,
        this.fieldReadyAttack,
        field.fieldID,
        this.enemiesID
      );
    }
  }
  //sa bekenda dobijamo polje sa koga smo napali i polje koje smo napali
  getFieldsIncludedInAttack() {
    this.signalrService.hubConnection.on(
      'GetFieldsIncludedInAttack',
      (fieldID: any, attackedFieldID: any) => {
        this.getSwordsTranslation(fieldID, attackedFieldID);
      }
    );
  }

  //izracunavaju se centri napadackog polja i napadnutog,i ugao pod kojim mac treba da leti
  getSwordsTranslation(fieldID: any, attackedFieldID: any) {
    //da li mac jos uvek leti
    this.transitionIsActive = true;
    const fieldThatAttk = document.getElementById(fieldID);
    var attack: any = fieldThatAttk?.getElementsByClassName('attack')[0];
    const attackedField = document.getElementById(attackedFieldID);
    const attackCentar = this.getCenter(attack);
    const attackedCentar = this.getCenter(attackedField);

    var angle = Math.atan2(
      attackCentar.y - attackedCentar.y,
      attackCentar.x - attackedCentar.x
    );
    if (angle >= 0) {
      var distance = Math.sqrt(
        Math.pow(attackedCentar.x - attackCentar.x, 2) +
          Math.pow(attackedCentar.y - attackCentar.y, 2)
      );
      var translateY = distance * Math.sin(angle) * -1;
      var translateX = distance * Math.cos(angle) * -1;
      angle = angle - 3.14 / 2;
    } else {
      var distance = Math.sqrt(
        Math.pow(attackedCentar.x - attackCentar.x, 2) +
          Math.pow(attackCentar.y - attackedCentar.y, 2)
      );
      var translateY = distance * Math.sin(angle);
      var translateX = distance * Math.cos(angle) * -1;
      angle = -angle - 3.14 / 2;
    }
    attack.style.transform =
      'translate(' +
      translateX +
      'px,' +
      translateY +
      'px) rotate(' +
      angle +
      'rad)';
    attack.style.transition = 'transform 0.5s ';

    setTimeout(() => {
      const index = this.fieldsAbleToAttack.indexOf(fieldID);
      const x = this.fieldsAbleToAttack.splice(index, 1);
      this.transitionIsActive = false;
    }, 500);
  }
  //dobijamo broj izgubljenih poena i polje koje je te poene izgubilo
  getFieldThatLostPoints() {
    this.signalrService.hubConnection.on(
      'FieldsThatLostPoints',
      (diference: any, field: any) => {
        console.log(diference, field);
        this.fieldThatLostPoints = field;
        if (diference > 0) {
          diference = -diference;
        }
        this.pointsLost = diference;
        setTimeout(() => {
          this.pointsLost = undefined;
          this.fieldThatLostPoints = undefined;
        }, 2000);
      }
    );
  }

  //kad dodje do promene poena pojavljuje se ispod tj iznad zivotnih poena
  getPointsReduce(
    paragrafHtml: any,
    curentLifePoints: any,
    newLifePoints: any
  ) {
    const lpReduce = newLifePoints - curentLifePoints;
    if (lpReduce != 0) {
      curentLifePoints = newLifePoints;
      if (lpReduce < 0) {
        paragrafHtml.nativeElement.style.color = 'rgb(255, 96, 96)';
        paragrafHtml.nativeElement.innerText = lpReduce;
      } else if (lpReduce > 0) {
        paragrafHtml.nativeElement.style.color = 'rgb(0 161 255)';
        paragrafHtml.nativeElement.innerText = '+' + lpReduce;
      }
      setTimeout(() => {
        paragrafHtml.nativeElement.innerText = null;
      }, 1000);
    }
  }

  checkNumberOfMonstersOnField(cardFields: any) {
    var counter: any = 0;
    cardFields.forEach((element: any) => {
      if (element.cardOnField != null) {
        if (element.fieldIndex < 5) {
          counter++;
        }
      }
    });
    return counter;
  }
  getFieldsIDsThatCanActivateTrapCard() {
    this.signalrService.hubConnection.on(
      'EnemiseFieldsThatCanActivateTrapCard',
      (listOFfieldsIDs: any) => {
        console.log('odje', listOFfieldsIDs);
        const allIDsMatch = listOFfieldsIDs.every((id: any) =>
          this.enemiesField.cardFields.some((obj: any) => obj.fieldID === id)
        );
        console.log(allIDsMatch);
        if (allIDsMatch == true) {
          this.doYouNeedToWaitEnemiesChoice = allIDsMatch;
        } else {
          this.fieldsThatCanActivateTrapCard = listOFfieldsIDs;
          this.canYouActivateTrapCard = true;
        }
      }
    );
  }

  onYouWantToActivateTrapCardClick() {
    this.doYouWantToActivateTrapCard = true;
  }
  onYouDontWantToActivateTrapCardClick() {
    this.gameService.didTrapEffectExecutedInv(this.gameID, this.playerID);
  }
  getFieldThatMonsterIsSummonedOn() {
    this.signalrService.hubConnection.on(
      'GetLastSummonedEnemiesMonster',
      (fieldID: any) => {
        this.lastSummonedMonster = fieldID;
      }
    );
  }

  getIfYouCanContinuePlaying() {
    this.signalrService.hubConnection.on('DidTrapEffectExecuted', () => {
      console.log('bababababa');
      this.canYouActivateTrapCard = undefined;
      this.fieldsThatCanActivateTrapCard = undefined;
      this.doYouWantToActivateTrapCard = undefined;
      setTimeout(() => {
        this.doYouNeedToWaitEnemiesChoice = false;
      }, 1000);
    });
  }
  chackAreaOfCliciking() {
    var check=false;
    if (this.areaOfClicking.areaOfClicking == 5) {
      if (this.enemiesHand.length == 0) {
        check=true;
      }
    } else if (this.areaOfClicking.areaOfClicking == 4) {
      var monsterOnYourField = this.checkNumberOfMonstersOnField(
        this.playerField.cardFields
      );
      var monstersOnEnemies = this.checkNumberOfMonstersOnField(
        this.enemiesField.cardFields
      );
      if (monsterOnYourField == 5 || monstersOnEnemies == 0) {
        check=true;
      }
    }
    //monster grave
    else if(this.areaOfClicking.areaOfClicking == 2){
      this.getGraveByType("MonsterCard",1);
    }
    //st grave
    else if(this.areaOfClicking.areaOfClicking == 3){
      this.getGraveByType("Card",1);
    }
    if(check){
      this.gameService.removeCardFromFieldToGraveInv(
        this.areaOfClicking.fieldID,
        this.playerID,
        this.gameID
      );
      this.areaOfClicking=undefined;
    }
  }
  onGraveCardClick(card:any){
    if(this.areaOfClicking.areaOfClicking == 3 || this.areaOfClicking.areaOfClicking == 2){
      this.gameService.executeEffectInv(
        [card.id],
        this.areaOfClicking.fieldID,
        this.playerID,
        this.enemiesID,
        Number(this.gameID)
      );
      this.isGraveShown=false;
      this.areaOfClicking = undefined;
    }
  }
  getGraveByType(typeFilter: any, pageNumber:any ) {
    this.curentFilter=typeFilter;
    this.isExitGraveShown=true;
    this.subscriptions.push(
      this.gameService.getGraveByType(this.gameID, typeFilter, pageNumber, 5).subscribe({
        next: (res: any) => {
          this.graveStats = res;
          if(this.areaOfClicking?.areaOfClicking == 3 || this.areaOfClicking?.areaOfClicking == 2){
            if(this.graveStats.totalItems==0){
                this.gameService.removeCardFromFieldToGraveInv(
                this.areaOfClicking.fieldID,
                this.playerID,
                this.gameID
              );
              this.areaOfClicking=undefined;
            }
            this.isExitGraveShown=false;
          }
          console.log(res);
          if(this.graveStats.totalItems>0){
          this.isGraveShown=true;
          }
        },
        error: (err) => {
          console.log(err.error);
        },
      })
    );
  }
  onGraveClick(){
    console.log("baebae")
    this.getGraveByType('',1);
  }
  exitGrave(){
    this.isGraveShown=false;
  }
  onPageChanged(event:any){
    const nextPageIndex = event.page + 1;
    event.page = 0;
    this.getGraveByType(this.curentFilter,nextPageIndex);
  }
  ngOnDestroy(): void {
    this.subscriptions.forEach((subscription) => subscription.unsubscribe());
    this.signalrService.hubConnection.off('GetYourField');
    this.signalrService.hubConnection.off('GetEnemiesField');
    this.signalrService.hubConnection.off('WinnerMessage');
    this.signalrService.hubConnection.off('GetCardsInYourHand');
    this.signalrService.hubConnection.off('GetCardsInEnemiesHand');
    this.signalrService.hubConnection.off('GetGraveData');
    this.signalrService.hubConnection.off('GetTurnInfo');
    this.signalrService.hubConnection.off('GetAreaOfClicking');
    this.signalrService.hubConnection.off('GetFieldsAbleToAttack');
    this.signalrService.hubConnection.off('GetFieldsIncludedInAttack');
    this.signalrService.hubConnection.off('FieldsThatLostPoints');
    this.signalrService.hubConnection.off('EnemiseFieldsThatCanActivateTrapCard');
    this.signalrService.hubConnection.off('GetLastSummonedEnemiesMonster');
    this.signalrService.hubConnection.off('DidTrapEffectExecuted');
  }
}
