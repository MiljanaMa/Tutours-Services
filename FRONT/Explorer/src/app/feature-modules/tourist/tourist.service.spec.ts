import { TestBed } from '@angular/core/testing';

import { TouristService } from './tourist.service';

describe('TouristService', () => {
  let service: TouristService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(TouristService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
