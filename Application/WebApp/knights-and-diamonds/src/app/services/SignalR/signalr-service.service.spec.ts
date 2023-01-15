import { TestBed } from '@angular/core/testing';

import { SignalrServiceService } from './signalr-service.service';

describe('SignalrServiceService', () => {
  let service: SignalrServiceService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(SignalrServiceService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
