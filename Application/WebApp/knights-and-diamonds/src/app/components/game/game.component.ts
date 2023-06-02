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
  fieldsAbleToAttack: any = [5571];
  swordRadyToAttack: any;
  angle: any;
  transitionIsActive: any = false;

  fieldReadyAttack: any;

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
  attackField(attackedFieldDiv: any, fieldID: any) {
    this.gameService.attackEnemiesFieldInv(
      Number(this.gameID),
      this.playerID,
      this.fieldReadyAttack,
      fieldID
    );
    // console.log(attackedFieldDiv);
    // this.transitionIsActive = true;
    // this.swordRadyToAttack.style.transform = `rotate(${this.angle - 30}rad)`;
    // const fieldCentar = this.getCenter(attackedFieldDiv);
    // const pom = this.getCenter(this.swordRadyToAttack);

    // console.log(fieldCentar);

    // const x = fieldCentar.x - pom.x;
    // const y = fieldCentar.y - pom.y;
    // console.log(x);
    // console.log(y);

    // if (this.swordRadyToAttack != undefined) {
    //   this.swordRadyToAttack.style.transform = `translate(${x}px,${y}px) rotate(${
    //     this.angle - 30
    //   }rad)`;
    //   this.swordRadyToAttack.style.transition = 'transform 0.5s ';

    //   setTimeout(() => {
    //     this.fieldsAbleToAttack = undefined;
    //     this.transitionIsActive = true;
    //   }, 500);
    // }
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
      var translateX = distance * Math.cos(angle);
      angle = angle * (4 / 9);
    } else {
      var distance = Math.sqrt(
        Math.pow(attackedCentar.x - attackCentar.x, 2) +
          Math.pow(attackCentar.y - attackedCentar.y, 2)
      );
      var translateY = distance * Math.sin(angle);
      var translateX = distance * Math.cos(angle);
      angle = angle * (-4 / 9);
    }
    attack.style.transform =
      'translate(' +
      -translateX +
      'px,' +
      translateY +
      'px) rotate(' +
      angle +
      'rad)';
    attack.style.transition = 'transform 0.5s ';

    setTimeout(() => {
      this.fieldsAbleToAttack = undefined;
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
    if (this.playerField.gameStarted == false) {
      this.gameService.drawPhaseInv(Number(this.gameID), this.playerID);
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
      console.log('O V A A M O', turnInfo);
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
    var timer: any = setTimeout(() => {
      this.phases.forEach((element) => {
        element.status = false;
      });
      let phase = this.phases.find((x) => x.key == turnInfo.turnPhase);
      phase.status = true;
      this.curentPhase = phase;
      console.log('f a z a', this.curentPhase);
    }, 1000);
  }
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
  getStartingDrawingEffect(i: any, targetArray: any, sourceArray: any) {
    var timer = setInterval(() => {
      if (i == sourceArray.length) {
        clearInterval(timer);
        return;
      }
      targetArray.push(sourceArray[i]);
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
    if (card.cardType == 'MonsterCard') {
      this.openSummoningMonsterWindow(card);
    }
    if (card.cardType == 'SpellCard') {
      console.log(card);
      this.playSpellCard(card.id, card.cardEffectID);
    }
  }
  openSummoningMonsterWindow(card: any) {
    console.log('cs', card);
    if (this.cardToBeSummoned == undefined && this.isPlayerOnTurn == true) {
      this.cardToBeSummoned = card;
    }
  }
  closeSummoningMonsterWindow() {
    console.log('bravo');
    this.cardToBeSummoned = undefined;
  }
  summonMonsterInAttackPosition() {
    this.normalSummon(this.cardToBeSummoned.id, true);
  }
  summonMonsterInDeffencePosition() {
    this.normalSummon(this.cardToBeSummoned.id, false);
  }
  battlePhase(phase: any) {
    if ((phase.name = 'BP')) {
      this.gameService.battlePhaseInv(this.gameID, this.playerID);
    }
  }
}
