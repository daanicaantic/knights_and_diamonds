import { Component, OnInit } from '@angular/core';
import { card } from 'src/classes/card-data';

@Component({
  selector: 'app-test',
  templateUrl: './test.component.html',
  styleUrls: ['./test.component.css']
})
export class TestComponent implements OnInit {
  card = card;
  constructor() { }

  ngOnInit(): void {
  }

}
