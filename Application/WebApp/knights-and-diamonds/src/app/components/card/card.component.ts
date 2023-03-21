import { Component, OnInit ,Input} from '@angular/core';
import { Card } from 'src/classes/card';
import { card } from 'src/classes/card-data';

@Component({
  selector: 'app-card',
  templateUrl: './card.component.html',
  styleUrls: ['./card.component.css']
})
export class CardComponent implements OnInit {
  @Input() card!: Card;
  
  constructor() { }

  ngOnInit(): void {
    console.log("odje",this.card)
  }
}
