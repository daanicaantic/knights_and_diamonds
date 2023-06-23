import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-rules',
  templateUrl: './rules.component.html',
  styleUrls: ['./rules.component.css']
})
export class RulesComponent implements OnInit {

  showText1: boolean = true;

  constructor() { }

  ngOnInit(): void {
  }

  translate() {
    this.showText1 = !this.showText1;
  }

}
