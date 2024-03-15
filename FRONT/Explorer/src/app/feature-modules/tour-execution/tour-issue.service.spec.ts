import { TestBed } from '@angular/core/testing';

import { TourIssueService } from './tour-issue.service';

describe('TourIssueService', () => {
  let service: TourIssueService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(TourIssueService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
