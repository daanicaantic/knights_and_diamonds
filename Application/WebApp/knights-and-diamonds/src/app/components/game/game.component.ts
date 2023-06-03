import { Component, OnDestroy, OnInit } from '@angular/core';
import { card } from 'src/classes/card-data';
import { IngameService } from 'src/app/services/ingame.service';
import { GameService } from 'src/app/services/game.service';
import { AuthService } from 'src/app/services/auth.service';
import { ActivatedRoute, Router } from '@angular/router';
import { SignalrService } from 'src/app/services/signalr.service';
import { TmplAstRecursiveVisitor } from '@angular/compiler';
import { DOCUMENT } from '@angular/common';

@Component({
  selector: 'app-game',
  templateUrl: './game.component.html',
  styleUrls: ['./game.component.css'],
})
export class GameComponent implements OnInit, OnDestroy {
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
  fieldsAbleToAttack: any = [5601, 5602, 5603, 5604, 5605];
  swordRadyToAttack: any;
  angle: any;
  transitionIsActive: any = false;
  turnInfo: any;
  fieldReadyAttack: any;
  pom1: any;
  pom2: any;
  constructor(
    public inGameService: IngameService,
    public gameService: GameService,
    private router: Router,
    private route: ActivatedRoute,
    private authService: AuthService,
    private signalrService: SignalrService
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
    this.getGraveByHub();
    this.getFieldsAbleToAttack();
    this.getFieldsIncludedInAttack();
    // this.getStartingDrawing();
    if (this.playerID != undefined) {
      if (this.signalrService.hubConnection.state == 'Connected') {
        this.gameService.getTurnInfoInv(Number(this.gameID), this.playerID);
      } else {
        this.signalrService.ssSubj.subscribe((obj: any) => {
          if (obj.type == 'HubConnStarted') {
            this.gameService.getTurnInfoInv(Number(this.gameID), this.playerID);
          }
        });
      }
    }
  }

