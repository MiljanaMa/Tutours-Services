import { TestBed } from '@angular/core/testing';

import { TourAuthoringService } from './tour-authoring.service';

describe('TourAuthoringService', () => {
  let service: TourAuthoringService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(TourAuthoringService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
