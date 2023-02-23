import { Component, OnInit } from '@angular/core';
import { card } from 'src/classes/card-data';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { Card } from 'src/classes/card';
import { CardType } from 'src/classes/card-type';
import { cardType } from 'src/classes/card-types-data';
import { elementType } from 'src/classes/element-type-datas';
import { monserType } from 'src/classes/monster-type-data';
import { ConfirmationService, MessageService } from 'primeng/api';
import { Message } from 'primeng//api';

@Component({
  selector: 'app-card-create',
  templateUrl: './card-create.component.html',
  styleUrls: ['./card-create.component.css']
})
export class CardCreateComponent implements OnInit {
  
  card=card;
  form!:FormGroup;
  cardType=cardType;
  elementType=elementType;
  monsterType=monserType;
  isDisabled=false;

  constructor(private fb: FormBuilder, 
    private httpClient:HttpClient,
    private messageService: MessageService,
    private confirmationService: ConfirmationService) 
    { 

    }

  ngOnInit(): void {
    this.form = this.fb.group({
      cardName: '',
      cardType: "MonsterCard",
      effect: "",
      elementType: "",
      numberOfStars: ['', [Validators.min(0), Validators.max(11), Validators.maxLength(2)]],
      monsterType: "Dragon",
      attackPoints: ['', [Validators.min(0), Validators.max(9999), Validators.maxLength(4)]],
      defencePoints: ['', [Validators.min(0), Validators.max(9999), Validators.maxLength(4)]]
    })
    this.form.value['elementType'].name;
  }

  onSearchChange(): void {  
    this.card.cardName=this.form.value['cardName'];
    this.card.effect=this.form.value['effect']; 
  }
  onSearchChangeLevel()
  {
    this.card.numberOfStars=this.form.value['numberOfStars'];
  }
  onFileSelected(event:any) {
    const file:File = event.target.files[0];
    const pom=file.name
    this.card.imgPath="../../../assets/"+pom;
  }
  onChange()
  {
    console.log(this.card.cardType)

    this.card.cardType=this.form.value['cardType'];
    elementTypePom:this.card.elementType
    if(this.form.value['cardType']=="TrapCard")
    {
      this.card.elementType="../../../assets/Trap.png";
    }
    if(this.form.value['cardType']=="SpellCard")
    {
      this.card.elementType="../../../assets/Spell.png";
    }
    if(this.form.value['cardType'].code=="MonsterCard")
    {
      this.card.elementType=this.form.value['elementType'].code;
      if(this.form.value['elementType'].code==undefined)
      {
        this.card.elementType="../../../assets/fire.png"
      }
    }
    this.card.numberOfStars=11
  }
  onChangeElement()
  {
    console.log(this.card.elementType)
    this.card.elementType=this.form.value['elementType']

  }
  onChangeMonsterType()
  {
    this.card.monsterType=this.form.value['monsterType'].code
  }
  onAttackChange()
  {
    this.card.attackPoints=this.form.value['attackPoints']
  }
  onDefenceChange()
  {
    this.card.defencePoints=this.form.value['defencePoints']
  }
  handleClick()
  {
    console.log("ovde")
    console.log(this.form.getRawValue())
    this.httpClient.post("https://localhost:7250/Card/AddCard",this.form.getRawValue()).subscribe({
      next: res=>{
      this.messageService.add({key: 'br', severity:'success', summary: 'Uspešno', detail: 'Dodali ste kartu!'});
      },
      error: err=>{
      this.messageService.add({key: 'br', severity:'error', summary: 'Neuspešno', detail: 'Pokušajte ponovo, došlo je do greške!'});
      }
    });
    
    // this.confirmationService.confirm({
    //   message: 'Da li ste sigurni da želite da izvršite ovu radnju?',
    //   accept: () => {
       
    //   }
    // });
  }
}