  getCenter(element: any) {
    const { left, top, width, height } = element.getBoundingClientRect();
    return { x: left + width / 2, y: top + height / 2 };
  }
  getSword(swordDiv: any, fieldID: any) {
    console.log(swordDiv);

    this.fieldReadyAttack = fieldID;
    console.log('pomara', this.fieldReadyAttack);
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
  attackField(fieldID: any) {
    this.gameService.attackEnemiesFieldInv(
      Number(this.gameID),
      this.playerID,
      this.fieldReadyAttack,
      fieldID
    );
  }
  pomfja(fieldID: any, attackedFieldID: any) {
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
    console.log(attackCentar);
    console.log(attackedCentar);
    console.log(angle);
  }
  getFieldsIncludedInAttack() {
    this.signalrService.hubConnection.on(
      'GetFieldsIncludedInAttack',
      (fieldID: any, attackedFieldID: any) => {
        console.log('BAEBAEBAE', fieldID, attackedFieldID);
        this.pomfja(fieldID, attackedFieldID);
      }
    );
  }

  ngOnDestroy(): void {
    this.inGameService.setGameOff();
    this.router.navigate(['/home']);
  }

  setGameStatus() {
    this.inGameService.setGameOn();
  }
  onLoadingOver(event: any) {
    this.isLoadingOver = event;
    console.log(this.playerField.gameStarted);
    if (
      this.playerField != undefined &&
      this.playerField.gameStarted == false
    ) {
      this.gameService.startingDrawingInv(Number(this.gameID), this.playerID);
      if (this.isPlayerOnTurn) {
        console.log('fiona2018xoffwhitevirginlabblock', this.isPlayerOnTurn);
        this.gameService.drawPhaseInv(Number(this.gameID), this.playerID);
      }
    }
    this.getStartingTurnInfo();
  }

  //koristimo fju prilikom ngOnInita vraca ceo prikaz table
  //preko kontrolera,preko signalr-a,ukoliko dodje do refresha
  //dolazi do gresaka
  getStartingField() {
    this.gameService.getStartingField(this.playerID, this.enemiesID).subscribe({
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
    });
  }
  getStartingTurnInfo() {
    this.gameService.getTurnInfo(this.gameID, this.playerID).subscribe({
      next: (res: any) => {
        this.setTurnInfo(res);
        console.log('T U R N I N F O', res);
      },
      error: (err) => {
        console.log(err.error);
      },
    });
  }
  getTurnInfo() {
    this.signalrService.hubConnection.on('GetTurnInfo', (turnInfo: any) => {
      this.turnInfo = turnInfo;
      this.setTurnInfo(turnInfo);
    });
  }
  setTurnInfo(turnInfo: any) {
    this.curentPhase = turnInfo.turnPhase;
    if (turnInfo.playerOnTurn == this.playerID) {
      this.isPlayerOnTurn = true;
    } else {
      this.isPlayerOnTurn = false;
    }
    // var timer: any = setTimeout(() => {
    this.phases.forEach((element) => {
      element.status = false;
    });
    if (turnInfo.turnPhase != 4) {
      let phase = this.phases.find((x) => x.key == turnInfo.turnPhase);
      phase.status = true;
      this.curentPhase = phase;
      console.log('f a z a', this.curentPhase);
    }
    // }, 1000);
  }
  getHands() {
    this.signalrService.hubConnection.on(
      'GetCardsInYourHand',
      (playerHand: any) => {
        if (this.playerField.gameStarted == false) {
          this.getStartingDrawingEffect(
            0,
            this.playerHand,
            playerHand,
            this.pom1
          );
        } else {
          this.playerHand = playerHand;
        }
      }
    );
    this.signalrService.hubConnection.on(
      'GetCardsInEnemiesHand',
      (enemiesHand: any) => {
        if (this.playerField.gameStarted == false) {
          this.getStartingDrawingEffect(
            0,
            this.enemiesHand,
            enemiesHand,
            this.pom2
          );
        } else {
          this.enemiesHand = enemiesHand;
        }
      }
    );
  }

  getPlayersField() {
    this.signalrService.hubConnection.on('GetYourField', (field: any) => {
      this.playerField = field;
      this.playerHand = field.hand;
      console.log(this.playerField);
    });
  }
  getEnemiesField() {
    this.signalrService.hubConnection.on(
      'GetEnemiesField',
      (enemiesField: any) => {
        this.enemiesField = enemiesField;
        this.enemiesHand = enemiesField.hand;

        console.log(this.enemiesField);
      }
    );
  }
  getGrave() {
    this.gameService.getGrave(this.gameID).subscribe({
      next: (res: any) => {
        this.grave = res;
        console.log('grave', res);
      },
      error: (err) => {
        console.log(err.error);
      },
    });
  }
  getGraveByHub() {
    this.signalrService.hubConnection.on('GetGraveData', (grave: any) => {
      this.grave = grave;
      console.log('grave', this.grave);
    });
  }
  //koristimo fju za prikaz pocetnog izvlacenja karata;
  getStartingDrawingEffect(
    i: any,
    targetArray: any,
    sourceArray: any,
    niz: any
  ) {
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
      // this.playerField.deckCount = this.playerField.deckCount - 1;
      i++;
    }, 300);
  }
  //koristimo fju za pri

  onEnemiesCardMouseOver(field: any) {
    this.mediumCard = field.cardOnField;
    if (field.cardShowen == true) {
      this.iscardshowen = true;
    } else {
      this.iscardshowen = false;
    }
  }

  changePhase() {
    let nextPhase: any;
    let phase = this.phases.find((x) => x.status == true);
    phase.status = false;
    if (phase.key == 3) {
      nextPhase = 0;
      if (this.isPlayerOnTurn == true) {
        this.isPlayerOnTurn = false;
      } else {
        this.isPlayerOnTurn = true;
      }
    } else {
      nextPhase = phase.key + 1;
    }
    phase = this.phases.find((x) => x.key == nextPhase);
    phase.status = true;
    console.log(this.phases);
  }
  normalSummon(cardID: any, position: any) {
    console.log(cardID);
    this.gameService.normalSummonInv(
      Number(this.gameID),
      this.playerID,
      cardID,
      position
    );
    this.cardToBeSummoned = undefined;
  }
  playSpellCard(cardID: any, cardEffectID: any) {
    this.gameService.playSpellCardInv(
      Number(this.gameID),
      this.playerID,
      cardID,
      cardEffectID
    );
  }
  getDataAffterPlayedEffectCard() {
    this.signalrService.hubConnection.on(
      'GetAreaOfClicking',
      (affterData: any) => {
        console.log('AREAOFCLICKING', affterData);

        this.setAreaOfClicking(affterData);
      }
    );
  }
  getFieldsAbleToAttack() {
    this.signalrService.hubConnection.on(
      'GetFieldsAbleToAttack',
      (listOFfieldsIDs: any) => {
        console.log('BAEBAEBAE', listOFfieldsIDs);
        this.fieldsAbleToAttack = listOFfieldsIDs;
      }
    );
  }
  setAreaOfClicking(affterData: any) {
    console.log('AREAOFCLICKING', affterData);

    if (affterData.areaOfClicking == 0) {
      this.gameService.executeEffectInv(
        [0],
        affterData.fieldID,
        this.playerID,
        Number(this.gameID)
      );
    }
  }
  playCard(card: any) {
    if (this.playerHand.length < 7) {
      if (
        card.cardType == 'MonsterCard' &&
        this.turnInfo.isMonsterSummoned == false &&
        this.curentPhase.name == 'MP'
      ) {
        this.turnInfo.isMonsterSummoned = true;
        this.openSummoningMonsterWindow(card);
      }
      if (
        card.cardType == 'SpellCard' &&
        this.curentPhase.name == 'MP' &&
        this.isPlayerOnTurn
      ) {
        this.playSpellCard(card.id, card.cardEffectID);
      }
    } else if (this.isPlayerOnTurn) {
      this.gameService.removeCardFromHandToGraveInv(
        this.playerID,
        card.id,
        this.gameID
      );
    }
  }
  openSummoningMonsterWindow(card: any) {
    if (this.cardToBeSummoned == undefined && this.isPlayerOnTurn == true) {
      this.cardToBeSummoned = card;
    }
  }
  closeSummoningMonsterWindow() {
    this.cardToBeSummoned = undefined;
  }
  summonMonsterInAttackPosition() {
    this.normalSummon(this.cardToBeSummoned.id, true);
  }
  summonMonsterInDeffencePosition() {
    this.normalSummon(this.cardToBeSummoned.id, false);
  }
  battlePhase(phase: any) {
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
}
