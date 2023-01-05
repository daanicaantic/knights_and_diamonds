import { Component, OnInit } from '@angular/core';
import { card } from 'src/classes/card-data';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { Card } from 'src/classes/card';
import { cardType } from 'src/classes/card-types-data';
import { elementType } from 'src/classes/element-type-datas';
import { monserType } from 'src/classes/monster-type-data';


@Component({
  selector: 'app-card-create',
  templateUrl: './card-create.component.html',
  styleUrls: ['./card-create.component.css']
})
export class CardCreateComponent implements OnInit {
  card=card;
  form!: FormGroup;
  cardType=cardType;
  elementType=elementType;
  monsterType=monserType;
  constructor(private fb: FormBuilder, 
    private httpClient:HttpClient) 
    { 

    }

  ngOnInit(): void {
    this.form = this.fb.group({
      card_name:'',
      number_of_stars:'',
      ddCardType:"",
      ddElementType:"",
      ddMonsterType:"",
      card_description:"",

    })
    this.form.value['ddElementType'].name;
  }
  onSearchChange(): void {  
    this.card.card_name=this.form.value['card_name'];
    this.card.card_description=this.form.value['card_description'];

    console.log(this.form.value['number_of_stars'])    
  }
  onSearchChangeLevel()
  {
    this.card.number_of_stars=this.form.value['number_of_stars'];
  }
  onFileSelected(event:any) {
    const file:File = event.target.files[0];
    const pom=file.name
    this.card.card_photo="../../../assets/"+pom;
  }
  onChange()
  {
    this.card.card_type=this.form.value['ddCardType'].code;
    elementTypePom:this.card.element_type
    if(this.form.value['ddCardType'].code=="TrapCard")
    {
      this.card.element_type="../../../assets/Trap.png";
    }
    if(this.form.value['ddCardType'].code=="SpellCard")
    {
      this.card.element_type="../../../assets/Spell.png";
    }
    if(this.form.value['ddCardType'].code=="MonsterCard")
    {
      this.card.element_type=this.form.value['ddElementType'].code;
      if(this.form.value['ddElementType'].code==undefined)
      {
        this.card.element_type="../../../assets/fire.png"
      }
    }
    this.card.number_of_stars=11
  }
  onChangeElement()
  {
    this.card.element_type=this.form.value['ddElementType'].code
  }
  onChangeMonsterType()
  {
    this.card.monster_type=this.form.value['ddMonsterType'].code
  }
}
