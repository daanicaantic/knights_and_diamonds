import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LoadinscreenComponent } from './loadinscreen.component';

describe('LoadinscreenComponent', () => {
  let component: LoadinscreenComponent;
  let fixture: ComponentFixture<LoadinscreenComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ LoadinscreenComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(LoadinscreenComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
