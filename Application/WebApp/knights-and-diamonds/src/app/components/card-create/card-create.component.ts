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
  card=card;
  stcard!:STCard;
  form!:FormGroup;
  cardTypes:any;
  elementTypes:any;
  monsterTypes:any;
  isDisabled=false;

  // progress!: number;
  // message!: string;
  pickur:any;
  // @Output() public onUploadFinished = new EventEmitter();
  subscripions: Subscription[] = []

  constructor(private fb: FormBuilder, 
    private http:HttpClient,
    private messageService: MessageService,
    private cardService:CardService,
    private confirmationService: ConfirmationService) 
    { 

    }


  ngOnInit(): void {
    this.getCardTypes();
    this.getElementTypes();
    this.getMonsterTypes();

    // this.card.cardType=this.cardTypes.type;
    console.log("ovdeeeee",this.card)

    this.form = this.fb.group({
      cardName: "",
      cardType: 3,
      description: "",
      elementType: 6,
      imgPath:"",
      numberOfStars: ['', [Validators.min(0), Validators.max(11), Validators.maxLength(2)]],
      monsterType: 1,
      attackPoints: ['', [Validators.min(0), Validators.max(9999), Validators.maxLength(4)]],
      defencePoints: ['', [Validators.min(0), Validators.max(9999), Validators.maxLength(4)]]
    })
  }

  uploadFinished = (event:any) => { 
    this.card.imgPath=event.dbPath;
    this.form.value["imgPath"]=event.dbPath;
  }

  onCardNameChange(): void {  
    this.card.cardName=this.form.value["cardName"]
  }
  onLevelChange() {
    this.card.numberOfStars=this.form.value["numberOfStars"]
  }
  onAttackChange() {
    this.card.attackPoints=this.form.value["attackPoints"]
  }
  onDefenceChange() {
    this.card.defencePoints=this.form.value["defencePoints"]
  }
  onDescriptionChange() {
    this.card.description=this.form.value["description"]
  }
  onChangeMonsterType() {
    let mt=this.monsterTypes.find((x: { id: any; })=>x.id==this.form.value["monsterType"])
    this.card.monsterType=mt.type
  }
  onElementTypeChange() {
    let et=this.elementTypes.find((x: { id: any; })=>x.id==this.form.value["elementType"])
    this.card.elementType=et.imgPath
  }
  onCardTypeChange() {
    let ct=this.cardTypes.find((x: { id: any; })=>x.id==this.form.value["cardType"])
    this.card.cardType=ct.type
    if(ct.type!=="Monster")
    {
      this.card.elementType=ct.imgPath
    }
    else
    {
      let et=this.elementTypes.find((x: { id: any; })=>x.id==this.form.value["elementType"])
      this.card.elementType=et.imgPath
      console.log(et)
    }

  }

  getCardTypes()
  {
    this.cardService.getCardTypes().subscribe({
      next: res=> {
        console.log(res);
        this.cardTypes=res;
        // console.log(this.cardTypes.type)
        console.log(this.cardTypes)
  
      },
      error: err=> {
        console.log("neuspesno")
      }
    })
  }

  getElementTypes()
  {
    this.cardService.getElementTypes().subscribe({
      next: res=> {
        console.log(res);
        this.elementTypes=res;
      },
      error: err=> {
        console.log("neuspesno")
      }
    })
  }

  getMonsterTypes()
  {
    this.cardService.getMonsterTypes().subscribe({
      next: res=> {
        console.log(res);
        this.monsterTypes=res;
      },
      error: err=> {
        console.log("neuspesno")
      }
    })
  }

  handleClick()
  {
    let p=this.form.getRawValue()

    if(p.cardType!==3)
    {
      this.form.controls["elementType"].disable()
      console.log(this.form.getRawValue());
    //   this.form.getRawValue()
      
    //   console.log("ovdeeeeeeeeeeeeeeeeeeeeeee",p)
    // }
    //   p.imgPath=this.form.value["imgPath"]
    //   console.log(p)

    //   this.cardService.addCard(p).subscribe({
    //     next: (res: any)=> {
    //       console.log(res);
    //       this.monsterTypes=res;
    //     },
    //     error: (err:any)=> {
    //       console.log("neuspesno")
    //     } 
    //   })
    // }
    }}  
  }
